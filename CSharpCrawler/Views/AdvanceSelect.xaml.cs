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
using ZT.Enhance;

namespace CSharpCrawler.Views
{
    /// <summary>
    /// AdvanceSelect.xaml 的交互逻辑
    /// </summary>
    public partial class AdvanceSelect : Page
    {
        public AdvanceSelect()
        {
            InitializeComponent();

            InitCSSSelectorTable();
            InitXPathTable();

            InitPresetData();
        }

        /// <summary>
        /// 初始化表格
        /// </summary>
        private void InitCSSSelectorTable()
        {
            //为了方便不定义新的数据类型了
            List<dynamic> list = new List<dynamic>()
            {
                new { Selector = ".class", Example = ".intro", Description = "选择 class=\"intro\" 的所有元素"},
                new { Selector = "#id", Example = "#firstname", Description = "选择 id=\"firstname\" 的所有元素"},
                new { Selector = "*", Example = "*", Description = "选择所有元素"},
                new { Selector = "element", Example = "p", Description = "选择所有 <p> 元素"},
                new { Selector = "element,element", Example = "div,p", Description = "选择所有 <div> 元素和所有 <p> 元素"},
                new { Selector = "element element", Example = "div p", Description = "选择 <div> 元素内部的所有 <p> 元素"},
                new { Selector = "element>element", Example = "div>p", Description = "选择父元素为 <div> 元素的所有 <p> 元素"},
                new { Selector = "element+element", Example = "div+p", Description = "选择紧接在 <div> 元素之后的所有 <p> 元素"},
                new { Selector = "[attribute]", Example = "[target]", Description = "选择带有 target 属性所有元素"},
                new { Selector = "[attribute=value]", Example = "[target=_blank]", Description = "选择 target=\"_blank\" 的所有元素"},
                new { Selector = "[attribute~=value]", Example = "[title~=flower]", Description = "选择 title 属性包含单词 \"flower\" 的所有元素"},
                new { Selector = "[attribute|=value]", Example = "[lang|=en]", Description = "选择 lang 属性值以 \"en\" 开头的所有元素"},
                new { Selector = ":link", Example = "a:link", Description = "选择所有未被访问的链接"},
                new { Selector = ":visited", Example = "a:visited", Description = "选择所有已被访问的链接"},
                new { Selector = ":active", Example = "a:active", Description = "选择活动链接"},
                new { Selector = ":hover", Example = "a:hover", Description = "选择鼠标指针位于其上的链接"},
                new { Selector = ":focus", Example = "input:focus", Description = "选择获得焦点的 input 元素"},
                new { Selector = ":first-letter", Example = "p:first-letter", Description = "选择每个 <p> 元素的首字母"},
                new { Selector = ":first-line", Example = "p:first-line", Description = "选择每个 <p> 元素的首行"},
                new { Selector = ":first-child", Example = "p:first-child", Description = "选择属于父元素的第一个子元素的每个 <p> 元素"},
                new { Selector = ":before", Example = "p:before", Description = "在每个 <p> 元素的内容之前插入内容"},
                new { Selector = ":after", Example = "p:after", Description = "在每个 <p> 元素的内容之后插入内容"},
                new { Selector = ":lang(language)", Example = "p:lang(it)", Description = ""},
                new { Selector = "element1~element2", Example = "p~ul", Description = "选择前面有 <p> 元素的每个 <ul> 元素"},
                new { Selector = "[attribute^=value]", Example = "a[src^=\"https\"]", Description = "选择其 src 属性值以 \"https\" 开头的每个 <a> 元素"},
                new { Selector = "[attribute$=value]", Example = "a[src$=\".pdf\"]", Description = "选择其 src 属性以 \".pdf\" 结尾的所有 <a> 元素"},
                new { Selector = "[attribute*=value]", Example = "a[src*=\"abc\"]", Description = "选择其 src 属性中包含 \"abc\" 子串的每个 <a> 元素"},
                new { Selector = ":first-of-type", Example = "p:first-of-type", Description = "选择属于其父元素的首个 <p> 元素的每个 <p> 元素"},
                new { Selector = ":last-of-type", Example = "p:last-of-type", Description = "选择属于其父元素的最后 <p> 元素的每个 <p> 元素"},
                new { Selector = ":only-of-type", Example = "p:only-of-type", Description = "选择属于其父元素唯一的 <p> 元素的每个 <p> 元素"},
                new { Selector = ":only-child", Example = "p:only-child", Description = "选择属于其父元素的唯一子元素的每个 <p> 元素"},
                new { Selector = ":nth-child(n)", Example = "p:nth-child(2)", Description = "选择属于其父元素的第二个子元素的每个 <p> 元素"},
                new { Selector = ":nth-last-child(n)", Example = "p:nth-last-child(2)", Description = "同上，从最后一个子元素开始计数"},
                new { Selector = ":nth-of-type(n)", Example = "p:nth-of-type(2)", Description = "选择属于其父元素第二个 <p> 元素的每个 <p> 元素"},
                new { Selector = ":nth-last-of-type(n)", Example = "p:nth-last-of-type(2)", Description = "同上，但是从最后一个子元素开始计数"},
                new { Selector = ":last-child", Example = "p:last-child", Description = "选择属于其父元素最后一个子元素每个 <p> 元素"},
                new { Selector = ":root", Example = ":root", Description = "选择文档的根元素"},
                new { Selector = ":empty", Example = "p:empty", Description = "选择没有子元素的每个 <p> 元素（包括文本节点）"},
                new { Selector = ":target", Example = "#news:target", Description = "选择当前活动的 #news 元素"},
                new { Selector = ":enabled", Example = "input:enabled", Description = "选择每个启用的 <input> 元素"},
                new { Selector = ":disabled", Example = "input:disabled", Description = "选择每个禁用的 <input> 元素"},
                new { Selector = ":checked", Example = "input:checked", Description = "选择每个被选中的 <input> 元素"},
                new { Selector = ":not(selector)", Example = ":not(p)", Description = "选择非 <p> 元素的每个元素"},
                new { Selector = "::selection", Example = "::selection", Description = "选择被用户选取的元素部分"}          
            };

            this.listview_CSSSelector.ItemsSource = list;
        }

        private void InitXPathTable()
        {
            List<dynamic> pathExpressionList = new List<dynamic>()
            {
                new { PathExpression = "nodename", Description = "选取此节点的所有子节点。"},
                new { PathExpression = "/", Description = "	从根节点选取。"},
                new { PathExpression = "//", Description = "从匹配选择的当前节点选择文档中的节点，而不考虑它们的位置。"},
                new { PathExpression = ".", Description = "选取当前节点。"},
                new { PathExpression = "..", Description = "选取当前节点的父节点。"},
                new { PathExpression = "@", Description = "选取属性。"},
            };

            List<dynamic> pathExpressionExampleList = new List<dynamic>()
            {
                new { PathExpression = "html", Description = "选取html元素的所有子节点。"},
                new { PathExpression = "/html", Description = "	选取根元素html。"},
                new { PathExpression = "body/div", Description = "选取属于body的子元素的所有div元素"},
                new { PathExpression = "//img", Description = "选取所有img元素，而不管它们在文档中的位置。"},
                new { PathExpression = "div//img", Description = "选择属于 div 元素的后代的所有 img 元素，而不管它们位于 div 之下的什么位置。。"},
                new { PathExpression = "//@src", Description = "选取名为src的所有属性"},
            };


            //说明:Html文件的根元素是Html，下面的路径表达式不会返回任何结果 
            //要返回结果要带上完整的路径 如：/html/body/div/img[1]
            List<dynamic> predicatesList = new List<dynamic>()
            {
                new { PathExpression = "div/img[1]", Description = "选取属于 div 子元素的第一个 img 元素"},
                new { PathExpression = "div/img[last()]", Description = "选取属于 div 子元素的最后一个 img 元素。"},
                new { PathExpression = "div/img[last()-1]", Description = "选取属于 div 子元素的倒数第二个 img 元素。"},
                new { PathExpression = "div/img[position()<3]", Description = "选取最前面的两个属于div元素的子元素的img元素"},
                new { PathExpression = "//img[@src]", Description = "选取所有拥有名为src的属性的img元素"},
                new { PathExpression = "//img[@src='abc']", Description = "选取所有img元素，且这些元素拥有值为abc的src属性"}               
            };

            List<dynamic> wildcardList = new List<dynamic>()
            {
                new { PathExpression = "*", Description = "匹配任何元素节点"},
                new { PathExpression = "@*", Description = "匹配任何属性节点"},
                new { PathExpression = "node()", Description = "匹配任何类型的节点"}                
            };

            List<dynamic> wildcardExampleList = new List<dynamic>()
            {
                new { PathExpression = "/html/*", Description = "选取html元素的所有子元素"},
                new { PathExpression = "//*", Description = "选取文档中的所有元素"},
                new { PathExpression = "//img[@*]", Description = "选取所有带有属性的img元素"}
            };

            //运算符下还有一些其它的运算符
            //目前可能我用不到，所以这里不记录了
            //https://www.w3schools.com/xml/xpath_operators.asp
            List<dynamic> operatorList = new List<dynamic>()
            {
                new { PathExpression = "/html/head|/html/body", Description = "选取html元素的head和body元素"},
                new { PathExpression = "//img|//p", Description = "选取文档中的所有img和p元素"},
                new { PathExpression = "//img|/html", Description = "选取文档中的所有img和根节点下的html元素"},
            };

            this.listview_XPathQuery.ItemsSource = pathExpressionList;
            this.listview_XPathQueryExample.ItemsSource = pathExpressionExampleList;
            this.listview_XPathPredicates.ItemsSource = predicatesList;
            this.listview_XPathWildcard.ItemsSource = wildcardList;
            this.listview_XPathWildcardExample.ItemsSource = wildcardExampleList;
            this.listview_XPathOperator.ItemsSource = operatorList;
        }

        private void InitPresetData()
        {
            this.rbox_Input.Document = new FlowDocument(new Paragraph(new Run(Properties.Resources.RegexHtml)));
            this.rbox_XPathInput.Document = new FlowDocument(new Paragraph(new Run(Properties.Resources.RegexHtml)));
        }

        /// <summary>
        /// 在 CSS 中，选择器是一种模式，用于选择需要添加样式的元素。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Query_Click(object sender, RoutedEventArgs e)
        {
            TextRange tr = new TextRange(rbox_Input.Document.ContentStart, rbox_Input.Document.ContentEnd);
            
            var selector = this.tbox_CSSSelector.Text.Trim();
            var html = tr.Text;

            if(string.IsNullOrEmpty(selector))
            {
                EMessageBox.Show("请输入CSS选择器");
                return;
            }

            var angleSharpObj = AngleSharpHelper.GetInstance(html).CSSQuery(selector);

            if(angleSharpObj != null)
            {
                //目前只输出一个结果 
                //过完年再搞 ╮(－_－)╭
                this.rbox_Output.Document = new FlowDocument(new Paragraph(new Run(angleSharpObj.OuterHtml)));
            }
            else
            {
                this.rbox_Output.Document = new FlowDocument(new Paragraph(new Run("未匹配到结果")));
            }
            
        }

        /// <summary>
        /// XPath 是一门在 XML 文档中查找信息的语言。XPath 用于在 XML 文档中通过元素和属性进行导航。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_XPathQuery_Click(object sender, RoutedEventArgs e)
        {
            TextRange tr = new TextRange(rbox_XPathInput.Document.ContentStart,rbox_XPathInput.Document.ContentEnd);
            var html = tr.Text;

            if(string.IsNullOrEmpty(html))
            {
                EMessageBox.Show("请输入html");
                return;
            }

            var result = HtmlAgilityPackUtil.XPathQuery(html,this.tbox_XPath.Text.Trim());
            if(result != null)
            {
                Paragraph paragraph = new Paragraph();
                foreach (var item in result)
                {
                    paragraph.Inlines.Add(new Run(item.OuterHtml + Environment.NewLine));
                }
                this.rbox_XPathOutput.Document = new FlowDocument(paragraph);
            }
            else
            {
                this.rbox_XPathOutput.Document.Blocks.Clear();
            }
        }
    }
}
