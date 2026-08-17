# YiKdWebClient

## YiKdWebClient 多语言项目

YiKdWebClient 是一个面向 **金蝶云星空 WebAPI** 的多语言开源客户端项目。各语言版本尽量保持一致的认证方式、公开方法名、参数顺序、服务路径和调用体验，方便不同技术栈对照接入。

当前项目提供 **C#、Java、Python、Go、PHP 和 HTTP (JSON)** 六种接入方式，均已完成适配。各版本使用独立仓库，并同时维护 Gitee 和 GitHub 地址。HTTP (JSON) 是不限定编程语言的通用接入版本；后续公共功能、协议报文和通用接入说明统一以其仓库 README 为准，各语言版本 README 主要维护安装、依赖、命名、异常/错误处理和同步/异步等语言特性。

| 接入版本 | 适配状态 | 当前基准 | Gitee | GitHub |
| --- | --- | --- | --- | --- |
| C# | 已适配，当前项目 | `1.0.0.32` | [YiKdWebClient C#](https://gitee.com/lnsyzjw/yi-kd-web-client) | [YiKdWebClient C#](https://github.com/1609676823/YiKdWebClient) |
| Java | 已适配 | 对标 C# `1.0.0.32` | [YiKdWebClient Java](https://gitee.com/lnsyzjw/yi-kd-web-client-java) | [YiKdWebClient Java](https://github.com/1609676823/YiKdWebClient-Java) |
| Python | 已适配 | 对标 C# `1.0.0.32` | [YiKdWebClient Python](https://gitee.com/lnsyzjw/yi-kd-web-client-python) | [YiKdWebClient Python](https://github.com/1609676823/YiKdWebClient-Python) |
| Go | 已适配 | Go `v1.0.0`，对标 C# `1.0.0.32` | [YiKdWebClient Go](https://gitee.com/lnsyzjw/yi-kd-web-client-go) | [YiKdWebClient Go](https://github.com/1609676823/YiKdWebClient-Go) |
| PHP | 已适配 | 对标 C# `1.0.0.32` | [YiKdWebClient PHP](https://gitee.com/lnsyzjw/yi-kd-web-client-php) | [YiKdWebClient PHP](https://github.com/1609676823/YiKdWebClient-PHP) |
| HTTP (JSON) | 已适配，通用接入 | 以 HTTP (JSON) 仓库 README 为准 | [YiKdWebClient HTTP](https://gitee.com/lnsyzjw/yi-kd-web-client-http) | [YiKdWebClient HTTP](https://github.com/1609676823/YiKdWebClient-HTTP) |

### 当前仓库

YiKdWebClient 是一个用于调用 **金蝶云星空 WebAPI** 的轻量级 .NET 客户端。项目使用原生 HTTP 协议实现，移除了对金蝶官方 SDK 和 `Newtonsoft.Json` 的依赖，可用于 .NET、.NET Framework 与 .NET Standard 项目。

### 共同功能范围

所有已适配语言版本共同覆盖：

- 7 个认证枚举：SHA256 签名、SHA1 签名、第三方系统登录授权、API 请求头签名、旧版用户名密码、集成密钥/CNF，以及仅为兼容旧系统保留的 `ValidateUserEnDeCode`；
- 查看、保存、批量保存、提交、审核、反审核、删除、查询、下推、分配等动态表单 WebAPI；
- 默认自动登录/登出、可选手动会话复用和 Cookie 管理；
- 单点登录 SSO V1～V4、SSO 登出参数与登出请求；
- 自定义 WebAPI 服务路径组装和调用；
- 文件路径与 Base64 附件分块上传、分块进度和最终返回；
- 默认 XML 配置、自定义配置路径和运行时动态传入授权信息；
- 登录与业务请求的实际 URL、请求头、请求体和响应体，便于使用 Postman、ApiPost 等工具排查问题。

> [!WARNING]
> 配置模板、mock 输出或本地测试截图只用于演示。接入自己的环境时，必须替换数据中心 ID、集成用户、应用 ID、应用密钥、服务地址和集成密钥文件。请勿把生产密钥、生产密码、CNF、Cookie 或长期有效的会话信息提交到公开仓库。

> [!IMPORTANT]
> 旧版用户名密码认证只用于协议兼容。示例代码使用 `123456` 作为明确占位符，复制后必须替换成目标环境中的真实测试密码；展示内容中的密码已统一脱敏。

> [!NOTE]
> 部分代码、测试、文档、示例或其他项目内容，可能在维护者指导和审查下借助 AI 工具生成、补全、重构或校对。AI 辅助内容在合并或发布前仍会由维护者进行审查和必要验证；使用者也应结合实际金蝶版本、补丁、权限和业务数据，自行评估正确性、安全性与适用性。

## 目录

- [1. 相关资料](#1-相关资料)
- [2. 安装](#2-安装)
- [3. 配置 appsettings.xml](#3-配置-appsettingsxml)
- [4. 五分钟运行第一个示例](#4-五分钟运行第一个示例)
- [5. ConsoleTestNet80 示例运行器](#5-consoletestnet80-示例运行器)
- [6. 认证与请求示例](#6-认证与请求示例)
- [7. JSON 参数与接口功能列表](#7-json-参数与接口功能列表)
- [8. 单点登录 SSO](#8-单点登录-sso)
- [9. 自定义 WebAPI](#9-自定义-webapi)
- [10. 文件与 Base64 分块上传](#10-文件与-base64-分块上传)
- [11. 框架兼容性与依赖](#11-框架兼容性与依赖)
- [12. 常见问题](#12-常见问题)
- [13. 项目地址](#13-项目地址)

## 1. 相关资料

- 金蝶云星空官方原始报文与地址结构说明：<https://vip.kingdee.com/knowledge/528587883691785472?productLineId=1&isKnowledge=2&lang=zh-CN>
- 官方 WebAPI 接口说明：<https://vip.kingdee.com/knowledge/407944297590364160?productLineId=1&isKnowledge=2&lang=zh-CN>
- 仓库内 WebAPI 接口说明书：[金蝶云星空 WebAPI 接口说明书 V6.0](./金蝶云星空WebAPI接口说明书_V6.0.docx)
- 仓库内 Postman 集合：[星空 WebAPI Postman Collection](./星空WebAPI.postman_collection.json.zip)
- NuGet：<https://www.nuget.org/packages/YiKdWebClient>

金蝶官方文档中的 JSON 是调用参数格式，不一定等于最终发送到 HTTP 接口的外层报文。YiKdWebClient 会把参数包装为金蝶 WebAPI 所需格式；最终报文可以通过客户端的 `ReturnLoginWebModel` 和 `ReturnOperationWebModel` 查看。

## 2. 安装

### 2.1 Visual Studio NuGet 管理器

在解决方案资源管理器中右击项目，选择“管理 NuGet 程序包”，搜索并安装 `YiKdWebClient`。

![Visual Studio 中安装 YiKdWebClient](docs/screenshots/00-nuget-install.png)

### 2.2 Package Manager Console

```powershell
Install-Package YiKdWebClient
```

### 2.3 .NET CLI

```powershell
dotnet add package YiKdWebClient
```

如果直接引用本仓库源码，可参考 `ConsoleTestNet80/ConsoleTestNet80.csproj` 中的 `ProjectReference`。

## 3. 配置 appsettings.xml

### 3.1 默认路径

默认配置文件相对运行目录的位置是：

```text
YiKdWebCfg/appsettings.xml
```

本仓库共有三份源配置，已统一为同一套本地测试认证信息：

- `YiKdWebClient/YiKdWebCfg/appsettings.xml`
- `ConsoleTestNet48/YiKdWebCfg/appsettings.xml`
- `ConsoleTestNet80/YiKdWebCfg/appsettings.xml`

构建时配置文件会复制到输出目录。请修改源文件，不要只修改 `bin` 或 `obj` 目录中的临时副本。

### 3.2 完整配置示例

下面是当前仓库的本地测试配置。**使用者必须替换为自己的授权信息。**

```xml
<?xml version="1.0" encoding="utf-8" ?>
<configuration>
  <appSettings>
    <!-- 数据中心 ID / 账套 ID -->
    <add key="X-KDApi-AcctID" value="6979b9812f3f89"/>

    <!-- 第三方系统登录授权中的集成用户 -->
    <add key="X-KDApi-UserName" value="Administrator"/>

    <!-- 第三方系统登录授权的应用 ID -->
    <add key="X-KDApi-AppID" value="354749_36dv7cio6mC5X8zLX/6tUa0M6JSU6sKE"/>

    <!-- 第三方系统登录授权的应用密钥；请替换 -->
    <add key="X-KDApi-AppSec" value="c1f59a3747c94804b6417872f1b272a6"/>

    <!-- 账套语系，简体中文通常为 2052 -->
    <add key="X-KDApi-LCID" value="2052"/>

    <!-- 启用多组织时可填写组织编码 -->
    <!--<add key="X-KDApi-OrgNum" value="100"/>-->

    <!-- 私有云通常填写以 K3Cloud/ 结尾的地址 -->
    <add key="X-KDApi-ServerUrl" value="http://127.0.0.1/K3Cloud/"/>
  </appSettings>
</configuration>
```

### 3.3 配置项说明

| 配置项 | 是否常用 | 说明 |
| --- | --- | --- |
| `X-KDApi-AcctID` | 是 | 数据中心 ID，也称账套 ID。可在第三方系统登录授权页面生成测试链接后查看。 |
| `X-KDApi-UserName` | 是 | 集成用户。PT-146894 `[7.7.0.202111]` 及后续版本可使用指定用户登录列表中的用户；若授权允许全部用户登录，则不受该列表限制。 |
| `X-KDApi-AppID` | 是 | 第三方系统登录授权的应用 ID。 |
| `X-KDApi-AppSec` | 是 | 第三方系统登录授权的应用密钥。不要使用生产密钥运行公开示例。 |
| `X-KDApi-LCID` | 是 | 账套语系，默认值为 `2052`。 |
| `X-KDApi-OrgNum` | 否 | 多组织场景中的组织编码，主要用于签名认证模式。 |
| `X-KDApi-ServerUrl` | 是 | 私有云填写产品地址，并以 `K3Cloud/` 结尾；使用公有云网关时按官方要求配置。 |

### 3.4 私有云与公有云网关

私有云通常配置产品地址并以 `K3Cloud/` 结尾；部分公有云环境可能要求通过 `https://api.kingdee.com/galaxyapi/` 网关并使用 API 请求头签名。实际地址与认证规则应以目标环境和金蝶官方当前要求为准。各语言客户端均保留普通登录与 API 请求头签名能力。

## 4. 五分钟运行第一个示例

以下步骤适合第一次接触项目的开发者。

1. 安装 .NET 8 SDK，并确认 `dotnet --info` 可以正常执行。
2. 确认本机能够访问 `X-KDApi-ServerUrl`，当前示例地址是 `http://127.0.0.1/K3Cloud/`。
3. 将三份 `appsettings.xml` 替换为自己的数据中心、应用和服务地址。
4. 在仓库根目录构建示例：

   ```powershell
   dotnet build .\ConsoleTestNet80\ConsoleTestNet80.csproj -f net8.0
   ```

5. 查看全部示例命令：

   ```powershell
   dotnet run --project .\ConsoleTestNet80\ConsoleTestNet80.csproj -f net8.0 -- help
   ```

6. 运行推荐的 SHA256 签名示例：

   ```powershell
   dotnet run --project .\ConsoleTestNet80\ConsoleTestNet80.csproj -f net8.0 -- sign-sha256
   ```

控制台会依次显示登录请求与响应、业务请求与响应以及方法返回值。HTTP 请求完成并不代表业务一定成功，请继续检查返回报文中的 `LoginResultType`、`IsSuccessByAPI`、`ResponseStatus.IsSuccess`、`ErrorCode` 和 `Message`。

## 5. ConsoleTestNet80 示例运行器

README 中的示例已经移植到 `ConsoleTestNet80`。每个示例都有独立命令，不需要反复注释和取消注释 `Program.cs`。

```text
sign-sha256       签名信息认证（SHA256）
sign-sha1         签名信息认证（SHA1）
app-secret        第三方系统登录授权
validate-login    旧版用户名密码认证
validate-user-endecode 已弃用的 ValidateUserEnDeCode
simple-passport   集成密钥文件认证
api-sign-headers  API 请求头签名认证
dynamic-config    代码动态配置授权信息
custom-config-path 自定义配置文件路径
custom-webapi     调用自定义 WebAPI
sso-v4            单点登录 V4
upload-file       文件分块上传
upload-progress   文件分块上传（进度回调）
upload-base64     Base64 流分块上传
```

统一运行格式：

```powershell
dotnet run --project .\ConsoleTestNet80\ConsoleTestNet80.csproj -f net8.0 -- <示例命令>
```

### 5.1 可选环境变量

环境变量适合临时切换环境，不会修改仓库文件。

| 环境变量 | 用途 | 默认值或来源 |
| --- | --- | --- |
| `YIKD_CONFIG_PATH` | 自定义 `appsettings.xml` 路径 | 输出目录中的 `YiKdWebCfg/appsettings.xml` |
| `YIKD_CNF_PATH` | 集成密钥文件路径 | 输出目录中的 `YiKdWebCfg/API测试.cnf` |
| `YIKD_SERVER_URL` | 临时覆盖服务地址 | 从 XML 读取 |
| `YIKD_ACCT_ID` | 动态配置示例的数据中心 ID | 从 XML 读取 |
| `YIKD_USER_NAME` | 动态配置示例的集成用户 | 从 XML 读取 |
| `YIKD_APP_ID` | 动态配置示例的应用 ID | 从 XML 读取 |
| `YIKD_APP_SECRET` | 动态配置示例的应用密钥 | 从 XML 读取 |
| `YIKD_LCID` | 动态配置示例的语系 | 从 XML 读取 |
| `YIKD_ORG_NUM` | 动态配置示例的组织编码 | 从 XML 读取，可为空 |
| `YIKD_VALIDATE_DBID` | 旧版登录的数据中心 ID | 从 XML 读取 |
| `YIKD_VALIDATE_USERNAME` | 旧版登录用户名 | `demo` |
| `YIKD_VALIDATE_PASSWORD` | 旧版登录密码 | 无默认值，必须显式设置 |
| `YIKD_VALIDATE_LCID` | 旧版登录语系 | `2052` |
| `YIKD_UPLOAD_FILE` | 上传示例文件路径 | 输出目录中的 `SampleFiles/upload-demo.txt` |
| `YIKD_UPLOAD_FORM_ID` | 上传目标表单 ID | `SAL_SaleOrder` |
| `YIKD_UPLOAD_INTER_ID` | 上传目标单据内码 | `100020` |
| `YIKD_UPLOAD_BILL_NO` | 上传目标单据编号 | `XSDD000019` |
| `YIKD_UPLOAD_CHUNK_SIZE` | 上传分块大小（字节） | `2 * 1024 * 1024` |
| `YIKD_CUSTOM_SQL` | 自定义 WebAPI 示例 SQL | `SELECT TOP 10 * FROM T_BD_MATERIAL_L` |

## 6. 认证与请求示例

以下示例都查看 `SEC_User` 表单中的 `Administrator` 用户。为了方便初学者直接复制使用，本节**不共用任何代码变量**：每个代码块都重新声明 `formId`、`json`、`client` 和输出变量。只要项目已经引用 `YiKdWebClient`，并按第 3 节准备好配置文件，就可以把任意一个代码块单独复制到控制台项目的 `Program.cs` 中运行。

C# 基准共有 7 个 `LoginType` 枚举值：6 种可选认证模式，以及 1 种只为旧系统保留的兼容模式。

| `LoginType` | 用途 | 是否先登录 | 建议 |
| --- | --- | --- | --- |
| `LoginBySignSHA256` | SHA256 签名信息认证 | 是 | 支持 SHA256 的环境优先使用 |
| `LoginBySignSHA1` | SHA1 签名信息认证 | 是 | 仅用于兼容旧版本 |
| `LoginByAppSecret` | 第三方系统登录授权 | 是 | 按目标环境授权方式选择 |
| `LoginByApiSignHeaders` | 每个业务请求独立生成 API 签名请求头 | 否 | 使用前确认目标环境/网关支持 |
| `ValidateLogin` | 旧版用户名密码认证 | 是 | 旧系统兼容，不建议新系统优先使用 |
| `LoginBySimplePassport` | CNF 文件或 Base64 集成密钥认证 | 是 | 集成密钥场景 |
| `ValidateUserEnDeCode` | 已弃用的旧式用户名密码编码兼容 | 是 | 仅保留旧场景兼容 |

「7 个枚举值」不等于 7 种推荐方案。新项目通常从 `LoginBySignSHA256`、`LoginByAppSecret` 或目标网关要求的 `LoginByApiSignHeaders` 中选择。

控制台输出与客户端属性的对应关系如下；每个示例也会把这些属性先赋值给名称明确的局部变量，再输出到控制台：

| 输出内容 | 示例变量 | 数据来源 |
| --- | --- | --- |
| 表单 ID | `formId` | 调用方传给 `View` 的第一个参数 |
| 业务 JSON 参数 | `json` | 调用方传给 `View` 的第二个参数 |
| 登录请求地址 | `loginRequestUrl` | `client.ReturnLoginWebModel.RequestUrl` |
| 登录请求报文 | `loginRequestBody` | `client.ReturnLoginWebModel.RealRequestBody` |
| 登录返回报文 | `loginResponseBody` | `client.ReturnLoginWebModel.RealResponseBody` |
| 业务请求地址 | `operationRequestUrl` | `client.ReturnOperationWebModel.RequestUrl` |
| 业务请求报文 | `operationRequestBody` | `client.ReturnOperationWebModel.RealRequestBody` |
| 业务返回报文 | `operationResponseBody` | `client.ReturnOperationWebModel.RealResponseBody` |
| 方法返回值 | `resultJson` | `client.View(...)` 等方法的直接返回值 |

### 6.1 签名信息认证（SHA256，推荐）

支持 SHA256 的金蝶云星空版本优先使用此方式。

```csharp
using System;
using YiKdWebClient;
using YiKdWebClient.Model;

// 1. 本次 View 接口的两个业务参数。
const string formId = "SEC_User";
const string json = "{\"IsUserModelInit\":\"true\",\"Number\":\"Administrator\",\"IsSortBySeq\":\"false\"}";

// 2. 创建客户端，并指定 SHA256 签名认证。
using YiK3CloudClient client = new YiK3CloudClient
{
    LoginType = LoginType.LoginBySignSHA256
};

// 3. 发起请求；resultJson 就是 View 方法直接返回的 JSON。
string resultJson = client.View(formId, json);

// 4. 从客户端取出真实登录报文和业务报文。
string loginRequestUrl = client.ReturnLoginWebModel.RequestUrl;
string loginRequestBody = client.ReturnLoginWebModel.RealRequestBody;
string loginResponseBody = client.ReturnLoginWebModel.RealResponseBody;
string operationRequestUrl = client.ReturnOperationWebModel.RequestUrl;
string operationRequestBody = client.ReturnOperationWebModel.RealRequestBody;
string operationResponseBody = client.ReturnOperationWebModel.RealResponseBody;

Console.WriteLine($"表单 ID（formId）：{formId}");
Console.WriteLine($"业务 JSON 参数（json）：{json}");
Console.WriteLine($"登录请求地址（loginRequestUrl）：{loginRequestUrl}");
Console.WriteLine($"登录请求报文（loginRequestBody）：{loginRequestBody}");
Console.WriteLine($"登录返回报文（loginResponseBody）：{loginResponseBody}");
Console.WriteLine($"业务请求地址（operationRequestUrl）：{operationRequestUrl}");
Console.WriteLine($"业务请求报文（operationRequestBody）：{operationRequestBody}");
Console.WriteLine($"业务返回报文（operationResponseBody）：{operationResponseBody}");
Console.WriteLine($"View 方法返回值（resultJson）：{resultJson}");
```

运行：

```powershell
dotnet run --project .\ConsoleTestNet80\ConsoleTestNet80.csproj -f net8.0 -- sign-sha256
```

![SHA256 签名认证的实际请求与响应](docs/screenshots/01-sign-sha256.png)

### 6.2 签名信息认证（SHA1，兼容旧版本）

PT-146911 `8.0.0.202205` 之前的版本不支持 SHA256 时，可改用 SHA1。

```csharp
using System;
using YiKdWebClient;
using YiKdWebClient.Model;

// 本代码块是独立示例，不依赖 6.1 中的变量。
const string formId = "SEC_User";
const string json = "{\"IsUserModelInit\":\"true\",\"Number\":\"Administrator\",\"IsSortBySeq\":\"false\"}";

using YiK3CloudClient client = new YiK3CloudClient
{
    LoginType = LoginType.LoginBySignSHA1
};

string resultJson = client.View(formId, json);

string loginRequestUrl = client.ReturnLoginWebModel.RequestUrl;
string loginRequestBody = client.ReturnLoginWebModel.RealRequestBody;
string loginResponseBody = client.ReturnLoginWebModel.RealResponseBody;
string operationRequestUrl = client.ReturnOperationWebModel.RequestUrl;
string operationRequestBody = client.ReturnOperationWebModel.RealRequestBody;
string operationResponseBody = client.ReturnOperationWebModel.RealResponseBody;

Console.WriteLine($"表单 ID（formId）：{formId}");
Console.WriteLine($"业务 JSON 参数（json）：{json}");
Console.WriteLine($"登录请求地址（loginRequestUrl）：{loginRequestUrl}");
Console.WriteLine($"登录请求报文（loginRequestBody）：{loginRequestBody}");
Console.WriteLine($"登录返回报文（loginResponseBody）：{loginResponseBody}");
Console.WriteLine($"业务请求地址（operationRequestUrl）：{operationRequestUrl}");
Console.WriteLine($"业务请求报文（operationRequestBody）：{operationRequestBody}");
Console.WriteLine($"业务返回报文（operationResponseBody）：{operationResponseBody}");
Console.WriteLine($"View 方法返回值（resultJson）：{resultJson}");
```

运行：

```powershell
dotnet run --project .\ConsoleTestNet80\ConsoleTestNet80.csproj -f net8.0 -- sign-sha1
```

![SHA1 签名认证的实际请求与响应](docs/screenshots/02-sign-sha1.png)

### 6.3 第三方系统登录授权

该方式读取 `appsettings.xml` 中的数据中心 ID、集成用户、应用 ID 和应用密钥。

```csharp
using System;
using YiKdWebClient;
using YiKdWebClient.Model;

const string formId = "SEC_User";
const string json = "{\"IsUserModelInit\":\"true\",\"Number\":\"Administrator\",\"IsSortBySeq\":\"false\"}";

// AppSettingsModel 默认从输出目录的 YiKdWebCfg/appsettings.xml 读取配置。
using YiK3CloudClient client = new YiK3CloudClient
{
    LoginType = LoginType.LoginByAppSecret
};

string resultJson = client.View(formId, json);

// 这些变量说明本次认证配置来自哪里；应用密钥不输出明文。
string dataCenterId = client.AppSettingsModel.XKDApiAcctID;
string integrationUser = client.AppSettingsModel.XKDApiUserName;
string appId = client.AppSettingsModel.XKDApiAppID;
string serverUrl = client.AppSettingsModel.XKDApiServerUrl;

string loginRequestUrl = client.ReturnLoginWebModel.RequestUrl;
string loginRequestBody = client.ReturnLoginWebModel.RealRequestBody;
string loginResponseBody = client.ReturnLoginWebModel.RealResponseBody;
string operationRequestUrl = client.ReturnOperationWebModel.RequestUrl;
string operationRequestBody = client.ReturnOperationWebModel.RealRequestBody;
string operationResponseBody = client.ReturnOperationWebModel.RealResponseBody;

Console.WriteLine($"配置文件中的数据中心 ID（dataCenterId）：{dataCenterId}");
Console.WriteLine($"配置文件中的集成用户（integrationUser）：{integrationUser}");
Console.WriteLine($"配置文件中的应用 ID（appId）：{appId}");
Console.WriteLine("配置文件中的应用密钥：******");
Console.WriteLine($"配置文件中的服务地址（serverUrl）：{serverUrl}");
Console.WriteLine($"表单 ID（formId）：{formId}");
Console.WriteLine($"业务 JSON 参数（json）：{json}");
Console.WriteLine($"登录请求地址（loginRequestUrl）：{loginRequestUrl}");
Console.WriteLine($"登录请求报文（loginRequestBody）：{loginRequestBody}");
Console.WriteLine($"登录返回报文（loginResponseBody）：{loginResponseBody}");
Console.WriteLine($"业务请求地址（operationRequestUrl）：{operationRequestUrl}");
Console.WriteLine($"业务请求报文（operationRequestBody）：{operationRequestBody}");
Console.WriteLine($"业务返回报文（operationResponseBody）：{operationResponseBody}");
Console.WriteLine($"View 方法返回值（resultJson）：{resultJson}");
```

运行：

```powershell
dotnet run --project .\ConsoleTestNet80\ConsoleTestNet80.csproj -f net8.0 -- app-secret
```

![第三方系统登录授权的实际请求与响应](docs/screenshots/03-app-secret.png)

### 6.4 旧版用户名密码认证

旧版认证不依赖 `appsettings.xml` 中的应用 ID 和应用密钥，但需要服务地址、数据中心 ID、用户名、密码和语系。除兼容旧系统外，不建议新项目优先使用用户名密码方式。

```csharp
using System;
using YiKdWebClient;
using YiKdWebClient.Model;

const string formId = "SEC_User";
const string json = "{\"IsUserModelInit\":\"true\",\"Number\":\"Administrator\",\"IsSortBySeq\":\"false\"}";

string serverUrl = "http://127.0.0.1/K3Cloud/";
string dataCenterId = "6979b9812f3f89";
string userName = "demo";
string password = "123456"; // 示例密码，请替换为目标环境用户的实际密码
int localeId = 2052;

using YiK3CloudClient client = new YiK3CloudClient
{
    LoginType = LoginType.ValidateLogin,
    validateLoginSettingsModel = new ValidateLoginSettingsModel
    {
        Url = serverUrl,
        DbId = dataCenterId,
        UserName = userName,
        Password = password,
        lcid = localeId
    }
};

string resultJson = client.View(formId, json);

string loginRequestUrl = client.ReturnLoginWebModel.RequestUrl;
string loginRequestBody = client.ReturnLoginWebModel.RealRequestBody;
string loginResponseBody = client.ReturnLoginWebModel.RealResponseBody;
string operationRequestUrl = client.ReturnOperationWebModel.RequestUrl;
string operationRequestBody = client.ReturnOperationWebModel.RealRequestBody;
string operationResponseBody = client.ReturnOperationWebModel.RealResponseBody;

// 登录请求体中含有密码，输出前替换为星号；真实请求不受影响。
string maskedLoginRequestBody = loginRequestBody.Replace(password, "******");

Console.WriteLine($"服务地址（serverUrl）：{serverUrl}");
Console.WriteLine($"数据中心 ID（dataCenterId）：{dataCenterId}");
Console.WriteLine($"用户名（userName）：{userName}");
Console.WriteLine("密码（password）：******");
Console.WriteLine($"语系（localeId）：{localeId}");
Console.WriteLine($"表单 ID（formId）：{formId}");
Console.WriteLine($"业务 JSON 参数（json）：{json}");
Console.WriteLine($"登录请求地址（loginRequestUrl）：{loginRequestUrl}");
Console.WriteLine($"登录请求报文（maskedLoginRequestBody）：{maskedLoginRequestBody}");
Console.WriteLine($"登录返回报文（loginResponseBody）：{loginResponseBody}");
Console.WriteLine($"业务请求地址（operationRequestUrl）：{operationRequestUrl}");
Console.WriteLine($"业务请求报文（operationRequestBody）：{operationRequestBody}");
Console.WriteLine($"业务返回报文（operationResponseBody）：{operationResponseBody}");
Console.WriteLine($"View 方法返回值（resultJson）：{resultJson}");
```

把代码复制到控制台项目的 `Program.cs` 后直接运行即可，不需要设置环境变量：

```powershell
dotnet run
```

`123456` 仅用于说明密码变量应填写在哪里，接入时必须替换成目标环境中 `userName` 对应用户的真实密码。截图中的登录请求使用了本地测试密码，但展示前已替换为 `******`。

![旧版用户名密码认证的实际请求与响应，密码已脱敏](docs/screenshots/04-validate-login.png)

### 6.5 已弃用的 `ValidateUserEnDeCode`

> [!CAUTION]
> `ValidateUserEnDeCode` 已通过 `[Obsolete]` 标记为弃用。它会对用户名和密码执行可逆的旧式编码，并调用 `Kingdee.BOS.WebApi.ServicesStub.AuthService.ValidateUserEnDeCode.common.kdsvc`。金蝶官方通用 WebAPI 登录说明未推荐这种方式，项目保留它只是为了兼容曾经出现过的旧版本附件等历史场景。编码后的密码仍然必须按密码本身保护；新项目请优先使用 SHA256 签名认证或当前环境支持的其他认证方式。

该模式与普通 `ValidateLogin` 使用相同的 `ValidateLoginSettingsModel`，区别是把 `LoginType` 设置为 `LoginType.ValidateUserEnDeCode`。下面的代码包含全部 `using`、认证参数、业务调用、真实请求/响应读取和脱敏输出，可直接复制到已经引用 `YiKdWebClient` 的控制台项目 `Program.cs` 中运行：

```csharp
using System;
using YiKdWebClient;
using YiKdWebClient.Model;

#pragma warning disable CS0618 // 本示例专门演示已弃用的旧系统兼容模式。

const string formId = "SEC_User";
const string json = "{\"IsUserModelInit\":\"true\",\"Number\":\"Administrator\",\"IsSortBySeq\":\"false\"}";

string serverUrl = "http://127.0.0.1/K3Cloud/";
string dataCenterId = "6979b9812f3f89";
string userName = "demo";
string password = "123456"; // 示例密码，请替换为目标环境用户的实际密码
int localeId = 2052;

using YiK3CloudClient client = new YiK3CloudClient
{
    LoginType = LoginType.ValidateUserEnDeCode,
    validateLoginSettingsModel = new ValidateLoginSettingsModel
    {
        Url = serverUrl,
        DbId = dataCenterId,
        UserName = userName,
        Password = password,
        lcid = localeId
    }
};

string resultJson = client.View(formId, json);

string compatibilityMode = client.LoginType?.ToString() ?? string.Empty;
string loginRequestUrl = client.ReturnLoginWebModel.RequestUrl;
string loginRequestBody = client.ReturnLoginWebModel.RealRequestBody;
string loginResponseBody = client.ReturnLoginWebModel.RealResponseBody;
string operationRequestUrl = client.ReturnOperationWebModel.RequestUrl;
string operationRequestBody = client.ReturnOperationWebModel.RealRequestBody;
string operationResponseBody = client.ReturnOperationWebModel.RealResponseBody;

// 旧式编码可逆，因此编码后的值也要按密码本身脱敏。
string encodedPassword = EnDecode.Encode(password);
string maskedLoginRequestBody = loginRequestBody
    .Replace(password, "******")
    .Replace(encodedPassword, "******（旧式编码值已脱敏）");

Console.WriteLine($"兼容模式（compatibilityMode）：{compatibilityMode}");
Console.WriteLine($"服务地址（serverUrl）：{serverUrl}");
Console.WriteLine($"数据中心 ID（dataCenterId）：{dataCenterId}");
Console.WriteLine($"用户名（userName）：{userName}");
Console.WriteLine("密码（password）：******");
Console.WriteLine($"语系（localeId）：{localeId}");
Console.WriteLine($"表单 ID（formId）：{formId}");
Console.WriteLine($"业务 JSON 参数（json）：{json}");
Console.WriteLine($"登录请求地址（loginRequestUrl）：{loginRequestUrl}");
Console.WriteLine($"登录请求报文（maskedLoginRequestBody）：{maskedLoginRequestBody}");
Console.WriteLine($"登录返回报文（loginResponseBody）：{loginResponseBody}");
Console.WriteLine($"业务请求地址（operationRequestUrl）：{operationRequestUrl}");
Console.WriteLine($"业务请求报文（operationRequestBody）：{operationRequestBody}");
Console.WriteLine($"业务返回报文（operationResponseBody）：{operationResponseBody}");
Console.WriteLine($"View 方法返回值（resultJson）：{resultJson}");

#pragma warning restore CS0618
```

把代码复制到控制台项目的 `Program.cs` 后直接运行即可，不需要设置环境变量：

```powershell
dotnet run
```

仓库内置的真实环境验证命令如下。它从环境变量读取实际测试密码，不会把密码写入源码；运行结束后请删除当前 PowerShell 会话中的临时变量：

```powershell
$env:YIKD_VALIDATE_PASSWORD = '<替换为目标环境的实际测试密码>'
dotnet run --project .\ConsoleTestNet80\ConsoleTestNet80.csproj -f net8.0 -- validate-user-endecode
Remove-Item Env:\YIKD_VALIDATE_PASSWORD
```

`123456` 仅用于说明密码变量应填写在哪里，接入时必须替换成 `userName` 对应用户的真实密码。下图展示本地测试环境的调用结果，密码在展示前已替换为 `******`。

![已弃用的 ValidateUserEnDeCode 实际请求与响应，密码已脱敏](docs/screenshots/14-validate-user-endecode.png)

### 6.6 集成密钥文件认证

将目标环境生成的 `.cnf` 集成密钥文件放到 `YiKdWebCfg`，并确保它会复制到输出目录。

```csharp
using System;
using System.IO;
using YiKdWebClient;
using YiKdWebClient.Model;

const string formId = "SEC_User";
const string json = "{\"IsUserModelInit\":\"true\",\"Number\":\"Administrator\",\"IsSortBySeq\":\"false\"}";

string cnfFilePath = Path.Combine(
    AppContext.BaseDirectory,
    "YiKdWebCfg",
    "API测试.cnf");
string serverUrl = "http://127.0.0.1/K3Cloud/";

using YiK3CloudClient client = new YiK3CloudClient
{
    LoginType = LoginType.LoginBySimplePassport,
    LoginBySimplePassportModel = new LoginBySimplePassportModel
    {
        Url = serverUrl,
        CnfFilePath = cnfFilePath
    }
};

string resultJson = client.View(formId, json);

string loginRequestUrl = client.ReturnLoginWebModel.RequestUrl;
string loginRequestBody = client.ReturnLoginWebModel.RealRequestBody;
string loginResponseBody = client.ReturnLoginWebModel.RealResponseBody;
string operationRequestUrl = client.ReturnOperationWebModel.RequestUrl;
string operationRequestBody = client.ReturnOperationWebModel.RealRequestBody;
string operationResponseBody = client.ReturnOperationWebModel.RealResponseBody;

Console.WriteLine($"服务地址（serverUrl）：{serverUrl}");
Console.WriteLine($"集成密钥文件路径（cnfFilePath）：{cnfFilePath}");
Console.WriteLine($"表单 ID（formId）：{formId}");
Console.WriteLine($"业务 JSON 参数（json）：{json}");
Console.WriteLine($"登录请求地址（loginRequestUrl）：{loginRequestUrl}");
Console.WriteLine($"登录请求报文（loginRequestBody）：{loginRequestBody}");
Console.WriteLine($"登录返回报文（loginResponseBody）：{loginResponseBody}");
Console.WriteLine($"业务请求地址（operationRequestUrl）：{operationRequestUrl}");
Console.WriteLine($"业务请求报文（operationRequestBody）：{operationRequestBody}");
Console.WriteLine($"业务返回报文（operationResponseBody）：{operationResponseBody}");
Console.WriteLine($"View 方法返回值（resultJson）：{resultJson}");
```

运行：

```powershell
dotnet run --project .\ConsoleTestNet80\ConsoleTestNet80.csproj -f net8.0 -- simple-passport
```

![集成密钥文件认证的实际请求与响应](docs/screenshots/05-simple-passport.png)

### 6.7 API 请求头签名认证

API 请求头签名模式不会先调用登录验证接口，而是直接给业务请求生成签名请求头，因此可以减少一次 Web 请求。原文档同时提醒：官方已经删除过该方式对应的帖子和算法说明，生产使用前应确认目标版本仍然支持。

```csharp
using System;
using YiKdWebClient;
using YiKdWebClient.Model;

const string formId = "SEC_User";
const string json = "{\"IsUserModelInit\":\"true\",\"Number\":\"Administrator\",\"IsSortBySeq\":\"false\"}";

using YiK3CloudClient client = new YiK3CloudClient
{
    LoginType = LoginType.LoginByApiSignHeaders
};

string resultJson = client.View(formId, json);

// 该模式没有单独的登录请求，认证信息位于业务请求头中。
string requestHeaders = client.RequestHeadersString;
string operationRequestUrl = client.ReturnOperationWebModel.RequestUrl;
string operationRequestBody = client.ReturnOperationWebModel.RealRequestBody;
string operationResponseBody = client.ReturnOperationWebModel.RealResponseBody;

Console.WriteLine($"表单 ID（formId）：{formId}");
Console.WriteLine($"业务 JSON 参数（json）：{json}");
Console.WriteLine($"可复制到 Postman/ApiPost 的请求头（requestHeaders）：{requestHeaders}");
Console.WriteLine($"业务请求地址（operationRequestUrl）：{operationRequestUrl}");
Console.WriteLine($"业务请求报文（operationRequestBody）：{operationRequestBody}");
Console.WriteLine($"业务返回报文（operationResponseBody）：{operationResponseBody}");
Console.WriteLine($"View 方法返回值（resultJson）：{resultJson}");
```

运行：

```powershell
dotnet run --project .\ConsoleTestNet80\ConsoleTestNet80.csproj -f net8.0 -- api-sign-headers
```

![API 请求头签名认证的实际请求头、请求与响应](docs/screenshots/06-api-sign-headers.png)

### 6.8 不通过固定配置文件，动态传入授权信息

以下场景适合代码动态配置：

1. 同一服务需要连接多个金蝶环境或多个数据中心；
2. 不同业务操作需要切换集成用户；
3. 配置来自数据库、配置中心或环境变量，而不是固定 XML 文件。

```csharp
using System;
using YiKdWebClient;
using YiKdWebClient.Model;

const string formId = "SEC_User";
const string json = "{\"IsUserModelInit\":\"true\",\"Number\":\"Administrator\",\"IsSortBySeq\":\"false\"}";

// 这些值可以来自数据库、配置中心或环境变量；请替换为自己的环境信息。
string dataCenterId = "替换为数据中心 ID";
string integrationUser = "替换为集成用户";
string appId = "替换为应用 ID";
string appSecret = "替换为应用密钥";
string localeId = "2052";
string organizationNumber = ""; // 可为空
string serverUrl = "http://127.0.0.1/K3Cloud/";

AppSettingsModel settings = new AppSettingsModel
{
    XKDApiAcctID = dataCenterId,
    XKDApiUserName = integrationUser,
    XKDApiAppID = appId,
    XKDApiAppSec = appSecret,
    XKDApiLCID = localeId,
    XKDApiOrgNum = organizationNumber,
    XKDApiServerUrl = serverUrl
};

using YiK3CloudClient client = new YiK3CloudClient
{
    AppSettingsModel = settings,
    LoginType = LoginType.LoginByAppSecret
};

string resultJson = client.View(formId, json);

string loginRequestUrl = client.ReturnLoginWebModel.RequestUrl;
string loginRequestBody = client.ReturnLoginWebModel.RealRequestBody;
string loginResponseBody = client.ReturnLoginWebModel.RealResponseBody;
string operationRequestUrl = client.ReturnOperationWebModel.RequestUrl;
string operationRequestBody = client.ReturnOperationWebModel.RealRequestBody;
string operationResponseBody = client.ReturnOperationWebModel.RealResponseBody;

Console.WriteLine($"代码传入的数据中心 ID（dataCenterId）：{dataCenterId}");
Console.WriteLine($"代码传入的集成用户（integrationUser）：{integrationUser}");
Console.WriteLine($"代码传入的应用 ID（appId）：{appId}");
Console.WriteLine("代码传入的应用密钥（appSecret）：******");
Console.WriteLine($"代码传入的语系（localeId）：{localeId}");
Console.WriteLine($"代码传入的组织编码（organizationNumber）：{organizationNumber}");
Console.WriteLine($"代码传入的服务地址（serverUrl）：{serverUrl}");
Console.WriteLine($"表单 ID（formId）：{formId}");
Console.WriteLine($"业务 JSON 参数（json）：{json}");
Console.WriteLine($"登录请求地址（loginRequestUrl）：{loginRequestUrl}");
Console.WriteLine($"登录请求报文（loginRequestBody）：{loginRequestBody}");
Console.WriteLine($"登录返回报文（loginResponseBody）：{loginResponseBody}");
Console.WriteLine($"业务请求地址（operationRequestUrl）：{operationRequestUrl}");
Console.WriteLine($"业务请求报文（operationRequestBody）：{operationRequestBody}");
Console.WriteLine($"业务返回报文（operationResponseBody）：{operationResponseBody}");
Console.WriteLine($"View 方法返回值（resultJson）：{resultJson}");
```

运行：

```powershell
dotnet run --project .\ConsoleTestNet80\ConsoleTestNet80.csproj -f net8.0 -- dynamic-config
```

![代码动态配置授权信息的实际请求与响应](docs/screenshots/07-dynamic-config.png)

### 6.9 自定义配置文件路径

必须在创建 `YiK3CloudClient` 之前设置路径。

```csharp
using System;
using YiKdWebClient;
using YiKdWebClient.CommonService;
using YiKdWebClient.Model;

const string formId = "SEC_User";
const string json = "{\"IsUserModelInit\":\"true\",\"Number\":\"Administrator\",\"IsSortBySeq\":\"false\"}";

string configFilePath = @"D:\configs\kingdee\appsettings.xml";

// 必须先设置配置文件路径，再创建客户端。
XmlConfigHelper.AppConfigPath = configFilePath;

using YiK3CloudClient client = new YiK3CloudClient
{
    LoginType = LoginType.LoginBySignSHA256
};

string resultJson = client.View(formId, json);

string loginRequestUrl = client.ReturnLoginWebModel.RequestUrl;
string loginRequestBody = client.ReturnLoginWebModel.RealRequestBody;
string loginResponseBody = client.ReturnLoginWebModel.RealResponseBody;
string operationRequestUrl = client.ReturnOperationWebModel.RequestUrl;
string operationRequestBody = client.ReturnOperationWebModel.RealRequestBody;
string operationResponseBody = client.ReturnOperationWebModel.RealResponseBody;

Console.WriteLine($"本次配置文件路径（configFilePath）：{configFilePath}");
Console.WriteLine($"表单 ID（formId）：{formId}");
Console.WriteLine($"业务 JSON 参数（json）：{json}");
Console.WriteLine($"登录请求地址（loginRequestUrl）：{loginRequestUrl}");
Console.WriteLine($"登录请求报文（loginRequestBody）：{loginRequestBody}");
Console.WriteLine($"登录返回报文（loginResponseBody）：{loginResponseBody}");
Console.WriteLine($"业务请求地址（operationRequestUrl）：{operationRequestUrl}");
Console.WriteLine($"业务请求报文（operationRequestBody）：{operationRequestBody}");
Console.WriteLine($"业务返回报文（operationResponseBody）：{operationResponseBody}");
Console.WriteLine($"View 方法返回值（resultJson）：{resultJson}");
```

示例运行器默认通过 `YIKD_CONFIG_PATH` 指定路径：

```powershell
$env:YIKD_CONFIG_PATH = 'D:\configs\kingdee\appsettings.xml'
dotnet run --project .\ConsoleTestNet80\ConsoleTestNet80.csproj -f net8.0 -- custom-config-path
```

![自定义配置文件路径的实际请求与响应](docs/screenshots/08-custom-config-path.png)

### 6.10 如何查看真实请求和响应

前面每个完整示例都已经演示了真实报文属性，不需要再从本小节复制一段公共代码。理解下面三个对象的职责，就能判断应该读取哪个变量：

- `resultJson` 是当前方法的直接返回值，适合业务代码继续反序列化和判断成功状态；
- `client.ReturnLoginWebModel` 保存本次自动登录的 URL、请求体和响应体；
- `client.ReturnOperationWebModel` 保存本次业务操作的 URL、请求体和响应体；
- `client.RequestHeadersString` 仅在 API 请求头签名模式下用于查看可复制的签名请求头。

复制 URL、请求头和请求体到 Postman/ApiPost 时，要注意时间戳、随机数、签名和会话信息可能很快失效。调试完成后也不要把包含密码、密钥、Cookie 或 SessionId 的导出文件提交到仓库。

## 7. JSON 参数与接口功能列表

传给 YiKdWebClient 方法的 JSON 与金蝶官方文档要求的参数格式一致。客户端会负责外层 HTTP 报文包装。例如：

```csharp
using System;
using YiKdWebClient;
using YiKdWebClient.Model;

// formId 决定调用哪个业务对象；json 是该接口要求的业务参数。
string formId = "SEC_User";
string json = @"{""IsUserModelInit"":""true"",""Number"":""Administrator"",""IsSortBySeq"":""false""}";

using YiK3CloudClient client = new YiK3CloudClient
{
    LoginType = LoginType.LoginBySignSHA256
};

string resultJson = client.View(formId, json);

// resultJson 是可以直接反序列化的 View 方法返回值。
Console.WriteLine($"View 方法返回值（resultJson）：{resultJson}");
```

已封装的主要接口如下。功能名称尽量与金蝶官方名称保持一致：

| 方法 | 用途 |
| --- | --- |
| `View` | 查看单据或基础资料 |
| `Save`、`BatchSave`、`Draft`、`GroupSave`、`FlexSave` | 保存、批量保存、暂存、分组保存、弹性域保存 |
| `Submit`、`Audit`、`UnAudit`、`Delete`、`GroupDelete` | 提交、审核、反审核、删除和分组删除 |
| `ExecuteOperation`、`Push`、`Allocate`、`CancelAllocate`、`CancelAssign`、`Disassembly` | 通用操作、下推、分配、取消和拆单 |
| `ExecuteBillQuery`、`GetSysReportData`、`QueryBusinessInfo`、`QueryGroupInfo` | 单据查询、报表和业务信息查询 |
| `SendMsg`、`SwitchOrg`、`WorkflowAudit` | 消息、组织切换和工作流审批 |
| `AttachmentUpLoad`、`AttachmentDownLoad`、`UploadFile` | 原始附件/文件服务接口 |
| `CustomBusinessService`、`CustomBusinessServiceByParameters` | 自定义 WebAPI |
| `GetDataCenterList` | 获取数据中心列表 |

`ExecuteOperation` 的参数顺序为 `formId, opNumber, json`。自定义 WebAPI、附件高级封装和会话复用分别见后续对应章节。

## 8. 单点登录 SSO

项目支持 SSO V1、V2、V3 和 V4。下面以 V4 为例。它会在本地生成签名参数和入口 URL，不发送 HTTP 请求，因此没有“返回报文”。

```csharp
using System;
using YiKdWebClient.SSO;

// 本代码块可以独立运行。userName 是要免密登录的金蝶用户名。
string userName = "Administrator";
SSOHelper helper = new SSOHelper();

// 未单独传 URL 时，服务地址及认证信息来自 YiKdWebCfg/appsettings.xml。
helper.GetSsoUrlsV4(userName);

// 把输出中的每一项先赋给名称明确的变量，便于复制后继续使用。
string? dataCenterId = helper.simplePassportLoginArg.dbid;
string? appId = helper.simplePassportLoginArg.appid;
string? loginUserName = helper.simplePassportLoginArg.username;
long? timestamp = helper.timestamp;
string? signedData = helper.simplePassportLoginArg.signeddata;
string? argumentJson = helper.argJosn;
string? argumentBase64 = helper.argJsonBase64;
string silverlightUrl = helper.SSOLoginUrlObject.silverlightUrl;
string html5Url = helper.SSOLoginUrlObject.html5Url;
string wpfUrl = helper.SSOLoginUrlObject.wpfUrl;

Console.WriteLine($"数据中心 ID（dataCenterId）：{dataCenterId}");
Console.WriteLine($"应用 ID（appId）：{appId}");
Console.WriteLine($"登录用户名（loginUserName）：{loginUserName}");
Console.WriteLine($"时间戳（timestamp）：{timestamp}");
Console.WriteLine($"签名（signedData）：{signedData}");
Console.WriteLine($"原始 SSO 参数 JSON（argumentJson）：{argumentJson}");
Console.WriteLine($"Base64 参数（argumentBase64）：{argumentBase64}");
Console.WriteLine($"Silverlight 入口（silverlightUrl）：{silverlightUrl}");
Console.WriteLine($"HTML5 入口（html5Url）：{html5Url}");
Console.WriteLine($"WPF 入口（wpfUrl）：{wpfUrl}");

// 旧版本按目标环境需要选择：
// helper.GetSsoUrlsV3(userName);
// helper.GetSsoUrlsV2(userName);
// helper.GetSsoUrlsV1(userName);
```

构造并执行 SSO V4 登出：

```csharp
using System;
using YiKdWebClient.SSO;

string userName = "Administrator";
SSOHelper helper = new SSOHelper();

SSOLogoutObject logoutRequest = helper.GetSSOLogoutap0StrV4(userName);
string logoutResponse = helper.SSOExcuteLogout(logoutRequest);

Console.WriteLine($"登出用户名：{userName}");
Console.WriteLine($"登出地址：{logoutRequest.RequestLogoutUrl}");
Console.WriteLine($"登出响应：{logoutResponse}");
```

V3、V2/V1 分别使用 `GetSSOLogoutap0StrV3` 和 `GetSSOLogoutap0StrV2V1`。SSO URL 和签名参数属于敏感登录材料，不应写入公开日志。

运行：

```powershell
dotnet run --project .\ConsoleTestNet80\ConsoleTestNet80.csproj -f net8.0 -- sso-v4
```

![SSO V4 生成的真实签名参数与入口地址](docs/screenshots/10-sso-v4.png)

## 9. 自定义 WebAPI

官方自定义 WebAPI 报文格式与参数说明：<https://vip.kingdee.com/article/97030089581136896?specialId=448928749460099072&productLineId=1&isKnowledge=2&lang=zh-CN>

目标环境必须先部署服务端自定义 WebAPI。本仓库示例调用：

```text
GlobalServiceCustom.WebApi.DataServiceHandler.CommonRunnerService
```

客户端提供两类调用：`CustomBusinessService` 由客户端完成标准外层参数包装；`CustomBusinessServiceByParameters` 将调用者准备的 JSON 作为原始请求体发送。服务路径既可直接传字符串，也可通过 `CustomServicesStubpath` 由命名空间、类名和公开方法名生成；这些定位值必须与服务端部署内容完全一致。

> [!CAUTION]
> 不要把任意用户输入直接拼接到 SQL 或其他高权限服务参数中。服务端必须实施身份授权、参数校验、最小权限和审计。

### 9.1 服务端项目采用直接 DLL 引用

`GlobalServiceCustom.WebApi` 是 .NET Framework 4.8 类库，当前不再使用 `app.config`、程序集绑定重定向或 `packages.config`。编译所需程序集直接放在项目的 `kdbin` 目录，并通过 `.csproj` 中的相对 `HintPath` 引用：

- `Kingdee.BOS.dll`
- `Kingdee.BOS.ServiceFacade.KDServiceFx.dll`
- `Kingdee.BOS.ServiceHelper.dll`
- `Kingdee.BOS.WebApi.ServicesStub.dll`
- `Newtonsoft.Json.dll`

这些引用均设置了 `Private=False`。因此构建输出只有自定义插件 DLL 和可选 PDB，不会把金蝶运行时程序集复制到输出目录：

```powershell
dotnet build .\GlobalServiceCustom.WebApi\GlobalServiceCustom.WebApi.csproj -c Release
```

部署时使用 `GlobalServiceCustom.WebApi/bin/Release/GlobalServiceCustom.WebApi.dll`，不要用仓库 `kdbin` 中的 DLL 覆盖服务器程序集。更换目标金蝶环境或产品版本时，应从该目标环境取得同版本 DLL，替换 `kdbin` 中的编译引用后重新构建，避免不同补丁版本之间的 API 不兼容。

### 9.2 客户端调用

服务定位对象中的命名空间、类名和方法名必须与服务器端部署内容完全一致：

```csharp
using System;
using System.Text.Json;
using YiKdWebClient;
using YiKdWebClient.Model;

using YiK3CloudClient client = new YiK3CloudClient
{
    LoginType = LoginType.LoginByAppSecret
};

// sql 是传给服务端 CommonRunnerService 方法的实际参数。
string sql = "SELECT TOP 10 * FROM T_BD_MATERIAL_L";
string jsonString = JsonSerializer.Serialize(new
{
    parameters = new[] { sql }
});

// 三个定位值分别来自服务端项目的命名空间、类名和公开方法名。
string projectNamespace = "GlobalServiceCustom.WebApi";
string projectClassName = "DataServiceHandler";
string projectMethodName = "CommonRunnerService";

CustomServicesStubpath service = new CustomServicesStubpath
{
    ProjetNamespace = projectNamespace,
    ProjetClassName = projectClassName,
    ProjetClassMethod = projectMethodName
};

string resultJson = client.CustomBusinessServiceByParameters(jsonString, service);

string loginRequestUrl = client.ReturnLoginWebModel.RequestUrl;
string loginRequestBody = client.ReturnLoginWebModel.RealRequestBody;
string loginResponseBody = client.ReturnLoginWebModel.RealResponseBody;
string operationRequestUrl = client.ReturnOperationWebModel.RequestUrl;
string operationRequestBody = client.ReturnOperationWebModel.RealRequestBody;
string operationResponseBody = client.ReturnOperationWebModel.RealResponseBody;

Console.WriteLine($"服务端命名空间（projectNamespace）：{projectNamespace}");
Console.WriteLine($"服务端类名（projectClassName）：{projectClassName}");
Console.WriteLine($"服务端方法名（projectMethodName）：{projectMethodName}");
Console.WriteLine($"SQL 参数（sql）：{sql}");
Console.WriteLine($"接口参数 JSON（jsonString）：{jsonString}");
Console.WriteLine($"登录请求地址（loginRequestUrl）：{loginRequestUrl}");
Console.WriteLine($"登录请求报文（loginRequestBody）：{loginRequestBody}");
Console.WriteLine($"登录返回报文（loginResponseBody）：{loginResponseBody}");
Console.WriteLine($"业务请求地址（operationRequestUrl）：{operationRequestUrl}");
Console.WriteLine($"业务请求报文（operationRequestBody）：{operationRequestBody}");
Console.WriteLine($"业务返回报文（operationResponseBody）：{operationResponseBody}");
Console.WriteLine($"自定义接口返回值（resultJson）：{resultJson}");
```

原有服务调用结构截图保留在统一截图目录中：

![自定义 WebAPI 服务调用结构](docs/screenshots/custom-webapi-server.png)

运行：

```powershell
dotnet run --project .\ConsoleTestNet80\ConsoleTestNet80.csproj -f net8.0 -- custom-webapi
```

当前本地测试环境成功返回了 SQL 查询结果：

![自定义 WebAPI 的实际登录、请求与查询结果](docs/screenshots/09-custom-webapi.png)

## 10. 文件与 Base64 分块上传

官方附件上传报文结构与原理：<https://vip.kingdee.com/article/296577252589190400?productLineId=1&isKnowledge=2&lang=zh-CN>

附件上传会写入目标业务系统。接入前必须替换真实的表单 ID、单据内码和单据编号，并确认目标环境已配置附件或对象存储。高层封装支持文件路径、分块进度回调和 Base64 数据；每个成功分块返回的 `FileId` 会自动写回上传模型。

本仓库示例默认读取 `ConsoleTestNet80/SampleFiles/upload-demo.txt`。

### 10.1 文件路径分块上传，直接返回最终结果

```csharp
using System;
using System.IO;
using YiKdWebClient;
using YiKdWebClient.Model;
using YiKdWebClient.ToolsHelper;

// 本代码块包含全部输入，不依赖其他上传示例。
string serverUrl = "http://127.0.0.1/K3Cloud/";
string cnfFilePath = Path.Combine(AppContext.BaseDirectory, "YiKdWebCfg", "API测试.cnf");
string filePath = Path.Combine(AppContext.BaseDirectory, "SampleFiles", "upload-demo.txt");
string formId = "SAL_SaleOrder";
string interId = "100020";
string billNumber = "XSDD000019";
long chunkSize = 2L * 1024 * 1024;

if (!File.Exists(filePath))
{
    throw new FileNotFoundException("找不到待上传文件，请修改 filePath。", filePath);
}

using YiK3CloudClient client = new YiK3CloudClient
{
    LoginType = LoginType.LoginBySimplePassport,
    LoginBySimplePassportModel = new LoginBySimplePassportModel
    {
        Url = serverUrl,
        CnfFilePath = cnfFilePath
    }
};

UploadModel uploadModel = new UploadModel();
uploadModel.data.FormId = formId;
uploadModel.data.InterId = interId;
uploadModel.data.BillNO = billNumber;

string resultJson = AttachmentHelper.AttachmentUploadByFilePath(
    filePath,
    client,
    uploadModel,
    chunkSize);

string loginRequestUrl = client.ReturnLoginWebModel.RequestUrl;
string loginRequestBody = client.ReturnLoginWebModel.RealRequestBody;
string loginResponseBody = client.ReturnLoginWebModel.RealResponseBody;
string operationRequestUrl = client.ReturnOperationWebModel.RequestUrl;
string operationRequestBody = client.ReturnOperationWebModel.RealRequestBody;
string operationResponseBody = client.ReturnOperationWebModel.RealResponseBody;

Console.WriteLine($"待上传文件（filePath）：{filePath}");
Console.WriteLine($"文件大小：{new FileInfo(filePath).Length} 字节");
Console.WriteLine($"目标表单（formId）：{formId}");
Console.WriteLine($"单据内码（interId）：{interId}");
Console.WriteLine($"单据编号（billNumber）：{billNumber}");
Console.WriteLine($"分块大小（chunkSize）：{chunkSize} 字节");
Console.WriteLine($"登录请求地址（loginRequestUrl）：{loginRequestUrl}");
Console.WriteLine($"登录请求报文（loginRequestBody）：{loginRequestBody}");
Console.WriteLine($"登录返回报文（loginResponseBody）：{loginResponseBody}");
Console.WriteLine($"最后一块请求地址（operationRequestUrl）：{operationRequestUrl}");
Console.WriteLine($"最后一块请求报文（operationRequestBody）：{operationRequestBody}");
Console.WriteLine($"最后一块返回报文（operationResponseBody）：{operationResponseBody}");
Console.WriteLine($"上传方法返回值（resultJson）：{resultJson}");
```

运行：

```powershell
dotnet run --project .\ConsoleTestNet80\ConsoleTestNet80.csproj -f net8.0 -- upload-file
```

![从文件路径分块上传的实际请求与响应](docs/screenshots/11-upload-file.png)

### 10.2 文件路径分块上传，获取完整进度

```csharp
using System;
using System.IO;
using YiKdWebClient;
using YiKdWebClient.Model;
using YiKdWebClient.ToolsHelper;

// 10.2 是完整独立示例，因此重新声明客户端、文件参数和上传模型。
string serverUrl = "http://127.0.0.1/K3Cloud/";
string cnfFilePath = Path.Combine(AppContext.BaseDirectory, "YiKdWebCfg", "API测试.cnf");
string filePath = Path.Combine(AppContext.BaseDirectory, "SampleFiles", "upload-demo.txt");
string formId = "SAL_SaleOrder";
string interId = "100020";
string billNumber = "XSDD000019";
long chunkSize = 2L * 1024 * 1024;

if (!File.Exists(filePath))
{
    throw new FileNotFoundException("找不到待上传文件，请修改 filePath。", filePath);
}

using YiK3CloudClient client = new YiK3CloudClient
{
    LoginType = LoginType.LoginBySimplePassport,
    LoginBySimplePassportModel = new LoginBySimplePassportModel
    {
        Url = serverUrl,
        CnfFilePath = cnfFilePath
    }
};

UploadModel uploadModel = new UploadModel();
uploadModel.data.FormId = formId;
uploadModel.data.InterId = interId;
uploadModel.data.BillNO = billNumber;

Action<FileChunk, YiK3CloudClient> progress = (chunk, currentClient) =>
{
    long chunkNumber = chunk.Chunkindex + 1;
    bool isLastChunk = chunk.IsLast;
    string chunkRequestUrl = currentClient.ReturnOperationWebModel.RequestUrl;
    string chunkRequestBody = currentClient.ReturnOperationWebModel.RealRequestBody;
    string chunkResponseBody = currentClient.ReturnOperationWebModel.RealResponseBody;

    Console.WriteLine($"当前分块序号（chunkNumber）：{chunkNumber}");
    Console.WriteLine($"是否最后一块（isLastChunk）：{isLastChunk}");
    Console.WriteLine($"当前分块请求地址（chunkRequestUrl）：{chunkRequestUrl}");
    Console.WriteLine($"当前分块请求报文（chunkRequestBody）：{chunkRequestBody}");
    Console.WriteLine($"当前分块返回报文（chunkResponseBody）：{chunkResponseBody}");

    if (isLastChunk)
    {
        Console.WriteLine("所有分块处理结束");
    }
};

string resultJson = AttachmentHelper.AttachmentUploadByFilePath(
    filePath,
    client,
    uploadModel,
    chunkSize,
    progress);

Console.WriteLine($"待上传文件（filePath）：{filePath}");
Console.WriteLine($"目标表单（formId）：{formId}");
Console.WriteLine($"单据内码（interId）：{interId}");
Console.WriteLine($"单据编号（billNumber）：{billNumber}");
Console.WriteLine($"分块大小（chunkSize）：{chunkSize} 字节");
Console.WriteLine($"上传方法最终返回值（resultJson）：{resultJson}");
```

运行：

```powershell
dotnet run --project .\ConsoleTestNet80\ConsoleTestNet80.csproj -f net8.0 -- upload-progress
```

![带进度回调的文件分块上传实际请求与响应](docs/screenshots/12-upload-progress.png)

### 10.3 Base64 流分块上传

把文件内容转换为 Base64 后，调用 `AttachmentUploadByBase64`：

```csharp
using System;
using System.IO;
using YiKdWebClient;
using YiKdWebClient.Model;
using YiKdWebClient.ToolsHelper;

// 10.3 同样包含全部变量，可单独复制运行。
string serverUrl = "http://127.0.0.1/K3Cloud/";
string cnfFilePath = Path.Combine(AppContext.BaseDirectory, "YiKdWebCfg", "API测试.cnf");
string filePath = Path.Combine(AppContext.BaseDirectory, "SampleFiles", "upload-demo.txt");
string formId = "SAL_SaleOrder";
string interId = "100020";
string billNumber = "XSDD000019";
long chunkSize = 2L * 1024 * 1024;

if (!File.Exists(filePath))
{
    throw new FileNotFoundException("找不到待上传文件，请修改 filePath。", filePath);
}

string base64 = Convert.ToBase64String(File.ReadAllBytes(filePath));

using YiK3CloudClient client = new YiK3CloudClient
{
    LoginType = LoginType.LoginBySimplePassport,
    LoginBySimplePassportModel = new LoginBySimplePassportModel
    {
        Url = serverUrl,
        CnfFilePath = cnfFilePath
    }
};

UploadModel uploadModel = new UploadModel();
uploadModel.data.FormId = formId;
uploadModel.data.InterId = interId;
uploadModel.data.BillNO = billNumber;

string resultJson = AttachmentHelper.AttachmentUploadByBase64(
    base64,
    Path.GetFileName(filePath),
    client,
    uploadModel,
    chunkSize);

string loginRequestUrl = client.ReturnLoginWebModel.RequestUrl;
string loginRequestBody = client.ReturnLoginWebModel.RealRequestBody;
string loginResponseBody = client.ReturnLoginWebModel.RealResponseBody;
string operationRequestUrl = client.ReturnOperationWebModel.RequestUrl;
string operationRequestBody = client.ReturnOperationWebModel.RealRequestBody;
string operationResponseBody = client.ReturnOperationWebModel.RealResponseBody;

Console.WriteLine($"源文件（filePath）：{filePath}");
Console.WriteLine($"Base64 字符数（base64.Length）：{base64.Length}");
Console.WriteLine($"目标表单（formId）：{formId}");
Console.WriteLine($"单据内码（interId）：{interId}");
Console.WriteLine($"单据编号（billNumber）：{billNumber}");
Console.WriteLine($"分块大小（chunkSize）：{chunkSize} 字节");
Console.WriteLine($"登录请求地址（loginRequestUrl）：{loginRequestUrl}");
Console.WriteLine($"登录请求报文（loginRequestBody）：{loginRequestBody}");
Console.WriteLine($"登录返回报文（loginResponseBody）：{loginResponseBody}");
Console.WriteLine($"最后一块请求地址（operationRequestUrl）：{operationRequestUrl}");
Console.WriteLine($"最后一块请求报文（operationRequestBody）：{operationRequestBody}");
Console.WriteLine($"最后一块返回报文（operationResponseBody）：{operationResponseBody}");
Console.WriteLine($"上传方法返回值（resultJson）：{resultJson}");
```

运行：

```powershell
dotnet run --project .\ConsoleTestNet80\ConsoleTestNet80.csproj -f net8.0 -- upload-base64
```

![Base64 流分块上传的实际请求与响应](docs/screenshots/13-upload-base64.png)

### 10.4 `UploadModel` 字段用途

| 字段 | 用途 |
| --- | --- |
| `FileName` | 当前附件文件名，高层封装会按源文件填充 |
| `FormId` | 单据或表单 ID |
| `InterId` | 单据内码 |
| `Entrykey` | 单据体标识；表头附件留空 |
| `EntryinterId` | 单据体内码；表头附件通常使用默认值 `-1` |
| `BillNO` | 单据编号 |
| `AliasFileName` | 可选的附件别名 |
| `FileId` | 服务端返回的文件 ID，每个成功分块后自动更新 |
| `SendByte` | 当前分块的 Base64 内容，高层封装自动填充 |
| `IsLast` | 是否为最后一块，高层封装自动填充 |

> [!NOTE]
> 当前截图中的三个上传请求都真实到达了本地金蝶服务，但测试环境返回 `ErrorCode: 500`，消息指出附件存储配置项为空。它是服务器端测试环境配置结果，不是伪造的成功报文，也不是示例进程崩溃。正确配置附件存储，并替换成目标环境中真实存在的表单与单据后，再根据 `ResponseStatus.IsSuccess` 判断上传是否成功。

## 11. 框架兼容性与依赖

当前库项目配置的目标框架：

```text
net10.0
net9.0
net8.0
net7.0
net6.0
net5.0
net481
net48
net472
net471
net47
net462
netstandard2.1
netstandard2.0
```

框架主要基于 .NET/Microsoft 基础类库实现：

- `System.Net.Http`
- `System.Text.Json`
- `System.Security.Cryptography.Cng`

项目不依赖金蝶官方 SDK，也不依赖 `Newtonsoft.Json`。部分旧目标框架会通过 NuGet 引用相应版本的 Microsoft 基础包，业务方无需额外引入第三方 JSON 框架。

## 12. 常见问题

### 12.1 找不到 `YiKdWebCfg/appsettings.xml`

确认文件属性会复制到输出目录。使用本仓库示例时重新构建：

```powershell
dotnet build .\ConsoleTestNet80\ConsoleTestNet80.csproj -f net8.0
```

也可以设置 `YIKD_CONFIG_PATH`，或在创建客户端前设置 `XmlConfigHelper.AppConfigPath`。

### 12.2 返回登录失败

依次检查：

1. 数据中心 ID 是否属于当前服务地址；
2. 集成用户是否在第三方系统登录授权范围内；
3. 应用 ID 与应用密钥是否成对；
4. 语系和组织编码是否适用于当前账套；
5. 服务器时间是否准确，避免签名时间戳偏差；
6. 旧版登录是否正确设置 `YIKD_VALIDATE_PASSWORD`。

### 12.3 登录成功，但业务调用失败

登录成功只代表身份验证通过。继续检查业务返回中的 `ResponseStatus`、权限、表单 ID、字段名、单据状态和组织范围。将控制台打印的实际 URL、请求头和请求体复制到 Postman/ApiPost，可帮助区分客户端参数问题与服务端业务规则问题。

### 12.4 `API测试.cnf` 无法使用

`.cnf` 文件必须由目标环境生成，并与服务地址和数据中心匹配。复制其他环境的文件通常无法正常登录。替换文件后重新构建，或用 `YIKD_CNF_PATH` 指向新文件。

### 12.5 上传返回存储配置错误

这表示请求已经到达附件接口，但服务器没有正确配置文件存储。请先在金蝶环境中完成附件/对象存储配置，再确认表单 ID、单据内码和单据编号真实存在。

## 13. 项目地址

- C# Gitee：<https://gitee.com/lnsyzjw/yi-kd-web-client>
- C# GitHub：<https://github.com/1609676823/YiKdWebClient>
- Java Gitee：<https://gitee.com/lnsyzjw/yi-kd-web-client-java>
- Java GitHub：<https://github.com/1609676823/YiKdWebClient-Java>
- Python Gitee：<https://gitee.com/lnsyzjw/yi-kd-web-client-python>
- Python GitHub：<https://github.com/1609676823/YiKdWebClient-Python>
- Go Gitee：<https://gitee.com/lnsyzjw/yi-kd-web-client-go>
- Go GitHub：<https://github.com/1609676823/YiKdWebClient-Go>
- PHP Gitee：<https://gitee.com/lnsyzjw/yi-kd-web-client-php>
- PHP GitHub：<https://github.com/1609676823/YiKdWebClient-PHP>

本项目采用 [MIT License](./LICENSE)。
