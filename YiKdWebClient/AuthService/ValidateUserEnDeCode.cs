using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace YiKdWebClient.AuthService
{
     /*https://vip.kingdee.com/article/183394?productLineId=1&isKnowledge=2&lang=zh-CN*/
    /// <summary>
    /// (不推荐使用)
    /// 此方法也可以过登陆验证，并且对用户名密码进行了简单加密，但是官方提供的webapi登陆验证中没有提及此方法，仅在有一些旧版本附件上传的地方看见过
    /// Kingdee.BOS.WebApi.ServicesStub.AuthService.ValidateUserEnDeCode 
    ///
    /// </summary>
    public class ValidateUserEnDeCode
    {
        /// <summary>
        /// 
        /// </summary>
        public ValidateUserEnDeCode() { }
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
        /// <param name="validateLoginSettingsModel"></param>
        /// <param name="UnsafeRelaxedJsonEscaping"></param>
        /// <returns></returns>
        public string GetLoginJson(Model.ValidateLoginSettingsModel validateLoginSettingsModel, bool UnsafeRelaxedJsonEscaping=true)
        {
            JsonSerializerOptions options = new JsonSerializerOptions();
            options.WriteIndented = false; // 设置false格式化为非缩进格式，即不保留换行符;
            if (UnsafeRelaxedJsonEscaping)
            {
                options.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
            }
            else
            {
                options.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Default;
            }
            List<object> Parameters = new List<object>();
            Parameters.Add(validateLoginSettingsModel.DbId);//帐套Id
            Parameters.Add(EnDecode.Encode(validateLoginSettingsModel.UserName));//用户名
            Parameters.Add(EnDecode.Encode( validateLoginSettingsModel.Password));//密码
            Parameters.Add(validateLoginSettingsModel.lcid);//账套语系

           
            string KdContent = System.Text.Json.JsonSerializer.Serialize(Parameters.ToArray(), options);
            string RealContent = CommonService.JsonHelperServices.getLoginRequestBodystring(KdContent, UnsafeRelaxedJsonEscaping, true);
           // string RealContent = KdContent;
            return RealContent;
        }

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

            url = Model.ValidateLoginSettingsModel.GetServerUrl(url);
            string loginapiurl = url + @"Kingdee.BOS.WebApi.ServicesStub.AuthService.ValidateUserEnDeCode.common.kdsvc";
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
    }
}
