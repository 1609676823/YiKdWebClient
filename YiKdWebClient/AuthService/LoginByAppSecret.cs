using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace YiKdWebClient.AuthService
{
    /// <summary>
    /// 第三方登陆授权验证
    /// </summary>
    public class LoginByAppSecret


    {
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
            string loginapiurl = url + @"Kingdee.BOS.WebApi.ServicesStub.AuthService.LoginByAppSecret.common.kdsvc";
            requestWebModel.RequestUrl = loginapiurl;
            //requestWebModel.RealRequestBody = CommonService.JsonHelperServices.getRequestBodystring(json, UnsafeRelaxedJsonEscaping);
            requestWebModel.RealRequestBody = json;

            try
            {
                CommonService.WebHelperServices webHelperServices = new CommonService.WebHelperServices();
                webHelperServices.Timeout = this.Timeout;
                webHelperServices.RequestHeaders =this. RequestHeaders;
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
            List<object> Parameters = new List<object>();
            Parameters.Add(AppSettingsModel.XKDApiAcctID);//帐套Id
            Parameters.Add(AppSettingsModel.XKDApiUserName);//用户名
            Parameters.Add(AppSettingsModel.XKDApiAppID);//应用ID
            Parameters.Add(AppSettingsModel.XKDApiAppSec);//应用密钥
            Parameters.Add(AppSettingsModel.XKDApiLCID);//账套语系

            string KdContent = System.Text.Json.JsonSerializer.Serialize(Parameters, options);
            string RealContent = CommonService.JsonHelperServices.getLoginRequestBodystring(KdContent, UnsafeRelaxedJsonEscaping,true);

            return RealContent;
        }


    }
}
