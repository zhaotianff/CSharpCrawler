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
            ShowWindow();
            ImageBrush imageBrush = new ImageBrush();
            imageBrush.Stretch = Stretch.UniformToFill;
            imageBrush.ImageSource = new BitmapImage(new Uri(path, UriKind.Relative));
            this.Background = imageBrush;
        }

        public void SetBackgroundVideo(string path,UriKind uriKind)
        {
            mediaelement.Visibility = Visibility.Visible;
            ShowWindow();
            mediaelement.Source = new Uri(path, uriKind);
            mediaelement.Play();
        }

        private void mediaelement_MediaEnded(object sender, RoutedEventArgs e)
        {
            mediaelement.Stop();
            mediaelement.Play();
        }

        public void StopBackgroundVideo()
        {
            mediaelement.Visibility = Visibility.Hidden;
            HideWindow();
            mediaelement.Stop();
        }

        public void HideWindow()
        {
            this.Visibility = Visibility.Hidden;
        }

        public void ShowWindow()
        {
            this.Visibility = Visibility.Visible;
        }
    }
}
