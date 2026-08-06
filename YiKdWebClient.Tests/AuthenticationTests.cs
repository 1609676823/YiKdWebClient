using System.Text;
using System.Text.Json;
using YiKdWebClient.AuthService;
using YiKdWebClient.Model;
using YiKdWebClient.Tests.TestInfrastructure;

namespace YiKdWebClient.Tests;

public class AuthenticationTests
{
    private const string LoginResponse = "{\"LoginResultType\":1}";
    private const string RequestJson = "{\"parameters\":\"[]\"}";

    [Fact]
    public void LoginByAppSecret_GetLoginJson_builds_expected_parameters()
    {
        var settings = TestClientFactory.CreateSettings();

        var json = new LoginByAppSecret().GetLoginJson(settings, true);

        Assert.Equal(
            new[]
            {
                settings.XKDApiAcctID,
                settings.XKDApiUserName,
                settings.XKDApiAppID,
                settings.XKDApiAppSec,
                settings.XKDApiLCID
            },
            ReadWrappedParameters(json));
        AssertStandardLoginEnvelope(json);
    }

    [Fact]
    public void LoginByAppSecret_Login_posts_to_expected_endpoint()
    {
        AssertLoginRequest(
            (url, json) => new LoginByAppSecret
            {
                RequestHeaders = new Dictionary<string, string> { ["X-Test"] = "app-secret" },
                Timeout = TimeSpan.FromSeconds(5)
            }.Login(url, json),
            "Kingdee.BOS.WebApi.ServicesStub.AuthService.LoginByAppSecret.common.kdsvc",
            "app-secret");
    }

    [Fact]
    public void LoginBySign_GetLoginJson_builds_valid_sha256_signature()
    {
        var settings = TestClientFactory.CreateSettings();
        var service = new LoginBySign { LoginType = LoginType.LoginBySignSHA256 };

        var json = service.GetLoginJson(settings, true);

        var parameters = ReadParameterOnlyEnvelope(json);
        Assert.Equal(settings.XKDApiAcctID, parameters[0]);
        Assert.Equal(settings.XKDApiUserName, parameters[1]);
        Assert.Equal(settings.XKDApiAppID, parameters[2]);
        Assert.Equal(settings.XKDApiLCID, parameters[5]);
        Assert.True(long.TryParse(parameters[3], out var timestamp));
        Assert.InRange(timestamp, CommonFunctionHelper.GetTimestamp() - 5, CommonFunctionHelper.GetTimestamp() + 1);

        var signedValues = new[]
        {
            settings.XKDApiAcctID,
            settings.XKDApiUserName,
            settings.XKDApiAppID,
            settings.XKDApiAppSec,
            parameters[3]
        };
        Assert.Equal(CommonFunctionHelper.GetSHA256(signedValues), parameters[4]);
    }

    [Fact]
    public void LoginBySign_GetLoginJson_builds_valid_sha1_signature()
    {
        var settings = TestClientFactory.CreateSettings();
        var service = new LoginBySign { LoginType = LoginType.LoginBySignSHA1 };

        var json = service.GetLoginJson(settings, false);

        var parameters = ReadParameterOnlyEnvelope(json);
        var signedValues = new[]
        {
            settings.XKDApiAcctID,
            settings.XKDApiUserName,
            settings.XKDApiAppID,
            settings.XKDApiAppSec,
            parameters[3]
        };
        Assert.Equal(CommonFunctionHelper.GetSHA1(signedValues), parameters[4]);
    }

    [Fact]
    public void LoginBySign_Login_posts_to_expected_endpoint()
    {
        AssertLoginRequest(
            (url, json) => new LoginBySign
            {
                RequestHeaders = new Dictionary<string, string> { ["X-Test"] = "sign" },
                Timeout = TimeSpan.FromSeconds(5)
            }.Login(url, json),
            "Kingdee.BOS.WebApi.ServicesStub.AuthService.LoginBySign.common.kdsvc",
            "sign");
    }

    [Fact]
    public void ValidateLogin_GetLoginJson_builds_expected_parameters()
    {
        var settings = CreateValidateSettings();

        var json = new ValidateLogin().GetLoginJson(settings, true);

        Assert.Equal(
            new[] { settings.DbId, settings.UserName, settings.Password, settings.lcid.ToString() },
            ReadWrappedParameters(json));
    }

    [Fact]
    public void ValidateLogin_Login_posts_to_expected_endpoint()
    {
        AssertLoginRequest(
            (url, json) => new ValidateLogin
            {
                RequestHeaders = new Dictionary<string, string> { ["X-Test"] = "validate" },
                Timeout = TimeSpan.FromSeconds(5)
            }.Login(url, json),
            "Kingdee.BOS.WebApi.ServicesStub.AuthService.ValidateUser.common.kdsvc",
            "validate");
    }

    [Fact]
    public void ValidateUserEnDeCode_GetLoginJson_encrypts_user_and_password()
    {
        var settings = CreateValidateSettings();

        var json = new ValidateUserEnDeCode().GetLoginJson(settings, true);

        Assert.Equal(
            new[]
            {
                settings.DbId,
                EnDecode.Encode(settings.UserName),
                EnDecode.Encode(settings.Password),
                settings.lcid.ToString()
            },
            ReadWrappedParameters(json));
    }

    [Fact]
    public void ValidateUserEnDeCode_Login_posts_to_expected_endpoint()
    {
        AssertLoginRequest(
            (url, json) => new ValidateUserEnDeCode
            {
                RequestHeaders = new Dictionary<string, string> { ["X-Test"] = "encoded" },
                Timeout = TimeSpan.FromSeconds(5)
            }.Login(url, json),
            "Kingdee.BOS.WebApi.ServicesStub.AuthService.ValidateUserEnDeCode.common.kdsvc",
            "encoded");
    }

    [Fact]
    public void LoginBySimplePassport_GetCnfBytes_reads_exact_file_bytes()
    {
        var filePath = CreateTemporaryFile(new byte[] { 0, 1, 2, 250, 255 });
        try
        {
            var bytes = new LoginBySimplePassport().GetCnfBytes(filePath);

            Assert.Equal(new byte[] { 0, 1, 2, 250, 255 }, bytes);
        }
        finally
        {
            File.Delete(filePath);
        }
    }

    [Fact]
    public void LoginBySimplePassport_GetPassportForBase64_encodes_file()
    {
        var filePath = CreateTemporaryFile(Encoding.UTF8.GetBytes("passport"));
        try
        {
            var base64 = new LoginBySimplePassport().GetPassportForBase64(filePath);

            Assert.Equal(Convert.ToBase64String(Encoding.UTF8.GetBytes("passport")), base64);
        }
        finally
        {
            File.Delete(filePath);
        }
    }

    [Fact]
    public void LoginBySimplePassport_GetLoginJson_reads_cnf_file()
    {
        var filePath = CreateTemporaryFile(new byte[] { 1, 2, 3, 4 });
        try
        {
            var model = new LoginBySimplePassportModel
            {
                bySimplePassportType = BySimplePassportType.CnfFile,
                CnfFilePath = filePath,
                Lcid = 1033
            };

            var json = new LoginBySimplePassport().GetLoginJson(model, true);

            Assert.Equal(
                new[] { Convert.ToBase64String(new byte[] { 1, 2, 3, 4 }), "1033" },
                ReadWrappedParameters(json));
            Assert.Equal(Convert.ToBase64String(new byte[] { 1, 2, 3, 4 }), model.SimplePassportForBase64);
        }
        finally
        {
            File.Delete(filePath);
        }
    }

    [Fact]
    public void LoginBySimplePassport_GetLoginJson_accepts_preencoded_passport()
    {
        var model = new LoginBySimplePassportModel
        {
            bySimplePassportType = BySimplePassportType.ForBase64,
            SimplePassportForBase64 = "AQIDBA==",
            Lcid = 2052
        };

        var json = new LoginBySimplePassport().GetLoginJson(model, false);

        Assert.Equal(new[] { "AQIDBA==", "2052" }, ReadWrappedParameters(json));
    }

    [Fact]
    public void LoginBySimplePassport_GetLoginJson_rejects_missing_cnf_path()
    {
        var model = new LoginBySimplePassportModel
        {
            bySimplePassportType = BySimplePassportType.CnfFile
        };

        var exception = Assert.Throws<Exception>(() =>
            new LoginBySimplePassport().GetLoginJson(model, true));

        Assert.Contains("CnfFilePath", exception.Message);
    }

    [Fact]
    public void LoginBySimplePassport_GetLoginJson_rejects_missing_base64_value()
    {
        var model = new LoginBySimplePassportModel
        {
            bySimplePassportType = BySimplePassportType.ForBase64
        };

        var exception = Assert.Throws<Exception>(() =>
            new LoginBySimplePassport().GetLoginJson(model, true));

        Assert.Contains("SimplePassportForBase64", exception.Message);
    }

    [Fact]
    public void LoginBySimplePassport_Login_posts_to_expected_endpoint()
    {
        AssertLoginRequest(
            (url, json) => new LoginBySimplePassport
            {
                RequestHeaders = new Dictionary<string, string> { ["X-Test"] = "passport" },
                Timeout = TimeSpan.FromSeconds(5)
            }.Login(url, json),
            "Kingdee.BOS.WebApi.ServicesStub.AuthService.LoginBySimplePassport.common.kdsvc",
            "passport");
    }

    [Fact]
    public void LoginByApiSignHeaders_builds_kd_headers()
    {
        var settings = TestClientFactory.CreateSettings();

        var headers = LoginByApiSignHeaders.GetApiHeaders(
            settings,
            new Uri("https://example.test/k3cloud/service?x=1"));

        Assert.Equal(settings.XKDApiAppID, headers["X-Kd-Appkey"]);
        Assert.Equal(
            $"{settings.XKDApiAcctID},{settings.XKDApiUserName},{settings.XKDApiLCID},{settings.XKDApiOrgNum}",
            Encoding.UTF8.GetString(Convert.FromBase64String(headers["X-Kd-Appdata"])));

        var appData = $"{settings.XKDApiAcctID},{settings.XKDApiUserName},{settings.XKDApiLCID},{settings.XKDApiOrgNum}";
        Assert.Equal(
            EnDecode.HmacSHA256(settings.XKDApiAppID + appData, settings.XKDApiAppSec, Encoding.UTF8, true),
            headers["X-Kd-Signature"]);
        Assert.False(headers.ContainsKey("X-Api-ClientID"));
    }

    [Fact]
    public void LoginByApiSignHeaders_builds_api_v2_signature_when_client_id_is_embedded()
    {
        var settings = TestClientFactory.CreateSettings();
        settings.XKDApiAppID = "client_frperg"; // ROT13("secret") is the encoded API secret.
        var uri = new Uri("https://example.test/k3cloud/service?a=hello world");

        var headers = LoginByApiSignHeaders.GetApiHeaders(settings, uri);

        Assert.Equal("client", headers["X-Api-ClientID"]);
        Assert.Equal("2.0", headers["X-Api-Auth-Version"]);
        Assert.Equal(32, headers["x-api-nonce"].Length);
        Assert.True(long.TryParse(headers["x-api-timestamp"], out _));

        var message = string.Format(
            "POST\n{0}\n\nx-api-nonce:{1}\nx-api-timestamp:{2}\n",
            EnDecode.UrlEncodeWithUpperCode(uri.PathAndQuery, Encoding.ASCII),
            headers["x-api-nonce"],
            headers["x-api-timestamp"]);
        Assert.Equal(
            EnDecode.HmacSHA256(message, "secret", Encoding.ASCII, true),
            headers["X-Api-Signature"]);
    }

    [Fact]
    public void LoginByApiSignHeaders_default_settings_overload_returns_base_headers()
    {
        var headers = LoginByApiSignHeaders.GetApiHeaders(new Uri("https://example.test/service"));

        Assert.Equal(new AppSettingsModel().XKDApiAppID, headers["X-Kd-Appkey"]);
        Assert.True(headers.ContainsKey("X-Kd-Appdata"));
        Assert.True(headers.ContainsKey("X-Kd-Signature"));
    }

    [Fact]
    public void LoginByApiSignHeaders_formats_headers_as_lines()
    {
        var formatted = LoginByApiSignHeaders.GetApiHeadersStr(
            new Dictionary<string, string>
            {
                ["A"] = "one",
                ["B"] = "two"
            });

        Assert.Contains("A:one", formatted);
        Assert.Contains("B:two", formatted);
        Assert.EndsWith(Environment.NewLine, formatted);
    }

    private static void AssertLoginRequest(
        Func<string, string, RequestWebModel> login,
        string expectedServicePath,
        string expectedHeaderValue)
    {
        using var server = new LoopbackHttpServer(_ => new TestHttpResponse(
            Body: LoginResponse,
            Headers: new Dictionary<string, string> { ["Set-Cookie"] = "session=abc; Path=/" }));

        var result = login(server.K3CloudUrl.TrimEnd('/'), RequestJson);

        Assert.Equal(server.K3CloudUrl + expectedServicePath, result.RequestUrl);
        Assert.Equal(RequestJson, result.RealRequestBody);
        Assert.Equal(LoginResponse, result.RealResponseBody);
        Assert.Equal(1, result.Cookie.Count);

        var request = server.SingleRequest();
        Assert.Equal("POST", request.Method);
        Assert.Equal("/k3cloud/" + expectedServicePath, request.PathAndQuery);
        Assert.Equal(RequestJson, request.Body);
        Assert.Contains(expectedHeaderValue, request.Headers["X-Test"]);
    }

    private static ValidateLoginSettingsModel CreateValidateSettings()
    {
        return new ValidateLoginSettingsModel
        {
            DbId = "db-id",
            UserName = "user",
            Password = "password",
            lcid = 2052
        };
    }

    private static void AssertStandardLoginEnvelope(string json)
    {
        using var document = JsonDocument.Parse(json);
        var root = document.RootElement;
        Assert.Equal(1, root.GetProperty("format").GetInt32());
        Assert.Equal("ApiClient", root.GetProperty("useragent").GetString());
        Assert.Equal("1.0", root.GetProperty("v").GetString());
        Assert.True(root.TryGetProperty("rid", out _));
        Assert.True(root.TryGetProperty("timestamp", out _));
    }

    private static string[] ReadWrappedParameters(string json)
    {
        AssertStandardLoginEnvelope(json);
        using var document = JsonDocument.Parse(json);
        var serialized = document.RootElement.GetProperty("parameters").GetString();
        Assert.NotNull(serialized);
        return JsonSerializer.Deserialize<JsonElement[]>(serialized!)!
            .Select(value => value.ToString())
            .ToArray();
    }

    private static string[] ReadParameterOnlyEnvelope(string json)
    {
        using var document = JsonDocument.Parse(json);
        Assert.Single(document.RootElement.EnumerateObject());
        var serialized = document.RootElement.GetProperty("parameters").GetString();
        Assert.NotNull(serialized);
        return JsonSerializer.Deserialize<JsonElement[]>(serialized!)!
            .Select(value => value.ToString())
            .ToArray();
    }

    private static string CreateTemporaryFile(byte[] contents)
    {
        var path = Path.Combine(Path.GetTempPath(), $"YiKdWebClient-{Guid.NewGuid():N}.tmp");
        File.WriteAllBytes(path, contents);
        return path;
    }
}
