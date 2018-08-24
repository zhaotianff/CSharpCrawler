using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Text.RegularExpressions;

using CSharpCrawler.Model;
using CSharpCrawler.Util;
using System.Threading;

namespace CSharpCrawler.Views
{
    /// <summary>
    /// FetchUrl.xaml 的交互逻辑
    /// </summary>
    public partial class FetchUrl : Page
    {
        ObservableCollection<UrlStruct> urlCollection = new ObservableCollection<UrlStruct>();
        List<UrlStruct> ToVisitList = new List<UrlStruct>();
        List<UrlStruct> VisitedList = new List<UrlStruct>();

        GlobalDataUtil globalData = GlobalDataUtil.GetInstance();

        object obj = new object();
        int globalIndex = 1;
        string globalUrl = "";

        public FetchUrl()
        {
            InitializeComponent();
            this.listview_Url.ItemsSource = urlCollection;
        }

        private void btn_Surfing_Click(object sender, RoutedEventArgs e)
        {
            string url = this.tbox_Url.Text;
            bool isStartWithHttp = false;

            if(string.IsNullOrEmpty(url))
            {
                ShowStatusText("请输入Url");
                return;
            }

            if (globalData.CrawlerConfig.UrlConfig.IgnoreUrlCheck == false)
            {
                if (RegexUtil.IsUrl(url, out isStartWithHttp) == false)
                {
                    ShowStatusText("网址输入有误");
                    return;
                }
            }

            if(isStartWithHttp == false)
            {
                url = "http://" + url;
            }

            Surfing(url);
        }

        public void ShowStatusText(string content)
        {
            this.Dispatcher.Invoke(()=> {
                this.lbl_Status.Content = content;
            });
        }

        public async void Surfing(string url)
        {
            Reset();

            try
            {
                globalUrl = url;
                string html = await WebUtil.GetHtmlSource(url);

                Thread extractThread = new Thread(new ParameterizedThreadStart(ExtractUrl));
                extractThread.IsBackground = true;
                extractThread.Start(html);
            }
            catch(Exception ex)
            {
                //TODO
                ShowStatusText(ex.Message);
            }
        }


        public void AddToCollection(UrlStruct urlStruct)
        {
            lock(obj)
            {
                var query = urlCollection.Where(x=>x.Url == urlStruct.Url).FirstOrDefault();
                if (query != null)
                    return;
                Dispatcher.Invoke(()=> {
                    urlCollection.Add(urlStruct);
                }); 
            }
        }


        public void ClearCollection()
        {
            urlCollection.Clear();
        }

        public void Reset()
        {
            ClearCollection();
            globalIndex = 1;
            ShowStatusText("");
        }

        private void ExtractUrl(object html)
        {
            string value = "";

            MatchCollection mc = RegexUtil.Match(html.ToString(), RegexPattern.TagAPattern);
            foreach (Match item in mc)
            {
                value = item.Groups["url"].Value;

                if (value.StartsWith("http://") || value.StartsWith("https://") || value.StartsWith("ftp://"))
                {
                    AddToCollection(new UrlStruct() { Title = "", Id = globalIndex, Status = "", Url = item.Groups["url"].Value });
                    IncrementCount();
                }
                else if(value.StartsWith("/"))
                {
                    AddToCollection(new UrlStruct() { Title = "", Id = globalIndex, Status = "", Url =globalUrl + item.Groups["url"].Value });
                    IncrementCount();
                }
            }       
        }

        private void IncrementCount()
        {
            System.Threading.Interlocked.Increment(ref globalIndex);
        }
    }
}
