using System.Collections.Concurrent;
using System.Globalization;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace YiKdWebClient.Tests.TestInfrastructure;

internal sealed record RecordedHttpRequest(
    string Method,
    string PathAndQuery,
    IReadOnlyDictionary<string, string> Headers,
    string Body);

internal sealed record TestHttpResponse(
    int StatusCode = 200,
    string Body = "{\"ok\":true}",
    string ContentType = "application/json; charset=utf-8",
    IReadOnlyDictionary<string, string>? Headers = null);

/// <summary>
/// A dependency-free HTTP/1.1 server used to exercise the library's real HttpClient code.
/// Every test gets its own ephemeral loopback port, so no Kingdee server is required.
/// </summary>
internal sealed class LoopbackHttpServer : IDisposable
{
    private readonly CancellationTokenSource _cancellation = new();
    private readonly Func<RecordedHttpRequest, TestHttpResponse> _responseFactory;
    private readonly TcpListener _listener;
    private readonly Task _serverTask;

    public LoopbackHttpServer(Func<RecordedHttpRequest, TestHttpResponse>? responseFactory = null)
    {
        _responseFactory = responseFactory ?? (_ => new TestHttpResponse());
        _listener = new TcpListener(IPAddress.Loopback, 0);
        _listener.Start();

        var port = ((IPEndPoint)_listener.LocalEndpoint).Port;
        RootUrl = $"http://127.0.0.1:{port}/";
        K3CloudUrl = RootUrl + "k3cloud/";
        _serverTask = Task.Run(RunAsync);
    }

    public string RootUrl { get; }

    public string K3CloudUrl { get; }

    public ConcurrentQueue<RecordedHttpRequest> Requests { get; } = new();

    public RecordedHttpRequest SingleRequest()
    {
        return Assert.Single(Requests.ToArray());
    }

    public void Dispose()
    {
        _cancellation.Cancel();
        _listener.Stop();

        try
        {
            _serverTask.GetAwaiter().GetResult();
        }
        catch (OperationCanceledException)
        {
        }
        catch (ObjectDisposedException)
        {
        }

        _cancellation.Dispose();
    }

    private async Task RunAsync()
    {
        while (!_cancellation.IsCancellationRequested)
        {
            TcpClient client;
            try
            {
                client = await _listener.AcceptTcpClientAsync(_cancellation.Token);
            }
            catch (OperationCanceledException)
            {
                break;
            }
            catch (SocketException) when (_cancellation.IsCancellationRequested)
            {
                break;
            }

            await HandleClientAsync(client, _cancellation.Token);
        }
    }

    private async Task HandleClientAsync(TcpClient client, CancellationToken cancellationToken)
    {
        using (client)
        using (var stream = client.GetStream())
        using (var reader = new StreamReader(
                   stream,
                   new UTF8Encoding(encoderShouldEmitUTF8Identifier: false),
                   detectEncodingFromByteOrderMarks: false,
                   bufferSize: 4096,
                   leaveOpen: true))
        {
            var requestLine = await reader.ReadLineAsync(cancellationToken);
            if (string.IsNullOrWhiteSpace(requestLine))
            {
                return;
            }

            var requestParts = requestLine.Split(' ', 3, StringSplitOptions.RemoveEmptyEntries);
            if (requestParts.Length < 2)
            {
                return;
            }

            var headers = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            while (true)
            {
                var line = await reader.ReadLineAsync(cancellationToken);
                if (string.IsNullOrEmpty(line))
                {
                    break;
                }

                var separator = line.IndexOf(':');
                if (separator <= 0)
                {
                    continue;
                }

                var name = line[..separator].Trim();
                var value = line[(separator + 1)..].Trim();
                headers[name] = headers.TryGetValue(name, out var existing)
                    ? existing + ", " + value
                    : value;
            }

            var body = await ReadBodyAsync(reader, headers, cancellationToken);
            var request = new RecordedHttpRequest(requestParts[0], requestParts[1], headers, body);
            Requests.Enqueue(request);

            var response = _responseFactory(request);
            await WriteResponseAsync(stream, response, cancellationToken);
        }
    }

    private static async Task<string> ReadBodyAsync(
        StreamReader reader,
        IReadOnlyDictionary<string, string> headers,
        CancellationToken cancellationToken)
    {
        if (headers.TryGetValue("Transfer-Encoding", out var transferEncoding) &&
            transferEncoding.Contains("chunked", StringComparison.OrdinalIgnoreCase))
        {
            var chunkedBody = new StringBuilder();
            while (true)
            {
                var sizeLine = await reader.ReadLineAsync(cancellationToken) ?? "0";
                var extensionIndex = sizeLine.IndexOf(';');
                var sizeText = extensionIndex >= 0 ? sizeLine[..extensionIndex] : sizeLine;
                var chunkSize = int.Parse(sizeText, NumberStyles.HexNumber, CultureInfo.InvariantCulture);
                if (chunkSize == 0)
                {
                    while (!string.IsNullOrEmpty(await reader.ReadLineAsync(cancellationToken)))
                    {
                    }

                    break;
                }

                chunkedBody.Append(await ReadCharactersAsync(reader, chunkSize, cancellationToken));
                await reader.ReadLineAsync(cancellationToken);
            }

            return chunkedBody.ToString();
        }

        if (!headers.TryGetValue("Content-Length", out var contentLengthText) ||
            !int.TryParse(contentLengthText, NumberStyles.None, CultureInfo.InvariantCulture, out var contentLength) ||
            contentLength == 0)
        {
            return string.Empty;
        }

        // Test payloads are ASCII, therefore HTTP byte length equals the character count.
        return await ReadCharactersAsync(reader, contentLength, cancellationToken);
    }

    private static async Task<string> ReadCharactersAsync(
        TextReader reader,
        int count,
        CancellationToken cancellationToken)
    {
        var buffer = new char[count];
        var offset = 0;
        while (offset < count)
        {
            var read = await reader.ReadAsync(buffer.AsMemory(offset, count - offset), cancellationToken);
            if (read == 0)
            {
                break;
            }

            offset += read;
        }

        return new string(buffer, 0, offset);
    }

    private static async Task WriteResponseAsync(
        Stream stream,
        TestHttpResponse response,
        CancellationToken cancellationToken)
    {
        var bodyBytes = Encoding.UTF8.GetBytes(response.Body);
        var reasonPhrase = response.StatusCode switch
        {
            200 => "OK",
            201 => "Created",
            204 => "No Content",
            400 => "Bad Request",
            401 => "Unauthorized",
            404 => "Not Found",
            500 => "Internal Server Error",
            _ => "Status"
        };

        var responseHeaders = new StringBuilder()
            .Append("HTTP/1.1 ")
            .Append(response.StatusCode)
            .Append(' ')
            .Append(reasonPhrase)
            .Append("\r\nContent-Length: ")
            .Append(bodyBytes.Length)
            .Append("\r\nContent-Type: ")
            .Append(response.ContentType)
            .Append("\r\nConnection: close\r\n");

        if (response.Headers is not null)
        {
            foreach (var header in response.Headers)
            {
                responseHeaders.Append(header.Key).Append(": ").Append(header.Value).Append("\r\n");
            }
        }

        responseHeaders.Append("\r\n");
        var headerBytes = Encoding.ASCII.GetBytes(responseHeaders.ToString());
        await stream.WriteAsync(headerBytes, cancellationToken);
        await stream.WriteAsync(bodyBytes, cancellationToken);
        await stream.FlushAsync(cancellationToken);
    }
}
