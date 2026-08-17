using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YiKdWebClient.Model
{

    /// <summary>
    /// 第三方系统登录授权配置文件的信息
    /// </summary>
    public class AppSettingsModel
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        public AppSettingsModel(string path="")
        {
            try
            {
                Dictionary<string, string> dic = CommonService.XmlConfigHelper.GetAllCfgDic(path);

                XKDApiAcctID =GetDicValue(dic, "X-KDApi-AcctID");
                XKDApiAppID = GetDicValue(dic, "X-KDApi-AppID"); 
                XKDApiAppSec = GetDicValue(dic, "X-KDApi-AppSec"); 
                XKDApiUserName = GetDicValue(dic, "X-KDApi-UserName");  
                XKDApiLCID = GetDicValue(dic, "X-KDApi-LCID");
                XKDApiServerUrl = GetDicValue(dic, "X-KDApi-ServerUrl");
                XKDApiOrgNum = GetDicValue(dic, "X-KDApi-OrgNum");
            }
            catch (Exception)

            {

            }
        }
        /// <summary>
        /// 当前使用的 账套ID(即数据中心id)
        /// </summary>
        public string XKDApiAcctID { get; set; } = string.Empty;
        /// <summary>
        /// 第三方系统登录授权的 应用ID
        /// </summary>
        public string XKDApiAppID { get; set; } = string.Empty;
        /// <summary>
        /// 第三方系统登录授权的 应用密钥
        /// </summary>
        public string XKDApiAppSec { get; set; } = string.Empty;
        /// <summary>
        /// 第三方系统登录授权的 集成用户名称
        /// </summary>
        public string XKDApiUserName { get; set; } = string.Empty;
        /// <summary>
        /// 账套语系，默认2052
        /// </summary>
        public string XKDApiLCID { get; set; } = string.Empty;

        private string _xkdaApiServerUrl = string.Empty;

        /// <summary>
        /// 服务Url地址
        /// </summary>
        public string XKDApiServerUrl {

            get { return _xkdaApiServerUrl; }
            set
            {
                _xkdaApiServerUrl = CommonFunctionHelper.GetServerUrl(value);
            }

        }
        /// <summary>
        /// 
        /// </summary>
        public string XKDApiOrgNum{ get; set; } = string.Empty;

        
       

        private string GetDicValue(Dictionary<string, string> dic,string key) { 
            string resvalue = string.Empty;
            try
            {
                return dic[key];
            }
            catch (Exception)
            {

               // throw;
            }
            return resvalue;
        }

    }






}
