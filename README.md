# YiKdWebClient 框架介绍以及使用说明
实现金蝶云星空第三方webapi操作,使用原生框架以及纯HTTP协议实现,避免了各种框架冲突<br>
移除了对官方SDK的依赖<br>
移除了对Newtonsoft.Json的依赖<br>
兼容性强;同时兼容.net;.net framework;netstandard
# 官方原始的报文和地址结构说明
https://vip.kingdee.com/knowledge/528587883691785472?productLineId=1&isKnowledge=2&lang=zh-CN

# postman原始报文和url地址下载
[点此下载](%E6%98%9F%E7%A9%BAWebAPI.postman_collection.json.zip)
# 1.框架引入方式:
## nuget包的使用方法
使用vs自带的nuget管理器安装最新版的 YiKdWebClient ,如下图:<br>
![输入图片说明](nuget%E4%BD%BF%E7%94%A8.png)

## nuget发布地址(可以手动下载安装/引入):
https://www.nuget.org/packages/YiKdWebClient

# 2.配置文件设置:
## 配置文件路径
配置的相对路径如下  YiKdWebCfg/appsettings.xml ，用于依赖于第三方登录授权验证和API签名验证，也可以自己实例化YiK3CloudClient中的AppSettingsModel类

## 配置文件内容(相对路径YiKdWebCfg/appsettings.xml 文件，如果没有就手动创建)
注意：(最新公有云可能强制要求走网关(https://api.kingdee.com/galaxyapi/)<br>
走网关的方式需要使用API签名认证的模式。<br>
最新询问总部（2024年10月)，目前不再强制公有云使用网关模式，公有云可以正常调用api，后续实际情况根据官方为准。框架功能里面已经全部包含，均可使用

```
<?xml version="1.0" encoding="utf-8" ?>
<configuration>

  <appSettings>

    <!-- 当前使用的 账套ID(即数据中心id) -->

    <!-- 第三方系统登录授权的账套ID（即open.kingdee.com网站的第三方系统登录授权中的数据中心标识）-->

    <!-- 在第三方系统登录授权页面点击“生成测试链接”按钮后即可查看   -->

    <add key="X-KDApi-AcctID" value="629bd5285d655d"/>

    <!-- 第三方系统登录授权的 集成用户名称  -->

    <!-- 补丁版本为PT-146894 [7.7.0.202111]及后续的版本，则为指定用户登录列表中任一用户  -->

    <!-- 若第三方系统登录授权已勾选“允许全部用户登录”，则无以上限制  -->

    <add key="X-KDApi-UserName" value="Administrator"/>

    <!-- 第三方系统登录授权的 应用ID  -->

    <add key="X-KDApi-AppID" value="2********************P"/>

    <!-- 第三方系统登录授权的 应用密钥  -->

    <add key="X-KDApi-AppSec" value="a***********************7"/>

    <!-- 账套语系，默认2052  -->

    <add key="X-KDApi-LCID" value="2052"/>

    <!-- 组织编码，启用多组织时配置对应的组织编码才有效(使用签名模式认证有效，其他待测试，可以先不填写)  -->

    <!--<add key="X-KDApi-OrgNum" value="*****"/>-->

    <!-- 服务Url地址(私有云必须配置金蝶云星空产品地址，K3Cloud/结尾。若为需要走公有云网关模式,则必须置空)-->

    <add key="X-KDApi-ServerUrl" value="http://127.0.0.1/k3cloud/"/>
  </appSettings>

</configuration>
```

# 3.API调用教程以及代码示例:
## 1.签名信息认证:<br>(需要设置配置文件:YiKdWebCfg/appsettings.xml) 
(目前推荐方式)注意：PT-146911 8.0.0.202205 之前的版本不支持SHA256加密，需要使用SHA1加密算法

```
string Formid = "SEC_User";
string Json = @"{""IsUserModelInit"":""true"",""Number"":""Administrator"",""IsSortBySeq"":""false""}";
YiK3CloudClient yiK3CloudClient = new YiKdWebClient.YiK3CloudClient();
yiK3CloudClient.LoginType= LoginType.LoginBySignSHA1;
//yiK3CloudClient.LoginType= LoginType.LoginBySignSHA256;
string resultJson = yiK3CloudClient.View(Formid, Json);



/*如下信息为可以使用postman调试的报文和地址*/
Console.WriteLine("真实的登录地址: ");
Console.WriteLine(yiK3CloudClient.ReturnLoginWebModel.RequestUrl);
Console.WriteLine("真实的登录报文: ");
Console.WriteLine(yiK3CloudClient.ReturnLoginWebModel.RealRequestBody);
//真实的操作请求地址
string RequestUrl = yiK3CloudClient.ReturnOperationWebModel.RequestUrl;
Console.WriteLine("真实的操作请求地址: ");
Console.WriteLine(RequestUrl);
//真实的操作请求报文
string RealRequestBody = yiK3CloudClient.ReturnOperationWebModel.RealRequestBody;
Console.WriteLine("真实的操作请求报文: ");
Console.WriteLine(RealRequestBody);
Console.WriteLine("真实的操作请求返回结果: ");
Console.WriteLine(resultJson);
Console.ReadKey();
```
完整的请求以及返回示例:
![输入图片说明](%E8%AF%B7%E6%B1%82%E4%BB%A5%E5%8F%8A%E8%BF%94%E5%9B%9E%E7%A4%BA%E4%BE%8B.png)
## 2.第三方授权认证:<br>(需要设置配置文件:YiKdWebCfg/appsettings.xml)



```
///第三方授权认证
string Formid = "SEC_User";
string Json =@"{""IsUserModelInit"":""true"",""Number"":""Administrator"",""IsSortBySeq"":""false""}";
YiK3CloudClient yiK3CloudClient = new YiKdWebClient.YiK3CloudClient();
yiK3CloudClient.LoginType = LoginType.LoginByAppSecret;
var resultJson  = yiK3CloudClient.View(Formid, Json);
```



可以获取到请求的真实地址，和真实请求的body 如下图
![输入图片说明](%E8%AF%B7%E6%B1%82.png)

可以利用此信息，使用postman 等接口调试工具进行调试，更方便快捷。 也可以使用其它开发语言进行请求，原理一致


## 3.旧版用户名密码认证:(不需要设置appsettings.xml)
 
```
///旧版用户名密码认证
 string Formid = "SEC_User";
 string Json =@"{""IsUserModelInit"":""true"",""Number"":""Administrator"",""IsSortBySeq"":""false""}";
 YiK3CloudClient yiK3CloudClient = new YiKdWebClient.YiK3CloudClient();
 yiK3CloudClient.LoginType= LoginType.ValidateLogin;
 yiK3CloudClient.validateLoginSettingsModel=new ValidateLoginSettingsModel() { Url = @"http://127.0.0.1/K3Cloud/", DbId= "629bd5285d655d", UserName="demo",Password="123456",lcid=2052};
 var resultJson = yiK3CloudClient.View(Formid, Json);
```
## 4.集成密钥文件认证:(不需要设置appsettings.xml)
```
string Formid = "SEC_User";
string Json = @"{""IsUserModelInit"":""true"",""Number"":""Administrator"",""IsSortBySeq"":""false""}";
YiK3CloudClient yiK3CloudClient = new YiKdWebClient.YiK3CloudClient();
yiK3CloudClient.LoginType = LoginType.LoginBySimplePassport;
string cnfFilePath = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "YiKdWebCfg", "API测试.cnf");
yiK3CloudClient.LoginBySimplePassportModel = new LoginBySimplePassportModel() { Url = @"http://127.0.0.1/K3Cloud/", CnfFilePath = cnfFilePath };
var resultJson = yiK3CloudClient.View(Formid, Json);
```

## 5.API请求头签名:(需要设置配置文件:YiKdWebCfg/appsettings.xml)

~~~
string Formid = "SEC_User";
string Json = @"{""IsUserModelInit"":""true"",""Number"":""Administrator"",""IsSortBySeq"":""false""}";
YiK3CloudClient yiK3CloudClient = new YiKdWebClient.YiK3CloudClient();
yiK3CloudClient.LoginType=LoginType.LoginByApiSignHeaders;
var resultJson = yiK3CloudClient.View(Formid, Json);
Console.WriteLine(resultJson);
~~~
API请求头签名认证的最大特点是，真实的请求中，没有调用登陆验证接口，web请求次数会大幅度降低<br>
(注:但是官方已经删除了这种方式对应的帖子已经算法，使用的时候需要慎重)


## 工具调试postman，Apipost等：
如下为使用postman，Apipost 工具的方法
~~~
 //签名请求头的字符串，可以直接导入postman，Apipost
string RequestHeadersString = yiK3CloudClient.RequestHeadersString;
Console.WriteLine("签名请求头的字符串，可以直接导入postman，Apipost:");
Console.WriteLine(RequestHeadersString);
//真实的请求地址
string RequestUrl = yiK3CloudClient.ReturnOperationWebModel.RequestUrl;
Console.WriteLine("真实的请求地址: ");
Console.WriteLine(RequestUrl);
//真实的请求报文
string RealRequestBody = yiK3CloudClient.ReturnOperationWebModel.RealRequestBody;
Console.WriteLine("真实的请求报文: ");
Console.WriteLine(RealRequestBody);
Console.WriteLine("请求结果: ");
Console.WriteLine(resultJson);
~~~

运行结果如下:
![输入图片说明](API%E7%AD%BE%E5%90%8D%E6%A8%A1%E5%BC%8F%E8%BF%90%E8%A1%8C%E6%95%88%E6%9E%9C.png)

## JSON格式说明
传入方法的JSON格式，与金蝶官方文档要求的格式完全一致. 注:(官方文档的JSON格式，并不是最终http请求的格式)
## 功能列表
(功能名称与官方功能名方式相同，以此类推),具体如下:<br>

| 接口名称 | 接口含义 |
|------|------|
|Save|保存|
|BatchSave|批量保存|
|Audit|审核|
|Delete|删除|
|UnAudit|反审核|
|Submit|提交|
|View|查看|
|ExecuteBillQuery|单据查询|
|Draft|暂存|
|Allocate|分配|
|ExecuteOperation|操作接口|
|FlexSave|弹性域保存|
|SendMsg|发送消息|
|Push|下推|
|GroupSave|分组保存|
|Disassembly|拆单|
|QueryBusinessInfo|查询单据信息|
|QueryGroupInfo|查询分组信息|
|WorkflowAudit|工作流审批|
|GroupDelete|分组删除|
|CancelAllocate|取消分配|
|SwitchOrg|切换组织接口|
|CancelAssign|撤销服务接口|
|GetSysReportData|获取报表数据|
|AttachmentUpload|上传附件|
|AttachmentDownLoad|下载附件|

## 如何不通过配置文件配置第三方登陆授权信息
1、客户存在多个星空环境或者一个星空环境存在多个数据中心时，通过框架的配置文件只允许配置一个第三方登录授权信息，这个时候应该怎么做系统集成对接呢？<br>
2、第三方登录授权信息中配置了集成用户，做webapi集成时，操作用户就为配置信息中的集成用户，但是做不同操作需要切换不同的用户时，应该怎么来配置？<br>
当环境信息需要动态的变化时，这个时候我们就不能使用配置文件的方式初始化框架了，需要通过参数化的方式动态的初始化实例，传递不同的配置信息，不同的集成用户，再进行对应的接口调用。
```
string Formid = "SEC_User";
string Json = @"{""IsUserModelInit"":""true"",""Number"":""Administrator"",""IsSortBySeq"":""false""}";
AppSettingsModel appSettingsModel = new AppSettingsModel();
appSettingsModel.XKDApiAcctID = "账套ID(即数据中心id)";
appSettingsModel.XKDApiUserName = "第三方系统登录授权的用户名称";
appSettingsModel.XKDApiAppID = "第三方系统登录授权的 应用ID";
appSettingsModel.XKDApiAppSec = "第三方系统登录授权的 应用密钥";
appSettingsModel.XKDApiLCID = "账套语系，默认2052";
appSettingsModel.XKDApiServerUrl = "Url地址";
YiK3CloudClient yiK3CloudClient = new YiKdWebClient.YiK3CloudClient();
yiK3CloudClient.AppSettingsModel = appSettingsModel;
yiK3CloudClient.LoginType = LoginType.LoginByAppSecret;
string resultJson = yiK3CloudClient.View(Formid, Json);
```

# 4.单点登录功能

## 调用示例代码:
此方法(需要设置配置文件:YiKdWebCfg/appsettings.xml),或者在sSOHelper.appSettingsModel中参数动态指定

```
SSOHelper sSOHelper = new SSOHelper(){};
/*若指定了配置文件，仅需要在此指定用户即可，若不指定则自动获取配置文件中的集成用户*/
sSOHelper.GetSsoUrlsV4("Administrator");/*单点登录V4*/
//sSOHelper.GetSsoUrlsV3("Administrator");/*单点登录V3*/
//sSOHelper.GetSsoUrlsV2("Administrator");/*单点登录V2*/
//sSOHelper.GetSsoUrlsV1("Administrator");/*单点登录V1*/
/*****如下为获取到的相关单点登录相关数据***********************************/
//数据中心ID
Console.WriteLine("数据中心ID：" + " " + sSOHelper.simplePassportLoginArg.dbid);
//应用ID
Console.WriteLine("应用ID：" + " " + sSOHelper.simplePassportLoginArg.appid);
//用户名称
Console.WriteLine("用户名称：" + " " + sSOHelper.simplePassportLoginArg.username);
//时间戳
Console.WriteLine("时间戳：" + " " + sSOHelper.timestamp);
//签名
Console.WriteLine("签名：" + " " + sSOHelper.simplePassportLoginArg.signeddata);
//请求参数（json格式）
Console.WriteLine("请求参数（json格式）：" + " " + sSOHelper.argJosn);
//参数格式化（Base64）
Console.WriteLine("参数格式化（Base64）：" + " " + sSOHelper.argJsonBase64);
// Silverlight入口链接
Console.WriteLine("Silverlight入口链接:");
Console.WriteLine(sSOHelper.SSOLoginUrlObject.silverlightUrl);
// html5入口链接
Console.WriteLine("html5入口链接:");
Console.WriteLine(sSOHelper.SSOLoginUrlObject.html5Url);
//客户端入口链接
Console.WriteLine("客户端入口链接:");
Console.WriteLine(sSOHelper.SSOLoginUrlObject.wpfUrl);
Console.ReadKey();
```
## 返回结果:
![输入图片说明](SSOV4%E7%BB%93%E6%9E%9C.png)


# 5.其他特殊功能以及用法
## 自定义配置文件路径
```
/*在运行之前，对如下的参数指定配置文件的路径*/
YiKdWebClient.CommonService.XmlConfigHelper.AppConfigPath = @"C:\Users\Administrator\Desktop\test\appsettings.xml";
string Formid = "SEC_User";
string Json = @"{""IsUserModelInit"":""true"",""Number"":""Administrator"",""IsSortBySeq"":""false""}";
YiK3CloudClient yiK3CloudClient = new YiKdWebClient.YiK3CloudClient();
yiK3CloudClient.LoginType = LoginType.LoginBySignSHA256;
string resultJson = yiK3CloudClient.View(Formid, Json);
```
## 自定义webapi
报文格式和请求参数的获取参考如下官方文档:<br>
https://vip.kingdee.com/article/97030089581136896?specialId=448928749460099072&productLineId=1&isKnowledge=2&lang=zh-CN

```
YiK3CloudClient yiK3CloudClient = new YiKdWebClient.YiK3CloudClient();
string jsonString = @" { ""parameters"": [ ""SELECT TOP 10 * FROM T_BD_MATERIAL_L"" ] }";
YiKdWebClient.Model.CustomServicesStubpath customServicesStubpath = new()
{
  ProjetNamespace = "GlobalServiceCustom.WebApi",/*dll的命名空间*/
  ProjetClassName = "DataServiceHandler",/*对应的类名*/
  ProjetClassMethod = "CommonRunnerService"/*对应的方法名*/
};
string resultJson = yiK3CloudClient.CustomBusinessServiceByParameters(jsonString, customServicesStubpath);


/*如下信息为可以使用postman调试的报文和地址*/
Console.WriteLine("真实的登录地址: ");
Console.WriteLine(yiK3CloudClient.ReturnLoginWebModel.RequestUrl);
Console.WriteLine("真实的登录报文: ");
Console.WriteLine(yiK3CloudClient.ReturnLoginWebModel.RealRequestBody);
//真实的操作请求地址
string RequestUrl = yiK3CloudClient.ReturnOperationWebModel.RequestUrl;
Console.WriteLine("真实的操作请求地址: ");
Console.WriteLine(RequestUrl);
//真实的操作请求报文
string RealRequestBody = yiK3CloudClient.ReturnOperationWebModel.RealRequestBody;
Console.WriteLine("真实的操作请求报文: ");
Console.WriteLine(RealRequestBody);
Console.WriteLine("真实的操作请求返回结果: ");
Console.WriteLine(resultJson);
Console.ReadKey();
```
返回结果的示例:<br>
![输入图片说明](%E8%87%AA%E5%AE%9A%E4%B9%89webapi%E7%BB%93%E6%9E%9C.png)
## 文件上传:
### 文件分块上传(直接返回最终结果)

```
YiK3CloudClient yiK3CloudClient = new YiKdWebClient.YiK3CloudClient();

// 设置登录类型
yiK3CloudClient.LoginType = LoginType.LoginBySimplePassport;

// 配置集成密钥路径
string cnfFilePath = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "YiKdWebCfg", "API测试.cnf");

// 设置登录信息
yiK3CloudClient.LoginBySimplePassportModel = new LoginBySimplePassportModel()
{
    Url = @"http://127.0.0.1/K3Cloud/",
    CnfFilePath = cnfFilePath
};

// 文件路径
string path = @"D:\test1.pdf";

// 创建上传模型
UploadModel uploadModelTemplate = new UploadModel();
uploadModelTemplate.data.FormId = "BD_Currency";
uploadModelTemplate.data.InterId = "143717";
uploadModelTemplate.data.BillNO = "测试编码";

// 上传附件
string resJson = AttachmentHelper.AttachmentUploadByFilePath(path, yiK3CloudClient, uploadModelTemplate, 1024 * 1024 * 2);

// 输出结果
Console.WriteLine(resJson);

```


### 文件分块上传(获取完整的上传过程)

```
YiK3CloudClient yiK3CloudClient = new YiKdWebClient.YiK3CloudClient();

// 设置登录类型
yiK3CloudClient.LoginType = LoginType.LoginBySimplePassport;

// 配置集成密钥路径
string cnfFilePath = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "YiKdWebCfg", "API测试.cnf");

// 设置登录信息
yiK3CloudClient.LoginBySimplePassportModel = new LoginBySimplePassportModel()
{
    Url = @"http://127.0.0.1/K3Cloud/",
    CnfFilePath = cnfFilePath
};

// 文件路径
string path = @"D:\test1.pdf";

// 创建上传模型
UploadModel uploadModelTemplate = new UploadModel();
uploadModelTemplate.data.FormId = "BD_Currency";
uploadModelTemplate.data.InterId = "143717";
uploadModelTemplate.data.BillNO = "测试编码";

// 定义上传进度回调
Action<FileChunk, YiK3CloudClient> progressAction = (fileChunk, yiK3CloudClient) =>
{
    Console.WriteLine("正在处理第" + (fileChunk.Chunkindex + 1) + "分块");
    Console.WriteLine("请求报文为:" + yiK3CloudClient.ReturnOperationWebModel.RealRequestBody);
    Console.WriteLine("处理结果为:" + yiK3CloudClient.ReturnOperationWebModel.RealResponseBody);
    if (fileChunk.IsLast)
    {
        Console.WriteLine("所有分块处理结束");
    }
};

// 上传附件
string resJson = AttachmentHelper.AttachmentUploadByFilePath(path,yiK3CloudClient,uploadModelTemplate,1024 * 1024 * 2,progressAction);

// 输出结果
Console.WriteLine(resJson);

```

### base64流分块上传辅助函数
AttachmentUploadByFilePath函数更换为AttachmentUploadByBase64

### 官方报文结构以及原理
https://vip.kingdee.com/article/296577252589190400?productLineId=1&isKnowledge=2&lang=zh-CN

# 框架兼容性说明
当前已经支持编译的版本如下:<br>
net10.0;net9.0;net8.0;net7.0;net6.0;net5.0;net481;net48;net472;net471;net47;net462;netstandard2.1;netstandard2.0;

# 框架基础依赖说明
基于如下原生类库编写，不包含第三方插件(如下插件不需要额外引入)<br>
System.Net.Http;<br>System.Text.Json;<br>System.Security.Cryptography.Cng;
# 项目地址:
## gitee:
https://gitee.com/lnsyzjw/yi-kd-web-client
## github:
https://github.com/1609676823/YiKdWebClient
# 官方的webapi接口说明书
https://vip.kingdee.com/knowledge/407944297590364160?productLineId=1&isKnowledge=2&lang=zh-CN
<br>[点此下载](%E9%87%91%E8%9D%B6%E4%BA%91%E6%98%9F%E7%A9%BAWebAPI%E6%8E%A5%E5%8F%A3%E8%AF%B4%E6%98%8E%E4%B9%A6_V6.0.docx)





