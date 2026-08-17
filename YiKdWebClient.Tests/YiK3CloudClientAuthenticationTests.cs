using System.Net;
using YiKdWebClient.Model;
using YiKdWebClient.Tests.TestInfrastructure;

namespace YiKdWebClient.Tests;

public class YiK3CloudClientAuthenticationTests
{
    [Fact]
    public void Login_app_secret_mode_uses_app_secret_endpoint()
    {
        AssertClientLogin(
            LoginType.LoginByAppSecret,
            "AuthService.LoginByAppSecret.common.kdsvc");
    }

    [Fact]
    public void Login_sign_sha256_mode_uses_sign_endpoint()
    {
        AssertClientLogin(
            LoginType.LoginBySignSHA256,
            "AuthService.LoginBySign.common.kdsvc");
    }

    [Fact]
    public void Login_sign_sha1_mode_uses_sign_endpoint()
    {
        AssertClientLogin(
            LoginType.LoginBySignSHA1,
            "AuthService.LoginBySign.common.kdsvc");
    }

    [Fact]
    public void Login_validate_mode_uses_validate_user_endpoint()
    {
        AssertClientLogin(
            LoginType.ValidateLogin,
            "AuthService.ValidateUser.common.kdsvc",
            (client, serverUrl) =>
            {
                client.validateLoginSettingsModel = new ValidateLoginSettingsModel(serverUrl)
                {
                    DbId = "db",
                    UserName = "user",
                    Password = "password"
                };
            });
    }

    [Fact]
    public void Login_simple_passport_mode_uses_passport_endpoint()
    {
        AssertClientLogin(
            LoginType.LoginBySimplePassport,
            "AuthService.LoginBySimplePassport.common.kdsvc",
            (client, serverUrl) =>
            {
                client.LoginBySimplePassportModel = new LoginBySimplePassportModel(serverUrl)
                {
                    bySimplePassportType = BySimplePassportType.ForBase64,
                    SimplePassportForBase64 = "AQIDBA=="
                };
            });
    }

    [Fact]
    public void Login_encoded_validate_mode_uses_encoded_validate_endpoint()
    {
#pragma warning disable CS0618
        AssertClientLogin(
            LoginType.ValidateUserEnDeCode,
            "AuthService.ValidateUserEnDeCode.common.kdsvc",
            (client, serverUrl) =>
            {
                client.validateLoginSettingsModel = new ValidateLoginSettingsModel(serverUrl)
                {
                    DbId = "db",
                    UserName = "user",
                    Password = "password"
                };
            });
#pragma warning restore CS0618
    }

    [Fact]
    public void Logout_posts_cookie_and_headers_to_logout_endpoint()
    {
        using var server = new LoopbackHttpServer();
        var cookies = new CookieContainer();
        cookies.Add(new Uri(server.RootUrl), new Cookie("session", "abc"));
        using var client = new YiK3CloudClient
        {
            AppSettingsModel = TestClientFactory.CreateSettings(server.K3CloudUrl),
            Cookie = cookies,
            RequestHeaders = new Dictionary<string, string> { ["X-Test"] = "logout" },
            Timeout = TimeSpan.FromSeconds(5)
        };

        client.Logout();

        var request = server.SingleRequest();
        Assert.Equal("POST", request.Method);
        Assert.Equal(
            "/k3cloud/Kingdee.BOS.WebApi.ServicesStub.AuthService.Logout.common.kdsvc",
            request.PathAndQuery);
        Assert.Contains("session=abc", request.Headers["Cookie"]);
        Assert.Contains("logout", request.Headers["X-Test"]);
    }

    private static void AssertClientLogin(
        LoginType loginType,
        string expectedPathSuffix,
        Action<YiK3CloudClient, string>? configure = null)
    {
        using var server = new LoopbackHttpServer(_ => new TestHttpResponse(
            Body: "{\"LoginResultType\":1}",
            Headers: new Dictionary<string, string> { ["Set-Cookie"] = "session=abc; Path=/" }));
        using var client = new YiK3CloudClient
        {
            LoginType = loginType,
            AppSettingsModel = TestClientFactory.CreateSettings(server.K3CloudUrl),
            RequestHeaders = new Dictionary<string, string> { ["X-Test"] = "login" },
            Timeout = TimeSpan.FromSeconds(5)
        };
        configure?.Invoke(client, server.K3CloudUrl);

        var result = client.Login();

        Assert.Same(result, client.ReturnLoginWebModel);
        Assert.Equal("{\"LoginResultType\":1}", result.RealResponseBody);
        Assert.False(string.IsNullOrWhiteSpace(result.RealRequestBody));
        Assert.EndsWith(expectedPathSuffix, result.RequestUrl);
        Assert.Equal(1, client.Cookie.Count);

        var request = server.SingleRequest();
        Assert.EndsWith(expectedPathSuffix, request.PathAndQuery);
        Assert.Equal(result.RealRequestBody, request.Body);
        Assert.Contains("login", request.Headers["X-Test"]);
    }
}
