using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CefSharp;

namespace CSharpCrawler.Util
{
    class CEFUtil
    {
        private static object obj = new object();
        private static CEFUtil _instance;

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
            
        }

        public Task<string> GetHtmlSource(string url)
        {
            webBrowser.Address = url;

            return webBrowser.GetSourceAsync();
        }
    }
}
