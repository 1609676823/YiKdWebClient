using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YiKdWebClient.Model
{
#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
    public class ValidateLoginSettingsModel

    {
        public ValidateLoginSettingsModel(string url="") { 

            //if (string.IsNullOrWhiteSpace(url))
            //{
            //    try
            //    {
            //        AppSettingsModel appSettingsModel = new AppSettingsModel();
            //        Url = appSettingsModel.XKDApiServerUrl;
            //    }
            //    catch (Exception)
            //    {

            //        // throw;
            //    }
            //}

            //else { Url = url; }

            Url = GetServerUrl(url);


        }



        public string DbId { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public int lcid { get; set; } = 2052;


        private string? _Url;
        public string? Url
        {

            get { return _Url; }
            set
            {
                _Url = GetServerUrl(value!);
            }

        }

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
}
