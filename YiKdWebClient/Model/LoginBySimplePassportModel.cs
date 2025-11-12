using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace YiKdWebClient.Model
{
#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
    public class LoginBySimplePassportModel
    {
        public LoginBySimplePassportModel(string url="") { Url = url; }
        public string Url { get; set; }=string.Empty;
        /// <summary>
        /// 集成密钥文件路径
        /// </summary>
        public string CnfFilePath { get; set; } = string.Empty;
        /// <summary>
        /// 集成密钥Base64密码
        /// </summary>
        public string SimplePassportForBase64 { get; set; } = string.Empty;
        /// <summary>
        /// 账套语言
        /// </summary>
        public int Lcid { get; set; } = 2052;
        /// <summary>
        /// 使用密钥方式(文件/Base64)
        /// </summary>

        public BySimplePassportType bySimplePassportType { get; set; } = BySimplePassportType.CnfFile;
        public static string GetServerUrl(string url)
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
    }

    public enum BySimplePassportType
    {
        /// <summary>
        ///CNF文件
        /// </summary>
        [EnumMember(Value = "CnfFile")]
        [Description("CnfFile")]
        CnfFile,

        /// <summary>
        ///CNF对应的Base64字符串
        /// </summary>
        [EnumMember(Value = "ForBase64")]
        [Description("ForBase64")]
        ForBase64,
    }
}
