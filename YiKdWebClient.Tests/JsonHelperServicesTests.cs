using System.Text.Json;
using YiKdWebClient.CommonService;

namespace YiKdWebClient.Tests;

public class JsonHelperServicesTests
{
    [Fact]
    public void GetRequestBodyString_wraps_form_operation_and_payload()
    {
        var json = JsonHelperServices.getRequestBodystring(
            "TEST_Form",
            "{\"Id\":1}",
            true,
            "Forbid");

        AssertStandardEnvelope(json);
        Assert.Equal(
            new[] { "TEST_Form", "Forbid", "{\"Id\":1}" },
            ReadParameters(json));
    }

    [Fact]
    public void GetRequestBodyString_omits_blank_form_and_operation()
    {
        var json = JsonHelperServices.getRequestBodystring(string.Empty, "{}", true, string.Empty);

        Assert.Equal(new[] { "{}" }, ReadParameters(json));
    }

    [Fact]
    public void GetRequestBodyString_honors_safe_and_relaxed_escaping()
    {
        var relaxed = JsonHelperServices.getRequestBodystring("FORM", "<tag>", true, string.Empty);
        var safe = JsonHelperServices.getRequestBodystring("FORM", "<tag>", false, string.Empty);

        Assert.Contains("<tag>", relaxed);
        Assert.DoesNotContain("<tag>", safe);
        Assert.Contains(@"\\u003Ctag\\u003E", safe);
        Assert.Equal(ReadParameters(relaxed), ReadParameters(safe));
    }

    [Fact]
    public void GetLoginRequestBodyString_builds_standard_envelope()
    {
        var json = JsonHelperServices.getLoginRequestBodystring("[\"db\",\"user\"]", true);

        AssertStandardEnvelope(json);
        using var document = JsonDocument.Parse(json);
        Assert.Equal("[\"db\",\"user\"]", document.RootElement.GetProperty("parameters").GetString());
    }

    [Fact]
    public void GetLoginRequestBodyString_can_write_indented_json()
    {
        var json = JsonHelperServices.getLoginRequestBodystring("[]", true, true);

        Assert.Contains(Environment.NewLine, json);
        AssertStandardEnvelope(json);
    }

    [Fact]
    public void GetLoginRequestBodyStringByParameters_builds_parameter_only_envelope()
    {
        var json = JsonHelperServices.getLoginRequestBodystringByParameters("[1,2]", true);

        using var document = JsonDocument.Parse(json);
        Assert.Single(document.RootElement.EnumerateObject());
        Assert.Equal("[1,2]", document.RootElement.GetProperty("parameters").GetString());
    }

    private static void AssertStandardEnvelope(string json)
    {
        using var document = JsonDocument.Parse(json);
        var root = document.RootElement;
        Assert.Equal(1, root.GetProperty("format").GetInt32());
        Assert.Equal("ApiClient", root.GetProperty("useragent").GetString());
        Assert.Equal("1.0", root.GetProperty("v").GetString());
        Assert.False(string.IsNullOrWhiteSpace(root.GetProperty("rid").GetString()));
        Assert.True(root.GetProperty("timestamp").TryGetDateTime(out _));
    }

    private static string[] ReadParameters(string json)
    {
        using var document = JsonDocument.Parse(json);
        var serialized = document.RootElement.GetProperty("parameters").GetString();
        Assert.NotNull(serialized);
        return JsonSerializer.Deserialize<string[]>(serialized!)!;
    }
}
