# YiKdWebClient 测试说明

本测试项目使用 xUnit v3。每一个公开业务接口都对应独立的 `[Fact]`，因此可以在 Visual Studio“测试资源管理器”中单独运行某个接口，也可以按类运行一组接口，或一次运行全部测试。

## 在 Visual Studio 中运行

1. 打开 `YiKdWebClient.sln`。
2. 选择“测试”→“测试资源管理器”。
3. 展开 `YiKdWebClient.Tests`：
   - 点击某个测试方法旁的运行按钮：只测试一个接口；
   - 右键测试类并选择“运行”：测试一个模块；
   - 点击“全部运行”：运行整个项目的全部测试。

## 命令行运行

在解决方案根目录执行全部测试：

```powershell
dotnet test .\YiKdWebClient.Tests\YiKdWebClient.Tests.csproj
```

只运行一个接口测试（以 `View` 为例）：

```powershell
dotnet test .\YiKdWebClient.Tests\YiKdWebClient.Tests.csproj --filter "FullyQualifiedName=YiKdWebClient.Tests.YiK3CloudClientApiTests.View_sends_View_request"
```

只运行一个测试类：

```powershell
dotnet test .\YiKdWebClient.Tests\YiKdWebClient.Tests.csproj --filter "FullyQualifiedName~YiK3CloudClientApiTests"
```

生成覆盖率文件：

```powershell
dotnet test .\YiKdWebClient.Tests\YiKdWebClient.Tests.csproj --collect:"XPlat Code Coverage"
```

## 覆盖模块

- `YiK3CloudClientApiTests`：查看、保存、提交、审核、查询、附件、自定义服务等全部业务接口及重载；
- `YiK3CloudClientAuthenticationTests`：全部登录模式及登出；
- `AuthenticationTests`：登录报文、SHA1/SHA256/API Header 签名、集成密钥；
- `WebHelperTests`：HTTP 方法、Header、Cookie、Query、Raw、Form、Multipart 和错误状态；
- `JsonHelperServicesTests`：标准请求/登录请求封装与 JSON 转义；
- `SsoHelperTests`：SSO V1–V4 登录 URL、登出签名和登出请求；
- `AttachmentHelperTests`：文件/Base64 分块、上传进度、校验和失败响应；
- 其余测试类：配置、模型、枚举、媒体类型、哈希、编码和生命周期。

所有 HTTP 测试均连接到测试进程内的本机临时服务，不访问真实金蝶环境，也不需要账号、密钥或网络连接。
