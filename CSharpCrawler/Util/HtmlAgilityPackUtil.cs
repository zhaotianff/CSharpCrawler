using CSharpCrawler.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpCrawler.Util
{
    /// <summary>
    /// AngleSharp支持CSS选择器，但暂不支持XPath选取元素
    /// HtmlAgilityPack支持XPath，但暂不支持CSS选择器
    /// </summary>
    public class HtmlAgilityPackUtil
    {
        public async static Task<List<TagImg>> GetImgFromHtml(string html,bool isHotspot = false)
        {
            Task<List<TagImg>> task = Task.Run(() => {
                HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
                doc.LoadHtml(html);
                var imgList = doc.DocumentNode.SelectNodes("//img");
                var w = 0;
                var h = 0;
                HtmlAgilityPack.HtmlAttribute tempAttribute = null;

                List<TagImg> list = new List<TagImg>();
                foreach (var item in imgList)
                {
                    TagImg tagImg = new TagImg();
                    tempAttribute = item.Attributes["alt"];
                    tagImg.Alt = tempAttribute == null ? "":tempAttribute.Value;
                    tempAttribute = item.Attributes["src"];
                    tagImg.Src =tempAttribute == null ? "" : tempAttribute.Value;

                    tempAttribute = item.Attributes["h"];
                    if (tempAttribute != null)
                    {
                        int.TryParse(tempAttribute.Value, out h);
                    }                        
                    tempAttribute = item.Attributes["w"];
                    if(tempAttribute != null)
                    {
                        int.TryParse(tempAttribute.Value, out w);
                    }

                    //Search Detail
                    /*
                       <a class="iusc" style="height:208px;width:333px" m="{&quot;cid&quot;:&quot;1jz2ZvDM&quot;,&quot;purl&quot;:&quot;https://www.927tour.com/News_newsDetail_id_20180408195735146766.html&quot;,&quot;murl&quot;:&quot;http://ynwgm.ynurl.cn/uploadfile/s10/2018/0408/20180408075500850.jpg&quot;,&quot;turl&quot;:&quot;https://tse1-mm.cn.bing.net/th?id=OIP.1jz2ZvDMIyhtns4hK1ay-AHaFJ&amp;pid=15.1&quot;,&quot;md5&quot;:&quot;d63cf666f0cc23286d9ece212b56b2f8&quot;,&quot;shkey&quot;:&quot;&quot;,&quot;t&quot;:&quot;铁路、民航保障游客正常出游&quot;,&quot;mid&quot;:&quot;1034F8C523DE0FCD1B8302CF3C0D52E2DA5E1CD3&quot;,&quot;desc&quot;:&quot;&quot;}" onclick="sj_evt.fire('IFrame.Navigate', this.href); return false;" href="/images/search?view=detailV2&amp;ccid=1jz2ZvDM&amp;id=1034F8C523DE0FCD1B8302CF3C0D52E2DA5E1CD3&amp;thid=OIP.1jz2ZvDMIyhtns4hK1ay-AHaFJ&amp;mediaurl=http%3a%2f%2fynwgm.ynurl.cn%2fuploadfile%2fs10%2f2018%2f0408%2f20180408075500850.jpg&amp;exph=407&amp;expw=585&amp;q=%e6%b8%85%e6%98%8e%e5%81%87%e6%9c%9f%e5%9b%bd%e5%86%85%e6%97%85%e6%b8%b8%e6%8e%a5%e5%be%85%e6%80%bb%e4%ba%ba%e6%95%b01.12%e4%ba%bf&amp;simid=608053044385353052&amp;selectedIndex=32&amp;qft=+filterui%3aphoto-photo" h="ID=images.5601_7,5217.1">
                         <div class="img_cont hoff">
                             <img class="mimg" style="background-color:#c10a34;color:#c10a34" height="208" width="299" src="https://tse3-mm.cn.bing.net/th?id=OIP.1jz2ZvDMIyhtns4hK1ay-AHaFJ&amp;w=299&amp;h=208&amp;c=7&amp;o=5&amp;pid=1.7" alt="清明假期国内旅游接待总人数1.12亿 的图像结果" />
                         </div>
                       </a>
                    */

                    /*< a class="iusc" style="height:207px;width:276px" m="{&quot;cid&quot;:&quot;Ox2V7JRH&quot;,&quot;purl&quot;:&quot;http://www.wall001.com/nature/under_sky/html/image8.html&quot;,&quot;murl&quot;:&quot;http://wall001.com/nature/under_sky/mxxx01/[wall001.com]_sky_AP23070.jpg&quot;,&quot;turl&quot;:&quot;https://tse2-mm.cn.bing.net/th?id=OIP.Ox2V7JRHXMInhT3_WlPpVgHaFj&amp;pid=15.1&quot;,&quot;md5&quot;:&quot;3b1d95ec94475cc227853dff5a53e956&quot;,&quot;shkey&quot;:&quot;&quot;,&quot;t&quot;:&quot;桌布天堂 --- 晴朗天空 - 藍天白云8&quot;,&quot;mid&quot;:&quot;8A372FC995FECC38853858A07F4171C439B8FA58&quot;,&quot;desc&quot;:&quot;&quot;}" onclick="sj_evt.fire('IFrame.Navigate', this.href); return false;" href="/images/search?view=detailV2&amp;ccid=Ox2V7JRH&amp;id=8A372FC995FECC38853858A07F4171C439B8FA58&amp;thid=OIP.Ox2V7JRHXMInhT3_WlPpVgHaFj&amp;mediaurl=http%3a%2f%2fwall001.com%2fnature%2funder_sky%2fmxxx01%2f%5bwall001.com%5d_sky_AP23070.jpg&amp;exph=768&amp;expw=1024&amp;q=%e5%a4%a9%e7%a9%ba&amp;simid=608010515721882861&amp;selectedIndex=5&amp;qft=+filterui%3aphoto-photo" h="ID=images.5601_7,5055.1"><div class="img_cont hoff"><img class="mimg" style="background-color:#1543b6;color:#1543b6" height="207" width="276" src="https://tse4-mm.cn.bing.net/th?id=OIP.Ox2V7JRHXMInhT3_WlPpVgHaFj&amp;w=276&amp;h=207&amp;c=7&amp;o=5&amp;pid=1.7" alt="天空 的图像结果"></div></a>*/

                    Tuple<bool,string> extractResult = RegexUtil.ExtractBingImage(item.ParentNode.ParentNode.OuterHtml);

                    if (extractResult.Item1 == true || isHotspot == true)
                    {
                        tagImg.DetailUrl = extractResult.Item2;
                        tagImg.Width = w;
                        tagImg.Height = h;
                        list.Add(tagImg);
                    }
                }
                return list;
            });
            return await task;
        }

        public async static Task<List<TagImg>> GetImgFromUrl(string url,bool isHotspot = false)
        {
            var accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
            var userAgent= "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/48.0.2564.116 Safari/537.36 TheWorld 7";
            var html =await WebUtil.GetHtmlSource(url,accept,userAgent,Encoding.UTF8);
            return await GetImgFromHtml(html,isHotspot);
        }


        public async static Task<string> ExtractSingleImage(string url)
        {
            var accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
            var userAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/48.0.2564.116 Safari/537.36 TheWorld 7";
            string html =await WebUtil.GetHtmlSource(url,accept,userAgent);
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            var imgNode =  doc.DocumentNode.SelectNodes("//img");

            if (imgNode == null || imgNode.Count == 0)
                return "";

            var tempAttribute = imgNode[0].Attributes["src"];
            if (tempAttribute == null)
                return "";
            else
                return tempAttribute.Value;
        }

        public static HtmlAgilityPack.HtmlNodeCollection GetTagList(string html,string tagName)
        {
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            try
            {
                doc.LoadHtml(html);
                return doc.DocumentNode.SelectNodes("//" + tagName);
            }
            catch
            {
                return null;
            }
        }

        public static HtmlAgilityPack.HtmlNodeCollection XPathQuery(string html,string pathExpression)
        {
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            try
            {
                doc.LoadHtml(html);
                return doc.DocumentNode.SelectNodes(pathExpression);
            }
            catch
            {
                return null;
            }
        }

        public static HtmlAgilityPack.HtmlNode XPathQuerySingle(string html,string pathExpression)
        {
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            try
            {
                doc.LoadHtml(html);
                return doc.DocumentNode.SelectSingleNode(pathExpression);
            }
            catch
            {
                return null;
            }
        }
    }
}
