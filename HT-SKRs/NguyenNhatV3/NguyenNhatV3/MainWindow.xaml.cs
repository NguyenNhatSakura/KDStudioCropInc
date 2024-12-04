using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using System.Windows.Threading;
using System.Diagnostics;
using Microsoft.Web.WebView2.Core;
using ForlornApi;
using System.Windows.Media.Animation;
using DiscordRPC;
using DiscordRPC.Logging;

namespace NguyenNhat
{
    public partial class MainWindow : Window
    {
        DispatcherTimer time = new DispatcherTimer();
        private DiscordRpcClient client;

        public MainWindow()
        {
            InitializeComponent();
            this.Topmost = true;
            time.Interval = TimeSpan.FromMilliseconds(1000);
            time.Tick += TimerTick;
            time.Start();
            OpenAdDialog();
            InitializeWebView();
            Loaded += MainWindow_Loaded;
            InitializeDiscordRPC();
        }

        public void UpdateBackgroundImage()
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            string imagePath = System.IO.Path.Combine(currentDirectory, "nguyentranhongtham.png");
            if (File.Exists(imagePath))
            {
                string gifPath = $"file:///{imagePath.Replace("\\", "/")}";
                BackgroundImage.Source = new BitmapImage(new Uri(gifPath));
                BackgroundImage.Visibility = Visibility.Visible;
            }
            else
            {
                BackgroundImage.Visibility = Visibility.Collapsed;
            }
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateBackgroundImage();
        }

        private async void InitializeWebView()
        {
            await Editor.EnsureCoreWebView2Async(null);
            Editor.NavigationCompleted += Editor_NavigationCompleted;
            string localPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DebugMonaco", "monaco.html");
            if (File.Exists(localPath))
            {
                Editor.Source = new Uri(localPath);
            }
            else
            {
                MessageBox.Show("Không tìm thấy tệp monaco.html.");
            }
        }

        private void Editor_NavigationCompleted(object sender, CoreWebView2NavigationCompletedEventArgs e)
        {
            if (e.IsSuccess)
            {
                // WebView2 đã tải thành công
            }
            else
            {
                MessageBox.Show("Lỗi Load Dữ Liệu Trang Web, Vui Lòng Thử Lại.");
            }
        }

        private void InitializeDiscordRPC()
        {
            client = new DiscordRpcClient("1302870988186193960");
            client.Logger = new ConsoleLogger() { Level = LogLevel.Warning };
            client.Initialize();
            client.SetPresence(new RichPresence()
            {
                Details = "Đang Sử Dụng HT SKRs Executor",
                State = "Join: discord.gg/Hk4FTJt9sf",
                Timestamps = Timestamps.Now,
                Assets = new Assets()
                {
                    LargeImageKey = "mupsrup",
                    LargeImageText = "HTSKRs"
                }
            });
        }

        private void TimerTick(object sender, EventArgs e)
        {
            if (ForlornApi.Api.IsInjected())
            {
                Status.Foreground = System.Windows.Media.Brushes.DarkGreen;
            }
            else
            {
                Status.Foreground = System.Windows.Media.Brushes.DarkRed;
            }
        }

        private void DragMenu_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }

        private void DongMenu_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            client.Dispose();
        }

        private void ScriptHub_Click(object sender, RoutedEventArgs e)
        {
            ScriptHub scriptHubWindow = new ScriptHub(this);
            scriptHubWindow.Owner = this;
            scriptHubWindow.Topmost = true;
            scriptHubWindow.Show();
        }

        private void ThuNhoMenu_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void Injet_Click(object sender, RoutedEventArgs e)
        {
            Process[] robloxProcesses = Process.GetProcessesByName("RobloxPlayerBeta");

            if (robloxProcesses.Any())
            {
                ForlornApi.Api.Inject();
            }
            else
            {
                MessageBox.Show("Vui Lòng Bật Roblox Trước Khi Inject!", "Lỗi :<", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void Executor_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string script = "getValue();";
                string result = await Editor.ExecuteScriptAsync(script);
                string code = System.Text.Json.JsonSerializer.Deserialize<string>(result);
                ForlornApi.Api.ExecuteScript(code);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi thực thi mã: {ex.Message}");
            }
        }

        public void ExecutorScript(string scriptContent)
        {
            ForlornApi.Api.ExecuteScript(scriptContent);
        }


        private async void OpenFile_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.Filter = "Tệp txt (*.txt)|*.txt|Tệp Lua (*.lua)|*.lua|Tất cả các tệp (*.*)|*.*";

            if (dialog.ShowDialog() == true)
            {
                string script = File.ReadAllText(dialog.FileName);
                string escapedScript = script.Replace("\\", "\\\\").Replace("\"", "\\\"").Replace("\n", "\\n").Replace("\r", "\\r");
                await Editor.ExecuteScriptAsync($"setValue(\"{escapedScript}\");");
            }
        }

        private async void SaveFile_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog
            {
                Filter = "Tệp Lua (*.lua)|*.lua|Tệp văn bản (*.txt)|*.txt",
                DefaultExt = "lua",
                Title = "Lưu Lua hoặc tệp văn bản"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                try
                {
                    string result = await Editor.ExecuteScriptAsync("getValue();");
                    string textToSave = System.Text.Json.JsonSerializer.Deserialize<string>(result);
                    using (StreamWriter writer = new StreamWriter(saveFileDialog.FileName))
                    {
                        writer.Write(textToSave);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi lưu tệp: {ex.Message}");
                }
            }
        }

        private async void ClearText_Click(object sender, RoutedEventArgs e)
        {
            if (Editor != null && Editor.CoreWebView2 != null)
            {
                try
                {
                    await Editor.ExecuteScriptAsync("editor.setValue('');");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi xóa văn bản: {ex.Message}");
                }
            }
            else
            {
                MessageBox.Show("Tài liệu soạn thảo chưa sẵn sàng.");
            }
        }

        private void Discord_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "https://discord.com/Hk4FTJt9sf",
                UseShellExecute = true
            });
        }

        private void OpenAdDialog()
        {
            QuangCao quangCaoDialog = new QuangCao();
            quangCaoDialog.ShowDialog();
        }

        private void ShowAdButton_Click(object sender, RoutedEventArgs e)
        {
            OpenAdDialog();
        }
    }
}
