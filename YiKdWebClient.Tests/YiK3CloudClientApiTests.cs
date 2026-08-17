using System.Text.Json;
using YiKdWebClient.Model;
using YiKdWebClient.Tests.TestInfrastructure;

namespace YiKdWebClient.Tests;

public class YiK3CloudClientApiTests
{
    private const string Payload = "{\"Id\":123}";
    private const string SuccessResponse = "{\"ok\":true}";
    private const string DynamicFormPrefix =
        "Kingdee.BOS.WebApi.ServicesStub.DynamicFormService.";

    [Fact]
    public void ExecApiDynamicFormService_sends_wrapped_request()
    {
        YiK3CloudClient? invokedClient = null;
        AssertWrappedApi(
            client =>
            {
                invokedClient = client;
                return client.ExecApiDynamicFormService(
                    "TEST_Form",
                    Payload,
                    "Custom.Service.Run.common.kdsvc",
                    false,
                    false);
            },
            "Custom.Service.Run.common.kdsvc",
            "TEST_Form",
            Payload);

        Assert.Same(invokedClient, YiK3CloudClient.Instance);
    }

    [Fact]
    public void ExecApiDynamicFormService_can_send_raw_request()
    {
        AssertRawApi(
            client => client.ExecApiDynamicFormService(
                string.Empty,
                Payload,
                "Custom.Service.Raw.common.kdsvc",
                false,
                false,
                true),
            "Custom.Service.Raw.common.kdsvc");
    }

    [Fact]
    public void ExecuteOperation_sends_form_operation_and_payload_in_official_order()
    {
        AssertWrappedApi(
            client => client.ExecuteOperation("TEST_Form", "Forbid", Payload, false, false),
            DynamicFormPrefix + "ExecuteOperation.common.kdsvc",
            "TEST_Form",
            "Forbid",
            Payload);
    }

    [Fact]
    public void View_sends_View_request()
    {
        AssertFormApi((client, formId, json) => client.View(formId, json, false, false), "View");
    }

    [Fact]
    public void Save_sends_Save_request()
    {
        AssertFormApi((client, formId, json) => client.Save(formId, json, false, false), "Save");
    }

    [Fact]
    public void BatchSave_sends_BatchSave_request()
    {
        AssertFormApi((client, formId, json) => client.BatchSave(formId, json, false, false), "BatchSave");
    }

    [Fact]
    public void Submit_sends_Submit_request()
    {
        AssertFormApi((client, formId, json) => client.Submit(formId, json, false, false), "Submit");
    }

    [Fact]
    public void Audit_sends_Audit_request()
    {
        AssertFormApi((client, formId, json) => client.Audit(formId, json, false, false), "Audit");
    }

    [Fact]
    public void UnAudit_sends_UnAudit_request()
    {
        AssertFormApi((client, formId, json) => client.UnAudit(formId, json, false, false), "UnAudit");
    }

    [Fact]
    public void Delete_sends_Delete_request()
    {
        AssertFormApi((client, formId, json) => client.Delete(formId, json, false, false), "Delete");
    }

    [Fact]
    public void ExecuteBillQuery_sends_ExecuteBillQuery_request()
    {
        AssertPayloadApi((client, json) => client.ExecuteBillQuery(json, false, false), "ExecuteBillQuery");
    }

    [Fact]
    public void CustomBusinessService_string_overload_appends_service_suffix()
    {
        const string service = "Sample.WebApi.Service.Run,Sample.WebApi";
        AssertWrappedApi(
            client => client.CustomBusinessService(Payload, service, false, false),
            service + ".common.kdsvc",
            Payload);
    }

    [Fact]
    public void CustomBusinessService_model_overload_builds_service_path()
    {
        var service = new CustomServicesStubpath
        {
            ProjetNamespace = " Sample.WebApi ",
            ProjetClassName = " Service ",
            ProjetClassMethod = " Run "
        };

        AssertWrappedApi(
            client => client.CustomBusinessService(Payload, service, false, false),
            "Sample.WebApi.Service.Run,Sample.WebApi.common.kdsvc",
            Payload);
    }

    [Fact]
    public void CustomBusinessServiceByParameters_string_overload_sends_raw_parameters()
    {
        const string service = "Sample.WebApi.Service.Run,Sample.WebApi";
        AssertRawApi(
            client => client.CustomBusinessServiceByParameters(Payload, service, false, false),
            service + ".common.kdsvc");
    }

    [Fact]
    public void CustomBusinessServiceByParameters_model_overload_sends_raw_parameters()
    {
        var service = new CustomServicesStubpath
        {
            ProjetNamespace = "Sample.WebApi",
            ProjetClassName = "Service",
            ProjetClassMethod = "Run"
        };

        AssertRawApi(
            client => client.CustomBusinessServiceByParameters(Payload, service, false, false),
            "Sample.WebApi.Service.Run,Sample.WebApi.common.kdsvc");
    }

    [Fact]
    public void Draft_sends_Draft_request()
    {
        AssertFormApi((client, formId, json) => client.Draft(formId, json, false, false), "Draft");
    }

    [Fact]
    public void Allocate_sends_Allocate_request()
    {
        AssertFormApi((client, formId, json) => client.Allocate(formId, json, false, false), "Allocate");
    }

    [Fact]
    public void Push_sends_Push_request()
    {
        AssertFormApi((client, formId, json) => client.Push(formId, json, false, false), "Push");
    }

    [Fact]
    public void GroupSave_sends_GroupSave_request()
    {
        AssertFormApi((client, formId, json) => client.GroupSave(formId, json, false, false), "GroupSave");
    }

    [Fact]
    public void FlexSave_sends_FlexSave_request()
    {
        AssertFormApi((client, formId, json) => client.FlexSave(formId, json, false, false), "FlexSave");
    }

    [Fact]
    public void SendMsg_sends_SendMsg_request()
    {
        AssertPayloadApi((client, json) => client.SendMsg(json, false, false), "SendMsg");
    }

    [Fact]
    public void SwitchOrg_sends_SwitchOrg_request()
    {
        AssertPayloadApi((client, json) => client.SwitchOrg(json, false, false), "SwitchOrg");
    }

    [Fact]
    public void WorkflowAudit_sends_WorkflowAudit_request()
    {
        AssertPayloadApi((client, json) => client.WorkflowAudit(json, false, false), "WorkflowAudit");
    }

    [Fact]
    public void GetSysReportData_sends_GetSysReportData_request()
    {
        AssertFormApi(
            (client, formId, json) => client.GetSysReportData(formId, json, false, false),
            "GetSysReportData");
    }

    [Fact]
    public void AttachmentUpLoad_sends_raw_request()
    {
        AssertRawApi(
            client => client.AttachmentUpLoad(Payload, false, false),
            DynamicFormPrefix + "AttachmentUpLoad.common.kdsvc");
    }

    [Fact]
    public void AttachmentDownLoad_sends_raw_request()
    {
        AssertRawApi(
            client => client.AttachmentDownLoad(Payload, false, false),
            DynamicFormPrefix + "AttachmentDownLoad.common.kdsvc");
    }

    [Fact]
    public void UploadFile_sends_raw_request()
    {
        AssertRawApi(
            client => client.UploadFile(Payload, false, false),
            DynamicFormPrefix + "UploadFile.common.kdsvc");
    }

    [Fact]
    public void GroupDelete_sends_GroupDelete_request()
    {
        AssertPayloadApi((client, json) => client.GroupDelete(json, false, false), "GroupDelete");
    }

    [Fact]
    public void CancelAllocate_sends_CancelAllocate_request()
    {
        AssertFormApi(
            (client, formId, json) => client.CancelAllocate(formId, json, false, false),
            "CancelAllocate");
    }

    [Fact]
    public void CancelAssign_sends_CancelAssign_request()
    {
        AssertFormApi(
            (client, formId, json) => client.CancelAssign(formId, json, false, false),
            "CancelAssign");
    }

    [Fact]
    public void Disassembly_sends_Disassembly_request()
    {
        AssertFormApi(
            (client, formId, json) => client.Disassembly(formId, json, false, false),
            "Disassembly");
    }

    [Fact]
    public void QueryBusinessInfo_sends_QueryBusinessInfo_request()
    {
        AssertPayloadApi(
            (client, json) => client.QueryBusinessInfo(json, false, false),
            "QueryBusinessInfo");
    }

    [Fact]
    public void QueryGroupInfo_sends_QueryGroupInfo_request()
    {
        AssertPayloadApi((client, json) => client.QueryGroupInfo(json, false, false), "QueryGroupInfo");
    }

    [Fact]
    public void GetDataCenterList_uses_supplied_server_url()
    {
        using var server = new LoopbackHttpServer();
        using var client = TestClientFactory.CreateApiHeaderClient("http://unused.invalid/");

        var response = client.GetDataCenterList(server.K3CloudUrl.TrimEnd('/'));

        Assert.Equal(SuccessResponse, response);
        var request = server.SingleRequest();
        Assert.Equal("POST", request.Method);
        Assert.Equal(
            "/k3cloud/Kingdee.BOS.ServiceFacade.ServicesStub.Account.AccountService.GetDataCenterList.common.kdsvc",
            request.PathAndQuery);
    }

    private static void AssertFormApi(
        Func<YiK3CloudClient, string, string, string> invoke,
        string operation)
    {
        AssertWrappedApi(
            client => invoke(client, "TEST_Form", Payload),
            DynamicFormPrefix + operation + ".common.kdsvc",
            "TEST_Form",
            Payload);
    }

    private static void AssertPayloadApi(
        Func<YiK3CloudClient, string, string> invoke,
        string operation)
    {
        AssertWrappedApi(
            client => invoke(client, Payload),
            DynamicFormPrefix + operation + ".common.kdsvc",
            Payload);
    }

    private static void AssertWrappedApi(
        Func<YiK3CloudClient, string> invoke,
        string servicePath,
        params string[] expectedParameters)
    {
        using var server = new LoopbackHttpServer();
        using var client = TestClientFactory.CreateApiHeaderClient(server.K3CloudUrl);

        var response = invoke(client);

        Assert.Equal(SuccessResponse, response);
        var request = server.SingleRequest();
        Assert.Equal("POST", request.Method);
        Assert.Equal("/k3cloud/" + servicePath, request.PathAndQuery);
        Assert.Contains("application/json", request.Headers["Content-Type"]);
        Assert.True(request.Headers.ContainsKey("X-Kd-Appkey"));
        Assert.Equal(expectedParameters, ReadParameters(request.Body));

        Assert.Equal(server.K3CloudUrl + servicePath, client.ReturnOperationWebModel.RequestUrl);
        Assert.Equal(request.Body, client.ReturnOperationWebModel.RealRequestBody);
        Assert.Equal(SuccessResponse, client.ReturnOperationWebModel.RealResponseBody);
    }

    private static void AssertRawApi(Func<YiK3CloudClient, string> invoke, string servicePath)
    {
        using var server = new LoopbackHttpServer();
        using var client = TestClientFactory.CreateApiHeaderClient(server.K3CloudUrl);

        var response = invoke(client);

        Assert.Equal(SuccessResponse, response);
        var request = server.SingleRequest();
        Assert.Equal("/k3cloud/" + servicePath, request.PathAndQuery);
        Assert.Equal(Payload, request.Body);
        Assert.Equal(Payload, client.ReturnOperationWebModel.RealRequestBody);
    }

    private static string[] ReadParameters(string requestBody)
    {
        using var document = JsonDocument.Parse(requestBody);
        var serializedParameters = document.RootElement.GetProperty("parameters").GetString();
        Assert.NotNull(serializedParameters);
        return JsonSerializer.Deserialize<string[]>(serializedParameters!)!;
    }
}
