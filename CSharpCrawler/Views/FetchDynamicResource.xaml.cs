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
using CefSharp;
using CSharpCrawler.Model;

namespace CSharpCrawler.Views
{
    /// <summary>
    /// FetchDynamicResource.xaml 的交互逻辑
    /// </summary>
    public partial class FetchDynamicResource : Page
    {
        //动态网页加载次数
        int count = 0;
        //记录当前网址动态加载获取到的网页内容
        List<HtmlStruct> recordList = new List<HtmlStruct>();

        public FetchDynamicResource()
        {
            InitializeComponent();
        }

        #region 事件
        private void cbox_ShowChrome_Checked(object sender, RoutedEventArgs e)
        {
            ShowOrHideChromiumBrowser(true);
        }

        private void cbox_ShowChrome_Unchecked(object sender, RoutedEventArgs e)
        {
            ShowOrHideChromiumBrowser(false);
        }

       

        private void btn_Fetch_Click(object sender, RoutedEventArgs e)
        {
            string url = this.tbox_Url.Text.Trim();
            if(string.IsNullOrEmpty(url))
            {
                MessageBox.Show("请输入网址");
                return;
            }

            OpenUrlWithChromium(url);
        }

        /// <summary>
        /// Chromium加载网页完成事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void chromiumBrowser_FrameLoadEnd(object sender, CefSharp.FrameLoadEndEventArgs e)
        {
            count++;
            string url = "";
            string title = "";
            ShowCountMessage(count);
            string html =await chromiumBrowser.GetSourceAsync();
            this.Dispatcher.Invoke(()=> {
                url = chromiumBrowser.Address;
                title= chromiumBrowser.Title;
            });
            ShowHtml(html,url,title);
        }


        private void combox_Record_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = this.combox_Record.SelectedIndex;
            if (index > -1)
                ShowRecord(index);
        }

        #endregion

        #region 方法

        /// <summary>
        /// 显示或隐藏浏览器
        /// </summary>
        /// <param name="showFlag"></param>
        private void ShowOrHideChromiumBrowser(bool showFlag)
        {
            if (showFlag == true)
            {
                this.chromiumBrowser.Width = this.grid_Content.ActualWidth / 2;
            }
            else
            {
                this.chromiumBrowser.Width = 0;
            }
        }

        /// <summary>
        /// 使用Chromium浏览器打开网址
        /// </summary>
        /// <param name="url"></param>
        private void OpenUrlWithChromium(string url)
        {
            ResetCounter();
            ClearRecordList();
            this.chromiumBrowser.Address = url;
        }

        /// <summary>
        /// 重置计数器
        /// </summary>
        private void ResetCounter()
        {
            count = 0;
            ShowCountMessage(count);
        }

        /// <summary>
        /// 显示计数信息
        /// </summary>
        /// <param name="count"></param>
        private void ShowCountMessage(int count)
        {
            this.Dispatcher.Invoke(()=> {
                this.lbl_StatusText.Content = "当前网址抓取次数: " + count.ToString();
            });
        }

        /// <summary>
        /// 显示Html到文本框中
        /// </summary>
        /// <param name="html"></param>
        private void ShowHtml(string html,string url,string title)
        {
            this.Dispatcher.Invoke(()=> {
                this.rtbox_Resource.Document = new FlowDocument(new Paragraph(new Run(html)));
                HtmlStruct htmlStruct = new HtmlStruct() { Content = html,Url = url,Title = title };
                RecordHtml(htmlStruct);
            }); 
            
        }

        /// <summary>
        /// 记录动态加载的网页内容
        /// </summary>
        /// <param name="html"></param>
        private void RecordHtml(HtmlStruct htmlStruct)
        {
            recordList.Add(htmlStruct);
            this.combox_Record.ItemsSource = null;
            this.combox_Record.ItemsSource = recordList.Select(x=>x.Url); ;
        }

        /// <summary>
        /// 清空网页记录列表
        /// </summary>
        private void ClearRecordList()
        {
            recordList.Clear();
            this.combox_Record.ItemsSource = null;
        }

        /// <summary>
        /// 获取历史记录
        /// </summary>
        /// <param name="index"></param>
        private void ShowRecord(int index)
        {
            if (index >= 0 && index < recordList.Count)
            {
                this.Dispatcher.Invoke(()=> {
                    string html = recordList[index].Content;
                    this.rtbox_Resource.Document = new FlowDocument(new Paragraph(new Run(html)));
                });
            }
        }
        #endregion
    }
}
