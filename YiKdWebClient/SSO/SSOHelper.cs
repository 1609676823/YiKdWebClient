using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using YiKdWebClient.ComWebHelper;


namespace YiKdWebClient.SSO
{
    /// <summary>
    /// 单点登录辅助类
    /// </summary>
    public class SSOHelper
    {

        /// <summary>
        /// 单点登录辅助类
        /// </summary>      

        public SSOHelper() { InitLoginArgs(); }

        /// <summary>
        /// Unix时间戳,定义为从格林威治时间1970年01月01日00时00分00秒起
        /// </summary>
        public long? timestamp { get; set; }

        /// <summary>
        /// 请求参数（json格式）
        /// </summary>
        public string? argJosn { get; set; } = string.Empty;
        /// <summary>
        /// 参数格式化（Base64）
        /// </summary>
        public string? argJsonBase64 { get; set; } = string.Empty;


        /// <summary>
        /// 允许登录次数，0 允许重复登录 ，1 只允许登录一次
        /// </summary>
        public string? permitcount { get; set; } = string.Empty;

        /// <summary>
        /// 获取对编码内容不太严格的内置 JavaScript 编码器实例 
        /// https://learn.microsoft.com/zh-cn/dotnet/api/system.text.encodings.web.javascriptencoder.unsaferelaxedjsonescaping?view=net-8.0#system-text-encodings-web-javascriptencoder-unsaferelaxedjsonescaping
        /// </summary>
        public bool UnsafeRelaxedJsonEscaping { get; set; } = true;




        /// <summary>
        /// 单点登录参数类
        /// </summary>
        public SimplePassportLoginArg simplePassportLoginArg { get; set; } = new SimplePassportLoginArg();

        /// <summary>
        /// 系统的地址:例如【http://xxx.xxx.xxx.xxx/k3cloud/】以/结尾
        /// </summary>  
        private string? url;
        /// <summary>
        /// 系统的地址:例如【http://xxx.xxx.xxx.xxx/k3cloud/】以/结尾
        /// </summary>
        public string? Url
        {
            // 在 set 访问器中添加自定义赋值逻辑
            get
            {
                return url;
            }
            set
            {
                url = CommonFunctionHelper.GetServerUrl(value!);
            }
        }
        /// <summary>
        /// 所有登录方式的单点登录URL链接
        /// </summary>
        public SSOLoginUrlObject SSOLoginUrlObject { get; set; } = new SSOLoginUrlObject();

        /// <summary>
        /// 第三方系统登录授权配置文件的信息
        /// </summary>
        public Model.AppSettingsModel appSettingsModel = new Model.AppSettingsModel();

        /// <summary>
        /// 初始化本对象的基本参数
        /// </summary>
      
        private void InitLoginArgs()
        {
            this.simplePassportLoginArg = new SimplePassportLoginArg();
            this.url = appSettingsModel.XKDApiServerUrl;
            this.simplePassportLoginArg.dbid = appSettingsModel.XKDApiAcctID;
            this.simplePassportLoginArg.appid = appSettingsModel.XKDApiAppID;
            this.simplePassportLoginArg.username = appSettingsModel.XKDApiUserName;
            this.simplePassportLoginArg.lcid = appSettingsModel.XKDApiLCID;

        }

        /// <summary>
        /// 第三方系统单点登录V4(获取所有登录方式链接)
        /// </summary>
        /// <returns></returns>
        public SSOLoginUrlObject GetSsoUrlsV4(string usserName = "", string url = "")
        {
            
            string ServerUrl = string.Empty;
            if (string.IsNullOrWhiteSpace(url)) { ServerUrl = this.url!; }
            else 
            {
                ServerUrl=CommonFunctionHelper.GetServerUrl(url);
            }
            SSOLoginUrlObject ssoUrls = new SSOLoginUrlObject();
            timestamp = CommonFunctionHelper.GetTimestamp();

            string dbId = this.appSettingsModel.XKDApiAcctID;//数据中心ID
            if (string.IsNullOrWhiteSpace(usserName))
            {
                usserName = this.appSettingsModel.XKDApiUserName;//用户名称
            }


            string appId = this.appSettingsModel.XKDApiAppID;//第三方系统应用Id

            string appSecret = this.appSettingsModel.XKDApiAppSec;//第三方系统应用秘钥

            string[] arr = new string[] { dbId, usserName, appId, appSecret, timestamp.ToString()! };
            if (!string.IsNullOrWhiteSpace(permitcount))
            {
                arr = new string[] { dbId, usserName, appId, appSecret, timestamp.ToString()!, permitcount! };

                this.simplePassportLoginArg.otherargs = string.Format("|{{\'permitcount\':'{0}'}}", permitcount);
            }

            Array.Sort(arr, StringComparer.Ordinal);

            string sortdata = string.Join(string.Empty, arr);

            string sign = CommonFunctionHelper.Sha256Hex(sortdata);//签名V4 签名算法使用自己语言的sha256算法即可

            this.url = appSettingsModel.XKDApiServerUrl;
            this.simplePassportLoginArg.dbid = appSettingsModel.XKDApiAcctID;
            this.simplePassportLoginArg.appid = appSettingsModel.XKDApiAppID;
            this.simplePassportLoginArg.username = appSettingsModel.XKDApiUserName;
            this.simplePassportLoginArg.lcid = appSettingsModel.XKDApiLCID;
            this.simplePassportLoginArg.signeddata = sign;
            this.simplePassportLoginArg.timestamp = timestamp.ToString();
            this.simplePassportLoginArg.username = usserName;

            var options = new JsonSerializerOptions();
            options.WriteIndented = false; // 设置false格式化为非缩进格式，即不保留换行符;

            if (UnsafeRelaxedJsonEscaping)
            {
                options.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
            }
            else
            {
                options.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Default;
            }

            argJosn = System.Text.Json.JsonSerializer.Serialize(simplePassportLoginArg, options); ;//json格式

            argJsonBase64 = System.Text.UTF8Encoding.UTF8.GetBytes(argJosn).ToBase64();//base64编码

            // var argJsonBase641 = Convert.ToBase64String(System.Text.UTF8Encoding.UTF8.GetBytes(argJosn));//base64编码

            string silverlightUrl = this.Url + "Silverlight/index.aspx?ud=" + argJsonBase64;// Silverlight入口链接
            ssoUrls.silverlightUrl = silverlightUrl;
            string html5Url = this.Url + "html5/index.aspx?ud=" + argJsonBase64;// html5入口链接
            ssoUrls.html5Url = html5Url;

            Uri uri = new Uri(this.Url!);

            string wpfUrl = string.Format(@"K3cloud://{1}/k3cloud/Clientbin/K3cloudclient/K3cloudclient.manifest?Lcid=2052&ExeType=WPFRUNTIME&LoginUrl={0}&ud=", this.Url, uri.Host + ":" + uri.Port) + argJsonBase64;
            ssoUrls.wpfUrl = wpfUrl;

            SSOLoginUrlObject = ssoUrls;
            return SSOLoginUrlObject;
        }
        /// <summary>
        /// 第三方系统单点登录V3(获取所有登录方式链接)
        /// </summary>
        /// <param name="usserName"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public SSOLoginUrlObject GetSsoUrlsV3(string usserName = "", string url = "")
        {
            string ServerUrl = string.Empty;
            if (string.IsNullOrWhiteSpace(url)) { ServerUrl = this.url!; }
            else
            {
                ServerUrl = CommonFunctionHelper.GetServerUrl(url);
            }
            SSOLoginUrlObject ssoUrls = new SSOLoginUrlObject();
            timestamp = CommonFunctionHelper.GetTimestamp();

            string dbId = this.appSettingsModel.XKDApiAcctID;//数据中心ID
            if (string.IsNullOrWhiteSpace(usserName))
            {
                usserName = this.appSettingsModel.XKDApiUserName;//用户名称
            }


            string appId = this.appSettingsModel.XKDApiAppID;//第三方系统应用Id

            string appSecret = this.appSettingsModel.XKDApiAppSec;//第三方系统应用秘钥

            string[] arr = new string[] { dbId, usserName, appId, appSecret, timestamp.ToString()! };
            if (!string.IsNullOrWhiteSpace(permitcount))
            {
                arr = new string[] { dbId, usserName, appId, appSecret, timestamp.ToString()!, permitcount! };

                this.simplePassportLoginArg.otherargs = string.Format("|{{\'permitcount\':'{0}'}}", permitcount);
            }

            //Array.Sort(arr, StringComparer.Ordinal);

            //string sortdata = string.Join(string.Empty, arr);

            string sign = CommonFunctionHelper.GetSignatureSHA1Util(arr);//签名 V3

            this.url = appSettingsModel.XKDApiServerUrl;
            this.simplePassportLoginArg.dbid = appSettingsModel.XKDApiAcctID;
            this.simplePassportLoginArg.appid = appSettingsModel.XKDApiAppID;
            this.simplePassportLoginArg.username = appSettingsModel.XKDApiUserName;
            this.simplePassportLoginArg.lcid = appSettingsModel.XKDApiLCID;

            this.simplePassportLoginArg.signeddata = sign;
            this.simplePassportLoginArg.timestamp = timestamp.ToString();
            this.simplePassportLoginArg.username = usserName;

            var options = new JsonSerializerOptions();
            options.WriteIndented = false; // 设置false格式化为非缩进格式，即不保留换行符;

            if (UnsafeRelaxedJsonEscaping)
            {
                options.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
            }
            else
            {
                options.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Default;
            }

            argJosn = System.Text.Json.JsonSerializer.Serialize(simplePassportLoginArg, options); ;//json格式

            argJsonBase64 = System.Text.UTF8Encoding.UTF8.GetBytes(argJosn).ToBase64();//base64编码

            // var argJsonBase641 = Convert.ToBase64String(System.Text.UTF8Encoding.UTF8.GetBytes(argJosn));//base64编码

            string silverlightUrl = this.Url + "Silverlight/index.aspx?ud=" + argJsonBase64;// Silverlight入口链接
            ssoUrls.silverlightUrl = silverlightUrl;
            string html5Url = this.Url + "html5/index.aspx?ud=" + argJsonBase64;// html5入口链接
            ssoUrls.html5Url = html5Url;

            Uri uri = new Uri(this.Url!);

            string wpfUrl = string.Format(@"K3cloud://{1}/k3cloud/Clientbin/K3cloudclient/K3cloudclient.manifest?Lcid=2052&ExeType=WPFRUNTIME&LoginUrl={0}&ud=", this.Url, uri.Host + ":" + uri.Port) + argJsonBase64;
            ssoUrls.wpfUrl = wpfUrl;

            SSOLoginUrlObject = ssoUrls;
            return SSOLoginUrlObject;
        }

        /// <summary>
        /// 第三方系统单点登录V2(获取所有登录方式链接)
        /// </summary>
        /// <param name="usserName"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public SSOLoginUrlObject GetSsoUrlsV2(string usserName = "", string url = "")
        {
            string ServerUrl = string.Empty;
            if (string.IsNullOrWhiteSpace(url)) { ServerUrl = this.url!; }
            else
            {
                ServerUrl = CommonFunctionHelper.GetServerUrl(url);
            }
            SSOLoginUrlObject ssoUrls = new SSOLoginUrlObject();
            timestamp = CommonFunctionHelper.GetTimestamp();

            string dbId = this.appSettingsModel.XKDApiAcctID;//数据中心ID
            if (string.IsNullOrWhiteSpace(usserName))
            {
                usserName = this.appSettingsModel.XKDApiUserName;//用户名称
            }


            string appId = this.appSettingsModel.XKDApiAppID;//第三方系统应用Id

            string appSecret = this.appSettingsModel.XKDApiAppSec;//第三方系统应用秘钥

            string[] arr = new string[] { dbId, usserName, appId, appSecret, timestamp.ToString()! };
            //if (!string.IsNullOrWhiteSpace(permitcount))
            //{
            //    arr = new string[] { dbId, usserName, appId, appSecret, timestamp.ToString(), permitcount };

            //    this.simplePassportLoginArg.otherargs = string.Format("|{{\'permitcount\':'{0}'}}", permitcount);
            //}

            //Array.Sort(arr, StringComparer.Ordinal);

            //string sortdata = string.Join(string.Empty, arr);

            string sign = CommonFunctionHelper.GetSignatureSHA1Util(arr);//签名  V2


            this.url = appSettingsModel.XKDApiServerUrl;
            this.simplePassportLoginArg.dbid = appSettingsModel.XKDApiAcctID;
            this.simplePassportLoginArg.appid = appSettingsModel.XKDApiAppID;
            this.simplePassportLoginArg.username = appSettingsModel.XKDApiUserName;
            this.simplePassportLoginArg.lcid = appSettingsModel.XKDApiLCID;

            this.simplePassportLoginArg.signeddata = sign;
            this.simplePassportLoginArg.timestamp = timestamp.ToString();
            this.simplePassportLoginArg.username = usserName;

            var options = new JsonSerializerOptions();
            options.WriteIndented = false; // 设置false格式化为非缩进格式，即不保留换行符;

            if (UnsafeRelaxedJsonEscaping)
            {
                options.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
            }
            else
            {
                options.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Default;
            }

            argJosn = System.Text.Json.JsonSerializer.Serialize(simplePassportLoginArg, options); ;//json格式

            argJsonBase64 = System.Text.UTF8Encoding.UTF8.GetBytes(argJosn).ToBase64();//base64编码

            // var argJsonBase641 = Convert.ToBase64String(System.Text.UTF8Encoding.UTF8.GetBytes(argJosn));//base64编码

            string silverlightUrl = this.Url + "Silverlight/index.aspx?ud=" + argJsonBase64;// Silverlight入口链接
            ssoUrls.silverlightUrl = silverlightUrl;
            string html5Url = this.Url + "html5/index.aspx?ud=" + argJsonBase64;// html5入口链接
            ssoUrls.html5Url = html5Url;

            Uri uri = new Uri(this.Url!);

            string wpfUrl = string.Format(@"K3cloud://{1}/k3cloud/Clientbin/K3cloudclient/K3cloudclient.manifest?Lcid=2052&ExeType=WPFRUNTIME&LoginUrl={0}&ud=", this.Url, uri.Host + ":" + uri.Port) + argJsonBase64;
            ssoUrls.wpfUrl = wpfUrl;

            SSOLoginUrlObject = ssoUrls;
            return SSOLoginUrlObject;
        }


        /// <summary>
        /// 第三方系统单点登录V1(获取所有登录方式链接)
        /// </summary>
        /// <param name="usserName"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public SSOLoginUrlObject GetSsoUrlsV1(string usserName = "", string url = "")
        {
            string ServerUrl = string.Empty;
            if (string.IsNullOrWhiteSpace(url)) { ServerUrl = this.url!; }
            else
            {
                ServerUrl = CommonFunctionHelper.GetServerUrl(url);
            }
            SSOLoginUrlObject ssoUrls = new SSOLoginUrlObject();
            timestamp = CommonFunctionHelper.GetTimestamp();

            string dbId = this.appSettingsModel.XKDApiAcctID;//数据中心ID
            if (string.IsNullOrWhiteSpace(usserName))
            {
                usserName = this.appSettingsModel.XKDApiUserName;//用户名称
            }


            string appId = this.appSettingsModel.XKDApiAppID;//第三方系统应用Id

            string appSecret = this.appSettingsModel.XKDApiAppSec;//第三方系统应用秘钥

            string[] arr = new string[] { dbId, usserName, appId, appSecret, timestamp.ToString()! };
            //if (!string.IsNullOrWhiteSpace(permitcount))
            //{
            //    arr = new string[] { dbId, usserName, appId, appSecret, timestamp.ToString(), permitcount };

            //    this.simplePassportLoginArg.otherargs = string.Format("|{{\'permitcount\':'{0}'}}", permitcount);
            //}

            //Array.Sort(arr, StringComparer.Ordinal);

            //string sortdata = string.Join(string.Empty, arr);

            string sign = CommonFunctionHelper.GetSignatureSHA1Util(arr);//签名 V1

            string urlPara = string.Format("|{0}|{1}|{2}|{3}|{4}|{5}", dbId, usserName, appId, sign, timestamp, this.appSettingsModel.XKDApiLCID);

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("V1版本构建参数(非json): ");
            stringBuilder.AppendLine(urlPara);
            argJosn = stringBuilder.ToString();

            string urlBase64 = System.Text.UTF8Encoding.Default.GetBytes(urlPara).ToBase64();// Base64编码

            argJsonBase64 = urlBase64;

            //  argJosn = System.Text.Json.JsonSerializer.Serialize(simplePassportLoginArg, options); ;//json格式

            // argJsonBase64 = System.Text.UTF8Encoding.UTF8.GetBytes(argJosn).ToBase64();//base64编码

            // var argJsonBase641 = Convert.ToBase64String(System.Text.UTF8Encoding.UTF8.GetBytes(argJosn));//base64编码

            string silverlightUrl = this.Url + "Silverlight/index.aspx?ud=" + argJsonBase64;// Silverlight入口链接
            ssoUrls.silverlightUrl = silverlightUrl;
            string html5Url = this.Url + "html5/index.aspx?ud=" + argJsonBase64;// html5入口链接
            ssoUrls.html5Url = html5Url;

            Uri uri = new Uri(this.Url!);

            string wpfUrl = string.Format(@"K3cloud://{1}/k3cloud/Clientbin/K3cloudclient/K3cloudclient.manifest?Lcid=2052&ExeType=WPFRUNTIME&LoginUrl={0}&ud=", this.Url, uri.Host + ":" + uri.Port) + argJsonBase64;
            ssoUrls.wpfUrl = wpfUrl;

            SSOLoginUrlObject = ssoUrls;
            return SSOLoginUrlObject;
        }
        /// <summary>
        /// 单点登录登出类
        /// </summary>
        public SSOLogoutObject SSOLogoutObject { get; set; } = new SSOLogoutObject();

        /// <summary>
        /// 获取V4版本登出的信息
        /// </summary>
        /// <returns></returns>
        public SSOLogoutObject GetSSOLogoutap0StrV4(string usserName = "", string url = "")
        {
            string ServerUrl = string.Empty;
            if (string.IsNullOrWhiteSpace(url)) { ServerUrl = this.url!; }
            if (string.IsNullOrWhiteSpace(usserName)) { usserName = this.appSettingsModel.XKDApiUserName;/*用户名称*/ }
            SSOLogoutObject sSOLogoutObject = new SSOLogoutObject();
            sSOLogoutObject.RequestLogoutUrl = ServerUrl + "Kingdee.BOS.ServiceFacade.ServicesStub.User.UserService.LogoutByOtherSystem.common.kdsvc";
            string dbId = this.appSettingsModel.XKDApiAcctID;//数据中心ID

            string appId = this.appSettingsModel.XKDApiAppID;//第三方系统应用Id

            string appSecret = this.appSettingsModel.XKDApiAppSec;//第三方系统应用秘钥

            string[] arr = new string[] { dbId, usserName, appId, appSecret, timestamp.ToString()! };

            Array.Sort(arr, StringComparer.Ordinal);

            string sortdata = string.Join(string.Empty, arr);

            string sign = CommonFunctionHelper.Sha256Hex(sortdata);//签名V4 签名算法使用自己语言的sha256算法即可

            long timestamplogout = CommonFunctionHelper.GetTimestamp();



            var options = new JsonSerializerOptions();
            options.WriteIndented = false; // 设置false格式化为非缩进格式，即不保留换行符;
            if (UnsafeRelaxedJsonEscaping)
            {
                options.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
            }
            else
            {
                options.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Default;
            }
            System.Text.Json.Nodes.JsonObject jObjap0 = new System.Text.Json.Nodes.JsonObject();
            jObjap0.Add("AcctID", this.appSettingsModel.XKDApiAcctID);
            jObjap0.Add("AppId", this.appSettingsModel.XKDApiAppID);
            jObjap0.Add("Username", usserName);
            jObjap0.Add("SignedData", sign);
            jObjap0.Add("Timestamp", timestamplogout);

            string ap0json = JsonSerializer.Serialize(jObjap0, options);
            sSOLogoutObject.ap0 = ap0json;

            this.SSOLogoutObject = sSOLogoutObject;
            return sSOLogoutObject;
        }
        /// <summary>
        /// 获取V3版本登出的信息
        /// </summary>
        /// <param name="usserName"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public SSOLogoutObject GetSSOLogoutap0StrV3(string usserName = "", string url = "")
        {
            string ServerUrl = string.Empty;
            if (string.IsNullOrWhiteSpace(url)) { ServerUrl = this.url!; }
            if (string.IsNullOrWhiteSpace(usserName)) { usserName = this.appSettingsModel.XKDApiUserName;/*用户名称*/ }
            SSOLogoutObject sSOLogoutObject = new SSOLogoutObject();
            sSOLogoutObject.RequestLogoutUrl = ServerUrl + "Kingdee.BOS.ServiceFacade.ServicesStub.User.UserService.LogoutByOtherSystem.common.kdsvc";
            string dbId = this.appSettingsModel.XKDApiAcctID;//数据中心ID

            string appId = this.appSettingsModel.XKDApiAppID;//第三方系统应用Id

            string appSecret = this.appSettingsModel.XKDApiAppSec;//第三方系统应用秘钥

            string[] arr = new string[] { dbId, usserName, appId, appSecret, timestamp.ToString()! };

            // Array.Sort(arr, StringComparer.Ordinal);

            //   string sortdata = string.Join(string.Empty, arr);

            string sign = CommonFunctionHelper.GetSignatureSHA1Util(arr);//签名 V3

            long timestamplogout = CommonFunctionHelper.GetTimestamp();



            var options = new JsonSerializerOptions();
            options.WriteIndented = false; // 设置false格式化为非缩进格式，即不保留换行符;
            if (UnsafeRelaxedJsonEscaping)
            {
                options.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
            }
            else
            {
                options.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Default;
            }
            System.Text.Json.Nodes.JsonObject jObjap0 = new System.Text.Json.Nodes.JsonObject();
            jObjap0.Add("AcctID", this.appSettingsModel.XKDApiAcctID);
            jObjap0.Add("AppId", this.appSettingsModel.XKDApiAppID);
            jObjap0.Add("Username", usserName);
            jObjap0.Add("SignedData", sign);
            jObjap0.Add("Timestamp", timestamplogout);

            string ap0json = JsonSerializer.Serialize(jObjap0, options);
            sSOLogoutObject.ap0 = ap0json;

            this.SSOLogoutObject = sSOLogoutObject;
            return sSOLogoutObject;
        }
        /// <summary>
        /// 获取V1,V2版本登出的信息
        /// </summary>
        /// <param name="usserName"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public SSOLogoutObject GetSSOLogoutap0StrV2V1(string usserName = "", string url = "")
        {
            string ServerUrl = string.Empty;
            if (string.IsNullOrWhiteSpace(url)) { ServerUrl = this.url!; }
            if (string.IsNullOrWhiteSpace(usserName)) { usserName = this.appSettingsModel.XKDApiUserName;/*用户名称*/ }
            SSOLogoutObject sSOLogoutObject = new SSOLogoutObject();
            sSOLogoutObject.RequestLogoutUrl = ServerUrl + "Kingdee.BOS.ServiceFacade.ServicesStub.User.UserService.LogoutByOtherSystem.common.kdsvc";
            string dbId = this.appSettingsModel.XKDApiAcctID;//数据中心ID

            string appId = this.appSettingsModel.XKDApiAppID;//第三方系统应用Id

            string appSecret = this.appSettingsModel.XKDApiAppSec;//第三方系统应用秘钥

            string[] arr = new string[] { dbId, usserName, appId, appSecret, timestamp.ToString()! };

            //  Array.Sort(arr, StringComparer.Ordinal);

            // string sortdata = string.Join(string.Empty, arr);

            string sign = CommonFunctionHelper.GetSignatureSHA1Util(arr);//签名  V2

            long timestamplogout = CommonFunctionHelper.GetTimestamp();



            var options = new JsonSerializerOptions();
            options.WriteIndented = false; // 设置false格式化为非缩进格式，即不保留换行符;
            if (UnsafeRelaxedJsonEscaping)
            {
                options.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
            }
            else
            {
                options.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Default;
            }
            System.Text.Json.Nodes.JsonObject jObjap0 = new System.Text.Json.Nodes.JsonObject();
            jObjap0.Add("AcctID", this.appSettingsModel.XKDApiAcctID);
            jObjap0.Add("AppId", this.appSettingsModel.XKDApiAppID);
            jObjap0.Add("Username", usserName);
            jObjap0.Add("SignedData", sign);
            jObjap0.Add("Timestamp", timestamplogout);

            string ap0json = JsonSerializer.Serialize(jObjap0, options);
            sSOLogoutObject.ap0 = ap0json;

            this.SSOLogoutObject = sSOLogoutObject;
            return sSOLogoutObject;
        }


        /// <summary>
        /// 执行单点登录登出
        /// </summary>
        public string SSOExcuteLogout(SSOLogoutObject sSOLogoutObject)
        {

            string res=string.Empty;

            try
            {
                ComWebHelper.WebHelper webHelper = new ComWebHelper.WebHelper();
                webHelper.Timeout = TimeSpan.FromSeconds(30);

                Dictionary<string, string> UrlEncodedkeyValuePairs = new Dictionary<string, string>();
                UrlEncodedkeyValuePairs.Add("ap0", sSOLogoutObject.ap0);
                webHelper.Body_UrlEncoded = new FormUrlEncodedContent(UrlEncodedkeyValuePairs!);
                webHelper.bodyType = BodyType.urlencoded;


                Task<string> RequestTask = webHelper.SendHttpRequestAsync(sSOLogoutObject.RequestLogoutUrl);
                RequestTask.Wait(); // 阻塞直到任务完成 
                res = RequestTask.Result;
            }
            catch (Exception ex)
            {
                res = ex.Message;
              //  throw;
            }
          


            return res;
        }



    }













}
