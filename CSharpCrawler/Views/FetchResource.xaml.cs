using CSharpCrawler.Model;
using CSharpCrawler.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
    /// FetchResource.xaml 的交互逻辑
    /// </summary>
    public partial class FetchResource : Page
    {
        public FetchResource()
        {
            InitializeComponent();
        }

        private void btn_Fetch_Click(object sender, RoutedEventArgs e)
        {            
            string url = this.tbox_Url.Text;
            Encoding encoding = GetChoosedEncoding();

            if(UrlUtil.IsEmpty(url))
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

            ClearControls();

            GetHostIP(url,isStartWithHttp);
            GetHeaderInfo(url,isStartWithHttp);
            GetHtmlSource(url, isStartWithHttp,encoding);
        }

        private void ClearControls()
        {
            this.stackpanel.Children.Clear();
        }

        private Encoding GetChoosedEncoding()
        {
            switch(this.combox_Encoding.SelectedIndex)
            {
                case 0:
                    return null;
                case 1:
                    return Encoding.UTF8;
                case 2:
                    return Encoding.GetEncoding("gb2312");                                
                default:
                    return Encoding.Default;
            }
        }

        private void GetHostIP(string url,bool isStartWithHttp)
        {
            try
            {
                string ipTempStr = "";

                if(isStartWithHttp)
                {
                    url = url.Replace("http://", "");
                    url = url.Replace("https://", "");
                }

                WrapPanel wrapPanel = new WrapPanel();
                Label hostIPLabel = new Label();
                hostIPLabel.Width = 80;
                hostIPLabel.Margin = new Thickness(0, 3, 0, 3);
                IPAddress[] hostIPAddresses = WebUtil.GetHostIP(url);
                foreach (var item in hostIPAddresses)
                {
                    ipTempStr += item.ToString() + ";";
                }
                hostIPLabel.Content = "服务器IP:";
                TextBox hostIPTbx = new TextBox();
                hostIPTbx.Width = 300;
                hostIPTbx.Margin = new Thickness(0, 3, 0, 3);
                hostIPTbx.VerticalContentAlignment = VerticalAlignment.Center;
                hostIPTbx.IsReadOnly = true;
                hostIPTbx.Text = ipTempStr;
                wrapPanel.Children.Add(hostIPLabel);
                wrapPanel.Children.Add(hostIPTbx);

                //TODO
                //IP查询 

                this.stackpanel.Children.Add(wrapPanel);
            }
            catch(Exception ex)
            {
                EMessageBox.Show(ex.Message);
            }
        }

        private void GetHeaderInfo(string url,bool isStartWithHttp)
        {
            try
            {
                try
                {
                    if (isStartWithHttp == false)
                    {
                        url = "http://" + url;
                    }
                    HttpHeader header = WebUtil.GetHeader(url);


                    Label charsetLabel = new Label();
                    charsetLabel.Margin = new Thickness(0, 3, 0, 3);
                    charsetLabel.Content = "Charset:" + header.CharSet;

                    //Label contentEncodingLabel = new Label();
                    //contentEncodingLabel.Margin = new Thickness(0, 3, 0, 3);
                    //contentEncodingLabel.Content = "ContentEncoding:" + header.ContentEncoding; //不准确

                    Label contentTypeLabel = new Label();
                    contentTypeLabel.Margin = new Thickness(0, 3, 0, 3);
                    contentTypeLabel.Content = "ContentType:" + header.ContentType;

                    Label serverNameLabel = new Label();
                    serverNameLabel.Margin = new Thickness(0, 3, 0, 3);
                    serverNameLabel.Content = "Server:" + header.Server;

                    this.stackpanel.Children.Add(charsetLabel);
                    //this.stackpanel.Children.Add(contentEncodingLabel);
                    this.stackpanel.Children.Add(contentTypeLabel);
                    this.stackpanel.Children.Add(serverNameLabel);
                }
                catch(Exception ex)
                {
                    EMessageBox.Show(ex.Message);
                }    
            }
            catch(Exception ex)
            {
                EMessageBox.Show(ex.Message);
            }
        }

        private async void GetHtmlSource(string url, bool isStartWithHttp,Encoding encoding = null)
        {
            if (isStartWithHttp == false)
            {
                url = "http://" + url;
            }
            try
            {
                string sourceCode = await WebUtil.GetHtmlSource(url,encoding);
                RichTextBox richTextBox = new RichTextBox();
                richTextBox.Height = this.stackpanel.ActualHeight - 180;
                richTextBox.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
                richTextBox.Document = new FlowDocument(new Paragraph(new Run(sourceCode)));
                this.stackpanel.Children.Add(richTextBox);
            }
            catch(Exception ex)
            {
                EMessageBox.Show(ex.Message);
            }
        }
    }
}
