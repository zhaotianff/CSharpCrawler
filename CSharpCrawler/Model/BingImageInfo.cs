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
    public class BingImages
    {
        [XmlArray("image")]
        public List<BingImage> Images { get; set; }

        [XmlElement("tooltips")]
        public BingTooltips Tooltips { get; set; }
    }

    [XmlRootAttribute("image")]
    public class BingImage
    {
        [XmlAttribute("startdate")]
        public DateTime StartDate { get; set; }

        [XmlAttribute("enddate")]
        public DateTime EndDate { get; set; }

        [XmlAttribute("fullstartdate")]
        public DateTime FullStartDate { get; set; }

        [XmlAttribute("url")]
        public string Url { get; set; }

        [XmlAttribute("urlBase")]
        public string UrlBase { get; set; }

        [XmlAttribute("copyright")]
        public string Copyright { get; set; }

        [XmlAttribute("headline")]
        public string Headline { get; set; }

        [XmlAttribute("drk")]
        public string Drk { get; set; }

        [XmlAttribute("top")]
        public string Top { get; set; }

        [XmlAttribute("hot")]
        public string Hot { get; set; }

        [XmlAttribute("hotspots")]
        public string Hotspots { get; set; }
    }

    [XmlRoot("tooltips")]
    public class BingTooltips
    {
        //暂时还有点问题
        [XmlElement("loadMessage")]
        public string loadMessage;

        [XmlElement("loadMessage")]
        [XmlElement("message")]
        public string message;

        [XmlElement("previousImage")]
        public string previousImage;

        [XmlElement("previousImage")]
        [XmlElement("text")]
        public string text;

        [XmlElement("nextImage")]
        public string nextImage;

        [XmlElement("nextImage")]
        [XmlElement("text")]
        public string text2;

        [XmlChoiceIdentifier]
        [XmlElement("play")]
        public string play;

        [XmlElement("play")]
        [XmlElement("text")]
        public string text3;

        [XmlElement("pause")]
        public string pause;

        [XmlElement("pause")]
        [XmlElement("text")]
        public string text4;
    }
}
