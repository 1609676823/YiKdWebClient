using YiKdWebClient.Model;
using YiKdWebClient.Tests.TestInfrastructure;

namespace YiKdWebClient.Tests;

public class YiK3CloudClientBehaviorTests
{
    [Fact]
    public void Constructor_initializes_public_state()
    {
        using var client = new YiK3CloudClient();

        Assert.NotNull(client.AppSettingsModel);
        Assert.NotNull(client.Cookie);
        Assert.NotNull(client.RequestHeaders);
        Assert.NotNull(client.ReturnLoginWebModel);
        Assert.NotNull(client.ReturnOperationWebModel);
        Assert.Equal(LoginType.LoginByAppSecret, client.LoginType);
        Assert.Equal(TimeSpan.FromSeconds(60), client.Timeout);
        Assert.True(client.UnsafeRelaxedJsonEscaping);
    }

    [Fact]
    public void GetServerUrl_normalizes_url()
    {
        using var client = new YiK3CloudClient();

        Assert.Equal("https://example.test/k3cloud/", client.GetServerUrl("https://example.test/k3cloud"));
        Assert.Equal("https://example.test/k3cloud/", client.GetServerUrl("https://example.test/k3cloud/"));
        Assert.Equal(string.Empty, client.GetServerUrl(" "));
    }

    [Fact]
    public void EnsureSuffixServicesStub_appends_suffix_only_once()
    {
        Assert.Equal("Service.Run.common.kdsvc", YiK3CloudClient.EnsureSuffixServicesStub("Service.Run"));
        Assert.Equal(
            "Service.Run.common.kdsvc",
            YiK3CloudClient.EnsureSuffixServicesStub("Service.Run.common.kdsvc"));
        Assert.Equal("Service.Run.custom", YiK3CloudClient.EnsureSuffixServicesStub("Service.Run", ".custom"));
    }

    [Fact]
    public void Dispose_is_idempotent_and_clears_login_type()
    {
        var client = new YiK3CloudClient();

        client.Dispose();
        client.Dispose();

        Assert.Null(client.LoginType);
    }

    [Fact]
    public void Login_rejects_null_login_type()
    {
        using var client = new YiK3CloudClient { LoginType = null };

        var exception = Assert.Throws<Exception>(() => client.Login());

        Assert.Contains("LoginType", exception.Message);
    }

    [Fact]
    public void Login_api_header_mode_does_not_make_login_request()
    {
        using var client = new YiK3CloudClient { LoginType = LoginType.LoginByApiSignHeaders };

        var result = client.Login();

        Assert.Equal(string.Empty, result.RequestUrl);
        Assert.Equal(string.Empty, result.RealRequestBody);
        Assert.Equal(string.Empty, result.RealResponseBody);
    }

    [Fact]
    public void Login_validate_mode_requires_settings_model()
    {
        using var client = new YiK3CloudClient
        {
            LoginType = LoginType.ValidateLogin,
            validateLoginSettingsModel = null
        };

        var exception = Assert.Throws<Exception>(() => client.Login());

        Assert.Contains("validateLoginSettingsModel", exception.Message);
    }

    [Fact]
    public void Login_validate_mode_requires_server_url()
    {
        using var client = new YiK3CloudClient
        {
            LoginType = LoginType.ValidateLogin,
            validateLoginSettingsModel = new ValidateLoginSettingsModel()
        };

        var exception = Assert.Throws<Exception>(() => client.Login());

        Assert.Contains("Url", exception.Message);
    }

    [Fact]
    public void Login_simple_passport_mode_requires_model()
    {
        using var client = new YiK3CloudClient
        {
            LoginType = LoginType.LoginBySimplePassport,
            LoginBySimplePassportModel = null
        };

        var exception = Assert.Throws<Exception>(() => client.Login());

        Assert.Contains("LoginBySimplePassportModel", exception.Message);
    }

    [Fact]
    public void Login_simple_passport_mode_requires_server_url()
    {
        using var client = new YiK3CloudClient
        {
            LoginType = LoginType.LoginBySimplePassport,
            LoginBySimplePassportModel = new LoginBySimplePassportModel
            {
                bySimplePassportType = BySimplePassportType.ForBase64,
                SimplePassportForBase64 = "AQID"
            }
        };

        var exception = Assert.Throws<Exception>(() => client.Login());

        Assert.Contains("Url", exception.Message);
    }

    [Fact]
    public void ExecApiDynamicFormService_returns_failed_login_response_without_calling_operation()
    {
        const string rejected = "{\"LoginResultType\":0,\"Message\":\"denied\"}";
        using var server = new LoopbackHttpServer(_ => new TestHttpResponse(Body: rejected));
        using var client = new YiK3CloudClient
        {
            LoginType = LoginType.LoginByAppSecret,
            AppSettingsModel = TestClientFactory.CreateSettings(server.K3CloudUrl),
            Timeout = TimeSpan.FromSeconds(5)
        };

        var response = client.View("TEST_Form", "{}", true, false);

        Assert.Equal(rejected, response);
        var request = server.SingleRequest();
        Assert.Contains("AuthService.LoginByAppSecret", request.PathAndQuery);
    }

    [Fact]
    public void ExecApiDynamicFormService_auto_login_operation_and_logout_runs_full_flow()
    {
        using var server = new LoopbackHttpServer(request =>
        {
            if (request.PathAndQuery.Contains("AuthService.LoginByAppSecret", StringComparison.Ordinal))
            {
                return new TestHttpResponse(
                    Body: "{\"LoginResultType\":1}",
                    Headers: new Dictionary<string, string> { ["Set-Cookie"] = "session=abc; Path=/" });
            }

            return new TestHttpResponse(Body: "{\"ok\":true}");
        });
        using var client = new YiK3CloudClient
        {
            LoginType = LoginType.LoginByAppSecret,
            AppSettingsModel = TestClientFactory.CreateSettings(server.K3CloudUrl),
            Timeout = TimeSpan.FromSeconds(5)
        };

        var response = client.View("TEST_Form", "{}", true, true);

        Assert.Equal("{\"ok\":true}", response);
        var requests = server.Requests.ToArray();
        Assert.Equal(3, requests.Length);
        Assert.Contains("AuthService.LoginByAppSecret", requests[0].PathAndQuery);
        Assert.Contains("DynamicFormService.View", requests[1].PathAndQuery);
        Assert.Contains("AuthService.Logout", requests[2].PathAndQuery);
        Assert.Contains("session=abc", requests[1].Headers["Cookie"]);
    }
}
