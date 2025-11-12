using Kingdee.BOS;
using Kingdee.BOS.Core.DependencyRules;
using Kingdee.BOS.ServiceFacade.KDServiceFx;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;

namespace GlobalServiceCustom.WebApi
{
    [Kingdee.BOS.Util.HotUpdate]

    /*
     http://127.0.0.1/k3cloud/GlobalServiceCustom.WebApi.DataServiceHandler.CommonRunnerService,GlobalServiceCustom.WebApi.common.kdsvc
     */

    /*
     【注意】

自定义接口的命名空间必须包含独立的子命名空间WebApi！

自定义接口的命名空间必须包含独立的子命名空间WebApi！

自定义接口的命名空间必须包含独立的子命名空间WebApi！

否则可能会造成安全校验失败，请求被拦截，报403错误。

下面以本文中的自定义接口为例说明：

Jac.XkDemo.BOS.WebApi.CustomWebApiDemoWebApiService.DoSth2,Jac.XkDemo.BOS.WebApi

逗号前面部分为自定义接口类全名（命名空间+类名+方法名）

逗号后面部分为自定义接口类所在项目的编译后的组件（程序集）的名称。

作者：Jack
来源：金蝶云社区
原文链接：https://vip.kingdee.com/article/97030089581136896?specialId=448928749460099072&productLineId=1&isKnowledge=2&lang=zh-CN
著作权归作者所有。未经允许禁止转载，如需转载请联系作者获得授权。

     */
    public class DataServiceHandler : Kingdee.BOS.WebApi.ServicesStub.AbstractWebApiBusinessService
    {
        /// <summary>
        /// 主构造函数
        /// </summary>
        /// <param name="context"></param>
        public DataServiceHandler(KDServiceContext context) : base(context)
        {
        }

        /// <summary>
        /// 执行方法(定义多少个参数，在报文parameters数组中就可以接受多少个数组)
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public string CommonRunnerService(string parameter) 
        {
            string res="";

            Context context = this.KDContext.Session.AppContext;

            if (context == null) 
            {
                res = "会话超时，需重新登录";
              return res;
            }
            try
            {
                DataSet dataSet = Kingdee.BOS.ServiceHelper.DBServiceHelper.ExecuteDataSet(this.KDContext.Session.AppContext, parameter);
                DataTable dataTable = dataSet.Tables[0];
                res = Newtonsoft.Json.JsonConvert.SerializeObject(dataTable);
                //  List<Dictionary<string, string>> listkeyValuePairs = ConvertDataTableToList(dataTable);
                // res = System.Text.Json.JsonSerializer.Serialize(listkeyValuePairs, new JsonSerializerOptions() { WriteIndented = true, Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping });
            }
            catch (Exception ex)
            {
                res = ex.Message;
               // throw;
            }
            /*执行数据库逻辑*/
           

            return res; 
        }

       

        //public static List<Dictionary<string, string>> ConvertDataTableToList(DataTable dataTable)
        //{
        //    List<Dictionary<string, string>> list = new List<Dictionary<string, string>>();

        //    foreach (DataRow row in dataTable.Rows)
        //    {
        //        Dictionary<string, string> dict = new Dictionary<string, string>();

        //        foreach (DataColumn column in dataTable.Columns)
        //        {
        //            dict[column.ColumnName] = Convert.ToString(row[column]);
        //        }

        //        list.Add(dict);
        //    }

        //    return list;
        //}
    }
}
