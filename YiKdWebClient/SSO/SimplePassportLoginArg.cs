using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YiKdWebClient.SSO
{
#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
    public class SimplePassportLoginArg
    {
        public SimplePassportLoginArg() { }
        /// <summary>
        /// 数据中心的ID;
        /// </summary>
        public string? dbid { get; set; }
        /// <summary>
        /// 用户名称；
        /// </summary>
        public string? username { get; set; }
        /// <summary>
        /// 应用程序ID，通过Administrator登录数据中心后，在【系统管理】分类的【第三方系统登录授权】功能里面进行新增维护；（云之家可以不填由querystring参数决定）
        /// </summary>
        public string? appid { get; set; }
        /// <summary>
        /// 数据签名串，通过公钥和用户数据进行运算得到，https://vip.kingdee.com/article/37406
        /// </summary>
        public string? signeddata { get; set; }
        /// <summary>
        /// 登录时间戳(Unix时间戳,定义为从格林威治时间1970年01月01日00时00分00秒起至现在的总秒数)；
        /// </summary>
        public string? timestamp { get; set; }
        /// <summary>
        /// 语言ID，中文2052（默认），英文1033，繁体3076;
        /// </summary>
        public string? lcid { get; set; } = "2052";
        /// <summary>
        /// XT=云之家集成（同时要求entryrole=XT）；SimPas=简单通行证集成;
        /// </summary>
        public string? origintype { get; set; } = "SimPas";
        /// <summary>
        /// 验证权限的入口角色;
        /// </summary>
        public string? entryrole { get; set; }=string.Empty;
        /// <summary>
        /// 登录后默认打开功能的表单id；
        /// </summary>
        public string? formid { get; set; } = string.Empty;
        /// <summary>
        /// formtype = 单据：bill或空, 列表：list, 万能报表：wnreport, 直接sql报表：sqlreport, 系统报表：sysreport， 树形报表：treereport, 移动报表：movereport， 动态表单：dynamicform。
        /// </summary>
        public string? formtype { get; set; } = string.Empty;
        /// <summary>
        /// pkid：formid对应表单的主键；formtype为list时忽略，formtype为bill时起作用，如果为空表示新增状态；
        /// </summary>
        public string? pkid { get; set; } = string.Empty;
        /// <summary>
        /// 作为用户自定义参数传入，使用于二开；
        /// </summary>
        public string? otherargs { get; set; } = string.Empty;

        /*
        /// <summary>
        /// formargs: 表单初始化自定义参数formargs，能设置boside中发布的自定义参数。仅V2版本协议支持该参数，V1版本不支持。 formargs为json格式字符串，例如人人报销首页参数：{ "KD_Html5_FormTheme_Name": "Galaxy" } 。（PT-146869 [7.6.0.202103] 2021/3/25  7.6.2122.7 ）
        /// </summary>
        //public string formargs { get; set; }
        /// <summary>
        /// openmode: 登陆后打开指定功能单据的模式，空白=原有主控模式，Single=单独打开指定单据（没有主控功能，仅HTML5端支持，需要配合轻量级入口地址：【http://xxx.xxx.xxx.xxx/k3cloud/html5/lightstartapp.aspx?ud=...】）（2019-03-13补丁 PT132327  [7.3.1199.6]）；
        /// </summary>
    
        */
        public string? openmode { get; set; }
        /// <summary>
        /// 官方参数里有这个，暂不知具体作用，先不用赋值
        /// </summary>
        public string? loginthen { get; set; }
    }
}
