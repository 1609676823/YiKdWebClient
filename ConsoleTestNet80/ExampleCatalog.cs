namespace ConsoleTestNet80;

internal sealed class ExampleDefinition
{
    public ExampleDefinition(string command, string title, string description, Action run)
    {
        Command = command;
        Title = title;
        Description = description;
        Run = run;
    }

    public string Command { get; }

    public string Title { get; }

    public string Description { get; }

    public Action Run { get; }
}

internal static class ExampleCatalog
{
    private static readonly IReadOnlyList<ExampleDefinition> Examples = new[]
    {
        new ExampleDefinition("sign-sha256", "签名信息认证（SHA256）", "推荐用于支持 SHA256 的金蝶云星空版本。", ExampleRunner.RunSignSha256),
        new ExampleDefinition("sign-sha1", "签名信息认证（SHA1）", "用于 PT-146911 8.0.0.202205 之前不支持 SHA256 的版本。", ExampleRunner.RunSignSha1),
        new ExampleDefinition("app-secret", "第三方系统登录授权", "使用账套 ID、集成用户、应用 ID 和应用密钥登录。", ExampleRunner.RunAppSecret),
        new ExampleDefinition("validate-login", "旧版用户名密码认证", "调用 ValidateUser 接口；仅建议兼容旧系统时使用。", ExampleRunner.RunValidateLogin),
        new ExampleDefinition("validate-user-endecode", "已弃用的 ValidateUserEnDeCode", "对用户名和密码执行旧式编码；仅用于兼容历史系统。", ExampleRunner.RunValidateUserEnDeCode),
        new ExampleDefinition("simple-passport", "集成密钥文件认证", "读取 YiKdWebCfg/API测试.cnf 完成登录。", ExampleRunner.RunSimplePassport),
        new ExampleDefinition("api-sign-headers", "API 请求头签名认证", "不调用登录接口，直接为业务请求生成签名请求头。", ExampleRunner.RunApiSignHeaders),
        new ExampleDefinition("dynamic-config", "代码动态配置授权信息", "不用固定配置文件绑定客户端，适合多环境、多账套或动态用户。", ExampleRunner.RunDynamicConfig),
        new ExampleDefinition("custom-config-path", "自定义配置文件路径", "运行前显式指定 appsettings.xml 的路径。", ExampleRunner.RunCustomConfigPath),
        new ExampleDefinition("custom-webapi", "调用自定义 WebAPI", "调用 GlobalServiceCustom.WebApi.DataServiceHandler.CommonRunnerService。", ExampleRunner.RunCustomWebApi),
        new ExampleDefinition("sso-v4", "单点登录 V4", "生成 V4 签名参数以及 Silverlight、HTML5、WPF 入口链接。", ExampleRunner.RunSsoV4),
        new ExampleDefinition("upload-file", "文件分块上传", "从文件路径读取内容并上传，返回最终结果。", ExampleRunner.RunUploadFile),
        new ExampleDefinition("upload-progress", "文件分块上传（进度回调）", "在每个分块完成后输出真实请求和响应。", ExampleRunner.RunUploadWithProgress),
        new ExampleDefinition("upload-base64", "Base64 流分块上传", "将 Base64 内容分块上传，不依赖调用方提供文件路径。", ExampleRunner.RunUploadBase64)
    };

    private static readonly IReadOnlyDictionary<string, ExampleDefinition> ExamplesByCommand =
        Examples.ToDictionary(example => example.Command, StringComparer.OrdinalIgnoreCase);

    public static bool TryGet(string command, out ExampleDefinition? example)
    {
        return ExamplesByCommand.TryGetValue(command, out example);
    }

    public static void PrintHelp()
    {
        Console.WriteLine("YiKdWebClient / ConsoleTestNet80 示例运行器");
        Console.WriteLine();
        Console.WriteLine("用法：");
        Console.WriteLine("  dotnet run --project ConsoleTestNet80 -f net8.0 -- <示例命令>");
        Console.WriteLine();
        Console.WriteLine("示例命令：");

        int commandWidth = Examples.Max(example => example.Command.Length) + 2;
        foreach (ExampleDefinition example in Examples)
        {
            Console.WriteLine($"  {example.Command.PadRight(commandWidth)}{example.Title}");
            Console.WriteLine($"  {new string(' ', commandWidth)}{example.Description}");
        }

        Console.WriteLine();
        Console.WriteLine("运行前请先将 ConsoleTestNet80/YiKdWebCfg 中的本地测试配置替换为你自己的环境信息。");
        Console.WriteLine("旧版登录和附件示例还可以通过 YIKD_* 环境变量覆盖参数，详见项目根目录 README.md。");
    }
}
