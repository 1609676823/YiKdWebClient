using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

using System.Text;
using System.Threading.Tasks;

namespace YiKdWebClient.CommonService
{
    /// <summary>
    /// 
    /// </summary>
    public class WebHelperServices
    {
        /// <summary>
        /// 
        /// </summary>
        public CookieContainer cookies = new CookieContainer();

        /// <summary>
        /// 
        /// </summary>
        public Dictionary<string, string> RequestHeaders = new Dictionary<string, string>();

        /// <summary>
        /// 
        /// </summary>
        public HttpResponseHeaders? ResponseHeaders = null;

        /// <summary>
        /// 
        /// </summary>
        public HttpMethod HttpMethod = HttpMethod.Post;

        /// <summary>
        /// 
        /// </summary>
        public TimeSpan Timeout = System.Threading.Timeout.InfiniteTimeSpan;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="postData"></param>
        /// <returns></returns>
        public  async Task<string> SendHttpRequestAsync(string url, string postData = "")
        {

            HttpClientHandler handler = new HttpClientHandler();        
            CookieContainer cookieContainer = new CookieContainer();//创建CookieContainer
            if (cookies.Count > 0)
            {

                handler.CookieContainer = cookies;


            }    
             // 创建 HttpClient 实例  
            HttpClient client = new HttpClient(handler);
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("Accept-Charset", "utf-8");
            client.Timeout= Timeout;
            #region MyRegion
            //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //client.DefaultRequestHeaders.Add("Content-Type", "application/json; charset=utf-8");
            // client.DefaultRequestHeaders.Add("Accept", "application/json");
            #endregion
            

            // 创建一个 HttpRequestMessage 实例

            HttpRequestMessage request = new HttpRequestMessage();
           // request.Headers.Add("Accept-Charset", "utf-8");
            request.Method = HttpMethod; // 设置请求方法
            request.RequestUri = new Uri(url); // 设置请求的 URI  

            StringContent requestContent = new StringContent(postData, Encoding.UTF8, "application/json"); ; // 示例请求内容  
            request.Content = requestContent; // 设置内容类型为 JSON
          
            // string cookiestring = string.Join(";", cookies);
            //if (cookies.Count()>0)
            //{

            //    //client.DefaultRequestHeaders.Add("Cookie", cookiestring);
            //    //request.Headers.Add("Cookie", cookiestring);
            //    client.DefaultRequestHeaders.Add("Cookie", cookies);
            //    request.Headers.Add("Cookie", cookies);


            //}
            #region 添加自定义请求头


            if (RequestHeaders.Count>0)
            {
                foreach (KeyValuePair<string,string> RequestHeaderItem in RequestHeaders) 
                {
                    client.DefaultRequestHeaders.Add(RequestHeaderItem.Key, RequestHeaderItem.Value);
                    request.Headers.Add(RequestHeaderItem.Key, RequestHeaderItem.Value);
                }
            }
            #endregion

            //  HttpResponseMessage response= await  client.SendAsync(request); 此方法异步时候偶发卡顿无反应，待观察
            HttpResponseMessage response = client.SendAsync(request).Result;




            #region 弃用
            //// 创建一个 HttpRequestMessage 实例
            //// 创建 POST 数据  
            //HttpContent content = new StringContent(postData, Encoding.UTF8, "application/json");


            //// 发送 POST 请求  
            //HttpResponseMessage response = await client.PostAsync(url, content);
            #endregion



            // 确保请求成功  
            response.EnsureSuccessStatusCode();

            

            // 读取响应内容为字符串  
            //  string responseBody = await response.Content.ReadAsStringAsync();

            byte[] responseByte= await response.Content.ReadAsByteArrayAsync();

           string responseBody = Encoding.UTF8.GetString(responseByte);
            this.ResponseHeaders = response.Headers;
           // IEnumerable<string> responseHeadersCookie = response.Headers.GetValues("Set-Cookie");
           // webModel.Cookie = responseHeadersCookie;
            this.cookies = handler.CookieContainer ;
            //  webModel.RealResponseBody = responseBody;
            //IEnumerator< KeyValuePair< string, IEnumerable<string> > > a = response.Headers.GetEnumerator();

            this.cookies=handler.CookieContainer ;
            handler.Dispose();
            client.Dispose();
            // 返回响应内容字符串  
            return responseBody;
        }
    }
    }
