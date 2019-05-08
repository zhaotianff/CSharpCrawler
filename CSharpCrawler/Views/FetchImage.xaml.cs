using CSharpCrawler.Model;
using CSharpCrawler.Util;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
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
    /// FetchImage.xaml 的交互逻辑
    /// </summary>
    public partial class FetchImage : Page
    {
        GlobalDataUtil globalData = GlobalDataUtil.GetInstance();
        object obj = new object();
        ObservableCollection<UrlStruct> imageCollection = new ObservableCollection<UrlStruct>();
        List<UrlStruct> ToVisitList = new List<UrlStruct>();
        List<UrlStruct> VisitedList = new List<UrlStruct>();

        int globalIndex = 1;

        public FetchImage()
        {
            InitializeComponent();
            this.listview_Image.ItemsSource = imageCollection;
        }

        private void btn_Surfing_Click(object sender, RoutedEventArgs e)
        {
            string url = this.tbox_Url.Text;
            bool isStartWithHttp = false;

            if (string.IsNullOrEmpty(url))
            {
                ShowStatusText("请输入Url");
                return;
            }

            //判断Url
            if (globalData.CrawlerConfig.ImageConfig.IgnoreUrlCheck == false)
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

        public void ShowStatusText(string content)
        {
            this.Dispatcher.Invoke(() => {
                this.lbl_Status.Content = content;
            });
        }

        public void Surfing(string url)
        {
            ShowStatusText($"正在从{url}抓取图像");
            if(globalData.CrawlerConfig.ImageConfig.DynamicGrab == true)
            {
                SurfingByCEF(url);
            }
            else
            {
                SurfingByFCL(url);
            }
        }

        private void SurfingByCEF(string url)
        {
            globalData.Browser.GetHtmlSourceDynamic(url,ExtractImageCallBack);            
        }

        private async void SurfingByFCL(string url)
        {
            try
            {
                string html = await WebUtil.GetHtmlSource(url);
                StartExtractThread(html);
            }
            catch (Exception ex)
            {
                //TODO
                ShowStatusText(ex.Message);
            }
        }

        private void ExtractImage(object html)
        {                       
           switch(globalData.CrawlerConfig.ImageConfig.FetchMode)
            {
                case 0:
                    ExtractImageMixed(html);
                    break;
                case 1:
                    ExtractImageWithRegex(html);
                    break;
                case 2:
                    ExtractImageWithHtmlAgilityPack(html);
                    break;
                default:
                    break;
            }          
        }

        private void ExtractImageWithRegex(object html)
        {
            try
            {
                string value = "";
                MatchCollection mc = RegexUtil.Match(html.ToString(), RegexPattern.TagImgPattern);
                foreach (Match item in mc)
                {
                    value = item.Groups["image"].Value;
                    AddToCollection(new UrlStruct() { Id = globalIndex, Status = "", Title = "", Url = value });
                    IncrementCount();
                }
                ShowStatusText($"已抓取到{mc.Count}个图像");
            }
            catch(Exception ex)
            {
                ShowStatusText(ex.Message);
            }
        }

        private void ExtractImageWithHtmlAgilityPack(object html)
        {
            try
            {
                string value = "";
                HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
                doc.LoadHtml(html.ToString());

                HtmlAgilityPack.HtmlNodeCollection imgNodeCollection = doc.DocumentNode.SelectNodes("//img");
                for (int i = 0; i < imgNodeCollection.Count; i++)
                {
                    value = imgNodeCollection[i].Attributes["src"].Value;
                    AddToCollection(new UrlStruct() { Id = globalIndex, Status = "", Title = "", Url = value });
                    IncrementCount();
                }
                ShowStatusText($"已抓取到{imgNodeCollection.Count}个图像");
            }
            catch(Exception ex)
            {
                ShowStatusText(ex.Message);
            }
        }

        private void ExtractImageMixed(object html)
        {

        }

        private void StartExtractThread(object html)
        {
            Thread extractThread = new Thread(new ParameterizedThreadStart(ExtractImage));
            extractThread.IsBackground = true;
            extractThread.Start(html);
        }

        private void ExtractImageCallBack(string html)
        {
            StartExtractThread(html);
        }

        public void AddToCollection(UrlStruct urlStruct)
        {
            lock (obj)
            {
                var query = imageCollection.Where(x => x.Url == urlStruct.Url).FirstOrDefault();
                if (query != null)
                    return;
                Dispatcher.Invoke(() => {
                    imageCollection.Add(urlStruct);
                    ToVisitList.Add(urlStruct);
                });
            }
        }

        public void ClearCollection()
        {
            Dispatcher.Invoke(()=> {
                imageCollection.Clear();
            }); 
        }

        public void Reset()
        {
            ClearCollection();
            globalIndex = 1;
            ShowStatusText("");
        }

        private void IncrementCount()
        {
            System.Threading.Interlocked.Increment(ref globalIndex);
        }

        private void listview_Image_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = this.listview_Image.SelectedIndex;
            if(index != -1)
            {
                try
                {
                    string imgUrl = imageCollection[index].Url;
                    if (RegexUtil.IsInvalidImgUrl(imgUrl))
                        this.imgage_Thumbnail.Source = new BitmapImage(new Uri(imgUrl));
                }
                catch(Exception ex)
                {
                    ShowStatusText(ex.Message);
                }
            }
        }   
    }
}
