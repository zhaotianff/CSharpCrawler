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
            SolidColorBrush accentBaseColor = Application.Current.FindResource("AccentBaseColor") as SolidColorBrush;

            foreach (var item in globalData.CrawlerConfig.ThemeList)
            {
                Border border = new Border();
                border.Width = 100;
                border.Height = 100;
                border.CornerRadius = new CornerRadius(10);
                border.Cursor = Cursors.Hand;

                ImageBrush imageBrush = new ImageBrush();
                imageBrush.ImageSource = new BitmapImage(new Uri(item.Background, UriKind.Relative));
                border.Background = imageBrush;
                border.MouseDown += DefaultTheme_MouseDown;
                border.BorderBrush = accentBaseColor;
                border.BorderThickness = new Thickness(1);
                border.Margin = new Thickness(10);

                GlobalSettingPanel.Children.Add(border);
            }           
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
            var border = sender as Border;
            if(border != null)
            {
                Application.Current.MainWindow.Background = border.Background;
            }          
        }
        #endregion


    }
}
