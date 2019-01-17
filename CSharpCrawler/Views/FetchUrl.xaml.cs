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

            //判断Url
            if (globalData.CrawlerConfig.UrlConfig.IgnoreUrlCheck == false)
            {
                if (RegexUtil.IsUrl(url, out isStartWithHttp) == false)
                {
                    ShowStatusText("网址输入有误");
                    return;
                }

                if (isStartWithHttp == false)
                {
                    url = "http://" + url;
                }
            }

            Reset();
            Surfing(url);
        }

        private async void btn_SurfingDOM_Click(object sender, RoutedEventArgs e)
        {
            //暂使用Http相关类获取网页源码
            try
            {
                string url = this.tbox_Url.Text.Trim();
                string html = await WebUtil.GetHtmlSource(url);
                HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
                doc.LoadHtml(html);
                HtmlAgilityPack.HtmlNodeCollection nodeCollection = doc.DocumentNode.SelectNodes("//a");
                ClearCollection();
                for (int i = 0; i < nodeCollection.Count; i++)
                {
                    AddToCollection(new UrlStruct() { Id = (i + 1), Status = "", Title = "", Url = nodeCollection[i].Attributes["href"].Value });
                }
            }
            catch(Exception ex)
            {
                ShowStatusText(ex.Message);
            }
        }

        public void ShowStatusText(string content)
        {
            this.Dispatcher.Invoke(()=> {
                this.lbl_Status.Content = content;
            });
        }

        public void Surfing(string url)
        {  
            if(globalData.CrawlerConfig.UrlConfig.DynamicGrab == true)
            {
                SurfingByCEF(url);
            }
            else
            {
                SurfingByFCL(url);
            }
        }

        public void SurfingByCEF(string url)
        {

        }

        public async void SurfingByFCL(string url)
        {
            try
            {
                globalUrl = url;
                string html = await WebUtil.GetHtmlSource(url);

                Thread extractThread = new Thread(new ParameterizedThreadStart(ExtractUrl));
                extractThread.IsBackground = true;
                extractThread.Start(html);
            }
            catch (Exception ex)
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
                    ToVisitList.Add(urlStruct);
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

        private async void ExtractUrl(object html)
        {
            string value = "";
            string source = "";

            MatchCollection mc = RegexUtil.Match(html.ToString(), RegexPattern.TagAPattern);
            foreach (Match item in mc)
            {
                value = item.Groups["url"].Value;

                if (value.StartsWith("http://") || value.StartsWith("https://") || value.StartsWith("ftp://"))
                {
                    AddToCollection(new UrlStruct() { Title = "", Id = globalIndex, Status = "", Url = item.Groups["url"].Value });
                    IncrementCount();
                }
                else if (value.StartsWith("/"))
                {
                    AddToCollection(new UrlStruct() { Title = "", Id = globalIndex, Status = "", Url = globalUrl + item.Groups["url"].Value });
                    IncrementCount();
                }
            }

            //抓取网页标题
            for (int i = 0; i < urlCollection.Count; i++)
            {
                try
                {
                    source = await WebUtil.GetHtmlSource(urlCollection[i].Url);
                }
                catch (Exception ex)
                {
                    ShowStatusText(ex.Message);
                    continue;
                }

                mc = RegexUtil.Match(source, RegexPattern.TagTitlePattern);
                if (mc.Count > 0)
                {
                    urlCollection[i].Title = mc[0].Groups["title"].Value;
                }

                //Surfing(urlCollection[i].Url);  暂不进行深度爬取
            }
        }

        private void IncrementCount()
        {
            System.Threading.Interlocked.Increment(ref globalIndex);
        }      
    }
}
