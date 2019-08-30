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
    /// 重在演示功能
    /// </summary>
    public partial class FetchUrl : Page
    {
        private const int MaxSettingPanelWidth = 200;
        private const int MaxThreadCount = 3;
        private const int StartDepth = 1;

        ObservableCollection<UrlStruct> urlCollection = new ObservableCollection<UrlStruct>();
        List<UrlStruct> ToVisitList = new List<UrlStruct>();
        List<UrlStruct> VisitedList = new List<UrlStruct>();

        GlobalDataUtil globalData = GlobalDataUtil.GetInstance();

        object obj = new object();
        int globalIndex = 1;
        int recursionDepth = 1;
        string globalBaseUrl = "";
        bool isGrabCurrentPageUrl = false;

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
            //从界面获取值
            int.TryParse(this.tbx_RecursionDepth.Text, out recursionDepth);
            if (this.cbx_CurrentPage.IsChecked.Value == true)
                isGrabCurrentPageUrl = true;

            globalBaseUrl = UrlUtil.ExtractBaseUrl(url);

            if (recursionDepth == StartDepth)
            {
                //使用CEF
                SurfingByCEF(url,act);
            }
            else
            {
                SurfingByFCL(url,act);          
            }
        }

        public void SurfingByCEF(string url,Action<string> act)
        {
            globalData.Browser.GetHtmlSourceDynamic(url,act);
        }

        public async void SurfingByFCL(string url, Action<string> act)
        {
            try
            {
                //Url Check
                url = UrlUtil.FixUrl(url);

                string html = await WebUtil.GetHtmlSource(url);

                act?.Invoke(html);
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

        public void AddToCollection(UrlStruct urlStruct,string baseUrl)
        {
            lock(obj)
            {
                var query = urlCollection.Where(x=>x.Url == urlStruct.Url).FirstOrDefault();

                if (query != null)
                    return;

                if(isGrabCurrentPageUrl == true)
                {
                    if (urlStruct.Url.Contains(baseUrl) == false)
                        return;
                }
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
            string url = "";

            MatchCollection mc = RegexUtil.Matches(html.ToString(), RegexPattern.TagAPattern);
            foreach (Match item in mc)
            {
                value = item.Groups["url"].Value;

                if (value.StartsWith("http://") || value.StartsWith("https://") || value.StartsWith("ftp://"))
                {
                    url = item.Groups["url"].Value;                  
                }
                else if (value.StartsWith("/"))
                {
                    url = globalBaseUrl + item.Groups["url"].Value;
                }

                AddToCollection(new UrlStruct() { Title = "", Id = globalIndex, Status = "", Url = url }, globalBaseUrl);
                IncrementCount();
            }

            //对顶级页面抓取的Url进行递归
            if (recursionDepth > StartDepth)
            {
                
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
                    url = globalBaseUrl + url;
                AddToCollection(new UrlStruct() { Id = (i + 1), Status = "", Title = "", Url = url},globalBaseUrl);
            }

            //对顶级页面抓取的Url进行递归
            if(recursionDepth > StartDepth)
            {
                List<UrlStruct> tempList = new List<UrlStruct>(urlCollection);
                foreach (var item in tempList)
                {
                    GetUrlRecursion(item.Url, StartDepth);
                }
            }

            //Url清洗工作
        }

        private async void GetUrlRecursion(string url,int depth)
        {
            if (depth > recursionDepth)
                return;

            try
            {
                //Url Check
                var extractUrl = "";

                url = UrlUtil.FixUrl(url);
                
                string html = await WebUtil.GetHtmlSource(url);

                var recursionBaseUrl = UrlUtil.ExtractBaseUrl(url);

                await Task.Run(()=> {
                    HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
                    doc.LoadHtml(html.ToString());
                    HtmlAgilityPack.HtmlNodeCollection nodeCollection = doc.DocumentNode.SelectNodes("//a");

                    if (nodeCollection == null)
                        return;

                    for (int i = 0; i < nodeCollection.Count; i++)
                    {
                        var hrefAttribute = nodeCollection[i].Attributes["href"];
                        if (hrefAttribute == null)
                            continue;
                        extractUrl = hrefAttribute.Value;
                        if (string.IsNullOrEmpty(extractUrl))
                            continue;
                        if (extractUrl.StartsWith("/"))
                            extractUrl = recursionBaseUrl + extractUrl;
                        AddToCollection(new UrlStruct() { Id = (i + 1), Status = "", Title = "", Url = extractUrl },globalBaseUrl);

                        System.Threading.Thread.Sleep(3000);

                        GetUrlRecursion(extractUrl, depth);
                    }
                });
            }
            catch(Exception ex)
            {
                ShowStatusText(ex.Message);
            }

            depth++;
        }

        private void IncrementCount()
        {
            System.Threading.Interlocked.Increment(ref globalIndex);
        }          
    }
}
