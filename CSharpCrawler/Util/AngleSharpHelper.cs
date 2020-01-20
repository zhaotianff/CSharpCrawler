using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AngleSharp;
using AngleSharp.Dom;

namespace CSharpCrawler.Util
{
    /// <summary>
    /// AngleSharp帮助类
    /// 暂定 具体年后再说 今天放假了 
    /// 主程序版本最初是用VS2013弄的，.Net版本是4.5.2 直接升级其它包可能不支持。所以使用了老版本AngleSharp0.9.11
    /// </summary>
    public class AngleSharpHelper
    {
        private static object obj = new object();
        private static AngleSharpHelper instance;

        private IDocument doc;

        public static AngleSharpHelper GetInstance(string html)
        {
            if(instance == null)
            {
                lock(obj)
                {
                    if (instance == null)
                        instance = new AngleSharpHelper(html);
                }
            }
            return instance;
        }

        private AngleSharpHelper(string html)
        {
            Init(html);
        }

        private async void Init(string html)
        {
            var context = BrowsingContext.New(Configuration.Default);
            doc = await context.OpenAsync(x => x.Content(html));
        }

        public IElement CSSQuery(string selector)
        {
            return doc?.QuerySelector(selector);
        }

        public IHtmlCollection<IElement> CSSQueryAll(string selector)
        {
            return doc?.QuerySelectorAll(selector);
        }
    }
}
