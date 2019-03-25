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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CSharpCrawler.Views
{
    /// <summary>
    /// Setting.xaml 的交互逻辑
    /// </summary>
    public partial class Setting : Page
    {
        string defaultImgPath = Environment.CurrentDirectory + "\\User Data\\Theme\\Default.jpg";
        private GlobalDataUtil globalData = GlobalDataUtil.GetInstance();
        private int ignoreCount = 0;

        public Setting()
        {
            InitializeComponent();

            InitTheme();
            InitCfg();
        }

        private void InitTheme()
        {
            Border border = GlobalSettingPanel.Children[1] as Border;
            if (border == null)
                return;
        
            ImageBrush imageBrush = new ImageBrush();
            imageBrush.ImageSource = new BitmapImage(new Uri(defaultImgPath,UriKind.Absolute));
            border.Background = imageBrush;
            border.MouseDown += (a, b) => { Application.Current.MainWindow.Background = new ImageBrush() { ImageSource = new BitmapImage(new Uri(defaultImgPath,UriKind.Absolute)) }; };
        }

        private void InitCfg()
        {
            ignoreCount = 0;

            if (globalData.CrawlerConfig.UrlConfig.IgnoreUrlCheck == true)
                this.cbox_UrlCheck.IsChecked = true;
            else
                this.cbox_UrlCheck.IsChecked = false;
        }


        #region 事件
        private void cbox_UrlCheck_Checked(object sender, RoutedEventArgs e)
        {
            if (ignoreCount == 0)
            {
                ignoreCount++;
                return;
            }

            bool result = globalData.configUtil.SaveIgnoreUrlCheck(true);
            if (result)
                globalData.CrawlerConfig.UrlConfig.IgnoreUrlCheck = true;
        }

        private void cbox_UrlCheck_Unchecked(object sender, RoutedEventArgs e)
        {
            if (ignoreCount == 0)
            {
                ignoreCount++;
                return;
            }

            bool result = globalData.configUtil.SaveIgnoreUrlCheck(false);
            if (result)
                globalData.CrawlerConfig.UrlConfig.IgnoreUrlCheck = false;
        }

        private void DefaultTheme_MouseDown(object sender, MouseButtonEventArgs e)
        {        
            LinearGradientBrush gradientBrush = new LinearGradientBrush();
            gradientBrush.EndPoint = new Point(0.5, 1);
            gradientBrush.StartPoint = new Point(0.5, 0);
            GradientStop color1 = new GradientStop();
            color1.Color = (Color)(ColorConverter.ConvertFromString("#FFFFF9F9"));
            color1.Offset = 0;
            GradientStop color2 = new GradientStop();
            color2.Color = (Color)(ColorConverter.ConvertFromString("#FFA49B96"));
            color2.Offset = 1;
            gradientBrush.GradientStops.Add(color1);
            gradientBrush.GradientStops.Add(color2);
            Application.Current.MainWindow.Background = gradientBrush;
        }
        #endregion


    }
}
