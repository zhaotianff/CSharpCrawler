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
                imageBrush.Stretch = Stretch.UniformToFill;
                imageBrush.Opacity = 0.8;
                border.Background = imageBrush;
                border.MouseDown += ChangeImgBackground_MouseDown;
                border.BorderBrush = accentBaseColor;
                border.BorderThickness = new Thickness(1);
                border.Margin = new Thickness(10);

                if(item.BackgroundType == Model.BackgroundType.Dynamic)
                {
                    Label label = new Label();
                    label.Content = "动态";
                    label.FontSize = 20;
                    label.Foreground = accentBaseColor;
                    label.FontWeight = FontWeights.Bold;
                    border.Child = label;
                }

                ImgBackgroundSettingPanel.Children.Add(border);
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

        public void LoadSettingFromUI(double opacity)
        {
            this.slider_Opacity.Value = opacity;
        }

        public void LoadHostWindowCheck(bool isChecked)
        {
            this.cbx_HostWindow.Checked -= cbx_HostWindow_Checked;
            cbx_HostWindow.IsChecked = isChecked;
            this.cbx_HostWindow.Checked += cbx_HostWindow_Checked;
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

        private void ChangeBackground_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var border = sender as Border;

            if (border == null)
                return;
                           
            (Application.Current.MainWindow as MainWindow).SetPureColorBackground(border.Background,this.slider_Opacity.Value);     
        }

        private void ChangeImgBackground_MouseDown(object sender,MouseButtonEventArgs e)
        {
            var border = sender as Border;
            if (border == null)
                return;

            var theme = globalData.CrawlerConfig.ThemeList[ImgBackgroundSettingPanel.Children.IndexOf(border)];
            var fileName = theme.Background;

            if (theme.BackgroundType == Model.BackgroundType.Dynamic)
            {
                fileName = fileName.Replace(".jpg", ".mp4");
                (Application.Current.MainWindow as MainWindow).SetBackgroundVideo(fileName);
                                            
            }
            else
            {                
                (Application.Current.MainWindow as MainWindow).SetBackgroundImage(fileName,this.slider_Opacity.Value);                                           
            }            
        }

        private void slider_Opacity_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {         
            (Application.Current.MainWindow as MainWindow).Background.Opacity = e.NewValue;           
        }

        private void cbx_HostWindow_Checked(object sender, RoutedEventArgs e)
        {
            (Application.Current.MainWindow as MainWindow).EnableHostWindow();
        }

        private void cbx_HostWindow_Unchecked(object sender, RoutedEventArgs e)
        {
            (Application.Current.MainWindow as MainWindow).DisableHostWindow();
        }

        private void ChangeTheme_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //一切从简 
            //先做两个
            var uiElement = sender as UIElement;
            var index = ThemeSettingPanel.Children.IndexOf(uiElement);
            ResourceDictionary dic = new ResourceDictionary();

            if(index == 0)
            {
                dic.Source = new Uri("./Themes/Green.xaml",UriKind.Relative);
            }
            else
            {
                dic.Source = new Uri("./Themes/LightBlue.xaml",UriKind.Relative);
            }

            Application.Current.Resources.MergedDictionaries[0] = dic;
        }

        private BitmapImage GetBitmapImage(string fileName)
        {
            BitmapImage bi = new BitmapImage();
            bi.BeginInit();
            bi.UriSource = new Uri(fileName, UriKind.Relative);
            bi.EndInit();
            return bi;
        }

        #endregion
    }
}
