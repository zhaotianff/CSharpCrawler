using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CSharpCrawler.Model
{
    /*
     * Bing每日图片-XML结果
     * <images>
        <image>
            <startdate>20190311</startdate>
            <fullstartdate>201903110100</fullstartdate>
            <enddate>20190312</enddate>
            <url>
            /th?id=OHR.LeopardNamibia_EN-CN0293869969_1920x1080.jpg&rf=NorthMale_1920x1080.jpg&pid=hp
            </url>
            <urlBase>/th?id=OHR.LeopardNamibia_EN-CN0293869969</urlBase>
            <copyright>
            Leopard snoozing in a tree in Namibia for National Nap Day (© M. Watsonantheo/SuperStock)
            </copyright>
            <copyrightlink>
            http://www.bing.com/search?q=African+leopard&form=hpcapt&filters=HpDate:%2220190311_0800%22
            </copyrightlink>
            <headline>The perfect day for a nap</headline>
            <drk>1</drk>
            <top>1</top>
            <bot>1</bot>
            <hotspots/>
        </image>
        <tooltips>
            <loadMessage>
                <message>Loading...</message>
            </loadMessage>
            <previousImage>
                <text>Previous image</text>
            </previousImage>
            <nextImage>
                <text>Next image</text>
            </nextImage>
            <play>
             <text>Play video</text>
            </play>
            <pause>
                <text>Pause video</text>
            </pause>
        </tooltips>
      </images>
    */

    /// <summary>
    /// Bing图片信息
    /// </summary>
    [XmlRoot("images")]
    public class BingDailyImages
    {
        [XmlElement("image")]
        public BingDailyImage Images { get; set; }

        [XmlElement("tooltips")]
        public BingTooltips Tooltips { get; set; }
    }

    public class BingDailyImage
    {
        [XmlElement("startdate")]
        public string StartDate { get; set; }

        [XmlElement("enddate")]
        public string EndDate { get; set; }

        [XmlElement("fullstartdate")]
        public string FullStartDate { get; set; }

        [XmlElement("url")]
        public string Url { get; set; }

        [XmlElement("urlBase")]
        public string UrlBase { get; set; }

        [XmlElement("copyright")]
        public string Copyright { get; set; }

        [XmlElement("copyrightlink")]
        public string CopyrightLink { get; set; }

        [XmlElement("headline")]
        public string Headline { get; set; }

        [XmlElement("drk")]
        public string Drk { get; set; }

        [XmlElement("top")]
        public string Top { get; set; }

        [XmlElement("bot")]
        public string Bot { get; set; }

        [XmlElement("hotspots")]
        public string Hotspots { get; set; }
    }

    public class BingTooltips
    {
        
    }
}
