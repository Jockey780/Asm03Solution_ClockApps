using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Asm03Solution_ClockApps
{
    /// <summary>
    /// Interaction logic for CustomMessageBox.xaml
    /// </summary>
    public partial class CustomMessageBox : Window
    {
        private System.Media.SoundPlayer player;

        public CustomMessageBox()
        {
            InitializeComponent();
            player = new System.Media.SoundPlayer(@"D:\WEB DOWNLOAD\AlarmMusic\alarm1.wav");
            player.Play();
        }
        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            player.Stop();
            this.Close();
        }
    }
}
