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
        private const int MaxSettingPanelWidth = 200;
        private const int MaxThreadCount = 3;

        ObservableCollection<UrlStruct> urlCollection = new ObservableCollection<UrlStruct>();
        List<UrlStruct> ToVisitList = new List<UrlStruct>();
        List<UrlStruct> VisitedList = new List<UrlStruct>();

        GlobalDataUtil globalData = GlobalDataUtil.GetInstance();

        object obj = new object();
        int globalIndex = 1;
        int globalRecursionDepth = 1;
        string baseUrl = "";

        public FetchUrl()
        {
            InitializeComponent();
            this.listview_Url.ItemsSource = urlCollection;
        }

        private void btn_Surfing_Click(object sender, RoutedEventArgs e)
        {
            string url = this.tbox_Url.Text;

            if(string.IsNullOrEmpty(url))
            {
                ShowStatusText("请输入Url");
                return;
            }

            Reset();
            Surfing(url, StartUrlExtractThreadWithRegex);
        }

        private void btn_SurfingDOM_Click(object sender, RoutedEventArgs e)
        {
            string url = this.tbox_Url.Text;

            if (string.IsNullOrEmpty(url))
            {
                ShowStatusText("请输入Url");
                return;
            }

            Reset();
            Surfing(url, StartUrlExtractThreadWithDom);
        }

        private void btn_ShowSetting_Click(object sender, RoutedEventArgs e)
        {
            if (grid_Setting.Width == MaxSettingPanelWidth)
                grid_Setting.Width = 0;
            else
                grid_Setting.Width = MaxSettingPanelWidth;
        }

        public void ShowStatusText(string content)
        {
            this.Dispatcher.Invoke(()=> {
                this.lbl_Status.Content = content;
            });
        }

        public void Surfing(string url,Action<string> act)
        {
            //从界面获取设置
            var recursionDepth = 1;
            int.TryParse(this.tbx_RecursionDepth.Text, out recursionDepth);

            baseUrl = UrlUtil.ExtractBaseUrl(url);

            if (recursionDepth == 1)
            {
                //使用CEF
                SurfingByCEF(url, act);
            }
            else
            {
                //使用HttpWebRequest
                SurfingByFCL(url,recursionDepth);          
            }
        }

        public void SurfingByCEF(string url,Action<string> act)
        {
            globalData.Browser.GetHtmlSourceDynamic(url,act);
        }

        public async void SurfingByFCL(string url,int recursionDepth)
        {
            try
            {
                baseUrl =UrlUtil.ExtractBaseUrl(url);
                string html = await WebUtil.GetHtmlSource(url);

                Thread extractThread = new Thread(new ParameterizedThreadStart(ExtractUrlWithDOMRecursion));
                extractThread.IsBackground = true;
                extractThread.Start(html);
            }
            catch (Exception ex)
            {
                //TODO
                ShowStatusText(ex.Message);
            }
        }

        public void StartUrlExtractThreadWithRegex(string html)
        {
            new Thread(ExtractUrlWithRegex) { IsBackground = true }.Start(html);
        }

        public void StartUrlExtractThreadWithDom(string html)
        {
            new Thread(ExtractUrlWithDOM) { IsBackground = true }.Start(html);
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
                ShowStatusText("获取到" + urlCollection.Count + "条链接");
            }
        }


        public void ClearCollection()
        {
            this.Dispatcher.Invoke(()=> { urlCollection.Clear(); });
        }

        public void Reset()
        {
            ClearCollection();
            globalIndex = 1;
            ShowStatusText("");
        }

        private void ExtractUrlWithRegex(object html)
        {
            string value = "";

            MatchCollection mc = RegexUtil.Matches(html.ToString(), RegexPattern.TagAPattern);
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
                    AddToCollection(new UrlStruct() { Title = "", Id = globalIndex, Status = "", Url = baseUrl + item.Groups["url"].Value });
                    IncrementCount();
                }
            }
        }

        private void ExtractUrlWithDOM(object html)
        {
            var url = "";
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(html.ToString());
            HtmlAgilityPack.HtmlNodeCollection nodeCollection = doc.DocumentNode.SelectNodes("//a");
            ClearCollection();
            for (int i = 0; i < nodeCollection.Count; i++)
            {
                var hrefAttribute = nodeCollection[i].Attributes["href"];
                if (hrefAttribute == null)
                    continue;

                url = hrefAttribute.Value;

                if (string.IsNullOrEmpty(url))
                    continue;

                if (url.StartsWith("/"))
                    url = baseUrl + url;
                AddToCollection(new UrlStruct() { Id = (i + 1), Status = "", Title = "", Url = url});
            }
        }

        private void ExtractUrlWithDOMRecursion(object tupleObj)
        {
            Tuple<int, string> tuple = (Tuple<int, string>)tupleObj;
            var url = "";
            var html = tuple.Item2;
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(html.ToString());
            HtmlAgilityPack.HtmlNodeCollection nodeCollection = doc.DocumentNode.SelectNodes("//a");
            ClearCollection();
            for (int i = 0; i < nodeCollection.Count; i++)
            {
                var hrefAttribute = nodeCollection[i].Attributes["href"];
                if (hrefAttribute == null)
                    continue;

                url = hrefAttribute.Value;

                if (string.IsNullOrEmpty(url))
                    continue;

                if (url.StartsWith("/"))
                    url = baseUrl + url;
                AddToCollection(new UrlStruct() { Id = (i + 1), Status = "", Title = "", Url = url });
            }
        }

        private void IncrementCount()
        {
            System.Threading.Interlocked.Increment(ref globalIndex);
        }    
        
        private void IncrementRecursionDepthCount()
        {
            System.Threading.Interlocked.Increment(ref globalRecursionDepth);
        }
    }
}
