using CSharpCrawler.Util;
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
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CSharpCrawler.Views
{
    /// <summary>
    /// Interaction logic for HostWindow.xaml
    /// </summary>
    public partial class HostWindow : Window
    {
        public HostWindow()
        {
            InitializeComponent();
        }

        public void SetBackgroundImage(string path)
        {
            mediaelement.Visibility = Visibility.Hidden;
            this.Visibility = Visibility.Visible;
            ImageBrush imageBrush = new ImageBrush();
            imageBrush.Stretch = Stretch.UniformToFill;
            imageBrush.ImageSource = new BitmapImage(new Uri(path, UriKind.Relative));
            this.Background = imageBrush;
        }

        public void SetBackgroundVideo(string path)
        {
            mediaelement.Visibility = Visibility.Visible;
            this.Visibility = Visibility.Visible;
            mediaelement.Source = new Uri(path, UriKind.Relative);
            mediaelement.Play();
        }

        public void SetBackgroundColor(Brush brush, double opacity)
        {
            mediaelement.Visibility = Visibility.Hidden;
            this.Visibility = Visibility.Visible;
            this.Background = brush;
            this.Background.Opacity = opacity;
        }

        private void mediaelement_MediaEnded(object sender, RoutedEventArgs e)
        {
            mediaelement.Stop();
            mediaelement.Play();
        }

        public void StopBackgroundVideo()
        {
            mediaelement.Visibility = Visibility.Hidden;
            this.Visibility = Visibility.Hidden;
            mediaelement.Stop();
        }
    }
}
