using CSharpCrawler.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.Concurrent;
using ZT.Enhance;

namespace CSharpCrawler.Views
{
    /// <summary>
    /// UniversalCrawl.xaml 的交互逻辑
    /// </summary>
    public partial class UniversalCrawl : Page
    {
        ConcurrentQueue<string> urlQueue = new ConcurrentQueue<string>();
        AngleSharpHelper angleSharpHelper = new AngleSharpHelper();

        System.Threading.CancellationToken cancellationToken = new System.Threading.CancellationToken();

        public UniversalCrawl()
        {
            InitializeComponent();
            //即使掌握了爬虫技术，要写一个通用程序来抓取所有网页还是很难的，因为各个网站的结构都是不相同的
            //但还是可以写一些通用的抓取，并通过扩展来达到通用抓取某一类网站的目的

            //可以定义一些电商网站的通用结构
            
            /*
             * 商品名称
             * 销量
             * 价格
             * 型号
             * 图片描述
             * 详细参数
             * 评价/评分
             */
        }

        public class Good
        {
            public string Name { get; set; }
            public string Sales { get; set; }
            public string Price { get; set; }
            public string Type { get; set; }
            public List<string> DetailImageList { get; set; }
            public string DetailContent { get; set; }
            public List<Review> ReviewList { get; set; }
        }

        public class Review
        {
            public float Star { get; set; }

            public string Content { get; set; }

            public List<string> Image { get; set; }
        }

        private async void Btn_Surfing_Click(object sender, RoutedEventArgs e)
        {
            ScrollToEnd();

            var url = this.tbox_Url.Text.Trim();

            if(string.IsNullOrEmpty(url))
            {
                EMessageBox.Show("请输入要抓取的电商类网址");
            }

            AppendText($"正在从{url}获取........");

            var source = await WebUtil.GetDynamicHtmlSourceWithChromium(this,url);
            AppendText("获取页面完成，正在分析");

            var good = GetGood(source);

            if(good == null)
            {
                AppendText($"分析失败，{url}可能不是一个商品详情页地址");
            }
            else
            {
                AppendText("分析完成，结果如下.");
                ShowResult(good);
            }
        }

        private void AppendText(string text)
        {
            if (string.IsNullOrEmpty(text))
                return;

            this.Dispatcher.Invoke(()=> {
                this.rtbox_Result.Document.Blocks.Add(new Paragraph(new Run(text.Trim().Replace("\n",""))));
            });
        }

        private Good GetGood(string source)
        {
            try
            {
                if (string.IsNullOrEmpty(source))
                    return null;

                Good good = new Good();
                angleSharpHelper.Init(source);

                //大部分是使用h1标签做为商品标题
                var goodNameElement = angleSharpHelper.CSSQuery("h1");

                if(goodNameElement == null)
                {
                    //如果h1没有找到，用name去找
                    //忽略大小写
                    goodNameElement = angleSharpHelper.CSSQuery("[class~=name i]");

                    if(goodNameElement == null)
                    {
                        goodNameElement = angleSharpHelper.CSSQuery("[id~=name i]");
                    }
                }

                //商品详情一般会包含detail
                var goodDetailElement = angleSharpHelper.CSSQuery("[id~=detail i]");

                if(goodDetailElement == null)
                {
                    goodDetailElement = angleSharpHelper.CSSQuery("[class~=detail i]");                    
                }

                good.Name = goodNameElement?.TextContent;
                good.DetailContent = goodDetailElement?.TextContent;
                good.DetailImageList = goodDetailElement?.QuerySelectorAll("img").Select(x => x.Attributes["src"]?.Value).ToList();

                return good;
            }
            catch
            {
                return null;
            }
        }

        private void ShowResult(Good good)
        {
            AppendText($"商品名称:{good.Name}");
            AppendText($"商品详情:{good.DetailContent}");
            AppendText($"商品图片");
            good.DetailImageList.ForEach(x => AppendText(x));
        }

        private void ScrollToEnd()
        {
            this.rtbox_Result.ScrollToEnd();
        }
    }
}
