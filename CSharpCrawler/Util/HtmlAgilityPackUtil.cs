using CSharpCrawler.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpCrawler.Util
{
    public class HtmlAgilityPackUtil
    {
        public async static Task<List<TagImg>> GetImgFromHtml(string html)
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

                    tempAttribute = item.ParentNode.ParentNode.Attributes["href"];
                    tagImg.DetailUrl = tempAttribute == null ? "" :Urls.CNBingDailyImageBasicUrl +  tempAttribute.Value;

                    tagImg.Width = w;
                    tagImg.Height = h;
                    list.Add(tagImg);
                }
                return list;
            });
            return await task;
        }

        public async static Task<List<TagImg>> GetImgFromUrl(string url)
        {
            var accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
            var userAgent= "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/48.0.2564.116 Safari/537.36 TheWorld 7";
            var html =await WebUtil.GetHtmlSource(url,accept,userAgent,Encoding.UTF8);
            return await GetImgFromHtml(html);
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
    }
}
