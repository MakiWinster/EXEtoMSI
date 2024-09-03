using System.Diagnostics;
using System.IO;
using System.Windows;
using Microsoft.Win32; // 确保使用的是 WPF 的 OpenFileDialog
using WinForms = System.Windows.Forms; // 使用 WinForms 别名来处理 Windows Forms 类型

namespace MsiExtractorApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void SelectExeButton_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "Executable files (*.exe)|*.exe"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                ExeFilePathTextBox.Text = openFileDialog.FileName;
            }
        }

        private void SelectOutputFolderButton_Click(object sender, RoutedEventArgs e)
        {
            using (var dialog = new WinForms.FolderBrowserDialog())
            {
                var result = dialog.ShowDialog();
                if (result == WinForms.DialogResult.OK)
                {
                    OutputFolderTextBox.Text = dialog.SelectedPath;
                }
            }
        }

        private void ExtractButton_Click(object sender, RoutedEventArgs e)
        {
            string exePath = ExeFilePathTextBox.Text ?? string.Empty;
            string outputFolder = OutputFolderTextBox.Text ?? string.Empty;

            if (File.Exists(exePath) && Directory.Exists(outputFolder))
            {
                ExtractMsiUsingDark(exePath, outputFolder);
            }
            else
            {
                WinForms.MessageBox.Show("请确保选择了有效的EXE文件和输出路径。");
            }
        }

        private void ExtractMsiUsingDark(string exePath, string outputFolder)
        {
            // PowerShell 脚本的内容
            string scriptContent = $@"
        $ErrorActionPreference = 'Stop'
        cd $Env:WIX\bin
        ./dark.exe -x '{outputFolder}' '{exePath}'
    ";

            // 创建临时 PowerShell 脚本文件
            string scriptPath = Path.Combine(Path.GetTempPath(), "extract_msi.ps1");
            File.WriteAllText(scriptPath, scriptContent);

            var startInfo = new ProcessStartInfo
            {
                FileName = "powershell.exe",
                Arguments = $"-ExecutionPolicy Bypass -File \"{scriptPath}\"",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            try
            {
                using (Process process = Process.Start(startInfo))
                {
                    string output = process.StandardOutput.ReadToEnd();
                    string error = process.StandardError.ReadToEnd();

                    process.WaitForExit();

                    Debug.WriteLine("Output: " + output);
                    Debug.WriteLine("Error: " + error);

                    if (process.ExitCode != 0)
                    {
                        System.Windows.MessageBox.Show("提取失败，请检查日志。");
                    }
                    else
                    {
                        System.Windows.MessageBox.Show("提取完成！");
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("发生异常：" + ex.Message);
            }
        }

    }
}
