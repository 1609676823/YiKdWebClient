using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YiKdWebClient.SSO
{
    /// <summary>
    /// 单点登录退出请求的实际内容
    /// </summary>
    public class SSOLogoutObject
    {
        /// <summary>
        /// 单点登录退出请求的实际内容
        /// </summary>
        public void SSOLoginUrlObject() { }
        /// <summary>
        /// 退出实际请求的地址
        /// </summary>
        public string RequestLogoutUrl { get; set; } = string.Empty;
        /// <summary>
        /// 参数名称：ap0 ,使用  x-www-form-urlencoded 方式请求
        /// </summary>
        public string ap0 { get; set; } = string.Empty;

    }




}
