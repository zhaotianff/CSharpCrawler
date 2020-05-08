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
    public class ConfigUtil
    {
        private const string ConfigPath = "./config/0_0.xml";

        private XDocument doc = null;

        public ConfigUtil()
        {

        }


        public ConfigStruct LoadConfig()
        {
            ConfigStruct configStruct = new ConfigStruct();

            FetchUrlConfig urlConfig = new FetchUrlConfig();
            FetchImageConfig imageConfig = new FetchImageConfig();
            CommonConfig commonConfig = new CommonConfig();
            List<Theme> themeList = new List<Theme>();

            try
            {
                doc = XDocument.Load(ConfigPath);
                XElement eleUrl = doc.Root.Element("FetchUrl");
                XElement eleImage = doc.Root.Element("FetchImage");
                XElement eleCommon = doc.Root.Element("Common");
                IEnumerable<XElement> eleThemeList = doc.Root.XPathSelectElements("ThemeList/Theme");

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

                foreach (var item in eleThemeList)
                {
                    Theme theme = new Theme()
                    {
                        Background = item.Element("Background").Value ,
                        BackgroundType = (BackgroundType)int.Parse(item.Element("Background").Attribute("Type").Value)
                    };
                    themeList.Add(theme);
                }

                configStruct.ImageConfig = imageConfig;
                configStruct.UrlConfig = urlConfig;
                configStruct.CommonConfig = commonConfig;
                configStruct.ThemeList = themeList;
                
            }
            catch (Exception ex)
            {

            }
            return configStruct;
        }

        public bool SaveIgnoreUrlCheck(bool value)
        {
            try
            {
                var ele = doc.XPathSelectElement("Crawler/FetchUrl/IgnoreUrlCheck");
                if (ele != null)
                {
                    ele.Value = value == true ? "1" : "0";
                    doc.Save(ConfigPath);
                }             
                return true;

            }
            catch (Exception ex)
            {
                return false;
            }
            
        }
    }
}
