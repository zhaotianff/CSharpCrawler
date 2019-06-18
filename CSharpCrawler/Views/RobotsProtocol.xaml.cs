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
using System.IO;
using ZT.Enhance;
using CSharpCrawler.Util;
using System.Net;

namespace CSharpCrawler.Views
{
    /// <summary>
    /// RobotsProtocol.xaml 的交互逻辑
    /// </summary>
    public partial class RobotsProtocol : Page
    {
        private bool hasLoaded = false;

        public RobotsProtocol()
        {
            InitializeComponent();
        }

        public void LoadContent()
        {
            if (hasLoaded)
                return;

            try
            {
                using (FileStream fs = File.Open("../../../RobotsExclusionProtocol.txt", FileMode.Open))
                {
                    using (StreamReader sr = new StreamReader(fs))
                    {
                        this.lbl_Content.Document = new FlowDocument(new Paragraph(new Run(sr.ReadToEnd()))); 
                    }
                }

                hasLoaded = true;
            }
            catch(Exception ex)
            {
                this.lbl_Content.Document = new FlowDocument(new Paragraph(new Run(ex.Message))); 
            }
        }

        private async void btn_Fetch_Click(object sender, RoutedEventArgs e)
        {
            var url = this.tbox_Url.Text.Trim();
            if(string.IsNullOrEmpty(url))
            {
                EMessageBox.Show("请输入网址");
                return;
            }
         
            var robotsUrl = url.EndsWith("/") ? url + "robots.txt" : url + "/robots.txt";
            robotsUrl = UrlUtil.FixUrl(robotsUrl);

            if(WebUtil.IsResourceAvailable(robotsUrl) == false)
            {
                this.lbl_Result.Text = "该网站没有爬虫协议";
                return;
            }

            var stream = await WebUtil.GetHtmlStreamAsync(robotsUrl);
            StreamReader sr = new StreamReader(stream);
            this.lbl_Result.Text = sr.ReadToEnd();
            sr.Close();
            stream.Close();
        }
    }
}
