using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ForlornApi;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace HTSKR
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
        }

        private void timertick(object sender, EventArgs e)
        {
            if (ForlornApi.Api.IsInjected())
            {
                status.Text = "🟢";
                status.ForeColor = Color.LightGreen;  // Change text color to green
            }
            else
            {
                status.Text = "🔴";
                status.ForeColor = Color.Red;  // Change text color to red
            }
        }


        private void Inject_Click(object sender, EventArgs e)
        {
            ForlornApi.Api.Inject();
        }

        private void execute_Click(object sender, EventArgs e)
        {
            ForlornApi.Api.ExecuteScript(Editor.Text);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            ForlornApi.Api.SetAutoInject(true);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Bạn có muốn tắt Roblox không?", "Thông Báo !!", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (dialogResult == DialogResult.Yes)
            {
                ForlornApi.Api.KillRoblox();
            }
        }

        private void robloxopen_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            Editor.Clear();
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
            OpenFileDialog openFileDialog1 = new OpenFileDialog
            {
                Filter = "Tệp Lua (*.lua)|*.lua|Tệp văn bản (*.txt)|*.txt",
                Title = "Mở Lua hoặc tệp văn bản"
            };

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Editor.Text = File.ReadAllText(openFileDialog1.FileName);
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
                string textToSave = Editor.Text;
                using (Stream s = File.Open(saveFileDialog1.FileName, FileMode.Create))
                using (StreamWriter sw = new StreamWriter(s))
                {
                    sw.Write(textToSave);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Tham gia kênh Discord của chúng tôi, Để nhận thông báo update mới nhất?", "Thông Báo !!", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);

            if (dialogResult == DialogResult.OK)
            {
                System.Diagnostics.Process.Start("https://discord.com/Hk4FTJt9sf");
            }
        }
    }
}
