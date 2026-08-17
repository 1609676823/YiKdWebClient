# C# 截图工具使用说明

本文件专门说明同目录 `generate-readme-screenshots.ps1` 的使用方法。脚本运行 `ConsoleTestNet80` 中的示例，并使用 Microsoft Edge 无头模式把控制台输出渲染为 PNG。

## 文件与输出位置

- 生成脚本：`docs/generate-readme-screenshots.ps1`
- 示例项目：`ConsoleTestNet80/ConsoleTestNet80.csproj`
- 输出目录：`docs/screenshots`
- 默认构建配置：`Debug`
- 默认目标框架：`net8.0`

脚本会生成 `01-sign-sha256.png` 至 `14-validate-user-endecode.png`。现有的 `00-nuget-install.png` 不由该脚本生成。

## 前置条件

1. Windows PowerShell 5.1 或 PowerShell 7。
2. 已安装支持 `net8.0` 的 .NET SDK，`dotnet` 可在 `PATH` 中使用。
3. 已安装 Microsoft Edge。脚本会在 32 位和 64 位 Program Files 目录中自动查找 `msedge.exe`。
4. 已按测试环境准备项目配置、CNF 和上传示例文件。
5. 必须在当前进程设置 `YIKD_VALIDATE_PASSWORD`。脚本即使只生成非密码场景，也会在启动时检查该变量。

该脚本连接当前配置指向的环境，不会自动创建 mock 服务。只能使用专用测试环境；上传场景可能写入业务数据。

## 生成全部场景

在项目根目录执行：

~~~powershell
$env:YIKD_VALIDATE_PASSWORD = "请替换为测试用户的真实密码"

& .\docs\generate-readme-screenshots.ps1

Remove-Item Env:\YIKD_VALIDATE_PASSWORD
~~~

如果执行策略阻止脚本，可使用：

~~~powershell
$env:YIKD_VALIDATE_PASSWORD = "请替换为测试用户的真实密码"

powershell.exe -NoProfile -ExecutionPolicy Bypass `
  -File .\docs\generate-readme-screenshots.ps1

Remove-Item Env:\YIKD_VALIDATE_PASSWORD
~~~

## 只生成指定场景

单个场景：

~~~powershell
& .\docs\generate-readme-screenshots.ps1 `
  -ExampleCommand sign-sha256
~~~

多个场景：

~~~powershell
& .\docs\generate-readme-screenshots.ps1 `
  -ExampleCommand "sign-sha256","app-secret"
~~~

未知场景名称会直接报错，不会生成图片。

## 参数

| 参数 | 默认值 | 说明 |
| --- | --- | --- |
| `-Configuration` | `Debug` | 传给 `dotnet build` 和 `dotnet run` 的构建配置 |
| `-Framework` | `net8.0` | 目标框架 |
| `-ExampleCommand` | 全部场景 | 只运行一个或多个指定场景 |

示例：

~~~powershell
& .\docs\generate-readme-screenshots.ps1 `
  -Configuration Release `
  -Framework net8.0 `
  -ExampleCommand sign-sha256
~~~

## 支持的场景

| 命令 | 输出文件 |
| --- | --- |
| `sign-sha256` | `01-sign-sha256.png` |
| `sign-sha1` | `02-sign-sha1.png` |
| `app-secret` | `03-app-secret.png` |
| `validate-login` | `04-validate-login.png` |
| `simple-passport` | `05-simple-passport.png` |
| `api-sign-headers` | `06-api-sign-headers.png` |
| `dynamic-config` | `07-dynamic-config.png` |
| `custom-config-path` | `08-custom-config-path.png` |
| `custom-webapi` | `09-custom-webapi.png` |
| `sso-v4` | `10-sso-v4.png` |
| `upload-file` | `11-upload-file.png` |
| `upload-progress` | `12-upload-progress.png` |
| `upload-base64` | `13-upload-base64.png` |
| `validate-user-endecode` | `14-validate-user-endecode.png` |

## 执行流程

1. 构建 `ConsoleTestNet80`。
2. 设置进程级截图模式。
3. 逐个执行所选示例并检查退出码。
4. 再次替换 `YIKD_VALIDATE_PASSWORD`，避免密码进入图片。
5. 将 HTML 编码后的控制台输出写入临时 HTML。
6. 使用 Edge 无头模式生成 1440 像素宽的 PNG。
7. 删除每个临时 HTML，并恢复原来的进程环境变量。

## 失败排查

- “找不到 Microsoft Edge”：安装 Edge，或确认其位于标准 Program Files 路径。
- “请先设置 YIKD_VALIDATE_PASSWORD”：在同一 PowerShell 进程中设置该变量。
- 构建失败：先运行 `dotnet build .\ConsoleTestNet80\ConsoleTestNet80.csproj -f net8.0` 查看完整错误。
- 示例失败：直接运行对应的 `dotnet run` 命令，检查配置、CNF、权限、服务地址和目标业务数据。
- Edge 未生成文件：确认 `docs/screenshots` 可写，并检查安全软件是否阻止无头 Edge。

生成完成后应人工检查图片内容和 `git diff`，确认没有不应提交的认证信息或业务数据。
