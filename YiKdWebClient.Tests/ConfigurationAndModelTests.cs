using System.ComponentModel;
using System.Reflection;
using System.Runtime.Serialization;
using YiKdWebClient.CommonService;
using YiKdWebClient.ComWebHelper;
using YiKdWebClient.Model;
using YiKdWebClient.SSO;
using YiKdWebClient.ToolsHelper;

namespace YiKdWebClient.Tests;

public class ConfigurationAndModelTests
{
    [Fact]
    public void XmlConfigHelper_reads_the_supplied_configuration_file()
    {
        var path = CreateConfigurationFile();
        try
        {
            var values = XmlConfigHelper.GetAllCfgDic(path);

            Assert.Equal("account-from-test", values["X-KDApi-AcctID"]);
            Assert.Equal("https://example.test/k3cloud", values["X-KDApi-ServerUrl"]);
            Assert.Equal(7, values.Count);
        }
        finally
        {
            File.Delete(path);
        }
    }

    [Fact]
    public void XmlConfigHelper_returns_empty_dictionary_for_missing_file()
    {
        var missingPath = Path.Combine(Path.GetTempPath(), $"missing-{Guid.NewGuid():N}.xml");

        var values = XmlConfigHelper.GetAllCfgDic(missingPath);

        Assert.Empty(values);
    }

    [Fact]
    public void AppSettingsModel_loads_custom_file_and_normalizes_server_url()
    {
        var path = CreateConfigurationFile();
        try
        {
            var model = new AppSettingsModel(path);

            Assert.Equal("account-from-test", model.XKDApiAcctID);
            Assert.Equal("app-from-test", model.XKDApiAppID);
            Assert.Equal("secret-from-test", model.XKDApiAppSec);
            Assert.Equal("user-from-test", model.XKDApiUserName);
            Assert.Equal("2052", model.XKDApiLCID);
            Assert.Equal("https://example.test/k3cloud/", model.XKDApiServerUrl);
            Assert.Equal("100", model.XKDApiOrgNum);
        }
        finally
        {
            File.Delete(path);
        }
    }

    [Fact]
    public void AppSettingsModel_server_url_property_normalizes_values()
    {
        var model = new AppSettingsModel { XKDApiServerUrl = "https://example.test/k3cloud" };
        Assert.Equal("https://example.test/k3cloud/", model.XKDApiServerUrl);

        model.XKDApiServerUrl = string.Empty;
        Assert.Equal(string.Empty, model.XKDApiServerUrl);
    }

    [Fact]
    public void ValidateLoginSettingsModel_normalizes_constructor_and_property_urls()
    {
        var model = new ValidateLoginSettingsModel("https://example.test/k3cloud");
        Assert.Equal("https://example.test/k3cloud/", model.Url);

        model.Url = "https://other.test/root";
        Assert.Equal("https://other.test/root/", model.Url);
        Assert.Equal(string.Empty, ValidateLoginSettingsModel.GetServerUrl(" "));
    }

    [Fact]
    public void LoginBySimplePassportModel_exposes_defaults_and_url_helper()
    {
        var model = new LoginBySimplePassportModel();

        Assert.Equal(2052, model.Lcid);
        Assert.Equal(BySimplePassportType.CnfFile, model.bySimplePassportType);
        Assert.Equal("https://example.test/k3cloud/", LoginBySimplePassportModel.GetServerUrl("https://example.test/k3cloud"));
        Assert.Equal(string.Empty, LoginBySimplePassportModel.GetServerUrl(string.Empty));
    }

    [Fact]
    public void CustomServicesStubpath_builds_route_and_removes_spaces()
    {
        var model = new CustomServicesStubpath
        {
            ProjetNamespace = " Sample .WebApi ",
            ProjetClassName = " Service ",
            ProjetClassMethod = " Run "
        };

        Assert.Equal(
            "Sample.WebApi.Service.Run,Sample.WebApi.common.kdsvc",
            model.GetCustomServicesStubpathUrl());
        Assert.Equal("abc", CustomServicesStubpath.RemoveSpaces(" a b c "));
    }

    [Fact]
    public void RequestWebModel_has_safe_non_null_defaults()
    {
        var model = new RequestWebModel();

        Assert.NotNull(model.Cookie);
        Assert.Equal(string.Empty, model.RequestUrl);
        Assert.Equal(string.Empty, model.RealRequestBody);
        Assert.Equal(string.Empty, model.RealResponseBody);
    }

    [Fact]
    public void Upload_models_have_expected_defaults()
    {
        var upload = new UploadModel();

        Assert.NotNull(upload.data);
        Assert.False(upload.data.IsLast);
        Assert.Equal("-1", upload.data.EntryinterId);
        Assert.Equal(string.Empty, upload.data.FileName);
        Assert.Equal(string.Empty, upload.data.SendByte);
    }

    [Fact]
    public void Sso_models_have_expected_defaults()
    {
        var args = new SimplePassportLoginArg();
        var urls = new SSOLoginUrlObject();
        var logout = new SSOLogoutObject();

        Assert.Equal("2052", args.lcid);
        Assert.Equal("SimPas", args.origintype);
        Assert.Equal(string.Empty, urls.html5Url);
        Assert.Equal(string.Empty, urls.silverlightUrl);
        Assert.Equal(string.Empty, urls.wpfUrl);
        Assert.Equal(string.Empty, logout.RequestLogoutUrl);
        Assert.Equal(string.Empty, logout.ap0);
        Assert.Null(Record.Exception(logout.SSOLoginUrlObject));
    }

    [Fact]
    public void Public_enums_have_description_and_enum_member_metadata()
    {
        AssertEnumMetadata<LoginType>();
        AssertEnumMetadata<OperationType>();
        AssertEnumMetadata<BySimplePassportType>();
        AssertEnumMetadata<BodyType>();
    }

    [Fact]
    public void CustomMediaTypeNames_public_constants_are_non_empty_media_types()
    {
        var fields = typeof(CustomMediaTypeNames)
            .GetNestedTypes(BindingFlags.Public)
            .SelectMany(type => type.GetFields(BindingFlags.Public | BindingFlags.Static))
            .Where(field => field.IsLiteral && !field.IsInitOnly)
            .ToArray();

        Assert.NotEmpty(fields);
        Assert.All(fields, field =>
        {
            var value = Assert.IsType<string>(field.GetRawConstantValue());
            Assert.False(string.IsNullOrWhiteSpace(value));
            Assert.Contains('/', value);
        });
        Assert.Equal("application/json", CustomMediaTypeNames.Application.Json);
        Assert.Equal("multipart/form-data", CustomMediaTypeNames.Multipart.FormData);
        Assert.Equal("text/plain", CustomMediaTypeNames.Text.Plain);
    }

    private static void AssertEnumMetadata<TEnum>() where TEnum : struct, Enum
    {
        foreach (var name in Enum.GetNames<TEnum>())
        {
            var field = typeof(TEnum).GetField(name)!;
            Assert.NotNull(field.GetCustomAttribute<DescriptionAttribute>());
            Assert.NotNull(field.GetCustomAttribute<EnumMemberAttribute>());
        }
    }

    private static string CreateConfigurationFile()
    {
        var path = Path.Combine(Path.GetTempPath(), $"YiKdWebClient-{Guid.NewGuid():N}.xml");
        var xml = """
                  <?xml version="1.0" encoding="utf-8" ?>
                  <configuration>
                    <appSettings>
                      <add key="X-KDApi-AcctID" value="account-from-test" />
                      <add key="X-KDApi-AppID" value="app-from-test" />
                      <add key="X-KDApi-AppSec" value="secret-from-test" />
                      <add key="X-KDApi-UserName" value="user-from-test" />
                      <add key="X-KDApi-LCID" value="2052" />
                      <add key="X-KDApi-ServerUrl" value="https://example.test/k3cloud" />
                      <add key="X-KDApi-OrgNum" value="100" />
                    </appSettings>
                  </configuration>
                  """;
        File.WriteAllText(path, xml);
        return path;
    }
}
