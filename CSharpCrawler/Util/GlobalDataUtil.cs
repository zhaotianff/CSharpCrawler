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
        private const string ConfigPath = "./config/0_0.xml";

        private static object obj = new object();
        private static GlobalDataUtil _instance;
     
        private ConfigStruct crawlerConfig;
        private ChromiumBrowser browser;

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

            CrawlerConfig = LoadConfig();
        }

        private ConfigStruct LoadConfig()
        {
            ConfigStruct configStruct = new ConfigStruct();

            FetchUrlConfig urlConfig = new FetchUrlConfig();
            FetchImageConfig imageConfig = new FetchImageConfig();
            CommonConfig commonConfig = new CommonConfig();

            try
            {
                XDocument doc = XDocument.Load(ConfigPath);
                XElement eleUrl = doc.Root.Element("FetchUrl");
                XElement eleImage = doc.Root.Element("FetchImage");
                XElement eleCommon = doc.Root.Element("Common");

                urlConfig.Depth = eleUrl.Element("Depth").Value;
                urlConfig.IgnoreUrlCheck = eleUrl.Element("IgnoreUrlCheck").Value == "1" ? true : false;
                urlConfig.DynamicGrab = eleUrl.Element("DynamicGrab").Value == "1" ? true : false;

                imageConfig.Depth = eleImage.Element("Depth").Value;
                imageConfig.IgnoreUrlCheck = eleImage.Element("IgnoreUrlCheck").Value == "1" ? true : false;
                imageConfig.DynamicGrab = eleImage.Element("DynamicGrab").Value == "1" ? true : false;
                imageConfig.MaxResolution = eleImage.Element("MaxResolution").Value;
                imageConfig.MinResolution = eleImage.Element("MinResolution").Value;
                imageConfig.MinSize = Convert.ToInt32(eleImage.Element("MinSize").Value);
                imageConfig.MaxSize = Convert.ToInt32(eleImage.Element("MaxSize").Value);
                imageConfig.FetchMode = Convert.ToInt32(eleImage.Element("FetchMode").Value);

                commonConfig.UrlCheck = eleCommon.Element("UrlCheck").Value == "1" ? true : false;

                configStruct.ImageConfig = imageConfig;
                configStruct.UrlConfig = urlConfig;
                configStruct.CommonConfig = commonConfig;
            }
            catch(Exception ex)
            {
                
            }           
            return configStruct;
        }
    }
}
