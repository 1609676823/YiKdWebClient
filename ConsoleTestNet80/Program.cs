using System.ComponentModel.DataAnnotations;
using System.Data.SqlTypes;
using System.Net;
using System.Reflection;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using YiKdWebClient;
using YiKdWebClient.Model;
using YiKdWebClient.SSO;
using YiKdWebClient.ToolsHelper;

namespace ConsoleTestNet80
{
    internal class Program
    {
        static void Main(string[] args)
        {
          
            string Formid = "SEC_User";
            string Json = @"{""IsUserModelInit"":""true"",""Number"":""Administrator"",""IsSortBySeq"":""false""}";
            YiK3CloudClient yiK3CloudClient = new YiKdWebClient.YiK3CloudClient();
            yiK3CloudClient.LoginType = LoginType.LoginBySignSHA256;
            string resultJson = yiK3CloudClient.View(Formid, Json);


            #region 签名信息认证


            //string Formid = "SEC_User";
            //string Json = @"{""IsUserModelInit"":""true"",""Number"":""Administrator"",""IsSortBySeq"":""false""}";
            //YiK3CloudClient yiK3CloudClient = new YiKdWebClient.YiK3CloudClient();
            //yiK3CloudClient.LoginType= LoginType.LoginBySignSHA1;
            //string resultJson = yiK3CloudClient.View(Formid, Json);

            ///*登录地址*/
            //Console.WriteLine("真实的登录地址: ");
            //Console.WriteLine(yiK3CloudClient.ReturnLoginWebModel.RequestUrl);

            //Console.WriteLine("真实的登录报文: ");
            //Console.WriteLine(yiK3CloudClient.ReturnLoginWebModel.RealRequestBody);

            ////真实的操作请求地址
            //string RequestUrl = yiK3CloudClient.ReturnOperationWebModel.RequestUrl;
            //Console.WriteLine("真实的操作请求地址: ");
            //Console.WriteLine(RequestUrl);
            ////真实的操作请求报文
            //string RealRequestBody = yiK3CloudClient.ReturnOperationWebModel.RealRequestBody;

            //Console.WriteLine("真实的操作请求报文: ");
            //Console.WriteLine(RealRequestBody);
            //Console.WriteLine("真实的操作请求返回结果: ");
            //Console.WriteLine(resultJson);
            //Console.ReadKey();
            //var resultJson = yiK3CloudClient.GetDataCenterList();
            // var res = yiK3CloudClient.ExecuteOperation("view",Formid, Json);

            #endregion


            #region 自定义AppSettings


            //AppSettingsModel appSettingsModel = new AppSettingsModel();
            //YiK3CloudClient yiK3CloudClient = new YiKdWebClient.YiK3CloudClient();
            //yiK3CloudClient.AppSettingsModel = appSettingsModel;

            #endregion


            #region 自定义webapi
            // //string cnfFilePath = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "YiKdWebCfg", "API测试.cnf");
            // //YiK3CloudClient yiK3CloudClient = new YiKdWebClient.YiK3CloudClient()
            // //{
            // //    LoginType = LoginType.LoginBySimplePassport,
            // //    LoginBySimplePassportModel = new LoginBySimplePassportModel() { Url = @"http://localhost:1200/k3cloud/", CnfFilePath = cnfFilePath }
            // //};

            // YiK3CloudClient yiK3CloudClient = new YiKdWebClient.YiK3CloudClient();
            //// JsonSerializerOptions options = new JsonSerializerOptions() { WriteIndented = true, Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping };

            // YiKdWebClient.Model.CustomServicesStubpath customServicesStubpath = new()
            // {
            //     ProjetNamespace = "GlobalServiceCustom.WebApi",/*dll的命名空间*/
            //     ProjetClassName = "DataServiceHandler",/*对应的类名*/
            //     ProjetClassMethod = "CommonRunnerService"/*对应的方法名*/
            // };

            // string sql = string.Format(@"SELECT TOP 10 *from T_BD_MATERIAL_L");

            // object parametersdata = new { parameters = new string[] { sql } };
            // string parametersdatajson = System.Text.Json.JsonSerializer.Serialize(parametersdata, new JsonSerializerOptions() { WriteIndented = true, Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping });

            // string jsonString = @" { ""parameters"": [ ""SELECT TOP 10 * FROM T_BD_MATERIAL_L"" ] }";

            // string resultJson = yiK3CloudClient.CustomBusinessServiceByParameters(jsonString, customServicesStubpath);
            //// Console.WriteLine(resultJson);

            // /*如下信息为可以使用postman调试的报文和地址*/
            // Console.WriteLine("真实的登录地址: ");
            // Console.WriteLine(yiK3CloudClient.ReturnLoginWebModel.RequestUrl);

            // Console.WriteLine("真实的登录报文: ");
            // Console.WriteLine(yiK3CloudClient.ReturnLoginWebModel.RealRequestBody);

            // //真实的操作请求地址
            // string RequestUrl = yiK3CloudClient.ReturnOperationWebModel.RequestUrl;
            // Console.WriteLine("真实的操作请求地址: ");
            // Console.WriteLine(RequestUrl);
            // //真实的操作请求报文
            // string RealRequestBody = yiK3CloudClient.ReturnOperationWebModel.RealRequestBody;

            // Console.WriteLine("真实的操作请求报文: ");
            // Console.WriteLine(RealRequestBody);
            // Console.WriteLine("真实的操作请求返回结果: ");
            // Console.WriteLine(resultJson);
            // Console.ReadKey();
            #endregion


            #region 元数据查询


            //string cnfFilePath = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "YiKdWebCfg", "API测试.cnf");
            //YiK3CloudClient yiK3CloudClient = new YiKdWebClient.YiK3CloudClient()
            //{
            //    LoginType = LoginType.LoginBySimplePassport,
            //    LoginBySimplePassportModel = new LoginBySimplePassportModel() { Url = @"http://127.0.0.1/K3Cloud/", CnfFilePath = cnfFilePath }
            //};

            //JsonSerializerOptions options = new JsonSerializerOptions() { WriteIndented=true,Encoder= System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping};
            //object QueryBusinessInfodata = new { FormId = "ER_ExpenseRequest"};
            //string QueryBusinessInfojson= System.Text.Json.JsonSerializer.Serialize(QueryBusinessInfodata, options);
            //string resultJson= yiK3CloudClient.QueryBusinessInfo(QueryBusinessInfojson);
            //Console.WriteLine(resultJson);
            //Console.ReadKey();

            #endregion

            #region 分块上传


            //YiK3CloudClient yiK3CloudClient = new YiKdWebClient.YiK3CloudClient();
            //yiK3CloudClient.LoginType = LoginType.ValidateUserEnDeCode;
            //yiK3CloudClient.validateLoginSettingsModel= new ValidateLoginSettingsModel() { Url = @"http://127.0.0.1/K3Cloud/", DbId = "675c0162520ad7", UserName = "demo", Password = "123456", lcid = 2052 };
            ////string cnfFilePath = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "YiKdWebCfg", "API测试.cnf");
            //// yiK3CloudClient.LoginBySimplePassportModel = new LoginBySimplePassportModel() { Url = @"http://127.0.0.1/K3Cloud/", CnfFilePath = cnfFilePath };
            //string path = @"D:\test1.pdf";
            //UploadModel uploadModeltemplate =new UploadModel();
            //uploadModeltemplate.data.FormId = "BD_Currency";
            //uploadModeltemplate.data.InterId = "143717";
            //uploadModeltemplate.data.BillNO = "测试编码";


            // /*获取分块上传进度*/
            // Action<FileChunk, YiK3CloudClient> progressaction = (fileChunk,yiK3CloudClient)=>
            // {
            //     Console.WriteLine("正在处理第" + (fileChunk.Chunkindex + 1) + "分块");
            //     Console.WriteLine("请求报文为:" + yiK3CloudClient.ReturnOperationWebModel.RealRequestBody);
            //     Console.WriteLine("处理结果为:" + yiK3CloudClient.ReturnOperationWebModel.RealResponseBody);
            //     if (fileChunk.IsLast)
            //     {
            //         Console.WriteLine("所有分块处理结束");
            //     }
            // };

            // string resjosn=  AttachmentHelper.AttachmentUploadByFilePath(path, yiK3CloudClient, uploadModeltemplate,1024*1024*2, progressaction);


            //// var resultJson = yiK3CloudClient.View(Formid, Json);
            // Console.WriteLine(resjosn);
            //Console.ReadKey();

            #endregion

            #region 第三方授权验证


            //string Formid = "SEC_User";
            //string Json = @"{""IsUserModelInit"":""true"",""Number"":""Administrator"",""IsSortBySeq"":""false""}";
            //YiK3CloudClient yiK3CloudClient = new YiKdWebClient.YiK3CloudClient();
            //var resultJson = yiK3CloudClient.View(Formid, Json);


            ///*登陆地址*/
            //Console.WriteLine("真实的登录地址: ");
            //Console.WriteLine(yiK3CloudClient.ReturnLoginWebModel.RequestUrl);

            //Console.WriteLine("真实的登录报文: ");
            //Console.WriteLine(yiK3CloudClient.ReturnLoginWebModel.RealRequestBody);

            ////真实的请求地址
            //string RequestUrl = yiK3CloudClient.ReturnOperationWebModel.RequestUrl;
            //Console.WriteLine("真实的请求地址: ");
            //Console.WriteLine(RequestUrl);
            ////真实的请求报文
            //string RealRequestBody = yiK3CloudClient.ReturnOperationWebModel.RealRequestBody;

            //Console.WriteLine("真实的请求报文: ");
            //Console.WriteLine(RealRequestBody);
            //Console.WriteLine("真实的请求返回结果: ");
            //Console.WriteLine(resultJson);
            //Console.ReadKey();
            ////var resultJson = yiK3CloudClient.GetDataCenterList();
            //// var res = yiK3CloudClient.ExecuteOperation("view",Formid, Json);

            #endregion



            #region API签名认证
            // string Formid = "SEC_User";
            // string Json = @"{""IsUserModelInit"":""true"",""Number"":""Administrator"",""IsSortBySeq"":""false""}";
            // YiK3CloudClient yiK3CloudClient = new YiKdWebClient.YiK3CloudClient();
            // yiK3CloudClient.LoginType = LoginType.LoginByApiSignHeaders;
            //// var resultJson = yiK3CloudClient.ExecuteOperation("View", Formid, Json);

            // var resultJson = yiK3CloudClient.View(Formid, Json);
            // //签名请求头的字符串，可以直接导入postman，Apipost
            // string RequestHeadersString = yiK3CloudClient.RequestHeadersString;
            // Console.WriteLine("签名请求头的字符串，可以直接导入postman，Apipost:");
            // Console.WriteLine(RequestHeadersString);
            // //真实的请求地址
            // string RequestUrl = yiK3CloudClient.ReturnOperationWebModel.RequestUrl;
            // Console.WriteLine("真实的请求地址: ");
            // Console.WriteLine(RequestUrl);
            // //真实的请求报文
            // string RealRequestBody = yiK3CloudClient.ReturnOperationWebModel.RealRequestBody;

            // Console.WriteLine("真实的请求报文: ");
            // Console.WriteLine(RealRequestBody);

            // Console.WriteLine("请求结果: ");
            // Console.WriteLine(resultJson);
            //  Console.ReadKey();
            #endregion

            #region 旧版用户名密码验证
            //string Formid = "SEC_User";
            //string Json = @"{""IsUserModelInit"":""true"",""Number"":""Administrator"",""IsSortBySeq"":""false""}";
            //YiK3CloudClient yiK3CloudClient = new YiKdWebClient.YiK3CloudClient();
            //yiK3CloudClient.LoginType = LoginType.ValidateLogin;
            //yiK3CloudClient.validateLoginSettingsModel = new ValidateLoginSettingsModel() { Url = @"http://127.0.0.1/K3Cloud/", DbId = "629bd5285d655d", UserName = "demo", Password = "123456", lcid = 2052 };
            //var resultJson = yiK3CloudClient.View(Formid, Json);
            //Console.WriteLine(resultJson);
            #endregion


            #region 集成密钥文件认证
            //string Formid = "SEC_User";
            //string Json = @"{""IsUserModelInit"":""true"",""Number"":""Administrator"",""IsSortBySeq"":""false""}";
            //YiK3CloudClient yiK3CloudClient = new YiKdWebClient.YiK3CloudClient();
            //yiK3CloudClient.LoginType = LoginType.LoginBySimplePassport;
            //string cnfFilePath = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "YiKdWebCfg", "API测试.cnf");
            //yiK3CloudClient.LoginBySimplePassportModel = new LoginBySimplePassportModel() { Url = @"http://127.0.0.1/K3Cloud/", CnfFilePath = cnfFilePath };
            //var resultJson = yiK3CloudClient.View(Formid, Json);
            //Console.WriteLine(resultJson);

            #endregion



            #region 集成密钥Base64认证


            //string Formid = "SEC_User";
            //string Json = @"{""IsUserModelInit"":""true"",""Number"":""Administrator"",""IsSortBySeq"":""false""}";
            //YiK3CloudClient yiK3CloudClient = new YiKdWebClient.YiK3CloudClient() { LoginType=LoginType.LoginBySimplePassport} ;
            //string simplePassportForBase64 = @"MKuiZlgby8qu3VTiY89wT3TXjLapXeQcZrnl3pYte9rvyv8nCllx1ybSz+iTdbZfV7/ggFufoXXNAVvPilcQtbLxiqmN8Iw9+R8HaMaf4lLta6BOuBA0r13MvXui0rIFXW6XgCyc1C3ppsikT3wUY5Ua39R+pZS0HApZmxGxc5pRC/AllsUDEcIIX7sMDXTzVz90wa6RVSguPTLo0vZ5ug5bZqKH3XT9wK4QSnHdzr9QbaULG4EIM/VvJjX3ttswL8yt4HP64IdjwYUR6uZhfRDotnsHhQAZSb/Wi4zXUNh7DKyRyZd8IcjWzfC/lvLqXleyqEi8P07HP8CSDzWooIjiQ77D6IVTeydBfqQ5W5ax6kWJOQBRC+rY8RzZTGG4DlZwosnjSfkS+Ydr+KNf9E2ppujYGPLWaXhIV5QKLDn8CzNW7KqtmihRlfRcueobhJ6JQwAwh0Kc4+nBldUaVep7kOeKtjARJaPaOW/r3nHFat1UhxAPC58tEd2cQP6GZ6HUaTqYAhe9XM4HEQ9rRC1Gt0PBUdwfoll3oBPv20qxUI3uj58NFijca3UeXhwWmW+CW/G5XIbesRX3WQsa7NdKLSYB/Vb4tSKrgCIpKL6Y8ivBlDohwYTXh/y5DxhkOyncyQo4nuZZJkFoxXVaie7/S8LFrVz5y2kjRuumWfvU7s/NDXgHF5+jLTtVyrRfBtpifFiKbLhKV+lnDX0Ho8CypV9fsF7vQkH13Tyann0Ye1J50M4OxyyTNuaiBPnlz3MREjBruNR+h5pp/YWeqeB0ZdtqUIBOD0YvBE5+BBilkZIsF937vBi3BKykgOCvBZ93C+1hmxA9P2xMU3+5DJifFlIvoJbkismNqciycQslrtDCmCG/CYlvMClLTCjs";
            //yiK3CloudClient.LoginBySimplePassportModel = new LoginBySimplePassportModel() { Url = @"http://127.0.0.1/K3Cloud/", SimplePassportForBase64 = simplePassportForBase64,bySimplePassportType=BySimplePassportType.ForBase64};
            //var resultJson = yiK3CloudClient.View(Formid, Json);
            // Console.WriteLine(resultJson);
            #endregion


            #region 单点登录V4
            // SSOHelper sSOHelper = new SSOHelper() { Url = @"http://127.0.0.1/K3Cloud" };
            // sSOHelper.appSettingsModel.XKDApiUserName = "demo"; /*指定用户，若不指定则取配置文件默认的集成用户*/
            //// sSOHelper.simplePassportLoginArg.formid = "BD_MATERIAL";
            //// sSOHelper.simplePassportLoginArg.formtype = "list";
            // sSOHelper.GetSsoUrlsV4();



            //SSOLogoutObject sSOLogoutObject = sSOHelper.GetSSOLogoutap0StrV2V1();

            #endregion

            #region 单点登录V3
            //SSOHelper sSOHelper = new SSOHelper() { Url = @"http://127.0.0.1:9980/K3Cloud" };
            ////sSOHelper.appSettingsModel.XKDApiUserName = "demo"; /*指定用户，若不指定则取配置文件默认的集成用户*/
            //sSOHelper.GetSsoUrlsV3();
            #endregion

            #region 单点登录V2
            //SSOHelper sSOHelper = new SSOHelper() { Url = @"http://127.0.0.1:9980/K3Cloud" };
            ////sSOHelper.appSettingsModel.XKDApiUserName = "demo"; /*指定用户，若不指定则取配置文件默认的集成用户*/
            //sSOHelper.GetSsoUrlsV2();
            #endregion

            #region 单点登录V1
            //SSOHelper sSOHelper = new SSOHelper() { Url = @"http://127.0.0.1:9980/K3Cloud" };
            ////sSOHelper.appSettingsModel.XKDApiUserName = "demo"; /*指定用户，若不指定则取配置文件默认的集成用户*/
            //sSOHelper.GetSsoUrlsV1();
            #endregion

            #region 单点登录结果
            /*****如下为获取到的相关链接数据***********************************/
            ////数据中心ID
            //Console.WriteLine("数据中心ID："+" "+ sSOHelper.simplePassportLoginArg.dbid);
            ////应用ID
            //Console.WriteLine("应用ID：" + " " + sSOHelper.simplePassportLoginArg.appid);
            ////用户名称
            //Console.WriteLine("用户名称：" + " " + sSOHelper.simplePassportLoginArg.username);
            ////时间戳
            //Console.WriteLine("时间戳：" + " " + sSOHelper.timestamp);
            ////签名
            //Console.WriteLine("签名：" + " " + sSOHelper.simplePassportLoginArg.signeddata);
            ////请求参数（json格式）
            //Console.WriteLine("请求参数（json格式）：" + " " + sSOHelper.argJosn);
            ////参数格式化（Base64）
            //Console.WriteLine("参数格式化（Base64）：" + " " + sSOHelper.argJsonBase64);
            //// Silverlight入口链接
            //Console.WriteLine("Silverlight入口链接:");
            //Console.WriteLine(sSOHelper.SSOLoginUrlObject.silverlightUrl);
            //// html5入口链接
            //Console.WriteLine("html5入口链接:");
            //Console.WriteLine(sSOHelper.SSOLoginUrlObject.html5Url);
            ////客户端入口链接
            //Console.WriteLine("客户端入口链接:");
            //Console.WriteLine(sSOHelper.SSOLoginUrlObject.wpfUrl);

            //   Console.ReadKey();

            //登出ap0参数
            //Console.WriteLine("登出ap0参数:");
            // Console.WriteLine(sSOHelper.SSOLogoutObject.ap0);

            // var logout=  sSOHelper.SSOExcuteLogout(sSOHelper.GetSSOLogoutap0StrV3("demo"));
            //Console.WriteLine("登出验证:");
            //Console.WriteLine(logout);
            #endregion



            Console.ReadKey();


        }


    }
}
