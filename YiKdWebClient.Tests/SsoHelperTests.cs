using System.Net;
using System.Text;
using System.Text.Json;
using YiKdWebClient.SSO;
using YiKdWebClient.Tests.TestInfrastructure;

namespace YiKdWebClient.Tests;

public class SsoHelperTests
{
    private const string OverrideUrl = "https://override.test/k3cloud/";

    [Fact]
    public void GetSsoUrlsV4_builds_sha256_signed_urls_and_honors_url_override()
    {
        var helper = CreateHelper();
        helper.permitcount = "1";

        var urls = helper.GetSsoUrlsV4("override-user", OverrideUrl.TrimEnd('/'));

        var args = ReadJsonLoginArguments(helper, urls);
        Assert.Equal("override-user", args.GetProperty("username").GetString());
        Assert.Equal("|{'permitcount':'1'}", args.GetProperty("otherargs").GetString());
        Assert.Equal(
            CalculateSha256Signature(helper, "override-user", args.GetProperty("timestamp").GetString()!, "1"),
            args.GetProperty("signeddata").GetString());
        Assert.StartsWith(OverrideUrl, urls.html5Url);
        Assert.StartsWith(OverrideUrl, urls.silverlightUrl);
        Assert.Same(urls, helper.SSOLoginUrlObject);
    }

    [Fact]
    public void GetSsoUrlsV3_builds_sha1_signed_urls()
    {
        var helper = CreateHelper();

        var urls = helper.GetSsoUrlsV3("v3-user", OverrideUrl);

        var args = ReadJsonLoginArguments(helper, urls);
        Assert.Equal(
            CalculateSha1Signature(helper, "v3-user", args.GetProperty("timestamp").GetString()!),
            args.GetProperty("signeddata").GetString());
        Assert.StartsWith(OverrideUrl, urls.html5Url);
    }

    [Fact]
    public void GetSsoUrlsV2_builds_sha1_signed_urls()
    {
        var helper = CreateHelper();

        var urls = helper.GetSsoUrlsV2("v2-user", OverrideUrl);

        var args = ReadJsonLoginArguments(helper, urls);
        Assert.Equal(
            CalculateSha1Signature(helper, "v2-user", args.GetProperty("timestamp").GetString()!),
            args.GetProperty("signeddata").GetString());
        Assert.StartsWith(OverrideUrl, urls.html5Url);
    }

    [Fact]
    public void GetSsoUrlsV1_builds_pipe_delimited_sha1_payload()
    {
        var helper = CreateHelper();

        var urls = helper.GetSsoUrlsV1("v1-user", OverrideUrl);

        var encoded = GetUd(urls.html5Url);
        var payload = Encoding.Default.GetString(Convert.FromBase64String(encoded));
        var parts = payload.Split('|');
        Assert.Equal(7, parts.Length);
        Assert.Equal(string.Empty, parts[0]);
        Assert.Equal(helper.appSettingsModel.XKDApiAcctID, parts[1]);
        Assert.Equal("v1-user", parts[2]);
        Assert.Equal(helper.appSettingsModel.XKDApiAppID, parts[3]);
        Assert.Equal(
            CalculateSha1Signature(helper, "v1-user", parts[5]),
            parts[4]);
        Assert.Equal(helper.appSettingsModel.XKDApiLCID, parts[6]);
        Assert.Equal(encoded, helper.argJsonBase64);
        Assert.Contains(payload, helper.argJosn);
        Assert.StartsWith(OverrideUrl, urls.html5Url);
    }

    [Fact]
    public void GetSSOLogoutap0StrV4_signs_its_timestamp_and_honors_url_override()
    {
        var helper = CreateHelper();

        var logout = helper.GetSSOLogoutap0StrV4("logout-user", OverrideUrl);

        AssertLogoutPayload(helper, logout, "logout-user", useSha256: true);
        Assert.StartsWith(OverrideUrl, logout.RequestLogoutUrl);
        Assert.Same(logout, helper.SSOLogoutObject);
    }

    [Fact]
    public void GetSSOLogoutap0StrV3_signs_its_timestamp_and_honors_url_override()
    {
        var helper = CreateHelper();

        var logout = helper.GetSSOLogoutap0StrV3("logout-user", OverrideUrl);

        AssertLogoutPayload(helper, logout, "logout-user", useSha256: false);
        Assert.StartsWith(OverrideUrl, logout.RequestLogoutUrl);
    }

    [Fact]
    public void GetSSOLogoutap0StrV2V1_signs_its_timestamp_and_honors_url_override()
    {
        var helper = CreateHelper();

        var logout = helper.GetSSOLogoutap0StrV2V1("logout-user", OverrideUrl);

        AssertLogoutPayload(helper, logout, "logout-user", useSha256: false);
        Assert.StartsWith(OverrideUrl, logout.RequestLogoutUrl);
    }

    [Fact]
    public void SSOExcuteLogout_posts_ap0_as_urlencoded_form()
    {
        using var server = new LoopbackHttpServer(_ => new TestHttpResponse(Body: "logout-ok"));
        var helper = CreateHelper();
        const string ap0 = "{\"AcctID\":\"db\",\"Username\":\"user\"}";
        var logout = new SSOLogoutObject
        {
            RequestLogoutUrl = server.RootUrl + "logout",
            ap0 = ap0
        };

        var response = helper.SSOExcuteLogout(logout);

        Assert.Equal("logout-ok", response);
        var request = server.SingleRequest();
        Assert.Equal("POST", request.Method);
        Assert.Equal("/logout", request.PathAndQuery);
        Assert.Contains("application/x-www-form-urlencoded", request.Headers["Content-Type"]);
        Assert.StartsWith("ap0=", request.Body);
        Assert.Equal(ap0, WebUtility.UrlDecode(request.Body[4..]));
    }

    private static SSOHelper CreateHelper()
    {
        var helper = new SSOHelper
        {
            appSettingsModel = TestClientFactory.CreateSettings("https://configured.test/k3cloud/"),
            Url = "https://configured.test/k3cloud/"
        };
        return helper;
    }

    private static JsonElement ReadJsonLoginArguments(SSOHelper helper, SSOLoginUrlObject urls)
    {
        var encoded = GetUd(urls.html5Url);
        var json = Encoding.UTF8.GetString(Convert.FromBase64String(encoded));
        Assert.Equal(helper.argJsonBase64, encoded);
        Assert.Equal(helper.argJosn, json);

        using var document = JsonDocument.Parse(json);
        return document.RootElement.Clone();
    }

    private static string GetUd(string url)
    {
        return url[(url.IndexOf("?ud=", StringComparison.Ordinal) + 4)..];
    }

    private static string CalculateSha256Signature(
        SSOHelper helper,
        string username,
        string timestamp,
        string? permitCount = null)
    {
        var values = new List<string>
        {
            helper.appSettingsModel.XKDApiAcctID,
            username,
            helper.appSettingsModel.XKDApiAppID,
            helper.appSettingsModel.XKDApiAppSec,
            timestamp
        };
        if (permitCount is not null)
        {
            values.Add(permitCount);
        }

        values.Sort(StringComparer.Ordinal);
        return CommonFunctionHelper.Sha256Hex(string.Concat(values));
    }

    private static string CalculateSha1Signature(SSOHelper helper, string username, string timestamp)
    {
        return CommonFunctionHelper.GetSignatureSHA1Util(new[]
        {
            helper.appSettingsModel.XKDApiAcctID,
            username,
            helper.appSettingsModel.XKDApiAppID,
            helper.appSettingsModel.XKDApiAppSec,
            timestamp
        });
    }

    private static void AssertLogoutPayload(
        SSOHelper helper,
        SSOLogoutObject logout,
        string username,
        bool useSha256)
    {
        using var document = JsonDocument.Parse(logout.ap0);
        var root = document.RootElement;
        Assert.Equal(helper.appSettingsModel.XKDApiAcctID, root.GetProperty("AcctID").GetString());
        Assert.Equal(helper.appSettingsModel.XKDApiAppID, root.GetProperty("AppId").GetString());
        Assert.Equal(username, root.GetProperty("Username").GetString());
        var timestamp = root.GetProperty("Timestamp").GetInt64().ToString();
        var expectedSignature = useSha256
            ? CalculateSha256Signature(helper, username, timestamp)
            : CalculateSha1Signature(helper, username, timestamp);
        Assert.Equal(expectedSignature, root.GetProperty("SignedData").GetString());
    }
}
