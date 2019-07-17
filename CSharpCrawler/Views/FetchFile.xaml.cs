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
using CSharpCrawler.Model;
using CSharpCrawler.Util;
using ZT.Enhance;

namespace CSharpCrawler.Views
{
    /// <summary>
    /// FetchFile.xaml 的交互逻辑
    /// </summary>
    public partial class FetchFile : UserControl
    {
        GlobalDataUtil globalData = GlobalDataUtil.GetInstance();

        Paragraph paragraph = new Paragraph();

        public FetchFile()
        {
            InitializeComponent();
        }

        private async void btn_Donwload_Click(object sender, RoutedEventArgs e)
        {
            string url = this.tbox_Url.Text.Trim();
            if(string.IsNullOrEmpty(url))
            {
                EMessageBox.Show("请输入下载地址");
                return;
            }

            if (globalData.CrawlerConfig.ImageConfig.IgnoreUrlCheck == false)
            {
                //TODO 
                //文件URL检验
            }

            ShowStatusText($"check {url} status");
            HttpHeader httpHeader =  WebUtil.GetHeader(url);
            if (httpHeader.StatusCode != System.Net.HttpStatusCode.OK)
            {
                ShowStatusText($"{url} is not available");
                return;
            }

            ShowStatusText($"{url} is available");
            ShowStatusText($"Download file {url}.....");
            string fileName = await WebUtil.DownloadFileAsync(url);
            ShowStatusText($"Download : {url} Finished\r\n");

            //Temp
            if(System.IO.Path.GetExtension(url) == ".mp4")
            {
                (Application.Current.MainWindow as MainWindow).SetTransparentBackground();
                (Application.Current.MainWindow as MainWindow).SetBackgroundVideo(fileName);
            }
        }

        private void btn_MultiThreadDonwload_Click(object sender, RoutedEventArgs e)
        {
            string url = this.tbox_Url.Text.Trim();
            if (string.IsNullOrEmpty(url))
            {
                EMessageBox.Show("请输入下载地址");
                return;
            }

            if (globalData.CrawlerConfig.ImageConfig.IgnoreUrlCheck == false)
            {
                //TODO 
                //文件URL检验
            }

            ShowStatusText($"check {url} status");
            HttpHeader httpHeader = WebUtil.GetHeader(url);
            if (httpHeader.StatusCode != System.Net.HttpStatusCode.OK)
            {
                ShowStatusText($"{url} is not available");
                return;
            }

            ShowStatusText($"{url} is available");
            ShowStatusText($"Download file {url}.....");

            WebUtil.DownloadFileWithProgress(url, ShowStatusText);
        }

        private async void btn_DownLoadFromFile_Click(object sender, RoutedEventArgs e)
        {
            var downListFile = Environment.CurrentDirectory + "\\download\\list.txt";
            string str = "";
            string name = "";
            int count = 0;
            Dictionary<string, string> nameUrlPairs = new Dictionary<string, string>();
            List<Task<string>> taskList = new List<Task<string>>();

            if (!System.IO.File.Exists(downListFile))
            {
                ShowStatusText($"{downListFile} is not exist!!!");
                return;
            }

            //还是比较乱，有空再整理吧
            using (System.IO.FileStream fs = System.IO.File.Open(downListFile, System.IO.FileMode.Open))
            {
                using (System.IO.StreamReader sr = new System.IO.StreamReader(fs))
                {
                    while((str = sr.ReadLine()) != null)
                    {
                        count++;

                        if (count % 2 == 0)
                        {
                            nameUrlPairs.Add(name, str);
                        }
                        else
                        {
                            name = str;
                        }
                    }
                }
            }

            ShowStatusText("load download list from list.txt");

            foreach (var item in nameUrlPairs)
            {
                ShowStatusText(item.Value);
                taskList.Add(WebUtil.DownloadFileAsync(item.Value));
            }

            while (taskList.Count > 0)
            {
                
                Task<string> finishedTask = await Task.WhenAny(taskList);
                taskList.Remove(finishedTask);

                try
                {
                    string fileName = await finishedTask;
                    ShowStatusText($"{fileName} download finished");
                }
                catch(Exception ex)
                {
                    ShowStatusText(ex.Message);
                }
            }

        }

        private void ShowStatusText(string text)
        {
            this.Dispatcher.Invoke(()=> {
                paragraph.Inlines.Add(new Run(DateTime.Now.ToString() + "\t" +  text + "\r\n"));
                this.rtbox_Status.Document = new FlowDocument(paragraph);
            });
        }
    }
}
