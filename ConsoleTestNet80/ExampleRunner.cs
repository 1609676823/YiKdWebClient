using YiKdWebClient;
using YiKdWebClient.CommonService;
using YiKdWebClient.Model;
using YiKdWebClient.SSO;
using YiKdWebClient.ToolsHelper;

namespace ConsoleTestNet80;

internal static class ExampleRunner
{
    private const string FormId = "SEC_User";
    private const string ViewJson = "{\"IsUserModelInit\":\"true\",\"Number\":\"Administrator\",\"IsSortBySeq\":\"false\"}";

    public static void RunSignSha256()
    {
        RunViewExample(new YiK3CloudClient { LoginType = LoginType.LoginBySignSHA256 });
    }

    public static void RunSignSha1()
    {
        RunViewExample(new YiK3CloudClient { LoginType = LoginType.LoginBySignSHA1 });
    }

    public static void RunAppSecret()
    {
        RunViewExample(new YiK3CloudClient { LoginType = LoginType.LoginByAppSecret });
    }

    public static void RunValidateLogin()
    {
        using YiK3CloudClient client = new YiK3CloudClient
        {
            LoginType = LoginType.ValidateLogin,
            validateLoginSettingsModel = new ValidateLoginSettingsModel
            {
                Url = SampleEnvironment.ServerUrl,
                DbId = SampleEnvironment.ValidateDbId,
                UserName = SampleEnvironment.ValidateUserName,
                Password = SampleEnvironment.ValidatePassword,
                lcid = SampleEnvironment.ValidateLcid
            }
        };

        ConsoleReport.WriteSection("本次旧版登录参数");
        ConsoleReport.PrintSetting("服务地址", client.validateLoginSettingsModel.Url);
        ConsoleReport.PrintSetting("数据中心 ID", client.validateLoginSettingsModel.DbId);
        ConsoleReport.PrintSetting("用户名", client.validateLoginSettingsModel.UserName);
        ConsoleReport.PrintSecretSetting("密码");
        ConsoleReport.PrintSetting("语系", client.validateLoginSettingsModel.lcid.ToString());

        string result = client.View(FormId, ViewJson);
        ConsoleReport.PrintClientExchange(client, result);
    }

    public static void RunSimplePassport()
    {
        SampleEnvironment.EnsureFileExists(SampleEnvironment.CnfFilePath, "集成密钥文件");
        using YiK3CloudClient client = CreateSimplePassportClient();

        ConsoleReport.WriteSection("本次集成密钥配置");
        ConsoleReport.PrintSetting("服务地址", client.LoginBySimplePassportModel?.Url);
        ConsoleReport.PrintSetting("CNF 路径", client.LoginBySimplePassportModel?.CnfFilePath);

        string result = client.View(FormId, ViewJson);
        ConsoleReport.PrintClientExchange(client, result);
    }

    public static void RunApiSignHeaders()
    {
        RunViewExample(new YiK3CloudClient { LoginType = LoginType.LoginByApiSignHeaders });
    }

    public static void RunDynamicConfig()
    {
        AppSettingsModel settings = SampleEnvironment.CreateDynamicAppSettings();
        using YiK3CloudClient client = new YiK3CloudClient
        {
            AppSettingsModel = settings,
            LoginType = LoginType.LoginByAppSecret
        };

        ConsoleReport.WriteSection("本次由代码动态传入的配置");
        ConsoleReport.PrintSetting("账套 ID", settings.XKDApiAcctID);
        ConsoleReport.PrintSetting("集成用户", settings.XKDApiUserName);
        ConsoleReport.PrintSetting("应用 ID", settings.XKDApiAppID);
        ConsoleReport.PrintSetting("应用密钥", settings.XKDApiAppSec);
        ConsoleReport.PrintSetting("语系", settings.XKDApiLCID);
        ConsoleReport.PrintSetting("组织编码", settings.XKDApiOrgNum);
        ConsoleReport.PrintSetting("服务地址", settings.XKDApiServerUrl);

        string result = client.View(FormId, ViewJson);
        ConsoleReport.PrintClientExchange(client, result);
    }

    public static void RunCustomConfigPath()
    {
        SampleEnvironment.EnsureFileExists(SampleEnvironment.ConfigPath, "配置文件");
        XmlConfigHelper.AppConfigPath = SampleEnvironment.ConfigPath;

        ConsoleReport.WriteSection("自定义配置文件路径");
        ConsoleReport.PrintSetting("XmlConfigHelper.AppConfigPath", XmlConfigHelper.AppConfigPath);

        using YiK3CloudClient client = new YiK3CloudClient { LoginType = LoginType.LoginBySignSHA256 };
        string result = client.View(FormId, ViewJson);
        ConsoleReport.PrintClientExchange(client, result);
    }

    public static void RunCustomWebApi()
    {
        using YiK3CloudClient client = new YiK3CloudClient { LoginType = LoginType.LoginByAppSecret };
        string json = System.Text.Json.JsonSerializer.Serialize(new
        {
            parameters = new[] { SampleEnvironment.CustomSql }
        });

        CustomServicesStubpath service = new CustomServicesStubpath
        {
            ProjetNamespace = "GlobalServiceCustom.WebApi",
            ProjetClassName = "DataServiceHandler",
            ProjetClassMethod = "CommonRunnerService"
        };

        ConsoleReport.WriteSection("自定义服务定位信息");
        ConsoleReport.PrintSetting("命名空间", service.ProjetNamespace);
        ConsoleReport.PrintSetting("类名", service.ProjetClassName);
        ConsoleReport.PrintSetting("方法名", service.ProjetClassMethod);
        ConsoleReport.PrintSetting("SQL 参数", SampleEnvironment.CustomSql);

        string result = client.CustomBusinessServiceByParameters(json, service);
        ConsoleReport.PrintClientExchange(client, result);
    }

    public static void RunSsoV4()
    {
        SSOHelper helper = new SSOHelper();
        string userName = SampleEnvironment.CreateDynamicAppSettings().XKDApiUserName;
        helper.GetSsoUrlsV4(userName);
        ConsoleReport.PrintSsoResult(helper);
    }

    public static void RunUploadFile()
    {
        string filePath = SampleEnvironment.UploadFilePath;
        SampleEnvironment.EnsureFileExists(filePath, "待上传文件");
        SampleEnvironment.EnsureFileExists(SampleEnvironment.CnfFilePath, "集成密钥文件");

        using YiK3CloudClient client = CreateSimplePassportClient();
        UploadModel uploadModel = SampleEnvironment.CreateUploadModel();
        PrintUploadSettings(filePath, uploadModel);

        string result = AttachmentHelper.AttachmentUploadByFilePath(
            filePath,
            client,
            uploadModel,
            SampleEnvironment.UploadChunkSize);

        ConsoleReport.PrintClientExchange(client, result);
    }

    public static void RunUploadWithProgress()
    {
        string filePath = SampleEnvironment.UploadFilePath;
        SampleEnvironment.EnsureFileExists(filePath, "待上传文件");
        SampleEnvironment.EnsureFileExists(SampleEnvironment.CnfFilePath, "集成密钥文件");

        using YiK3CloudClient client = CreateSimplePassportClient();
        UploadModel uploadModel = SampleEnvironment.CreateUploadModel();
        PrintUploadSettings(filePath, uploadModel);

        string result = AttachmentHelper.AttachmentUploadByFilePath(
            filePath,
            client,
            uploadModel,
            SampleEnvironment.UploadChunkSize,
            (chunk, currentClient) => ConsoleReport.PrintProgress(chunk.Chunkindex + 1, chunk.IsLast, currentClient));

        ConsoleReport.WriteSection("上传方法最终返回值");
        ConsoleReport.WritePayload(result);
        Console.WriteLine();
    }

    public static void RunUploadBase64()
    {
        string filePath = SampleEnvironment.UploadFilePath;
        SampleEnvironment.EnsureFileExists(filePath, "待转换的示例文件");
        SampleEnvironment.EnsureFileExists(SampleEnvironment.CnfFilePath, "集成密钥文件");

        string base64 = Convert.ToBase64String(File.ReadAllBytes(filePath));
        using YiK3CloudClient client = CreateSimplePassportClient();
        UploadModel uploadModel = SampleEnvironment.CreateUploadModel();
        PrintUploadSettings(filePath, uploadModel);
        ConsoleReport.PrintSetting("Base64 字符数", base64.Length.ToString());

        string result = AttachmentHelper.AttachmentUploadByBase64(
            base64,
            Path.GetFileName(filePath),
            client,
            uploadModel,
            SampleEnvironment.UploadChunkSize);

        ConsoleReport.PrintClientExchange(client, result);
    }

    private static void RunViewExample(YiK3CloudClient client)
    {
        using (client)
        {
            string result = client.View(FormId, ViewJson);
            ConsoleReport.PrintClientExchange(client, result);
        }
    }

    private static YiK3CloudClient CreateSimplePassportClient()
    {
        return new YiK3CloudClient
        {
            LoginType = LoginType.LoginBySimplePassport,
            LoginBySimplePassportModel = new LoginBySimplePassportModel
            {
                Url = SampleEnvironment.ServerUrl,
                CnfFilePath = SampleEnvironment.CnfFilePath
            }
        };
    }

    private static void PrintUploadSettings(string filePath, UploadModel uploadModel)
    {
        ConsoleReport.WriteSection("本次附件上传参数");
        ConsoleReport.PrintSetting("文件路径", filePath);
        ConsoleReport.PrintSetting("文件大小（字节）", new FileInfo(filePath).Length.ToString());
        ConsoleReport.PrintSetting("分块大小（字节）", SampleEnvironment.UploadChunkSize.ToString());
        ConsoleReport.PrintSetting("表单 ID", uploadModel.data.FormId);
        ConsoleReport.PrintSetting("单据内码", uploadModel.data.InterId);
        ConsoleReport.PrintSetting("单据编号", uploadModel.data.BillNO);
    }
}
