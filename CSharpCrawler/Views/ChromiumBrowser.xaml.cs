using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using CefSharp;

namespace CSharpCrawler.Views
{
    /// <summary>
    /// ChromiumBrowser.xaml 的交互逻辑
    /// </summary>
    public partial class ChromiumBrowser : Window
    {
        private AutoResetEvent waitEvent = new AutoResetEvent(false);
        private Action<string> extractImageCallBack;               
        
        public ChromiumBrowser()
        {
            InitializeComponent();
        }


        public Task<string> GetHtmlSource(string url)
        {
            browser.Address = url;
            return browser.GetSourceAsync();
        }

        public void GetHtmlSourceDynamic(string url,Action<string> act)
        {
            ThreadPool.QueueUserWorkItem((object obj) => {            
                this.Dispatcher.Invoke(()=> {
                    browser.Address = url;
                    extractImageCallBack = act;          
                });                
            });                   
        }

        private async void browser_FrameLoadEnd(object sender, CefSharp.FrameLoadEndEventArgs e)
        {
            string source = await browser.GetSourceAsync();

            if (extractImageCallBack != null)
                extractImageCallBack(source);       
            
        }
    }
}
