using System.Xml.Linq;
using YiKdWebClient.Model;
using YiKdWebClient.ToolsHelper;

namespace ConsoleTestNet80;

internal static class SampleEnvironment
{
    public static string ConfigPath => GetValue("YIKD_CONFIG_PATH", Path.Combine(AppContext.BaseDirectory, "YiKdWebCfg", "appsettings.xml"));

    public static string CnfFilePath => GetValue("YIKD_CNF_PATH", Path.Combine(AppContext.BaseDirectory, "YiKdWebCfg", "API测试.cnf"));

    public static string UploadFilePath => GetValue("YIKD_UPLOAD_FILE", Path.Combine(AppContext.BaseDirectory, "SampleFiles", "upload-demo.txt"));

    public static string ValidateDbId => GetValue("YIKD_VALIDATE_DBID", ReadXmlSetting("X-KDApi-AcctID"));

    public static string ValidateUserName => GetValue("YIKD_VALIDATE_USERNAME", "demo");

    public static string ValidatePassword => GetRequiredValue(
        "YIKD_VALIDATE_PASSWORD",
        "旧版用户名密码认证示例不会把密码写入源码。运行 validate-login 或 validate-user-endecode 前，请先设置 YIKD_VALIDATE_PASSWORD 环境变量。");

    public static int ValidateLcid => GetIntValue("YIKD_VALIDATE_LCID", 2052);

    public static string ServerUrl => GetValue("YIKD_SERVER_URL", ReadXmlSetting("X-KDApi-ServerUrl"));

    public static string UploadFormId => GetValue("YIKD_UPLOAD_FORM_ID", "SAL_SaleOrder");

    public static string UploadInterId => GetValue("YIKD_UPLOAD_INTER_ID", "100020");

    public static string UploadBillNo => GetValue("YIKD_UPLOAD_BILL_NO", "XSDD000019");

    public static long UploadChunkSize => GetLongValue("YIKD_UPLOAD_CHUNK_SIZE", 2L * 1024 * 1024);

    public static string CustomSql => GetValue("YIKD_CUSTOM_SQL", "SELECT TOP 10 * FROM T_BD_MATERIAL_L");

    public static AppSettingsModel CreateDynamicAppSettings()
    {
        return new AppSettingsModel
        {
            XKDApiAcctID = GetValue("YIKD_ACCT_ID", ReadXmlSetting("X-KDApi-AcctID")),
            XKDApiUserName = GetValue("YIKD_USER_NAME", ReadXmlSetting("X-KDApi-UserName")),
            XKDApiAppID = GetValue("YIKD_APP_ID", ReadXmlSetting("X-KDApi-AppID")),
            XKDApiAppSec = GetValue("YIKD_APP_SECRET", ReadXmlSetting("X-KDApi-AppSec")),
            XKDApiLCID = GetValue("YIKD_LCID", ReadXmlSetting("X-KDApi-LCID")),
            XKDApiOrgNum = GetValue("YIKD_ORG_NUM", ReadXmlSetting("X-KDApi-OrgNum")),
            XKDApiServerUrl = ServerUrl
        };
    }

    public static UploadModel CreateUploadModel()
    {
        UploadModel uploadModel = new UploadModel();
        uploadModel.data.FormId = UploadFormId;
        uploadModel.data.InterId = UploadInterId;
        uploadModel.data.BillNO = UploadBillNo;
        return uploadModel;
    }

    public static void EnsureFileExists(string path, string displayName)
    {
        if (!File.Exists(path))
        {
            throw new FileNotFoundException($"找不到{displayName}。请检查路径，或使用对应的 YIKD_* 环境变量覆盖。", path);
        }
    }

    private static string ReadXmlSetting(string key)
    {
        EnsureFileExists(ConfigPath, "配置文件");

        XDocument document = XDocument.Load(ConfigPath);
        XElement? element = document
            .Descendants("add")
            .FirstOrDefault(item => string.Equals((string?)item.Attribute("key"), key, StringComparison.OrdinalIgnoreCase));

        return (string?)element?.Attribute("value") ?? string.Empty;
    }

    private static string GetValue(string name, string fallback)
    {
        string? value = Environment.GetEnvironmentVariable(name);
        return string.IsNullOrWhiteSpace(value) ? fallback : value.Trim();
    }

    private static string GetRequiredValue(string name, string message)
    {
        string? value = Environment.GetEnvironmentVariable(name);
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new InvalidOperationException(message);
        }

        return value;
    }

    private static int GetIntValue(string name, int fallback)
    {
        return int.TryParse(Environment.GetEnvironmentVariable(name), out int value) ? value : fallback;
    }

    private static long GetLongValue(string name, long fallback)
    {
        return long.TryParse(Environment.GetEnvironmentVariable(name), out long value) ? value : fallback;
    }
}
