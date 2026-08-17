using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace YiKdWebClient.AuthService
{
    /// <summary>
    /// 简易通行证/集成密钥文件方式
    /// </summary>
    public class LoginBySimplePassport
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

            url = Model.LoginBySimplePassportModel.GetServerUrl(url);
            string loginapiurl = url + @"Kingdee.BOS.WebApi.ServicesStub.AuthService.LoginBySimplePassport.common.kdsvc";
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
        /// 获取简易通行证/集成密钥文件方式的json
        /// </summary>
        /// <param name="loginBySimplePassportModel"></param>
        /// <param name="UnsafeRelaxedJsonEscaping"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public string GetLoginJson(Model.LoginBySimplePassportModel loginBySimplePassportModel, bool UnsafeRelaxedJsonEscaping)
        {

            string passportForBase64 = string.Empty;

            if (loginBySimplePassportModel.bySimplePassportType.Equals(Model.BySimplePassportType.CnfFile))
            {
                if (string.IsNullOrWhiteSpace(loginBySimplePassportModel.CnfFilePath))
                {
                    throw new Exception("CnfFile类型下,loginBySimplePassportModel.CnfFilePath必须传值");
                }
                passportForBase64 = GetPassportForBase64(loginBySimplePassportModel.CnfFilePath);
                loginBySimplePassportModel.SimplePassportForBase64 = passportForBase64;
            }
            if (loginBySimplePassportModel.bySimplePassportType.Equals(Model.BySimplePassportType.ForBase64))
            {
                if (string.IsNullOrWhiteSpace(loginBySimplePassportModel.SimplePassportForBase64)) { throw new Exception("ForBase64类型下,loginBySimplePassportModel.SimplePassportForBase64必须传值"); }
                passportForBase64 = loginBySimplePassportModel.SimplePassportForBase64;
            }

            JsonSerializerOptions options = new JsonSerializerOptions();
            options.WriteIndented = false; // 设置false 为格式化为非缩进格式，即不保留换行符;
            if (UnsafeRelaxedJsonEscaping)
            {
                options.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
            }
            else
            {
                options.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Default;
            }
            //List<object> Parameters = new List<object>();
            //Parameters.Add(passportForBase64);//密钥base64   
            //Parameters.Add(loginBySimplePassportModel);//账套语系

            object[] Parameters = [passportForBase64, loginBySimplePassportModel.Lcid];

            string KdContent = System.Text.Json.JsonSerializer.Serialize(Parameters, options);
            string RealContent = CommonService.JsonHelperServices.getLoginRequestBodystring(KdContent, UnsafeRelaxedJsonEscaping, true);

            return RealContent;
        }

        /// <summary>
        /// 获取文件的byte[]流
        /// </summary>
        /// <param name="cnffilepath"></param>
        /// <returns></returns>
        public byte[] GetCnfBytes(string cnffilepath)
        {
            byte[] passports = System.IO.File.ReadAllBytes(cnffilepath);

            return passports;
        }
        /// <summary>
        /// 获取文件的Base64
        /// </summary>
        /// <param name="cnffilepath"></param>
        /// <returns></returns>
        public string GetPassportForBase64(string cnffilepath)
        {
            byte[] passports = GetCnfBytes(cnffilepath);
            string passportForBase64 = Convert.ToBase64String(passports);
            return passportForBase64;
        }




    }
}
