using System.Text.Json;
using YiKdWebClient;
using YiKdWebClient.Model;

namespace ConsoleTestNet80;

internal static class ConsoleReport
{
    private const int SeparatorWidth = 108;

    public static void PrintExampleHeader(ExampleDefinition example)
    {
        WriteRule('=');
        Console.WriteLine($"YiKdWebClient 实际报文示例：{example.Title}");
        Console.WriteLine($"命令：{example.Command}");
        Console.WriteLine($"说明：{example.Description}");
        Console.WriteLine($"运行时间：{DateTimeOffset.Now:yyyy-MM-dd HH:mm:ss zzz}");
        WriteRule('=');
        Console.WriteLine(IsScreenshotMode()
            ? "截图模式：字段和值来自本次真实调用；仅为控制图片高度折叠长字段和部分中间行。直接运行命令可查看完整报文。"
            : "提示：JSON 为便于阅读进行了缩进，字段和值来自本次真实调用，没有替换为演示报文。");
    }

    public static void PrintClientExchange(YiK3CloudClient client, string methodResult)
    {
        if (HasExchange(client.ReturnLoginWebModel))
        {
            PrintExchange("登录请求 / 响应", client.ReturnLoginWebModel);
        }
        else
        {
            WriteSection("登录请求 / 响应");
            Console.WriteLine("本示例没有发送登录请求。API 请求头签名模式会直接调用业务接口。\n");
        }

        if (client.RequestHeaders.Count > 0 || !string.IsNullOrWhiteSpace(client.RequestHeadersString))
        {
            WriteSection("实际请求头");
            if (!string.IsNullOrWhiteSpace(client.RequestHeadersString))
            {
                Console.WriteLine(RedactSecrets(client.RequestHeadersString.Trim()));
            }
            else
            {
                foreach (KeyValuePair<string, string> header in client.RequestHeaders)
                {
                    Console.WriteLine($"{header.Key}: {RedactSecrets(header.Value)}");
                }
            }

            Console.WriteLine();
        }

        PrintExchange("业务操作请求 / 响应", client.ReturnOperationWebModel);

        WriteSection("方法返回值");
        if (PayloadEquals(methodResult, client.ReturnOperationWebModel.RealResponseBody))
        {
            Console.WriteLine("与上面的“业务操作实际返回报文”相同。\n");
            return;
        }

        if (PayloadEquals(methodResult, client.ReturnLoginWebModel.RealResponseBody))
        {
            Console.WriteLine("业务请求未发送；方法返回值与上面的“登录实际返回报文”相同。\n");
            return;
        }

        WritePayload(methodResult);
        Console.WriteLine();
    }

    public static void PrintExchange(string title, RequestWebModel exchange)
    {
        WriteSection(title);
        Console.WriteLine("请求地址：");
        Console.WriteLine(ValueOrPlaceholder(exchange.RequestUrl));
        Console.WriteLine();
        Console.WriteLine("实际请求报文：");
        WritePayload(exchange.RealRequestBody);
        Console.WriteLine();
        Console.WriteLine("实际返回报文：");
        WritePayload(exchange.RealResponseBody);
        Console.WriteLine();
    }

    public static void PrintSsoResult(YiKdWebClient.SSO.SSOHelper helper)
    {
        WriteSection("SSO V4 请求参数");
        Console.WriteLine($"数据中心 ID：{helper.simplePassportLoginArg.dbid}");
        Console.WriteLine($"应用 ID：{helper.simplePassportLoginArg.appid}");
        Console.WriteLine($"用户名称：{helper.simplePassportLoginArg.username}");
        Console.WriteLine($"时间戳：{helper.timestamp}");
        Console.WriteLine($"签名：{helper.simplePassportLoginArg.signeddata}");
        Console.WriteLine();
        Console.WriteLine("请求参数（JSON）：");
        WritePayload(helper.argJosn);
        Console.WriteLine();
        Console.WriteLine("请求参数（Base64）：");
        Console.WriteLine(ValueOrPlaceholder(helper.argJsonBase64));
        Console.WriteLine();

        WriteSection("生成的单点登录入口");
        Console.WriteLine("Silverlight：");
        Console.WriteLine(helper.SSOLoginUrlObject.silverlightUrl);
        Console.WriteLine();
        Console.WriteLine("HTML5：");
        Console.WriteLine(helper.SSOLoginUrlObject.html5Url);
        Console.WriteLine();
        Console.WriteLine("WPF 客户端：");
        Console.WriteLine(helper.SSOLoginUrlObject.wpfUrl);
        Console.WriteLine();
        Console.WriteLine("说明：GetSsoUrlsV4 只在本地生成带签名的入口链接，不会发送 HTTP 请求，因此没有响应报文。");
        Console.WriteLine();
    }

    public static void PrintProgress(long chunkNumber, bool isLast, YiK3CloudClient client)
    {
        WriteSection($"上传分块 {chunkNumber}（IsLast={isLast.ToString().ToLowerInvariant()}）");
        Console.WriteLine("请求地址：");
        Console.WriteLine(ValueOrPlaceholder(client.ReturnOperationWebModel.RequestUrl));
        Console.WriteLine();
        Console.WriteLine("实际请求报文：");
        WritePayload(client.ReturnOperationWebModel.RealRequestBody);
        Console.WriteLine();
        Console.WriteLine("实际返回报文：");
        WritePayload(client.ReturnOperationWebModel.RealResponseBody);
        Console.WriteLine();
    }

    public static void PrintSetting(string name, string? value)
    {
        Console.WriteLine($"{name}：{ValueOrPlaceholder(value)}");
    }

    public static void PrintSecretSetting(string name)
    {
        Console.WriteLine($"{name}：******（已脱敏）");
    }

    public static void WriteSection(string title)
    {
        Console.WriteLine();
        WriteRule('-');
        Console.WriteLine(title);
        WriteRule('-');
    }

    public static void WritePayload(string? payload)
    {
        if (string.IsNullOrWhiteSpace(payload))
        {
            Console.WriteLine("（空）");
            return;
        }

        string formatted = FormatJsonIfPossible(RedactSecrets(payload));
        Console.WriteLine(IsScreenshotMode() ? CompactForScreenshot(formatted) : formatted);
    }

    public static void PrintUnhandledException(Exception exception)
    {
        WriteSection("示例运行异常");
        Console.WriteLine($"{exception.GetType().FullName}: {RedactSecrets(exception.Message)}");
        if (exception.InnerException != null)
        {
            Console.WriteLine($"InnerException: {exception.InnerException.GetType().FullName}: {RedactSecrets(exception.InnerException.Message)}");
        }

        Console.WriteLine();
        Console.WriteLine(RedactSecrets(exception.StackTrace));
    }

    public static void PrintExampleFooter(bool processCompleted)
    {
        WriteRule('=');
        Console.WriteLine(processCompleted
            ? "示例程序执行结束。请根据返回报文中的业务状态判断接口是否成功。"
            : "示例程序因未处理异常结束，请检查上面的异常和环境配置。");
        WriteRule('=');
    }

    private static bool HasExchange(RequestWebModel exchange)
    {
        return !string.IsNullOrWhiteSpace(exchange.RequestUrl)
            || !string.IsNullOrWhiteSpace(exchange.RealRequestBody)
            || !string.IsNullOrWhiteSpace(exchange.RealResponseBody);
    }

    private static bool PayloadEquals(string? left, string? right)
    {
        if (string.IsNullOrWhiteSpace(left) || string.IsNullOrWhiteSpace(right))
        {
            return false;
        }

        return string.Equals(left!.Trim(), right!.Trim(), StringComparison.Ordinal);
    }

    private static string FormatJsonIfPossible(string payload)
    {
        string trimmed = payload.Trim();
        if (!(trimmed.StartsWith("{", StringComparison.Ordinal) || trimmed.StartsWith("[", StringComparison.Ordinal)))
        {
            return payload;
        }

        try
        {
            using JsonDocument document = JsonDocument.Parse(trimmed);
            return JsonSerializer.Serialize(document.RootElement, new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            });
        }
        catch (JsonException)
        {
            return payload;
        }
    }

    private static string CompactForScreenshot(string value)
    {
        const int maxLineLength = 240;
        const int maxLineCount = 34;
        const int leadingLineCount = 24;
        const int trailingLineCount = 8;

        string[] lines = value.Replace("\r\n", "\n").Split('\n');
        string[] compactLines = lines
            .Select(line => line.Length <= maxLineLength
                ? line
                : $"{line.Substring(0, maxLineLength)} …（长字段已折叠，运行命令可查看完整值）")
            .ToArray();

        if (compactLines.Length <= maxLineCount)
        {
            return string.Join(Environment.NewLine, compactLines);
        }

        int omittedLineCount = compactLines.Length - leadingLineCount - trailingLineCount;
        return string.Join(
            Environment.NewLine,
            compactLines.Take(leadingLineCount)
                .Concat(new[] { $"  …（中间 {omittedLineCount} 行为本次真实返回字段，运行命令可查看完整报文）…" })
                .Concat(compactLines.Skip(compactLines.Length - trailingLineCount)));
    }

    private static bool IsScreenshotMode()
    {
        return string.Equals(
            Environment.GetEnvironmentVariable("YIKD_SCREENSHOT_MODE"),
            "1",
            StringComparison.Ordinal);
    }

    private static string ValueOrPlaceholder(string? value)
    {
        return string.IsNullOrWhiteSpace(value) ? "（空）" : value!;
    }

    private static string RedactSecrets(string? value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return value ?? string.Empty;
        }

        string? validatePassword = Environment.GetEnvironmentVariable("YIKD_VALIDATE_PASSWORD");
        if (string.IsNullOrEmpty(validatePassword))
        {
            return value!;
        }

        string redacted = value!.Replace(validatePassword!, "******");

        string jsonEscapedPassword = JsonSerializer.Serialize(validatePassword).Trim('"');
        if (!string.Equals(jsonEscapedPassword, validatePassword, StringComparison.Ordinal))
        {
            redacted = redacted.Replace(jsonEscapedPassword, "******");
        }

        string urlEncodedPassword = Uri.EscapeDataString(validatePassword);
        if (!string.Equals(urlEncodedPassword, validatePassword, StringComparison.Ordinal))
        {
            redacted = redacted.Replace(urlEncodedPassword, "******");
        }

        return redacted;
    }

    private static void WriteRule(char character)
    {
        Console.WriteLine(new string(character, SeparatorWidth));
    }
}
