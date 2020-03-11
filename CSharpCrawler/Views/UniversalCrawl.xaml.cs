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

namespace CSharpCrawler.Views
{
    /// <summary>
    /// UniversalCrawl.xaml 的交互逻辑
    /// </summary>
    public partial class UniversalCrawl : Page
    {
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
             * 详细（多为图片）
             * 评价/评分
             */
        }

        public class Goods
        {

        }

        public class Review
        {
            public float Star { get; set; }

            public string Content { get; set; }

            public List<string> Image { get; set; }
        }
    }
}
