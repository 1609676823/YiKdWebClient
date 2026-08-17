using System.Net;
using System.Net.Http.Headers;
using System.Text;
using YiKdWebClient.CommonService;
using YiKdWebClient.ComWebHelper;
using YiKdWebClient.Tests.TestInfrastructure;

namespace YiKdWebClient.Tests;

public class WebHelperServicesTests
{
    [Fact]
    public async Task SendHttpRequestAsync_posts_json_headers_and_cookies()
    {
        using var server = new LoopbackHttpServer(_ => new TestHttpResponse(
            Body: "response-body",
            ContentType: "text/plain; charset=utf-8",
            Headers: new Dictionary<string, string>
            {
                ["Set-Cookie"] = "response-cookie=yes; Path=/",
                ["X-Response"] = "seen"
            }));
        var cookies = new CookieContainer();
        cookies.Add(new Uri(server.RootUrl), new Cookie("request-cookie", "yes"));
        var helper = new WebHelperServices
        {
            cookies = cookies,
            RequestHeaders = new Dictionary<string, string> { ["X-Test"] = "value" },
            Timeout = TimeSpan.FromSeconds(5)
        };

        var response = await helper.SendHttpRequestAsync(server.RootUrl + "api", "{\"value\":1}");

        Assert.Equal("response-body", response);
        var request = server.SingleRequest();
        Assert.Equal("POST", request.Method);
        Assert.Equal("/api", request.PathAndQuery);
        Assert.Equal("{\"value\":1}", request.Body);
        Assert.Contains("application/json", request.Headers["Content-Type"]);
        Assert.Contains("value", request.Headers["X-Test"]);
        Assert.Contains("request-cookie=yes", request.Headers["Cookie"]);
        Assert.Equal(2, helper.cookies.Count);
        Assert.NotNull(helper.ResponseHeaders);
        Assert.Contains("seen", helper.ResponseHeaders!.GetValues("X-Response"));
    }

    [Fact]
    public async Task SendHttpRequestAsync_honors_configured_http_method()
    {
        using var server = new LoopbackHttpServer();
        var helper = new WebHelperServices
        {
            HttpMethod = HttpMethod.Put,
            Timeout = TimeSpan.FromSeconds(5)
        };

        await helper.SendHttpRequestAsync(server.RootUrl + "resource", "payload");

        var request = server.SingleRequest();
        Assert.Equal("PUT", request.Method);
        Assert.Equal("payload", request.Body);
    }

    [Fact]
    public async Task SendHttpRequestAsync_throws_for_non_success_status()
    {
        using var server = new LoopbackHttpServer(_ => new TestHttpResponse(500, "failure"));
        var helper = new WebHelperServices { Timeout = TimeSpan.FromSeconds(5) };

        var exception = await Assert.ThrowsAsync<HttpRequestException>(() =>
            helper.SendHttpRequestAsync(server.RootUrl + "failure"));

        Assert.Equal(HttpStatusCode.InternalServerError, exception.StatusCode);
    }
}

public class WebHelperTests
{
    [Fact]
    public void CreateQueryString_url_encodes_keys_and_values()
    {
        var helper = new WebHelper();

        var query = helper.CreateQueryString(new Dictionary<string, string>
        {
            ["search term"] = "A+B & C",
            ["page"] = "2"
        });

        Assert.Equal("search+term=A%2BB+%26+C&page=2", query);
    }

    [Fact]
    public async Task SendHttpRequestAsync_sends_query_headers_and_cookies()
    {
        using var server = new LoopbackHttpServer(_ => new TestHttpResponse(
            Headers: new Dictionary<string, string>
            {
                ["Set-Cookie"] = "response-cookie=yes; Path=/",
                ["X-Response"] = "seen"
            }));
        var requestCookies = new CookieContainer();
        requestCookies.Add(new Uri(server.RootUrl), new Cookie("request-cookie", "yes"));
        var helper = new WebHelper
        {
            HttpMethod = HttpMethod.Get,
            queryParameters = new Dictionary<string, string>
            {
                ["search"] = "A B",
                ["page"] = "2"
            },
            RequestHeaders = new Dictionary<string, string> { ["X-Test"] = "query" },
            Requestcookies = requestCookies,
            Timeout = TimeSpan.FromSeconds(5)
        };

        var response = await helper.SendHttpRequestAsync(server.RootUrl + "items");

        Assert.Equal("{\"ok\":true}", response);
        var request = server.SingleRequest();
        Assert.Equal("GET", request.Method);
        Assert.Equal("/items?search=A+B&page=2", request.PathAndQuery);
        Assert.Contains("query", request.Headers["X-Test"]);
        Assert.Contains("request-cookie=yes", request.Headers["Cookie"]);
        Assert.Equal(2, helper.Responsecookies.Count);
        Assert.NotNull(helper.ResponseHeaders);
        Assert.Contains("seen", helper.ResponseHeaders!.GetValues("X-Response"));
    }

    [Fact]
    public async Task SendHttpRequestAsync_sends_raw_body_and_media_type()
    {
        using var server = new LoopbackHttpServer();
        var helper = new WebHelper
        {
            HttpMethod = HttpMethod.Post,
            bodyType = BodyType.raw,
            Body_Raw = "raw-body",
            RequestmediaType = "text/plain",
            Timeout = TimeSpan.FromSeconds(5)
        };

        await helper.SendHttpRequestAsync(server.RootUrl + "raw");

        var request = server.SingleRequest();
        Assert.Equal("raw-body", request.Body);
        Assert.Contains("text/plain", request.Headers["Content-Type"]);
    }

    [Fact]
    public async Task SendHttpRequestAsync_sends_urlencoded_body()
    {
        using var server = new LoopbackHttpServer();
        using var form = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            ["name"] = "A B",
            ["token"] = "x+y"
        });
        var helper = new WebHelper
        {
            HttpMethod = HttpMethod.Post,
            bodyType = BodyType.urlencoded,
            Body_UrlEncoded = form,
            Timeout = TimeSpan.FromSeconds(5)
        };

        await helper.SendHttpRequestAsync(server.RootUrl + "form");

        var request = server.SingleRequest();
        Assert.Equal("name=A+B&token=x%2By", request.Body);
        Assert.Contains("application/x-www-form-urlencoded", request.Headers["Content-Type"]);
    }

    [Fact]
    public async Task SendHttpRequestAsync_sends_multipart_form_data()
    {
        using var server = new LoopbackHttpServer();
        using var multipart = new MultipartFormDataContent("test-boundary");
        multipart.Add(new StringContent("field-value", Encoding.UTF8), "field-name");
        var helper = new WebHelper
        {
            HttpMethod = HttpMethod.Post,
            bodyType = BodyType.formdata,
            Body_FormData = multipart,
            Timeout = TimeSpan.FromSeconds(5)
        };

        await helper.SendHttpRequestAsync(server.RootUrl + "multipart");

        var request = server.SingleRequest();
        Assert.Contains("multipart/form-data", request.Headers["Content-Type"]);
        Assert.Contains("test-boundary", request.Headers["Content-Type"]);
        Assert.Contains("field-name", request.Body);
        Assert.Contains("field-value", request.Body);
    }

    [Fact]
    public async Task SendHttpRequestAsync_throws_for_non_success_status()
    {
        using var server = new LoopbackHttpServer(_ => new TestHttpResponse(404, "missing"));
        var helper = new WebHelper
        {
            HttpMethod = HttpMethod.Get,
            Timeout = TimeSpan.FromSeconds(5)
        };

        var exception = await Assert.ThrowsAsync<HttpRequestException>(() =>
            helper.SendHttpRequestAsync(server.RootUrl + "missing"));

        Assert.Equal(HttpStatusCode.NotFound, exception.StatusCode);
    }
}
