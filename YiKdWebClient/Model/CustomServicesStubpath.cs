using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YiKdWebClient.Model
{
#pragma warning disable CS1570 // XML 注释出现 XML 格式错误 --“引用未定义的实体“productLineId”。”
    /// <summary>
    /// 自定义webapi路由地址模型
    /// </summary>
    public class CustomServicesStubpath
    {
        /// <summary>
        /// dll项目工程的命名空间(必须要以.WebApi结尾，否则系统可能拦截或者不识别)
        /// https://vip.kingdee.com/article/602100140303705088?lang=zh-CN&productLineId=1
        /// </summary>
        public string ProjetNamespace {  get; set; }=string.Empty;

        /// <summary>
        /// dll项目工程的类名
        /// </summary>
        public string ProjetClassName { get; set; } = string.Empty;

        /// <summary>
        ///  dll项目工程的类中的方法名
        /// </summary>
        public string ProjetClassMethod { get; set; } = string.Empty;

        /// <summary>
        /// 获取ServicesStubpath
        /// </summary>
        /// <returns></returns>
        public string GetCustomServicesStubpathUrl() 
        {
            return RemoveSpaces(ProjetNamespace) + "." + RemoveSpaces(ProjetClassName) + "."+ RemoveSpaces(ProjetClassMethod) + ","+ RemoveSpaces(ProjetNamespace)+ ".common.kdsvc";
        }

        /// <summary>
        /// 去除空格
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string RemoveSpaces(string input) { 
            string res=input;

            try
            {
                res= input.Replace(" ", "");
            }
            catch (Exception)
            {

               // throw;
            }
            return res;
        }
    }
}
