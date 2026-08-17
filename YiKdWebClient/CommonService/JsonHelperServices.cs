using System;
using System.Collections.Generic;
using System.Linq;
//using System.Reflection.Metadata;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Text.Unicode;
using System.Threading.Tasks;
using System.Web;
using static System.Net.Mime.MediaTypeNames;

namespace YiKdWebClient.CommonService
{
    /// <summary>
    /// 
    /// </summary>
    public static class JsonHelperServices
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Formid"></param>
        /// <param name="strjson"></param>
        /// <param name="UnsafeRelaxedJsonEscaping">获取对编码内容不太严格的内置 JavaScript 编码器实例。--允许某些不安全的、不严格的转义 https://learn.microsoft.com/zh-cn/dotnet/api/system.text.encodings.web.javascriptencoder?view=net-8.0</param>
        /// <param name="opNumber">操作代码</param>
        /// <returns></returns>
        public static string getRequestBodystring(string Formid,string strjson, bool UnsafeRelaxedJsonEscaping,string opNumber)

        {
            var options = new JsonSerializerOptions();
            options.WriteIndented = true; // 设置格式化为非缩进格式，即不保留换行符;

            if (UnsafeRelaxedJsonEscaping)
            {
                options.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
            }
            else
            {
                options.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Default;
            }
            
            List<object> Parameters = new List<object>();

            if (!string.IsNullOrWhiteSpace(Formid)) { Parameters.Add(Formid); }
           

            if (!string.IsNullOrWhiteSpace(opNumber)) { Parameters.Add(opNumber); }

         

            Parameters.Add(strjson);


            string Content = JsonSerializer.Serialize(Parameters, new JsonSerializerOptions { WriteIndented=false, Encoder=options.Encoder }); 

            // StringBuilder sb = new StringBuilder();

            System.Text.Json.Nodes.JsonObject jObj=new System.Text.Json.Nodes.JsonObject();
            jObj.Add("format", 1);
            jObj.Add("useragent", "ApiClient");
            jObj.Add("rid", Guid.NewGuid().ToString().GetHashCode().ToString());
            jObj.Add("parameters",Content);
            jObj.Add("timestamp",DateTime.Now);
            jObj.Add("v", "1.0");
            string jsonString = JsonSerializer.Serialize(jObj, options);
            return jsonString;
             
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strjson"></param>
        /// <param name="UnsafeRelaxedJsonEscaping">获取对编码内容不太严格的内置 JavaScript 编码器实例。--允许某些不安全的、不严格的转义 https://learn.microsoft.com/zh-cn/dotnet/api/system.text.encodings.web.javascriptencoder?view=net-8.0</param>
        /// <param name="WriteIndented"></param>
        /// <returns></returns>
        public static string getLoginRequestBodystring(string strjson, bool UnsafeRelaxedJsonEscaping,bool WriteIndented=false)

        {
            var options = new JsonSerializerOptions();
             options.WriteIndented = WriteIndented; // 设置false格式化为非缩进格式，即不保留换行符;

            if (UnsafeRelaxedJsonEscaping)
            {
                options.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
            }
            else
            {
                options.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Default;
            }



            // string Content = JsonSerializer.Serialize(strjson, options); 

            string Content = strjson;

            // StringBuilder sb = new StringBuilder();

            System.Text.Json.Nodes.JsonObject jObj = new System.Text.Json.Nodes.JsonObject();
            jObj.Add("format", 1);
            jObj.Add("useragent", "ApiClient");
            jObj.Add("rid", Guid.NewGuid().ToString().GetHashCode().ToString());
            jObj.Add("parameters", Content);
            jObj.Add("timestamp", DateTime.Now);
            jObj.Add("v", "1.0");

           

            string jsonString = JsonSerializer.Serialize(jObj, options);
            return jsonString;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strjson"></param>
        /// <param name="UnsafeRelaxedJsonEscaping"></param>
        /// <param name="WriteIndented"></param>
        /// <returns></returns>
        public static string getLoginRequestBodystringByParameters(string strjson, bool UnsafeRelaxedJsonEscaping, bool WriteIndented = false)

        {
            var options = new JsonSerializerOptions();
            options.WriteIndented = WriteIndented; // 设置false格式化为非缩进格式，即不保留换行符;

            if (UnsafeRelaxedJsonEscaping)
            {
                options.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
            }
            else
            {
                options.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Default;
            }



            // string Content = JsonSerializer.Serialize(strjson, options); 

            string Content = strjson;

            // StringBuilder sb = new StringBuilder();

            System.Text.Json.Nodes.JsonObject jObj = new System.Text.Json.Nodes.JsonObject();
            //jObj.Add("format", 1);
            //jObj.Add("useragent", "ApiClient");
            //jObj.Add("rid", Guid.NewGuid().ToString().GetHashCode().ToString());
            jObj.Add("parameters", Content);
            //jObj.Add("timestamp", DateTime.Now);
            //jObj.Add("v", "1.0");



            string jsonString = JsonSerializer.Serialize(jObj, options);
            return jsonString;

        }

    }
}
