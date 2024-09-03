# EXEtoMSI

EXEtoMSI 是一个用于将 EXE 文件提取为 MSI 文件的 Windows 应用程序。该程序可以帮助用户将安装包转换为 MSI 格式，便于进行后续的安装和管理。

## 技术栈

- **编程语言**: C#
- **框架**: WPF (.NET 8.0)
- **开发环境**: Visual Studio 2022
- **工具**: Wix Toolset（用于处理 MSI 文件）

## 安装和使用

 **安装 Wix Toolkit**:
   - 请从 [Wix Toolset 官方网站](https://wixtoolset.org/) 下载并安装 Wix Toolset。  
   - 安装完成后，请确保 `dark.exe` 和其他 Wix 工具位于 `Wix\bin` 目录下，并设置环境变量 `WIX` 指向该目录。  
   - 运行程序


## 技术细节

- 该应用程序使用 WPF 进行界面开发，使用 `System.Diagnostics.Process` 启动 PowerShell 脚本来运行 Wix 工具。


## 许可证

该项目使用 [MIT 许可证](LICENSE) 进行许可。
