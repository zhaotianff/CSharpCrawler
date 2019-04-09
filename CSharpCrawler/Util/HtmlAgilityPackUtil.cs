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
    }
}
