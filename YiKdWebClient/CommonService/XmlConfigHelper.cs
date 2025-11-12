using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace YiKdWebClient.CommonService
{
    /// <summary>
    /// 
    /// </summary>
    public static class XmlConfigHelper
    {
        /// <summary>
        /// 
        /// </summary>
        public static string Currentpath = AppDomain.CurrentDomain.BaseDirectory;/*应用程序文件的目录AppDomain*/
        /// <summary>
        /// 
        /// </summary>
        public static string AppConfigPath = Path.Combine(Currentpath, "YiKdWebCfg", "appsettings.xml");
        private static Dictionary<string, Dictionary<string, string>> GetAllServerInfo(string path = "")
        {
            Dictionary<string, Dictionary<string, string>> keyValuePairs = new Dictionary<string, Dictionary<string, string>>();
            if (string.IsNullOrWhiteSpace(path))
            {
                path = AppConfigPath;
            }


            XmlDocument xmlDoc = new XmlDocument();

            try
            {
                //指定XML文件的路径并加载
                xmlDoc.Load(path);

                //获取根节点
                XmlNode root = xmlDoc.DocumentElement!;

                //遍历子节点并输出节点名称和节点值
                foreach (XmlNode node in root.ChildNodes)
                {
                    if (node.NodeType != XmlNodeType.Element) { continue; }
                    XmlNodeList serverinfo = node.ChildNodes;
                    Dictionary<string, string> serverinfos = new Dictionary<string, string>();
                    foreach (XmlNode nodeinfo in serverinfo)
                    {


                        serverinfos.Add(nodeinfo.Name, nodeinfo.InnerText);
                        Console.WriteLine("节点名称：{0}，节点值：{1}", nodeinfo.Name, nodeinfo.InnerText);


                    }
                    keyValuePairs.Add(node.Name, serverinfos);
                    // Console.WriteLine("节点名称：{0}，节点值：{1}", node.Name, node.InnerText);
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return keyValuePairs;
        }

        private static XmlDocument GetAllCfg(string path = "") 
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                path = AppConfigPath;
            }

            XmlDocument xmlDoc = new XmlDocument();

            try
            {
                //指定XML文件的路径并加载
                xmlDoc.Load(path);
            }
            catch (Exception)
            {

              //  throw;
            }

            return xmlDoc;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static Dictionary<string, string> GetAllCfgDic(string path = "")
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                path = AppConfigPath;
            }
            Dictionary<string, string> DicappSettings = new Dictionary<string, string>();

            XmlDocument xmlDoc = YiKdWebClient.CommonService.XmlConfigHelper.GetAllCfg();
            //获取根节点
            XmlNode root = xmlDoc.DocumentElement!;

            XmlNode appSettings = root.FirstChild!;


            XmlNodeList appSettingsNodes = appSettings.ChildNodes;
            foreach (XmlNode itemappSettings in appSettingsNodes)
            {

                string key = string.Empty;
                string value = string.Empty;

                if (itemappSettings.NodeType.Equals(XmlNodeType.Element))
                {
                    XmlAttributeCollection xmlAttributeCollection = itemappSettings!.Attributes!;

                    foreach (XmlAttribute itemxmlAttribute in xmlAttributeCollection!)
                    {
                        if (itemxmlAttribute.Name.Equals("key", StringComparison.OrdinalIgnoreCase)) { key = itemxmlAttribute.InnerText; }
                        if (itemxmlAttribute.Name.Equals("value", StringComparison.OrdinalIgnoreCase)) { value = itemxmlAttribute.InnerText; }
                    }
                    if (!string.IsNullOrWhiteSpace(key) && !DicappSettings.Keys.Contains(key))
                    {
                        DicappSettings.Add(key, value);
                    }

                }



            }

            return DicappSettings;

        }
    }
}
