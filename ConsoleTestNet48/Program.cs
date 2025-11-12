using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;
using YiKdWebClient;
using YiKdWebClient.Model;

namespace ConsoleTestNet48
{
    internal class Program
    {
        static void Main(string[] args)
        {


            #region 第三方授权验证


            //string Formid = "SEC_User";
            //string Json = @"{""IsUserModelInit"":""true"",""Number"":""Administrator"",""IsSortBySeq"":""false""}";
            //YiK3CloudClient yiK3CloudClient = new YiKdWebClient.YiK3CloudClient();
            //var resultJson = yiK3CloudClient.View(Formid, Json);
            //Console.WriteLine(resultJson);

            //// var resultJson = yiK3CloudClient.GetDataCenterList();
            /// //// var res = yiK3CloudClient.ExecuteOperation("view",Formid, Json);

            #endregion



            #region API签名认证
            //string Formid = "SEC_User";
            //string Json = @"{""IsUserModelInit"":""true"",""Number"":""Administrator"",""IsSortBySeq"":""false""}";
            //YiK3CloudClient yiK3CloudClient = new YiKdWebClient.YiK3CloudClient();
            //yiK3CloudClient.LoginType = LoginType.LoginByApiSignHeaders;
            //var resultJson = yiK3CloudClient.View(Formid, Json);
            ////签名请求头的字符串，可以直接导入postman，Apipost
            //string RequestHeadersString = yiK3CloudClient.RequestHeadersString;
            //Console.WriteLine("签名请求头的字符串，可以直接导入postman，Apipost:");
            //Console.WriteLine(RequestHeadersString);
            ////真实的请求地址
            //string RequestUrl = yiK3CloudClient.ReturnOperationWebModel.RequestUrl;
            //Console.WriteLine("真实的请求地址: ");
            //Console.WriteLine(RequestUrl);
            ////真实的请求报文
            //string RealRequestBody = yiK3CloudClient.ReturnOperationWebModel.RealRequestBody;

            //Console.WriteLine("真实的请求报文: ");
            //Console.WriteLine(RealRequestBody);

            //Console.WriteLine("请求结果: ");
            //Console.WriteLine(resultJson);

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
            string Formid = "SEC_User";
            string Json = @"{""IsUserModelInit"":""true"",""Number"":""Administrator"",""IsSortBySeq"":""false""}";
            YiK3CloudClient yiK3CloudClient = new YiKdWebClient.YiK3CloudClient();
            yiK3CloudClient.LoginType = LoginType.LoginBySimplePassport;
            string cnfFilePath = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "YiKdWebCfg", "API测试.cnf");
            yiK3CloudClient.LoginBySimplePassportModel = new LoginBySimplePassportModel() { Url = @"http://127.0.0.1/K3Cloud/", CnfFilePath = cnfFilePath };
            var resultJson = yiK3CloudClient.View(Formid, Json);
            Console.WriteLine(resultJson);

            #endregion
            Console.ReadKey();


        }
    }
}
