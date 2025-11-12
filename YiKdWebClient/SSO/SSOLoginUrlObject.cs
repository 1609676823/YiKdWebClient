using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YiKdWebClient.SSO
{
    /// <summary>
    /// 所有登录方式的单点登录URL链接
    /// </summary>
    public class SSOLoginUrlObject
    {
        /// <summary>
        /// 所有登录方式的单点登录URL链接
        /// </summary>
        public SSOLoginUrlObject() { }
        /// <summary>
        /// Silverlight入口链接
        /// </summary>
        public string silverlightUrl { get; set; } = string.Empty;
        /// <summary>
        /// html5入口链接
        /// </summary>
        public string html5Url { get; set; } = string.Empty;
        /// <summary>
        /// 客户端入口链接
        /// </summary>
        public string wpfUrl { get; set; } = string.Empty;
    }
}
