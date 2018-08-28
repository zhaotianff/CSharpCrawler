using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CefSharp;
using System.Threading;
using System.Windows;

namespace CSharpCrawler.Util
{
    class CEFUtil
    {
        private static object obj = new object();
        private static CEFUtil _instance;

        private AutoResetEvent waitEvent = new AutoResetEvent(false);

        string globalSource = "";
        
        CefSharp.Wpf.ChromiumWebBrowser webBrowser = new CefSharp.Wpf.ChromiumWebBrowser();

        public static CEFUtil GetInstance()
        {
            if (_instance == null)
            {
                lock (obj)
                {
                    if (_instance == null)
                        _instance = new CEFUtil();
                }
            }
            return _instance;
        }

        public CEFUtil()
        {
            webBrowser.FrameLoadEnd += new EventHandler<FrameLoadEndEventArgs>(FrameEndFunc);
        }

        public Task<string> GetHtmlSource(string url)
        {
            webBrowser.Address = url;
            return webBrowser.GetSourceAsync();
        }
       
        public string GetHtmlSourceDynamic(string url)
        {           
            waitEvent.Reset();

            ThreadPool.QueueUserWorkItem(new WaitCallback((object obj)=> {
                Application.Current.MainWindow.Dispatcher.Invoke(() =>
                {
                    webBrowser.Address = url;
                });
            }));
          

            waitEvent.WaitOne();
            return globalSource;
        }

        private async void FrameEndFunc(object sender,FrameLoadEndEventArgs args)
        {
            globalSource = await webBrowser.GetSourceAsync();
            waitEvent.Set();
        }


    }
}
