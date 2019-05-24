using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ISeer.GUI
{
    /// <summary>
    /// MessageUI.xaml 的交互逻辑
    /// </summary>
    public partial class MessageUI : Window
    {
        Storyboard end;
        bool result = true;
        public MessageUI(string content)
        {
            InitializeComponent();
            end = (Storyboard)this.FindResource("end");
            end.Completed += (a, b) => {
                this.DialogResult = result;
                this.Close();
            };
            this.content.Text = content;          
        }

        public MessageUI(string content,string title)
        {
            InitializeComponent();
            end = (Storyboard)this.FindResource("end");
            end.Completed += (a, b) =>
            {
                this.DialogResult = result;
                this.Close();
            };
            this.content.Text = content;
            this.title.Content = title;          
        }

        public MessageUI(string content, string title,Utilities.EMessageBoxType.ButtonType type)
        {
            InitializeComponent();
            end = (Storyboard)this.FindResource("end");
            end.Completed += (a, b) =>
            {
                this.DialogResult = result;
                this.Close();
            };
            this.content.Text = content;
            this.title.Content = title;
            if(type == Utilities.EMessageBoxType.ButtonType.YesOrNo)
            {
                this.cancel.Visibility = System.Windows.Visibility.Visible;
                this.ok.Margin = new Thickness(0,0,60,0);
                this.ok.Foreground = Brushes.Red;
            }
            SystemSounds.Beep.Play();

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //启动时动画
            this.BeginStoryboard((Storyboard)this.FindResource("start"));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            end.Begin();           
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //窗体移动
            this.DragMove();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            result = false;
            end.Begin();
        }
    }
}
