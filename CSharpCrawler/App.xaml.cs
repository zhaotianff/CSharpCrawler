using CSharpCrawler.Util;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;

namespace CSharpCrawler
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            e.Handled = true;
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            //初始化全局数据 
            GlobalDataUtil.GetInstance();
        }
    }
}
