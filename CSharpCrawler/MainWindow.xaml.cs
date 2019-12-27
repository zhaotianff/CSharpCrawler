using CSharpCrawler.Model;
using CSharpCrawler.Util;
using System;
using System.Collections.Generic;
using System.Linq;
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

using CSharpCrawler.Views;
using System.Windows.Controls.Primitives;

namespace CSharpCrawler
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        FetchImage imagePage = new FetchImage();
        FetchImageEx imagePageEx = new FetchImageEx();
        FetchUrl urlPage = new FetchUrl();
        FetchResource resourcePage = new FetchResource();
        FetchDynamicResource dynamicResourcePage = new FetchDynamicResource();
        FetchResourceWithDOM fetchResourceWithDOM = new FetchResourceWithDOM();
        Setting setting = new Setting();
        Basic basic = new Basic();
        InvokeWebAPI invokeWeb = new InvokeWebAPI();
        FetchFile fetchFile = new FetchFile();
        BingImageSearch bingImageSearch = new BingImageSearch();
        RegularExpressionUsage regularExpressionUsage = new RegularExpressionUsage();
        DataStorage dataStorage = new DataStorage();
        RobotsProtocol robotsProtocol = new RobotsProtocol();
        DishesPrice dishesPrice = new DishesPrice();
        SimulateLogin simulateLogin = new SimulateLogin();
        AnalysisPacket analysicPacket = new AnalysisPacket();
        AdvanceSelect advanceSelect = new AdvanceSelect();

        ToggleButton toggleButton = null;

        public MainWindow()
        {
            InitializeComponent();
            
            InitializeCommands();
            Application.Current.MainWindow = this;
        }

        #region Commands
        private void InitializeCommands()
        {
            CommandBindings.Add(new CommandBinding(SystemCommands.CloseWindowCommand, CloseWindow));
            CommandBindings.Add(new CommandBinding(SystemCommands.MaximizeWindowCommand, MaximizeWindow, CanResizeWindow));
            CommandBindings.Add(new CommandBinding(SystemCommands.MinimizeWindowCommand, MinimizeWindow, CanMinimizeWindow));
            CommandBindings.Add(new CommandBinding(SystemCommands.RestoreWindowCommand, RestoreWindow, CanResizeWindow));
            CommandBindings.Add(new CommandBinding(SystemCommands.ShowSystemMenuCommand, ShowSystemMenu));
        }

        private void CanResizeWindow(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ResizeMode == ResizeMode.CanResize || ResizeMode == ResizeMode.CanResizeWithGrip;
        }

        private void CanMinimizeWindow(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ResizeMode != ResizeMode.NoResize;
        }

        private void CloseWindow(object sender, ExecutedRoutedEventArgs e)
        {
            this.Close();
            //SystemCommands.CloseWindow(this);
        }

        private void MaximizeWindow(object sender, ExecutedRoutedEventArgs e)
        {
            SystemCommands.MaximizeWindow(this);
        }

        private void MinimizeWindow(object sender, ExecutedRoutedEventArgs e)
        {
            SystemCommands.MinimizeWindow(this);
        }

        private void RestoreWindow(object sender, ExecutedRoutedEventArgs e)
        {
            SystemCommands.RestoreWindow(this);
        }


        private void ShowSystemMenu(object sender, ExecutedRoutedEventArgs e)
        {
            var element = e.OriginalSource as FrameworkElement;
            if (element == null)
                return;

            var point = WindowState == WindowState.Maximized ? new Point(0, element.ActualHeight)
                : new Point(Left + BorderThickness.Left, element.ActualHeight + Top + BorderThickness.Top);
            point = element.TransformToAncestor(this).Transform(point);
            SystemCommands.ShowSystemMenu(this, point);
        }
        #endregion

        public void SetBackgroundVideo(string path,UriKind uriKind = UriKind.Relative)
        {
            mediaelement.Source = new Uri(path, uriKind);
            mediaelement.Play();
        }

        public void SetTransparentBackground()
        {
            this.Background = Brushes.Transparent;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            WindowUtil.BlurWindow(this);
        }

        private void mediaelement_MediaEnded(object sender, RoutedEventArgs e)
        {
            mediaelement.Stop();
            mediaelement.Play();
        }

        private void btn_FetchUrl_Click(object sender, RoutedEventArgs e)
        {
            this.frame.Content = urlPage;
        }

        private void btn_FetchImage_Click(object sender, RoutedEventArgs e)
        {
            this.frame.Content = imagePage;
        }

        private void btn_FetchResource_Click(object sender, RoutedEventArgs e)
        {
            this.frame.Content = resourcePage;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if (GlobalDataUtil.GetInstance().Browser != null)
                GlobalDataUtil.GetInstance().Browser.Close();
        }

        private void btn_FetchDynamicResource_Click(object sender, RoutedEventArgs e)
        {
            this.frame.Content = dynamicResourcePage;
        }

        private void btn_FetchResourceWithDOM_Click(object sender, RoutedEventArgs e)
        {
            this.frame.Content = fetchResourceWithDOM;
        }

        private void btn_Setting_Click(object sender, RoutedEventArgs e)
        {
            setting.LoadSettingFromUI();
            this.frame.Content = setting;
        }

        private void btn_Basic_Click(object sender, RoutedEventArgs e)
        {
            this.frame.Content = basic;
        }

        private void btn_WebAPI_Click(object sender, RoutedEventArgs e)
        {
            this.frame.Content = invokeWeb;
        }

        private void btn_SimulateLogin_Click(object sender, RoutedEventArgs e)
        {
            this.frame.Content = simulateLogin;
        }

        private void btn_FileDownLoad_Click(object sender, RoutedEventArgs e)
        {
            this.frame.Content = fetchFile;
        }

        private void btn_BingImageSearch_Click(object sender, RoutedEventArgs e)
        {
            this.frame.Content = bingImageSearch;
            bingImageSearch.StartAnimation();
        }

        private void ToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            if (toggleButton != null)
                toggleButton.IsChecked = false;

            toggleButton = sender as ToggleButton;
        }

        private void btn_DataStorage_Click(object sender, RoutedEventArgs e)
        {
            this.frame.Content = dataStorage;
        }

        private void btn_RegularExpression_Click(object sender, RoutedEventArgs e)
        {
            this.frame.Content = regularExpressionUsage;
        }

        private void btn_RobotsProtocol_Click(object sender, RoutedEventArgs e)
        {
            this.frame.Content = robotsProtocol;
            robotsProtocol.LoadContent();
        }

        private void btn_FetchImageEx_Click(object sender, RoutedEventArgs e)
        {
            imagePageEx.Page = 1;
            this.frame.Content = imagePageEx;
        }

        private void btn_DishesPrice_Click(object sender, RoutedEventArgs e)
        {
            this.frame.Content = dishesPrice;
        }

        private void btn_AnalysisPacket_Click(object sender, RoutedEventArgs e)
        {
            this.frame.Content = analysicPacket;
            analysicPacket.LoadContent();
        }

        private void btn_AdvanceSelect_Click(object sender, RoutedEventArgs e)
        {
            this.frame.Content = advanceSelect;
        }
    }
}
