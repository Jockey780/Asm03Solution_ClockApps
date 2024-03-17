using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Asm03Solution_ClockApps
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private CancellationTokenSource cts;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Điền số giờ từ 0 đến 23 vào ComboBox
            for (int i = 0; i < 24; i++)
            {
                cb_Hour.Items.Add(i);
            }

            // Điền số phút từ 0 đến 59 vào ComboBox
            for (int i = 0; i < 60; i++)
            {
                cb_Minute.Items.Add(i);
            }

            // Khởi tạo thread cập nhật thời gian
            Thread timeThread = new Thread(UpdateTime); //Thread thứ 1
            timeThread.Start();
        }
        private void UpdateTime()
        {
            while (true)
            {
                // Lấy thời gian hiện tại
                DateTime now = DateTime.Now;
                Dispatcher.Invoke(() =>
                {
                    lb_Hour.Content = now.Hour.ToString("00");
                    lb_Minute.Content = now.Minute.ToString("00");
                    lb_Second.Content = now.Second.ToString("00");
                });
                // Dừng thread trong 1 giây
                Thread.Sleep(1000);
            }
        }

        private void btn_SetAlarm_Click(object sender, RoutedEventArgs e)
        {
            // Lấy giờ và phút từ ComboBox
            int hour = (int)cb_Hour.SelectedItem;
            int minute = (int)cb_Minute.SelectedItem;

            // Tính thời gian hiện tại và thời gian báo thức
            DateTime now = DateTime.Now;
            DateTime alarmTime = new DateTime(now.Year, now.Month, now.Day, hour, minute, 0);

            // Nếu thời gian báo thức nhỏ hơn thời gian hiện tại, thêm 1 ngày
            if (alarmTime < now)
            {
                alarmTime = alarmTime.AddDays(1);
            }

            // Tính thời gian còn lại (tính bằng giây)
            int countdownTime = (int)(alarmTime - now).TotalSeconds;

            // Hiển thị thông báo thời gian còn lại
            lb_TimeLeft.Content = $"Your time left before alarm: {countdownTime / 3600:00}:{(countdownTime / 60) % 60:00}:{countdownTime % 60:00}";
            lb_TimeLeft.Visibility = Visibility.Visible;

            if (cts != null)
            {
                cts.Cancel();
                cts = null;
            }

            // Tạo và khởi động hoạt động đếm ngược mới
            cts = new CancellationTokenSource();
            var token = cts.Token;
            Task.Run(() => Countdown(countdownTime, token), token); //Thread thứ 2
        }

        private async Task Countdown(int countdownTime, CancellationToken token)
        {
            while (countdownTime > 0)
            {
                // Tính số giờ, phút, và giây còn lại
                int hour = countdownTime / 3600;
                int minute = (countdownTime % 3600) / 60;
                int second = countdownTime % 60;

                // Cập nhật giao diện người dùng để hiển thị thời gian còn lại
                Dispatcher.Invoke(() =>
                {
                    lb_TimeLeft.Content = $"Your time left before alarm: {hour:00}:{minute:00}:{second:00}";
                });

                countdownTime--;

                // Dừng task trong 1 giây
                await Task.Delay(1000);

                if (token.IsCancellationRequested)
                {
                    return;
                }
            }

            Dispatcher.Invoke(() =>
            {
                lb_TimeLeft.Visibility = Visibility.Hidden;
                var messageBox = new CustomMessageBox();
                messageBox.Show();
            });
        }
    }
}