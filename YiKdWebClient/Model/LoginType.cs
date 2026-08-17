using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace YiKdWebClient.Model
{
#pragma warning disable CS1570 // XML 注释出现 XML 格式错误 --“引用未定义的实体“productLineId”。”
    /// <summary>
    /// 验证的类型
    /// </summary>
    public enum LoginType
    {
        /// <summary>
        ///使用签名信息登录接口（目前推荐方式）注意：PT-146911 8.0.0.202205 之前的版本不支持SHA256加密，需要使用SHA1加密算法
        ///原文链接：https://vip.kingdee.com/article/650369502730859776?specialId=650386937144032256&productLineId=1&isKnowledge=2&lang=zh-CN
        /// </summary>
        [EnumMember(Value = "LoginBySignSHA256")]
        [Description("LoginBySignSHA256")]
        LoginBySignSHA256,

        /// <summary>
        ///使用签名信息登录接口（目前推荐方式）注意：PT-146911 8.0.0.202205 之前的版本不支持SHA256加密，需要使用SHA1加密算法
        ///原文链接：https://vip.kingdee.com/article/650369502730859776?specialId=650386937144032256&productLineId=1&isKnowledge=2&lang=zh-CN
        /// </summary>
        [EnumMember(Value = "LoginBySignSHA1")]
        [Description("LoginBySignSHA1")]
        LoginBySignSHA1,
        /// <summary>
        ///第三方登录授权
        /// </summary>
        [EnumMember(Value = "LoginByAppSecret")]
        [Description("LoginByAppSecret")]
        LoginByAppSecret,
        /// <summary>
        ///API签名认证
        /// </summary>
        [EnumMember(Value = "ApiSignHeaders")]
        [Description("ApiSignHeaders")]
        LoginByApiSignHeaders,

        /// <summary>
        /// 用户密码验证
        /// </summary>
        [EnumMember(Value = "ValidateLogin")]
        [Description("ValidateLogin")]
        ValidateLogin,

        /// <summary>
        ///集成密钥验证
        /// </summary>
        [EnumMember(Value = "LoginBySimplePassport")]
        [Description("LoginBySimplePassport")]
        LoginBySimplePassport,

        /// <summary>
        ///EnDeCode用户密码验证
        /// </summary>
        /// <remarks>
        /// 此枚举成员不再推荐使用，请使用其他方法。
        /// </remarks>
        [EnumMember(Value = "ValidateUserEnDeCode")]
        [Description("ValidateUserEnDeCode")]
        [Obsolete("不推荐使用,官方API验证没有推出这个方式，仅某些附件场景提供过")]
        ValidateUserEnDeCode,

    }
}
