
using System.ComponentModel;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace YiKdWebClient
{
#pragma warning disable CS0618 // 类型或成员已过时
    /// <summary>
    /// 
    /// </summary>
    public class YiK3CloudClient:IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        public YiK3CloudClient() { initialize(); }

        /// <summary>
        /// 最后一个启动的 YiK3CloudClient实例
        /// </summary>
        public static YiK3CloudClient? Instance { get; set; }
        private void initialize()
        {

            //  RealRequestBody= Services.JsonHelperServices.getRequestBodystring(Formid,Jsoninput,UnsafeRelaxedJsonEscaping);

        }
        /// <summary>
        /// Cookie临时存放器
        /// </summary>
       
        [Description("Cookie临时存放器")]
        //[EditorBrowsable(EditorBrowsableState.Always)] // 这个方法将始终在 IntelliSense 中显示
       // [EditorBrowsable(EditorBrowsableState.Never)] // 这个方法将不会在 IntelliSense 中显示 
        public CookieContainer Cookie { get; set; } = new CookieContainer();
        /// <summary>
        /// 请求头存放
        /// </summary>
        public Dictionary<string, string> RequestHeaders { get; set; } = new Dictionary<string, string>();
        /// <summary>
        /// 请求头字符串
        /// </summary>
        public string RequestHeadersString { get; set; } =string.Empty;
        /// <summary>
        /// 登录验证类型
        /// </summary>
        public Model.LoginType?  LoginType { get; set; } = Model.LoginType.LoginByAppSecret;
        /// <summary>
        /// AppSettings的设置
        /// </summary>
        public Model.AppSettingsModel AppSettingsModel { get; set; } = new Model.AppSettingsModel();
        /// <summary>
        /// 登录验证真正完整的请求和返回报文
        /// </summary>
        public Model.RequestWebModel ReturnLoginWebModel { get; set; } = new Model.RequestWebModel();
        /// <summary>
        /// 执行本次操作接口的真实请求和返回值
        /// </summary>
        public Model.RequestWebModel ReturnOperationWebModel { get; set; } = new Model.RequestWebModel();


        /// <summary>
        /// 获取对编码内容不太严格的内置 JavaScript 编码器实例 
        /// https://learn.microsoft.com/zh-cn/dotnet/api/system.text.encodings.web.javascriptencoder.unsaferelaxedjsonescaping?view=net-8.0#system-text-encodings-web-javascriptencoder-unsaferelaxedjsonescaping
        /// </summary>
        public bool UnsafeRelaxedJsonEscaping { get; set; } = true;

        /// <summary>
        /// 请求超时时间
        /// </summary>
        public TimeSpan Timeout { get; set; } = TimeSpan.FromSeconds(60);

        //TimeSpan timeSpan = new TimeSpan(0, 0, 10);
        //TimeSpan timeSpan = TimeSpan.FromSeconds(10);
        //   public TimeSpan Timeout = System.Threading.Timeout.InfiniteTimeSpan;

        private string ExecServicesStubByformid(string formid, string json, string ServicesStubpath, string opNumber = "",bool userawjson=false)
        {
            if (LoginType==null)
            {
                throw new Exception("LoginType is not null");
            }
            string apiurl = string.Empty;
            //string apiurl = this.AppSettingsModel.XKDApiServerUrl + ServicesStubpath;

            if (this.LoginType.Equals(Model.LoginType.LoginByAppSecret))
            {
                apiurl = this.AppSettingsModel.XKDApiServerUrl + ServicesStubpath;
            }
            if (this.LoginType.Equals(Model.LoginType.LoginByApiSignHeaders))
            {
                apiurl = this.AppSettingsModel.XKDApiServerUrl + ServicesStubpath;
            }
            if (this.LoginType.Equals(Model.LoginType.ValidateLogin))
            {
                apiurl = this.validateLoginSettingsModel!.Url + ServicesStubpath;
            }
            if (this.LoginType.Equals(Model.LoginType.LoginBySimplePassport))
            {
                apiurl = this.LoginBySimplePassportModel!.Url + ServicesStubpath;
            }

            if (this.LoginType.Equals(Model.LoginType.ValidateUserEnDeCode))
            {
                apiurl = this.validateLoginSettingsModel!.Url + ServicesStubpath;
            }

            if (this.LoginType.Equals(Model.LoginType.LoginBySignSHA256)|| this.LoginType.Equals(Model.LoginType.LoginBySignSHA1))
            {
                apiurl = this.AppSettingsModel.XKDApiServerUrl + ServicesStubpath;
            }
            string resjson = string.Empty;


            #region 执行操作
            CommonService.WebHelperServices webHelperServices = new CommonService.WebHelperServices();


            if (this.LoginType.Equals(Model.LoginType.LoginByApiSignHeaders))
            {
                Dictionary<string, string> ApiHeaders = AuthService.LoginByApiSignHeaders.GetApiHeaders(this.AppSettingsModel, new Uri(apiurl));
                this.RequestHeaders = ApiHeaders;
                this.RequestHeadersString = AuthService.LoginByApiSignHeaders.GetApiHeadersStr(ApiHeaders);
            }
            else { webHelperServices.cookies = this.Cookie; }

            
            webHelperServices.Timeout = this.Timeout;
            webHelperServices.RequestHeaders = this.RequestHeaders;
          
            this.ReturnOperationWebModel.RequestUrl = apiurl;

            string jsonString=string.Empty;
            if (userawjson) { jsonString = json; }
            else
            { 
             jsonString = CommonService.JsonHelperServices.getRequestBodystring(formid, json, UnsafeRelaxedJsonEscaping, opNumber);
            }
            this.ReturnOperationWebModel.RealRequestBody = jsonString;


            Task<string> postTask = webHelperServices.SendHttpRequestAsync(apiurl, jsonString);
            postTask.Wait(); // 阻塞直到任务完成 
            resjson = postTask.Result;
            this.ReturnOperationWebModel.RealResponseBody = resjson;
            if (this.LoginType.Equals(Model.LoginType.LoginByApiSignHeaders))
            {
                this.Cookie = webHelperServices.cookies;
            }
           
            #endregion
            return resjson;
        }

        /// <summary>
        /// 根据ServicesStubpath接口服务地址调用例如:Kingdee.BOS.WebApi.ServicesStub.DynamicFormService.Allocate.common.kdsvc
        /// </summary>
        /// <param name="formid"></param>
        /// <param name="json"></param>
        /// <param name="ServicesStubpath">接口服务地址，k3cloud/后面的部分</param>
        /// <param name="AutoLogin"></param>
        /// <param name="AutoLogout"></param>
        ///  <param name="userawjson"></param>
        /// <returns></returns>
        public string ExecApiDynamicFormService(string formid, string json, string ServicesStubpath, bool AutoLogin = true, bool AutoLogout = true,bool userawjson=false)
        {
            Instance = this;
            string resjson = string.Empty;
            #region 校验登录是否成功

            if (this.LoginType.Equals(Model.LoginType.LoginByApiSignHeaders))
            {

            }
            else {
                string LoginResultType = string.Empty;
                try
                {
                    if (AutoLogin)
                    {
                        this.Login();
                    }
                    try
                    {
                        System.Text.Json.Nodes.JsonNode JsonNodes = System.Text.Json.Nodes.JsonNode.Parse(this.ReturnLoginWebModel.RealResponseBody)!;
                        LoginResultType = Convert.ToString(JsonNodes["LoginResultType"])!;
                    }
                    catch (Exception)
                    {

                        //throw;
                    }
                   

                }
                catch (Exception ex)
                {
                    return ex.Message;
                    // throw;
                }
                if (!"1".Equals(LoginResultType, StringComparison.OrdinalIgnoreCase))
                {
                    return this.ReturnLoginWebModel.RealResponseBody;
                }
            }
           
            #endregion
       

            try
            {
                //resjson = ExecServicesStubByformid(formid, json, @"Kingdee.BOS.WebApi.ServicesStub.DynamicFormService.View.common.kdsvc");
                resjson = ExecServicesStubByformid(formid, json, ServicesStubpath,"", userawjson);
                if (AutoLogout) { Logout(); }
            }
            catch (Exception ex)
            {
                resjson = ex.Message;
                //  throw;
            }
            return resjson;
        }
        /// <summary>
        /// 登录
        /// </summary>
        /// <returns></returns>
        public Model.RequestWebModel Login()
        {
            Model.RequestWebModel requestWebModel = new Model.RequestWebModel();

            if (LoginType == null)
            {
                throw new Exception("LoginType is not null");
            }

            if (LoginType.Equals(Model.LoginType.LoginByAppSecret))
            {
                AuthService.LoginByAppSecret loginByAppSecret = new AuthService.LoginByAppSecret();
                loginByAppSecret.Timeout = this.Timeout;
                loginByAppSecret.RequestHeaders = this.RequestHeaders;
                string jsonString = loginByAppSecret.GetLoginJson(AppSettingsModel, UnsafeRelaxedJsonEscaping);
                ReturnLoginWebModel.RealRequestBody = jsonString;
                requestWebModel = loginByAppSecret.Login(AppSettingsModel.XKDApiServerUrl, jsonString, true);
            }
       

            if (LoginType.Equals(Model.LoginType.LoginByApiSignHeaders))
            {
                return requestWebModel;
            }

            if (LoginType.Equals(Model.LoginType.ValidateLogin))
            {
                if (validateLoginSettingsModel == null)
                {
                    throw new Exception("LoginType使用ValidateLogin方式的时候，validateLoginSettingsModel 需要实例化赋值");
                }
                if (string.IsNullOrWhiteSpace(validateLoginSettingsModel.Url))
                {
                    throw new Exception("LoginType使用ValidateLogin方式的时候，validateLoginSettingsModel.Url需要赋值");
                }
                AuthService.ValidateLogin validateLogin = new AuthService.ValidateLogin();
                validateLogin.Timeout = this.Timeout;
                validateLogin.RequestHeaders = this.RequestHeaders;
                string jsonString = validateLogin.GetLoginJson(validateLoginSettingsModel, UnsafeRelaxedJsonEscaping);
                ReturnLoginWebModel.RealRequestBody = jsonString;
                requestWebModel = validateLogin.Login(this.validateLoginSettingsModel.Url!, jsonString, true);

            }

            if (LoginType.Equals(Model.LoginType.LoginBySimplePassport))
            {
                if (LoginBySimplePassportModel==null)
                {
                    throw new Exception("LoginType使用LoginBySimplePassport方式的时候，LoginBySimplePassportModel需要实例化赋值");
                }
                if (string.IsNullOrWhiteSpace(LoginBySimplePassportModel.Url))
                {
                    throw new Exception("LoginType使用LoginBySimplePassport方式的时候，LoginBySimplePassportModel.Url需要赋值");
                }
                AuthService.LoginBySimplePassport loginBySimplePassport=new AuthService.LoginBySimplePassport();
                loginBySimplePassport.Timeout = this.Timeout;
                loginBySimplePassport.RequestHeaders = this.RequestHeaders;
                string jsonString = loginBySimplePassport.GetLoginJson(this.LoginBySimplePassportModel, UnsafeRelaxedJsonEscaping);
                ReturnLoginWebModel.RealRequestBody = jsonString;
                requestWebModel = loginBySimplePassport.Login(this.LoginBySimplePassportModel.Url, jsonString, true);

            }

            if (LoginType.Equals(Model.LoginType.ValidateUserEnDeCode))
            {
                if (validateLoginSettingsModel == null)
                {
                    throw new Exception("LoginType使用ValidateUserEnDeCode方式的时候，validateLoginSettingsModel 需要实例化赋值");
                }
                if (string.IsNullOrWhiteSpace(validateLoginSettingsModel.Url))
                {
                    throw new Exception("LoginType使用ValidateUserEnDeCode方式的时候，validateLoginSettingsModel.Url需要赋值");
                }
                AuthService.ValidateUserEnDeCode validateUserEnDeCode = new AuthService.ValidateUserEnDeCode();
                validateUserEnDeCode.Timeout = this.Timeout;
                validateUserEnDeCode.RequestHeaders = this.RequestHeaders;
                string jsonString = validateUserEnDeCode.GetLoginJson(validateLoginSettingsModel, UnsafeRelaxedJsonEscaping);
                ReturnLoginWebModel.RealRequestBody = jsonString;
                requestWebModel = validateUserEnDeCode.Login(this.validateLoginSettingsModel.Url!, jsonString, true);

            }

            if (this.LoginType.Equals(Model.LoginType.LoginBySignSHA256) || this.LoginType.Equals(Model.LoginType.LoginBySignSHA1))
            {
                AuthService.LoginBySign loginBySign = new AuthService.LoginBySign();
                loginBySign.LoginType = (Model.LoginType)this.LoginType;
                loginBySign.Timeout = this.Timeout;
                loginBySign.RequestHeaders = this.RequestHeaders;
                string jsonString = loginBySign.GetLoginJson(AppSettingsModel, UnsafeRelaxedJsonEscaping);
                ReturnLoginWebModel.RealRequestBody = jsonString;
                requestWebModel = loginBySign.Login(AppSettingsModel.XKDApiServerUrl, jsonString, true);
            }

            Cookie = requestWebModel.Cookie;
            this.ReturnLoginWebModel = requestWebModel;


            return requestWebModel;

        }
        /// <summary>
        /// 登出
        /// </summary>
        public void Logout()

        {
            try
            {

                string resjson = string.Empty;

                CommonService.WebHelperServices webHelperServices = new CommonService.WebHelperServices();
                webHelperServices.cookies = this.Cookie;
                webHelperServices.Timeout = this.Timeout;
                webHelperServices.RequestHeaders = this.RequestHeaders;
                string apiurl = this.AppSettingsModel.XKDApiServerUrl + "Kingdee.BOS.WebApi.ServicesStub.AuthService.Logout.common.kdsvc";

                Task<string> postTask = webHelperServices.SendHttpRequestAsync(apiurl);
                postTask.Wait(); // 阻塞直到任务完成 
                resjson = postTask.Result;

            }
            catch (Exception)
            {

                //  throw;
            }
        }

        /// <summary>
        /// 通用操作接口
        /// </summary>
        /// <param name="opNumber"></param>
        /// <param name="formid"></param>
        /// <param name="json"></param>
        /// <param name="AutoLogin"></param>
        /// <param name="AutoLogout"></param>
        /// <returns></returns>
        public string ExecuteOperation(string opNumber, string formid, string json, bool AutoLogin = true, bool AutoLogout = true)
        {
            string resjson = string.Empty;
            #region 校验登录是否成功旧版
            //string LoginResultType = string.Empty;
            //try
            //{
            //    if (AutoLogin)
            //    {
            //        this.Login();
            //    }
            //    try
            //    {
            //        System.Text.Json.Nodes.JsonNode JsonNodes = System.Text.Json.Nodes.JsonNode.Parse(this.ReturnLoginWebModel.RealResponseBody)!;
            //        LoginResultType = Convert.ToString(JsonNodes["LoginResultType"])!;
            //    }
            //    catch (Exception)
            //    {

            //      //  throw;
            //    }
            //if (!"1".Equals(LoginResultType, StringComparison.OrdinalIgnoreCase))
            //{
            //    return this.ReturnLoginWebModel.RealResponseBody;
            //}


            //}
            //catch (Exception ex)
            //{
            //    return ex.Message;
            //    // throw;
            //}
            #endregion

            #region 校验登录是否成功

            if (this.LoginType.Equals(Model.LoginType.LoginByApiSignHeaders))
            {

            }
            else
            {
                string LoginResultType = string.Empty;
                try
                {
                    if (AutoLogin)
                    {
                        this.Login();
                    }
                    try
                    {
                        System.Text.Json.Nodes.JsonNode JsonNodes = System.Text.Json.Nodes.JsonNode.Parse(this.ReturnLoginWebModel.RealResponseBody)!;
                        LoginResultType = Convert.ToString(JsonNodes["LoginResultType"])!;
                    }
                    catch (Exception)
                    {

                        //throw;
                    }


                }
                catch (Exception ex)
                {
                    return ex.Message;
                    // throw;
                }
                if (!"1".Equals(LoginResultType, StringComparison.OrdinalIgnoreCase))
                {
                    return this.ReturnLoginWebModel.RealResponseBody;
                }
            }

            #endregion


            try
            {
                resjson = ExecServicesStubByformid(formid, json, @"Kingdee.BOS.WebApi.ServicesStub.DynamicFormService.ExecuteOperation.common.kdsvc", opNumber);
                if (AutoLogout) { Logout(); }
            }
            catch (Exception ex)
            {
                resjson = ex.Message;
                //  throw;
            }
            return resjson;

        }

        /// <summary>
        /// 查看表单数据接口
        /// </summary>
        /// <param name="formid"></param>
        /// <param name="json"></param>
        /// <param name="AutoLogin"></param>
        /// <param name="AutoLogout"></param>
        /// <returns></returns>
        public string View(string formid, string json, bool AutoLogin = true, bool AutoLogout = true)
        {
            return ExecApiDynamicFormService(formid, json, @"Kingdee.BOS.WebApi.ServicesStub.DynamicFormService.View.common.kdsvc", AutoLogin, AutoLogout);

        }

        /// <summary>
        ///保存表单数据接口
        /// </summary>
        /// <param name="formid"></param>
        /// <param name="json"></param>
        /// <param name="AutoLogin"></param>
        /// <param name="AutoLogout"></param>
        /// <returns></returns>
        public string Save(string formid, string json, bool AutoLogin = true, bool AutoLogout = true)
        {
            return ExecApiDynamicFormService(formid, json, @"Kingdee.BOS.WebApi.ServicesStub.DynamicFormService.Save.common.kdsvc", AutoLogin, AutoLogout);

        }

        /// <summary>
        /// 批量保存表单数据接口
        /// </summary>
        /// <param name="formid"></param>
        /// <param name="json"></param>
        /// <param name="AutoLogin"></param>
        /// <param name="AutoLogout"></param>
        /// <returns></returns>
        public string BatchSave(string formid, string json, bool AutoLogin = true, bool AutoLogout = true)
        {
            return ExecApiDynamicFormService(formid, json, @"Kingdee.BOS.WebApi.ServicesStub.DynamicFormService.BatchSave.common.kdsvc", AutoLogin, AutoLogout);

        }

        /// <summary>
        /// 提交表单数据接口
        /// </summary>
        /// <param name="formid"></param>
        /// <param name="json"></param>
        /// <param name="AutoLogin"></param>
        /// <param name="AutoLogout"></param>
        /// <returns></returns>
        public string Submit(string formid, string json, bool AutoLogin = true, bool AutoLogout = true)
        {
            return ExecApiDynamicFormService(formid, json, @"Kingdee.BOS.WebApi.ServicesStub.DynamicFormService.Submit.common.kdsvc", AutoLogin, AutoLogout);

        }

        /// <summary>
        /// 审核表单数据接口
        /// </summary>
        /// <param name="formid"></param>
        /// <param name="json"></param>
        /// <param name="AutoLogin"></param>
        /// <param name="AutoLogout"></param>
        /// <returns></returns>
        public string Audit(string formid, string json, bool AutoLogin = true, bool AutoLogout = true)
        {
            return ExecApiDynamicFormService(formid, json, @"Kingdee.BOS.WebApi.ServicesStub.DynamicFormService.Audit.common.kdsvc", AutoLogin, AutoLogout);

        }
        /// <summary>
        /// 反审核表单数据接口
        /// </summary>
        /// <param name="formid"></param>
        /// <param name="json"></param>
        /// <param name="AutoLogin"></param>
        /// <param name="AutoLogout"></param>
        /// <returns></returns>
        public string UnAudit(string formid, string json, bool AutoLogin = true, bool AutoLogout = true)
        {
            return ExecApiDynamicFormService(formid, json, @"Kingdee.BOS.WebApi.ServicesStub.DynamicFormService.UnAudit.common.kdsvc", AutoLogin, AutoLogout);

        }
        /// <summary>
        /// 删除表单数据接口
        /// </summary>
        /// <param name="formid"></param>
        /// <param name="json"></param>
        /// <param name="AutoLogin"></param>
        /// <param name="AutoLogout"></param>
        /// <returns></returns>
        public string Delete(string formid, string json, bool AutoLogin = true, bool AutoLogout = true)
        {
            return ExecApiDynamicFormService(formid, json, @"Kingdee.BOS.WebApi.ServicesStub.DynamicFormService.Delete.common.kdsvc", AutoLogin, AutoLogout);

        }

        /// <summary>
        /// 表单数据查询接口
        /// </summary>
        /// <param name="json"></param>
        /// <param name="AutoLogin"></param>
        /// <param name="AutoLogout"></param>
        /// <returns></returns>
        public string ExecuteBillQuery(string json, bool AutoLogin = true, bool AutoLogout = true)
        {
            return ExecApiDynamicFormService("", json, @"Kingdee.BOS.WebApi.ServicesStub.DynamicFormService.ExecuteBillQuery.common.kdsvc", AutoLogin, AutoLogout);

        }
        /// <summary>
        ///自定义WebAPI接口,直接接收到原始发送的json
        /// </summary>
        /// <param name="json"></param>
        /// <param name="apiname">接口命名空间.接口实现类名.方法,组件名.common.kdsvc
        /// 例:"http://192.168.66.60/k3cloud/Kingdee.K3Erp.WebAPI.ServiceExtend.ServicesStub.CustomBusinessService.ExecuteService.common.kdsvc"
        /// </param>
        /// <param name="AutoLogin"></param>
        /// <param name="AutoLogout"></param>
        /// <returns></returns>
        public string CustomBusinessService(string json, string apiname, bool AutoLogin = true, bool AutoLogout = true)
        {

            apiname = EnsureSuffixServicesStub(apiname);
            return ExecApiDynamicFormService("",json, apiname, AutoLogin, AutoLogout);

        }
        /// <summary>
        /// 自定义WebAPI接口,直接接收到原始发送的json
        /// </summary>
        /// <param name="json"></param>
        /// <param name="customServicesStubpath"></param>
        /// <param name="AutoLogin"></param>
        /// <param name="AutoLogout"></param>
        /// <returns></returns>
        public string CustomBusinessService(string json, Model.CustomServicesStubpath customServicesStubpath, bool AutoLogin = true, bool AutoLogout = true)
        {

            string apiname = customServicesStubpath.GetCustomServicesStubpathUrl();
            return ExecApiDynamicFormService("", json, apiname, AutoLogin, AutoLogout);

        }

        /// <summary>
        /// 自定义WebAPI接口,直接接受parameters数组的参数值
        /// </summary>
        /// <param name="json"></param>
        /// <param name="apiname"></param>
        /// <param name="AutoLogin"></param>
        /// <param name="AutoLogout"></param>
        /// <returns></returns>
        public string CustomBusinessServiceByParameters(string json, string apiname, bool AutoLogin = true, bool AutoLogout = true)
        {
            apiname = EnsureSuffixServicesStub(apiname);
            return ExecApiDynamicFormService("", json, apiname, AutoLogin, AutoLogout,true);

        }

        /// <summary>
        /// 自定义WebAPI接口,直接接受parameters数组的参数值
        /// </summary>
        /// <param name="json"></param>
        /// <param name="customServicesStubpath"></param>
        /// <param name="AutoLogin"></param>
        /// <param name="AutoLogout"></param>
        /// <returns></returns>
        public string CustomBusinessServiceByParameters(string json, Model.CustomServicesStubpath customServicesStubpath, bool AutoLogin = true, bool AutoLogout = true)
        {
             string apiname = customServicesStubpath.GetCustomServicesStubpathUrl();
            return ExecApiDynamicFormService("", json, apiname, AutoLogin, AutoLogout, true);

        }


        /// <summary>
        /// 暂存表单数据接口
        /// </summary>
        /// <param name="formid"></param>
        /// <param name="json"></param>
        /// <param name="AutoLogin"></param>
        /// <param name="AutoLogout"></param>
        /// <returns></returns>
        public string Draft(string formid, string json, bool AutoLogin = true, bool AutoLogout = true)
        {
            return ExecApiDynamicFormService(formid, json, @"Kingdee.BOS.WebApi.ServicesStub.DynamicFormService.Draft.common.kdsvc", AutoLogin, AutoLogout);

        }
        /// <summary>
        /// 分配表单数据接口
        /// </summary>
        /// <param name="formid"></param>
        /// <param name="json"></param>
        /// <param name="AutoLogin"></param>
        /// <param name="AutoLogout"></param>
        /// <returns></returns>
        public string Allocate(string formid, string json, bool AutoLogin = true, bool AutoLogout = true)
        {
            return ExecApiDynamicFormService(formid, json, @"Kingdee.BOS.WebApi.ServicesStub.DynamicFormService.Allocate.common.kdsvc", AutoLogin, AutoLogout);

        }

        /// <summary>
        /// 下推接口
        /// </summary>
        /// <param name="formid"></param>
        /// <param name="json"></param>
        /// <param name="AutoLogin"></param>
        /// <param name="AutoLogout"></param>
        /// <returns></returns>
        public string Push(string formid, string json, bool AutoLogin = true, bool AutoLogout = true)
        {
            return ExecApiDynamicFormService(formid, json, @"Kingdee.BOS.WebApi.ServicesStub.DynamicFormService.Push.common.kdsvc", AutoLogin, AutoLogout);

        }
        /// <summary>
        /// 分组保存接口
        /// </summary>
        /// <param name="formid"></param>
        /// <param name="json"></param>
        /// <param name="AutoLogin"></param>
        /// <param name="AutoLogout"></param>
        /// <returns></returns>
        public string GroupSave(string formid, string json, bool AutoLogin = true, bool AutoLogout = true)
        {
            return ExecApiDynamicFormService(formid, json, @"Kingdee.BOS.WebApi.ServicesStub.DynamicFormService.GroupSave.common.kdsvc", AutoLogin, AutoLogout);

        }
        /// <summary>
        /// 弹性域保存接口
        /// </summary>
        /// <param name="formid"></param>
        /// <param name="json"></param>
        /// <param name="AutoLogin"></param>
        /// <param name="AutoLogout"></param>
        /// <returns></returns>
        public string FlexSave(string formid, string json, bool AutoLogin = true, bool AutoLogout = true)
        {
         return ExecApiDynamicFormService(formid, json, @"Kingdee.BOS.WebApi.ServicesStub.DynamicFormService.FlexSave.common.kdsvc", AutoLogin, AutoLogout);
        
        }
        /// <summary>
        ///发送消息接口
        /// </summary>
        /// <param name="json"></param>
        /// <param name="AutoLogin"></param>
        /// <param name="AutoLogout"></param>
        /// <returns></returns>
        public string SendMsg(string json, bool AutoLogin = true, bool AutoLogout = true)
        {
            return ExecApiDynamicFormService("",json, @"Kingdee.BOS.WebApi.ServicesStub.DynamicFormService.SendMsg.common.kdsvc", AutoLogin, AutoLogout);
           
        }
        /// <summary>
        /// 切换上下文默认组织接口
        /// </summary>
        /// <param name="json"></param>
        /// <param name="AutoLogin"></param>
        /// <param name="AutoLogout"></param>
        /// <returns></returns>
        public string SwitchOrg(string json, bool AutoLogin = true, bool AutoLogout = true)
        {
            return ExecApiDynamicFormService("", json, @"Kingdee.BOS.WebApi.ServicesStub.DynamicFormService.SwitchOrg.common.kdsvc", AutoLogin, AutoLogout);

        }
        /// <summary>
        /// 工作流审批接口
        /// </summary>
        /// <param name="json"></param>
        /// <param name="AutoLogin"></param>
        /// <param name="AutoLogout"></param>
        /// <returns></returns>
        public string WorkflowAudit(string json, bool AutoLogin = true, bool AutoLogout = true)
        {
            return ExecApiDynamicFormService("", json, @"Kingdee.BOS.WebApi.ServicesStub.DynamicFormService.WorkflowAudit.common.kdsvc", AutoLogin, AutoLogout);
        }
        /// <summary>
        /// 简单账表查询接口
        /// </summary>
        /// <param name="formid"></param>
        /// <param name="json"></param>
        /// <param name="AutoLogin"></param>
        /// <param name="AutoLogout"></param>
        /// <returns></returns>
        public string GetSysReportData(string formid, string json, bool AutoLogin = true, bool AutoLogout = true)
        {
            return ExecApiDynamicFormService(formid, json, @"Kingdee.BOS.WebApi.ServicesStub.DynamicFormService.GetSysReportData.common.kdsvc", AutoLogin, AutoLogout);

        }

        /// <summary>
        /// 附件上传接口(上传附件并绑定单据)
        /// </summary>
        /// <param name="json"></param>
        /// <param name="AutoLogin"></param>
        /// <param name="AutoLogout"></param>
        /// <returns></returns>
        public string AttachmentUpLoad(string json, bool AutoLogin = true, bool AutoLogout = true)
        {
            return ExecApiDynamicFormService("", json, @"Kingdee.BOS.WebApi.ServicesStub.DynamicFormService.AttachmentUpLoad.common.kdsvc", AutoLogin, AutoLogout,true);
        }

        /// <summary>
        /// 附件下载接口
        /// </summary>
        /// <param name="json"></param>
        /// <param name="AutoLogin"></param>
        /// <param name="AutoLogout"></param>
        /// <returns></returns>
        public string AttachmentDownLoad(string json, bool AutoLogin = true, bool AutoLogout = true)
        {
            return ExecApiDynamicFormService("", json, @"Kingdee.BOS.WebApi.ServicesStub.DynamicFormService.AttachmentDownLoad.common.kdsvc", AutoLogin, AutoLogout,true);
        }

        /// <summary>
        /// 文件上传接口(不绑定单据)
        /// </summary>
        /// <param name="json"></param>
        /// <param name="AutoLogin"></param>
        /// <param name="AutoLogout"></param>
        /// <returns></returns>
        public string UploadFile(string json, bool AutoLogin = true, bool AutoLogout = true)
        {
            return ExecApiDynamicFormService("", json, @"Kingdee.BOS.WebApi.ServicesStub.DynamicFormService. UploadFile.common.kdsvc", AutoLogin, AutoLogout, true);
        }


        /// <summary>
        /// 分组删除
        /// </summary>
        /// <param name="json"></param>
        /// <param name="AutoLogin"></param>
        /// <param name="AutoLogout"></param>
        /// <returns></returns>
        public string GroupDelete(string json, bool AutoLogin = true, bool AutoLogout = true)
        {
            return ExecApiDynamicFormService("", json, @"Kingdee.BOS.WebApi.ServicesStub.DynamicFormService.GroupDelete.common.kdsvc", AutoLogin, AutoLogout);
        }
        /// <summary>
        /// 取消分配
        /// </summary>
        /// <param name="formid"></param>
        /// <param name="json"></param>
        /// <param name="AutoLogin"></param>
        /// <param name="AutoLogout"></param>
        /// <returns></returns>
        public string CancelAllocate(string formid, string json, bool AutoLogin = true, bool AutoLogout = true)
        {
            return ExecApiDynamicFormService(formid, json, @"Kingdee.BOS.WebApi.ServicesStub.DynamicFormService.CancelAllocate.common.kdsvc", AutoLogin, AutoLogout);

        }
        /// <summary>
        /// 撤销服务接口
        /// </summary>
        /// <param name="formid"></param>
        /// <param name="json"></param>
        /// <param name="AutoLogin"></param>
        /// <param name="AutoLogout"></param>
        /// <returns></returns>
        public string CancelAssign(string formid, string json, bool AutoLogin = true, bool AutoLogout = true)
        {
            return ExecApiDynamicFormService(formid, json, @"Kingdee.BOS.WebApi.ServicesStub.DynamicFormService.CancelAssign.common.kdsvc", AutoLogin, AutoLogout);

        }

        /// <summary>
        /// 拆单
        /// </summary>
        /// <param name="formid"></param>
        /// <param name="json"></param>
        /// <param name="AutoLogin"></param>
        /// <param name="AutoLogout"></param>
        /// <returns></returns>
        public string Disassembly(string formid, string json, bool AutoLogin = true, bool AutoLogout = true)
        {
            return ExecApiDynamicFormService(formid, json, @"Kingdee.BOS.WebApi.ServicesStub.DynamicFormService.Disassembly.common.kdsvc", AutoLogin, AutoLogout);

        }
        /// <summary>
        /// 查询单据信息(元数据查询)
        /// </summary>
        /// <param name="json"></param>
        /// <param name="AutoLogin"></param>
        /// <param name="AutoLogout"></param>
        /// <returns></returns>

        public string QueryBusinessInfo( string json, bool AutoLogin = true, bool AutoLogout = true)
        {
            return ExecApiDynamicFormService("", json, @"Kingdee.BOS.WebApi.ServicesStub.DynamicFormService.QueryBusinessInfo.common.kdsvc", AutoLogin, AutoLogout);

        }
        /// <summary>
        /// 查询分组信息
        /// </summary>
        /// <param name="json"></param>
        /// <param name="AutoLogin"></param>
        /// <param name="AutoLogout"></param>
        /// <returns></returns>
        public string QueryGroupInfo(string json, bool AutoLogin = true, bool AutoLogout = true)
        {
            return ExecApiDynamicFormService("", json, @"Kingdee.BOS.WebApi.ServicesStub.DynamicFormService.QueryGroupInfo.common.kdsvc", AutoLogin, AutoLogout);

        }

        /// <summary>
        /// 获取数据中心列表
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public string GetDataCenterList(string url="") 
        {
            string resjson = string.Empty;

            try
            {



                CommonService.WebHelperServices webHelperServices = new CommonService.WebHelperServices();
                webHelperServices.cookies = this.Cookie;
                webHelperServices.Timeout = this.Timeout;
                webHelperServices.RequestHeaders = this.RequestHeaders;

                string apiurl = string.Empty;
                if (string.IsNullOrWhiteSpace(url))
                {
                     apiurl = this.AppSettingsModel.XKDApiServerUrl + "Kingdee.BOS.ServiceFacade.ServicesStub.Account.AccountService.GetDataCenterList.common.kdsvc";
                }
                else
                {
                    apiurl= GetServerUrl(url) + "Kingdee.BOS.ServiceFacade.ServicesStub.Account.AccountService.GetDataCenterList.common.kdsvc";
                }
                

                Task<string> postTask = webHelperServices.SendHttpRequestAsync(apiurl);
                postTask.Wait(); // 阻塞直到任务完成 
                resjson = postTask.Result;

            }
            catch (Exception ex)
            {
                resjson= ex.Message;

                //  throw;


            }
            return resjson;
        }

        /// <summary>
        /// 
        /// </summary>
        public Model.ValidateLoginSettingsModel? validateLoginSettingsModel { get; set; }=null;
        /// <summary>
        /// url地址补充/
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public  string GetServerUrl(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                return string.Empty;
            }

            try
            {
                if (!url.EndsWith("/"))
                {
                    return url + "/";
                }
            }
            catch (Exception)
            {

                //throw;
            }

            return url;
        }
        /// <summary>
        /// SimplePassportModel集成文件验证模型
        /// </summary>

        public Model.LoginBySimplePassportModel? LoginBySimplePassportModel { get; set; } = null;

        /// <summary>
        /// ServicesStub结尾补充
        /// </summary>
        /// <param name="input"></param>
        /// <param name="suffix"></param>
        /// <returns></returns>
        public static string EnsureSuffixServicesStub(string input, string suffix= ".common.kdsvc")
        {
            if (!input.EndsWith(suffix))
            {
                input += suffix;
            }
            return input;
        }


        #region disposed释放

        // IDisposable implementation
        private bool disposed = false;
        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {

                if (disposing)
                {
                    try
                    {
                        // 清空所有字段
                        this.LoginType = null;

                    }
                    catch (Exception)
                    {

                        // throw;
                    }



                }
                // 释放非托管资源（如果有的话）
                disposed = true;
            }
        }
        /// <summary>
        /// ~符号用于定义类的析构函数（destructor）,当垃圾回收器（garbage collector）决定释放对象时，会调用这个方法。析构函数用于执行清理操作
        /// </summary>
        ~YiK3CloudClient()
        {
            Dispose(false);
        }
        #endregion

    }
}
