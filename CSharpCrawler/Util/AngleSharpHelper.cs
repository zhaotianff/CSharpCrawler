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
        private IDocument doc;

        public AngleSharpHelper()
        {
            
        }

        public async void Init(string html)
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
