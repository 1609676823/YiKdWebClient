using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mime;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace YiKdWebClient.ComWebHelper
{
    #pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
    /// <summary>
    /// 
    /// </summary>
    public class WebHelper
    {
        public WebHelper() { }
        /// <summary>
        /// queryParameters,构建请求的query拼接
        /// </summary>
        public Dictionary<string, string> queryParameters {  get; set; }=new Dictionary<string, string>();
        /// <summary>
        /// 请求的CookieContainer容器
        /// </summary>
        public CookieContainer Requestcookies { get; set; } = new CookieContainer();

        /// <summary>
        /// 响应的CookieContainer容器
        /// </summary>
        public CookieContainer Responsecookies { get; set; } = new CookieContainer();
        /// <summary>
        /// 请求头的键值对
        /// </summary>
        public Dictionary<string, string> RequestHeaders { get; set; } = new Dictionary<string, string>();
        /// <summary>
        /// 响应的头文件
        /// </summary>
        public HttpResponseHeaders? ResponseHeaders { get; set; } = null;
        /// <summary>
        /// 请求的类型 POST GET DELETE ...
        /// </summary>
        public HttpMethod HttpMethod { get; set; } = HttpMethod.Post;

        public BodyType bodyType { get; set; } = BodyType.none;

        /// <summary>
        /// 请求的Body类型为FormData
        /// </summary>
        public MultipartFormDataContent Body_FormData { get; set; } = new MultipartFormDataContent();
        /// <summary>
        /// 请求的Body类型为UrlEncoded(x-www-form-urlencoded)
        /// </summary>
        public FormUrlEncodedContent Body_UrlEncoded { get; set; } = new FormUrlEncodedContent(new Dictionary<string,string>()!);

        /// <summary>
        /// body中的raw
        /// </summary>

        public string Body_Raw { get; set; } = string.Empty;
        /// <summary>
        /// 请求的mediaType类型
        /// </summary>
        public string RequestmediaType { get; set; } = CustomMediaTypeNames.Application.Json;
        /// <summary>
        /// 请求的超时设置
        /// </summary>
       // public TimeSpan Timeout = System.Threading.Timeout.InfiniteTimeSpan;
        public TimeSpan Timeout = TimeSpan.FromSeconds(120);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public async Task<string> SendHttpRequestAsync(string url)
        {
            HttpClientHandler handler = new HttpClientHandler();
            CookieContainer cookieContainer = new CookieContainer();//创建CookieContainer
            if (Requestcookies.Count > 0)
            {

                handler.CookieContainer = Requestcookies;


            }
            // 创建 HttpClient 实例  
            HttpClient client = new HttpClient(handler);
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("Accept-Charset", "utf-8");
            client.Timeout = Timeout;
            // 创建一个 HttpRequestMessage 实例
            HttpRequestMessage request = new HttpRequestMessage();
            // request.Headers.Add("Accept-Charset", "utf-8");
            request.Method = HttpMethod; // 设置请求方法

            if (queryParameters.Count>0)
            {
                request.RequestUri = new Uri(url + "?" + CreateQueryString(queryParameters)); // 设置请求的 URI  
            }
            else
            {
                request.RequestUri = new Uri(url); // 设置请求的 URI  
            }

          

            #region 添加自定义请求头


            if (RequestHeaders.Count > 0)
            {
                foreach (KeyValuePair<string, string> RequestHeaderItem in RequestHeaders)
                {
                    client.DefaultRequestHeaders.Add(RequestHeaderItem.Key, RequestHeaderItem.Value);
                    request.Headers.Add(RequestHeaderItem.Key, RequestHeaderItem.Value);
                }
            }
            #endregion

            #region body构建
            
            /*formdata类型*/
            if (this.bodyType.Equals(BodyType.formdata))
            {
                request.Content = this.Body_FormData;
             
            }

            /*urlencoded类型*/
            if (this.bodyType.Equals(BodyType.urlencoded))
            {
                request.Content = this.Body_UrlEncoded;
                
            }

            /*raw类型*/
            if (this.bodyType.Equals(BodyType.raw))
            {
                StringContent raw = new StringContent(Body_Raw, Encoding.UTF8, RequestmediaType); ; // 示例请求内容  
                request.Content = raw;
                //request.Content.Headers.ContentType = new MediaTypeHeaderValue(RequestmediaType);

            }

            
          

            #endregion




            HttpResponseMessage response = await client.SendAsync(request);

            // 确保请求成功  
            response.EnsureSuccessStatusCode();

            byte[] responseByte = await response.Content.ReadAsByteArrayAsync();
            string responseBody = Encoding.UTF8.GetString(responseByte);
            this.ResponseHeaders = response.Headers;
            this.Responsecookies = handler.CookieContainer;


            handler.Dispose();
            client.Dispose();
            // 返回响应内容字符串  
            return responseBody;
        }


        // 创建 Query 参数的字符串表示形式
        public  string CreateQueryString(Dictionary<string, string> parameters)
        {
            StringBuilder queryString = new StringBuilder();
            foreach (KeyValuePair<string, string> parameter in parameters)
            {
                if (queryString.Length > 0)
                {
                    queryString.Append("&");
                }
                queryString.Append(parameter.Key).Append("=").Append(WebUtility.UrlEncode(parameter.Value));
            }
            return queryString.ToString();
        }

    }

    public enum BodyType
    {
        /// <summary>
        ///none
        /// </summary>
        [EnumMember(Value = "none")]
        [Description("none")]
        none,
        /// <summary>
        ///none
        /// </summary>
        [EnumMember(Value = "formdata")]
        [Description("formdata")]
        formdata,
        /// <summary>
        ///none
        /// </summary>
        [EnumMember(Value = "urlencoded")]
        [Description("urlencoded")]
        urlencoded,
        /// <summary>
        ///none
        /// </summary>
        [EnumMember(Value = "raw")]
        [Description("raw")]
        raw,
    }
}
