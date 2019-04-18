using System;
using System.Collections.Generic;
using System.Linq;
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

namespace CSharpCrawler.Views
{
    /// <summary>
    /// AnimationImageWindow.xaml 的交互逻辑
    /// </summary>
    public partial class AnimationImageWindow : Window
    {
        Storyboard start;
        Storyboard end;

        public AnimationImageWindow()
        {
            InitializeComponent();

            start = this.TryFindResource("start") as Storyboard;
            end = this.TryFindResource("end") as Storyboard;
            end.Completed += (a, b) => { this.Close(); }; 
        }

        public void ShowImage(string url,double centerX,double centerY)
        {
            this.scaleTransform.CenterX = centerX;
            this.scaleTransform.CenterY = centerY;
            this.image.Source = new BitmapImage(new Uri(url));
            this.Show();           
        }

        private void StartAnimation()
        {
            start.Begin();
        }

        private void EndAnimation()
        {
            end.Begin();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            StartAnimation();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            EndAnimation();
        }
    }
}
