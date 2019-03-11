using CSharpCrawler.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.XPath;
using CefSharp.Wpf;
using CSharpCrawler.Views;

namespace CSharpCrawler.Util
{
    public class GlobalDataUtil
    {     
        private static object obj = new object();
        private static GlobalDataUtil _instance;
     
        private ConfigStruct crawlerConfig;
        private ChromiumBrowser browser;

        public ConfigUtil configUtil;

        internal ConfigStruct CrawlerConfig
        {
            get
            {
                return crawlerConfig;
            }

            private set
            {
                crawlerConfig = value;
            }
        }

        public ChromiumBrowser Browser
        {
            get
            {
                return browser;
            }

            set
            {
                browser = value;
            }
        }

        public static GlobalDataUtil GetInstance()
        {
            if(_instance == null)
            {
                lock(obj)
                {
                    if (_instance == null)
                        _instance = new GlobalDataUtil();
                }
            }
            return _instance;
        }

        public GlobalDataUtil()
        {
            browser = new ChromiumBrowser();
            browser.Show();

            configUtil = new ConfigUtil();
            CrawlerConfig = configUtil.LoadConfig();
        }
    }
}
