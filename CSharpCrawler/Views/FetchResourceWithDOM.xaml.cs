using CSharpCrawler.Model;
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
    /// FetchResourceWithDOM.xaml 的交互逻辑
    /// </summary>
    public partial class FetchResourceWithDOM : Page
    {
        public FetchResourceWithDOM()
        {
            InitializeComponent();
        }

        private void cbox_ShowDOMTree_Checked(object sender, RoutedEventArgs e)
        {
            this.tree_DOM.Width = this.grid_Content.ActualWidth / 2;
        }

        private void cbox_ShowDOMTree_Unchecked(object sender, RoutedEventArgs e)
        {
            this.tree_DOM.Width = 0;
        }

        private void btn_Fetch_Click(object sender, RoutedEventArgs e)
        {
            string url = this.tbox_Url.Text;
            if (Urls.IsEmpty(url))
            {
                EMessageBox.Show("请输入网址");
                this.tbox_Url.Focus();
                return;
            }

            bool isStartWithHttp = false;
            if (RegexUtil.IsUrl(url, out isStartWithHttp) == false)
            {
                EMessageBox.Show("网址输入错误");
                this.tbox_Url.Focus();
                return;
            }

            GetHtmlSource(url, isStartWithHttp);
        }


        private async void GetHtmlSource(string url, bool isStartWithHttp)
        {
            if (isStartWithHttp == false)
            {
                url = "http://" + url;
            }
            try
            {
                string sourceCode = await WebUtil.GetHtmlSource(url);               
                this.rtbox_Resource.Document = new FlowDocument(new Paragraph(new Run(sourceCode)));
                GenerateDOMTree(sourceCode);             
            }
            catch (Exception ex)
            {
                EMessageBox.Show(ex.Message);
            }
        }


        /// <summary>
        /// 使用HtmlAgilityPack生成DOM树
        /// </summary>
        /// <param name="source"></param>
        private void GenerateDOMTree(string source)
        {
            Task.Run(()=> {
                HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
                doc.LoadHtml(source);
                var root = doc.DocumentNode;

                //获取html结果 
                var htmlNode = root.SelectSingleNode("html");

                this.Dispatcher.Invoke(()=> {
                    this.tree_DOM.Items.Clear();
                    NodeStruct nodeStruct = new NodeStruct();
                    nodeStruct.InnerHtml = "<html></html>";
                    nodeStruct.InnerText = "";
                    nodeStruct.DisplayName = "html";
                    NodeRecursion(ref nodeStruct, htmlNode);
                    this.tree_DOM.Items.Add(nodeStruct);

                    if(this.tree_DOM.ActualWidth < this.grid_Content.ActualWidth  / 2)
                        this.tree_DOM.Width = 0;
                });
            });
        }

        private void NodeRecursion(ref NodeStruct nodeStruct, HtmlAgilityPack.HtmlNode htmlNode)
        {
            HtmlAgilityPack.HtmlNodeCollection childCollection = htmlNode.ChildNodes;

            List<NodeStruct> list = new List<NodeStruct>();

            for (int i = 0; i < childCollection.Count; i++)
            {
                if (string.IsNullOrEmpty(childCollection[i].InnerHtml.Trim()))
                    continue;

                NodeStruct htmlStruct = new NodeStruct();
                htmlStruct.DisplayName = childCollection[i].Name;
                htmlStruct.OuterHtml = childCollection[i].OuterHtml;
                htmlStruct.InnerHtml = childCollection[i].InnerHtml;
                htmlStruct.InnerText = childCollection[i].InnerText;
                list.Add(htmlStruct);
                nodeStruct.Children = list;

                if (childCollection[i].HasChildNodes)
                    NodeRecursion(ref htmlStruct, childCollection[i]);
            }

            
        }
    }
}
