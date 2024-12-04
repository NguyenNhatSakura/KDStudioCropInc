using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Diagnostics;
using System.Windows;
using System.Windows.Threading;

namespace NguyenNhat
{
    public partial class QuangCao
    {
        private DispatcherTimer _timer;
        private int _timeLeft = 5;
        public QuangCao()
        {
            InitializeComponent();
            StartCountdown();
        }

        private void StartCountdown()
        {
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += Timer_Tick;
            _timer.Start();
            CloseButton.IsEnabled = false;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            _timeLeft--;

            if (_timeLeft <= 0)
            {
                _timer.Stop();
                CloseButton.IsEnabled = true;
                CloseButton.Content = "X";
            }
            else
            {
                CloseButton.Content = $"{_timeLeft}";
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            if (CloseButton.IsEnabled)
            {
                this.Close();
            }
            else
            {
                MessageBox.Show($"Vui lòng chờ {_timeLeft} giây nữa trước khi đóng!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void AccessButton_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "https://fakebill.htskrs.cfd",
                UseShellExecute = true
            });
        }
    }
}
