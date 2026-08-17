using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace YiKdWebClient.AuthService
{
    /// <summary>
    /// LoginBySign使用签名信息登录
    /// </summary>
    public class LoginBySign
    {
        /// <summary>
        /// 登录验证类型
        /// </summary>
        public Model.LoginType LoginType { get; set; } = Model.LoginType.LoginBySignSHA256;

        /// <summary>
        /// 
        /// </summary>
        public TimeSpan Timeout { get; set; } = TimeSpan.FromSeconds(30);
        /// <summary>
        /// 
        /// </summary>
        public Dictionary<string, string> RequestHeaders = new Dictionary<string, string>();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="json"></param>
        /// <param name="UnsafeRelaxedJsonEscaping"></param>
        /// <returns></returns>
        public Model.RequestWebModel Login(string url, string json, bool UnsafeRelaxedJsonEscaping = true)
        {
            Model.RequestWebModel requestWebModel = new Model.RequestWebModel();

            url = CommonFunctionHelper.GetServerUrl(url);
            string loginapiurl = url + @"Kingdee.BOS.WebApi.ServicesStub.AuthService.LoginBySign.common.kdsvc";
            requestWebModel.RequestUrl = loginapiurl;
            //requestWebModel.RealRequestBody = CommonService.JsonHelperServices.getRequestBodystring(json, UnsafeRelaxedJsonEscaping);
            requestWebModel.RealRequestBody = json;

            try
            {
                CommonService.WebHelperServices webHelperServices = new CommonService.WebHelperServices();
                webHelperServices.Timeout = this.Timeout;
                webHelperServices.RequestHeaders = this.RequestHeaders;
                Task<string> postTask = webHelperServices.SendHttpRequestAsync(loginapiurl, requestWebModel.RealRequestBody);
                postTask.Wait(); // 阻塞直到任务完成 

                requestWebModel.RealResponseBody = postTask.Result;
                requestWebModel.Cookie = webHelperServices.cookies;



            }
            catch (Exception ex)
            {
                requestWebModel.RealResponseBody = ex.Message;
                // throw;
            }
            return requestWebModel;


        }

        /// <summary>
        /// 第三方登陆授权验证的登录json
        /// </summary>
        /// <param name="AppSettingsModel"></param>
        /// <param name="UnsafeRelaxedJsonEscaping"></param>
        /// <returns></returns>
        public string GetLoginJson(Model.AppSettingsModel AppSettingsModel, bool UnsafeRelaxedJsonEscaping)
        {
            JsonSerializerOptions options = new JsonSerializerOptions();
            options.WriteIndented = false; // 设置fales 为格式化为非缩进格式，即不保留换行符;
            if (UnsafeRelaxedJsonEscaping)
            {
                options.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
            }
            else
            {
                options.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Default;
            }

            //long timestamp = CommonFunctionHelper.CurrentTimeMillis() / 1000;
            long timestamp = CommonFunctionHelper.GetTimestamp();
            string[] arr = new string[] { AppSettingsModel.XKDApiAcctID, AppSettingsModel.XKDApiUserName, AppSettingsModel.XKDApiAppID, AppSettingsModel.XKDApiAppSec, timestamp.ToString() };

            //获取签名信息

            string sign=String.Empty;

            if (Model.LoginType.LoginBySignSHA256.Equals(LoginType)) 
            {
                sign = CommonFunctionHelper.GetSHA256(arr);
            }

            if (Model.LoginType.LoginBySignSHA1.Equals(LoginType))
            {
                sign = CommonFunctionHelper.GetSHA1(arr);
            }

            

           // 参数依次为账套ID、用户名、应用ID、时间戳、签名信息、语言ID；



            List<object> Parameters = new List<object>();
            Parameters.Add(AppSettingsModel.XKDApiAcctID);//帐套Id
            Parameters.Add(AppSettingsModel.XKDApiUserName);//用户名
            Parameters.Add(AppSettingsModel.XKDApiAppID);//应用ID
           // Parameters.Add(AppSettingsModel.XKDApiAppSec);//应用密钥
            Parameters.Add(timestamp.ToString());//时间戳
            Parameters.Add(sign);//签名信息
            Parameters.Add(AppSettingsModel.XKDApiLCID);//账套语系

            string KdContent = System.Text.Json.JsonSerializer.Serialize(Parameters, options);
            string RealContent = CommonService.JsonHelperServices.getLoginRequestBodystringByParameters(KdContent, UnsafeRelaxedJsonEscaping, false);

            return RealContent;
        }


    }
}
