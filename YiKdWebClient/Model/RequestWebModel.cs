using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace YiKdWebClient.Model
{
#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
    public class RequestWebModel

    {
        public CookieContainer Cookie { get; set; } = new CookieContainer();
        public string RequestUrl { get; set; } = string.Empty;

        public string RealRequestBody { get; set; } = string.Empty;     
        public string RealResponseBody { get; set; } = string.Empty;
    }
}
