using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ForlornApi;
using Microsoft.Win32;

namespace NguyenNhat
{
    public partial class Form1 : RoundedForm
    {
        Timer time = new Timer();
        public Point mouseLocation;

        public Form1()
        {
            InitializeComponent();
            time.Tick += timertick;
            time.Start();
            Editor.Navigate(new Uri(string.Format("file:///{0}/Monaco/index.html", Directory.GetCurrentDirectory())));
            Editor.DocumentCompleted += Editor_DocumentCompleted;
        }

        private void Editor_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            // The document is ready, and you can now safely invoke scripts.
        }

        private void timertick(object sender, EventArgs e)
        {
            if (ForlornApi.Api.IsInjected())
            {
                status.Text = "🟢";
                status.ForeColor = Color.DarkGreen;  // Change text color to green
            }
            else
            {
                status.Text = "🔴";
                status.ForeColor = Color.DarkRed;  // Change text color to red
            }
        }

        private void Inject_Click(object sender, EventArgs e)
        {
            Process[] robloxProcesses = Process.GetProcessesByName("RobloxPlayerBeta");

            if (robloxProcesses.Any())
            {
                ForlornApi.Api.Inject();
                string NguyenNhat = @"loadstring(game:HttpGet('https://raw.githubusercontent.com/NguyenNhatSakura/NguyenNhatSakura/refs/heads/main/update/VERSION'))()";
                ForlornApi.Api.ExecuteScript(NguyenNhat);
            }
            else
            {
                MessageBox.Show("Vui Lòng Bật Roblox Trước Khi Inject!", "Lỗi :<", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void execute_Click(object sender, EventArgs e)
        {
            string script = Editor.Document.InvokeScript("getValue").ToString();
            ForlornApi.Api.ExecuteScript(script);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Bạn có muốn tắt Roblox không?", "Thông Báo !!", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (dialogResult == DialogResult.Yes)
            {
                ForlornApi.Api.KillRoblox();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // Ensure the document is loaded and then invoke JavaScript to clear the editor
            if (Editor.Document != null)
            {
                Editor.Document.InvokeScript("eval", new object[] { "editor.setValue('')" });
            }
            else
            {
                MessageBox.Show("Tài liệu soạn thảo chưa sẵn sàng.");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        class functions
        {
            public static void PopulateListBox(System.Windows.Forms.ListBox lsb, string Folder, string FileType)
            {
                DirectoryInfo dinfo = new DirectoryInfo(Folder);
                FileInfo[] Files = dinfo.GetFiles(FileType);
                foreach (FileInfo file in Files)
                {
                    lsb.Items.Add(file.Name);
                }
            }
        }

        private void label1_MouseDown(object sender, MouseEventArgs e)
        {
            mouseLocation = new Point(-e.X, -e.Y);
        }

        private void label1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Point mousePose = Control.MousePosition;
                mousePose.Offset(mouseLocation.X, mouseLocation.Y);
                Location = mousePose;
            }
        }

        private void openfile_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Tệp txt (*.txt)|*.txt|Tệp Lua (*.lua)|*.lua|Tất cả các tệp (*.*)|*.*";

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string script = File.ReadAllText(dialog.FileName);
                Editor.Document.InvokeScript("setValue", new object[] { script });
            }
        }

        private void savefile_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog
            {
                Filter = "Tệp Lua (*.lua)|*.lua|Tệp văn bản (*.txt)|*.txt",
                DefaultExt = "lua",
                Title = "Lưu Lua hoặc tệp văn bản"
            };

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                // Assuming you're retrieving text from your editor through a script method
                string textToSave = (string)Editor.Document.InvokeScript("getValue");

                // Save the file
                using (Stream s = File.Open(saveFileDialog1.FileName, FileMode.Create))
                using (StreamWriter sw = new StreamWriter(s))
                {
                    sw.Write(textToSave);
                }
            }
        }


        private int tabCount = 1;

        private void button6_Click_1(object sender, EventArgs e)
        {
            TabPage newTab = new TabPage($"Tab {tabCount + 1}");

            WebBrowser webBrowser = new WebBrowser
            {
                Dock = DockStyle.Fill // Fill the tab with the WebBrowser
            };

            webBrowser.Navigate(new Uri(string.Format("file:///{0}/Monaco/index.html", Directory.GetCurrentDirectory())));
            newTab.Controls.Add(webBrowser);
            tabControl1.TabPages.Add(newTab);
            tabCount++;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (tabControl1.TabCount > 0 && tabControl1.SelectedTab != null)
            {
                if (tabControl1.TabCount > 1)
                {
                    if (MessageBox.Show("Bạn có muốn đóng tab này không?", "Thông Báo !!", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        tabControl1.TabPages.Remove(tabControl1.SelectedTab);
                        tabCount--;
                    }
                }
                else
                {
                    MessageBox.Show("Không thể đóng tab, vui lòng thử lại.", "Thông Báo !!", MessageBoxButtons.OK);
                }
            }
        }

        private void Editor_DocumentCompleted_1(object sender, WebBrowserDocumentCompletedEventArgs e)
        {

        }

        private void status_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Tham gia kênh Discord của chúng tôi, Để nhận thông báo update mới nhất?", "Thông Báo !!", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);

            if (dialogResult == DialogResult.OK)
            {
                System.Diagnostics.Process.Start("https://discord.com/Hk4FTJt9sf");
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {

        }

        private void button8_Click_1(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn Sẵn Sàng Tham Gia Sever HTSKRS Chưa?", "Xác Nhận", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);

            if (result == DialogResult.OK)
            {
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                {
                    FileName = "https://discord.gg/Hk4FTJt9sf",
                    UseShellExecute = true
                });
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
