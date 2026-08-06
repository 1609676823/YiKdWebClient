using YiKdWebClient.Model;

namespace YiKdWebClient.Tests.TestInfrastructure;

internal static class TestClientFactory
{
    public static AppSettingsModel CreateSettings(string serverUrl = "")
    {
        return new AppSettingsModel
        {
            XKDApiAcctID = "test-account",
            XKDApiAppID = "test-app",
            XKDApiAppSec = "test-secret",
            XKDApiUserName = "test-user",
            XKDApiLCID = "2052",
            XKDApiOrgNum = "100",
            XKDApiServerUrl = serverUrl
        };
    }

    public static YiK3CloudClient CreateApiHeaderClient(string serverUrl)
    {
        return new YiK3CloudClient
        {
            LoginType = LoginType.LoginByApiSignHeaders,
            AppSettingsModel = CreateSettings(serverUrl),
            Timeout = TimeSpan.FromSeconds(5)
        };
    }
}
