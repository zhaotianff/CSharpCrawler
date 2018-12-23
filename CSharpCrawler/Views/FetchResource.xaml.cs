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
            if(Urls.IsEmpty(url))
            {
                MessageBox.Show("请输入网址");
                this.tbox_Url.Focus();
                return;
            }

            ClearControls();

            GetHostIP(url);
            GetHeaderInfo(url);
        }

        private void ClearControls()
        {
            this.stackpanel.Children.Clear();
        }

        private void GetHostIP(string url)
        {
            try
            {
                string ipTempStr = "";
                Label hostIPLabel = new Label();
                hostIPLabel.Margin = new Thickness(0, 3, 0, 3);
                IPAddress[] hostIPAddresses = WebUtil.GetHostIP(url);
                foreach (var item in hostIPAddresses)
                {
                    ipTempStr += item.ToString() + ";";
                }
                hostIPLabel.Content = "服务器IP:" + ipTempStr;

                //TODO
                //IP查询 

                this.stackpanel.Children.Add(hostIPLabel);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void GetHeaderInfo(string url)
        {
            try
            {
                HttpHeader header =  WebUtil.GetHeader(url);
                Label charsetLabel = new Label();
                charsetLabel.Margin = new Thickness(0, 3, 0, 3);
                charsetLabel.Content = "Charset:" + header.CharSet;
                Label contentEncodingLabel = new Label();
                contentEncodingLabel.Margin = new Thickness(0, 3, 0, 3);
                contentEncodingLabel.Content = "ContentEncoding:" + header.ContentEncoding; //不准确
                Label contentTypeLabel = new Label();
                contentTypeLabel.Margin = new Thickness(0, 3, 0, 3);
                contentTypeLabel.Content = "ContentType:" + header.ContentType;

                this.stackpanel.Children.Add(charsetLabel);
                this.stackpanel.Children.Add(contentEncodingLabel);
                this.stackpanel.Children.Add(contentTypeLabel);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void GetSource(string url)
        {

        }
    }
}
