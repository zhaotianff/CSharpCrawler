using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net;
using System.IO;

namespace CSharpCrawler
{
    /// <summary>
    /// GetHtmlSourceCode.xaml 的交互逻辑
    /// </summary>
    public partial class GetHtmlSourceCode : Page
    {
        public GetHtmlSourceCode()
        {
            InitializeComponent();
        }

        private void btn_Sync_Click(object sender, RoutedEventArgs e)
        {
            string url = tbx_Url.Text;
            if(string.IsNullOrEmpty(url))
            {
                MessageBox.Show("请输入网址");
                return;
            }

            //this.rtbx_Content.Document = new FlowDocument(new Paragraph(new Run(GetSourceCodeSync(url))));
            this.rtbx_Content.Document = new FlowDocument(new Paragraph(new Run(GetSourceCodeSync2(url))));
        }

        private void btn_Async_Click(object sender, RoutedEventArgs e)
        {

        }

        /// <summary>
        /// 同步获取网页源码
        /// </summary>
        /// <param name="url">网址</param>
        /// <returns></returns>
        public string GetSourceCodeSync(string url)
        {
            try
            {
                WebClient client = new WebClient();
                Stream stream =  client.OpenRead(url);
                //在这里指定编码
                using(StreamReader sr = new StreamReader(stream,Encoding.UTF8))
                {
                    return sr.ReadToEnd();
                }
            }
            catch(Exception ex)
            {
                return ex.Message;
            }
            
        }

        /// <summary>
        /// 同步获取网页源码2
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public string GetSourceCodeSync2(string url)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream stream = response.GetResponseStream();
                //在这里指定编码
                using (StreamReader sr = new StreamReader(stream, Encoding.UTF8))
                {
                    return sr.ReadToEnd();
                }
            }
            catch(Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
