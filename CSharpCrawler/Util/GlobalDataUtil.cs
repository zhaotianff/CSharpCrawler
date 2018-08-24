using CSharpCrawler.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.XPath;

namespace CSharpCrawler.Util
{
    public class GlobalDataUtil
    {
        private const string ConfigPath = "./config/0_0.xml";

        private static object obj = new object();
        private static GlobalDataUtil _instance;

        private ConfigStruct crawlerConfig;

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
            CrawlerConfig = LoadConfig();
        }

        private ConfigStruct LoadConfig()
        {
            ConfigStruct configStruct = new ConfigStruct();

            FetchUrlConfig urlConfig = new FetchUrlConfig()
            {
                Depth = "1",
                IgnoreUrlCheck = true
            };
            FetchImageConfig imageConfig = new FetchImageConfig()
            {
                
            };

            try
            {
                XDocument doc = XDocument.Load(ConfigPath);
                XElement eleUrl = doc.Root.Element("FetchUrl");

                urlConfig.Depth = eleUrl.Element("Depth").Value;
                urlConfig.IgnoreUrlCheck = Convert.ToBoolean(eleUrl.Element("IgnoreUrlCheck").Value);


            }
            catch(Exception ex)
            {
                
            }

            configStruct.ImageConfig = imageConfig;
            configStruct.UrlConfig = urlConfig;

            return configStruct;
        }
    }
}
