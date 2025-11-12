using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YiKdWebClient.AuthService
{
    /// <summary>
    /// API请求头签名模式认证
    /// </summary>
    public static class LoginByApiSignHeaders
    {
        /// <summary>
        /// 获取签名的请求头字典
        /// </summary>
        /// <param name="appSettingsModel"></param>
        /// <param name="uri"></param>
        /// <returns></returns>
        public static Dictionary<string,string> GetApiHeaders(Model.AppSettingsModel appSettingsModel,Uri uri) 
        {
            Dictionary<string, string> dictionary= new Dictionary<string, string>();

            string text = GetTimestamp().ToString();
            string nonce = GetNonce();

            string XApiClientID=string.Empty;
            string xapiSec = "";

            int num = appSettingsModel.XKDApiAppID.IndexOf("_");
            if (num > -1)
            {
                XApiClientID = appSettingsModel.XKDApiAppID.Substring(0, num);
                xapiSec = EnDecode.DecryptAppSecret(appSettingsModel.XKDApiAppID.Substring(num + 1));

            }
            if (!string.IsNullOrWhiteSpace(XApiClientID))
            {
                dictionary.Add("X-Api-ClientID",XApiClientID);
                dictionary.Add("X-Api-Auth-Version", "2.0");
                dictionary.Add("x-api-signheaders", "x-api-timestamp,x-api-nonce");
                dictionary.Add("x-api-nonce", nonce);
                dictionary.Add("x-api-timestamp", text);

                string message = string.Format("{0}\n{1}\n\n{2}\n{3}\n", new object[]
                {
                    "POST",
                    EnDecode.UrlEncodeWithUpperCode(uri.PathAndQuery, Encoding.ASCII),
                    "x-api-nonce:" + nonce,
                    "x-api-timestamp:" + text
                });
                dictionary.Add("X-Api-Signature", EnDecode.HmacSHA256(message, xapiSec, Encoding.ASCII, true));
            }

            dictionary.Add("X-Kd-Appkey", appSettingsModel.XKDApiAppID);
            string text2 = string.Format("{0},{1},{2},{3}", new object[]
            {
                appSettingsModel.XKDApiAcctID,
                appSettingsModel.XKDApiUserName,
                appSettingsModel.XKDApiLCID,
                appSettingsModel.XKDApiOrgNum,
            });
            dictionary.Add("X-Kd-Appdata", Convert.ToBase64String(Encoding.UTF8.GetBytes(text2)));
            dictionary.Add("X-Kd-Signature", EnDecode.HmacSHA256(appSettingsModel.XKDApiAppID + text2, appSettingsModel.XKDApiAppSec, Encoding.UTF8, true));

            return dictionary;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public static Dictionary<string, string> GetApiHeaders(Uri uri) { 

            return GetApiHeaders(new Model.AppSettingsModel(), uri);
        
        }
        /// <summary>
        /// 获取Header字符串
        /// </summary>
        /// <param name="HeadersDictionary"></param>
        /// <returns></returns>
        public static string GetApiHeadersStr(Dictionary<string, string> HeadersDictionary)
        {
        StringBuilder stringBuilder = new StringBuilder();
            if (HeadersDictionary!=null&& HeadersDictionary.Count>0)
            {
                foreach (var item in HeadersDictionary)
                {
                    stringBuilder.AppendLine(string.Format("{0}:{1}",item.Key,item.Value));
                }
            }

        return stringBuilder.ToString();    
        }





        private static string GetNonce()
        {
            return Guid.NewGuid().ToString().Replace("-", "");
        }

        //private static long CurrentTimeMillis()
        //{
        //    return (long)(DateTime.UtcNow - Jan1st1970).TotalMilliseconds;
        //}

        // Token: 0x0600003B RID: 59 RVA: 0x000029AD File Offset: 0x00000BAD
        private static long GetTimestamp()
        {
            DateTime Jan1st1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return (long)(DateTime.UtcNow - Jan1st1970).TotalMilliseconds; ;
        }

      //  private static DateTime Jan1st1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
    }
}
