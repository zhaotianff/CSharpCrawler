using CefSharp;
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
using ZT.Enhance;

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
        ObservableCollection<string> backgroundImageList = new ObservableCollection<string>();

        int globalIndex = 1;
        string baseUrl = "";

        public FetchImage()
        {
            InitializeComponent();
            this.listview_Image.ItemsSource = imageCollection;
            this.listbox_BackgroundImage.ItemsSource = backgroundImageList;
        }

        #region Img标签

        private void btn_Surfing_Click(object sender, RoutedEventArgs e)
        {
            string url = this.tbox_Url.Text;

            if (string.IsNullOrEmpty(url))
            {
                ShowStatusText("请输入Url");
                return;
            }

            if (globalData.CrawlerConfig.CommonConfig.UrlCheck == true)
            {
                if (RegexUtil.IsUrl(url) == false)
                {
                    ShowStatusText("网址输入有误");
                    return;
                }
            }

            baseUrl = UrlUtil.FixUrl(url);                 

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
                MatchCollection mc = RegexUtil.Matches(html.ToString(), RegexPattern.TagImgPattern);
                foreach (Match item in mc)
                {
                    value = item.Groups["image"].Value;
                    if(value.Contains("//") == false)
                    {
                        value = baseUrl + value;
                    }
                    AddToCollection(new UrlStruct() { Id = globalIndex, Status = "", Title = "", Url = value });
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
                    if(value.StartsWith("//"))
                    {
                        value = "http:" + value;
                    }

                    if (value.Contains(":") == false)
                    {
                        value = baseUrl + value;
                    }
                    AddToCollection(new UrlStruct() { Id = globalIndex, Status = "", Title = "", Url = value });
                }
                ShowStatusText($"已抓取到{imageCollection.Count}个图像");
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

                if (RegexUtil.IsInvalidImgUrl(urlStruct.Url) == false)
                    return;

                    Dispatcher.Invoke(() => {
                    imageCollection.Add(urlStruct);
                });
                globalIndex++;
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

        private void listview_Image_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = this.listview_Image.SelectedIndex;
            if(index != -1)
            {
                try
                {
                    string imgUrl = imageCollection[index].Url;                    
                    this.imgage_Thumbnail.Source = new BitmapImage(new Uri(imgUrl));
                }
                catch(Exception ex)
                {
                    ShowStatusText(ex.Message);
                }
            }
        }

        private void listview_Image_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (this.listview_Image.SelectedIndex == -1)
                return;

            var url = this.listview_Image.SelectedItem as UrlStruct;
            Clipboard.SetText(url.Url);
        }

        private async void btn_Download_Click(object sender, RoutedEventArgs e)
        {
            if(imageCollection.Count == 0)
            {
                EMessageBox.Show("请先输入网址，执行Surfing获取到图片后再单击下载");
                return;
            }

            var localFileList = await DownloadImage(imageCollection);

            if(cbox_Filter.IsChecked.Value == true)
            {
                if(string.IsNullOrEmpty(tbox_Height.Text)|| string.IsNullOrEmpty(tbox_Width.Text))
                {
                    EMessageBox.Show("请输入要过滤的宽高");
                    return;
                }

                //过滤
                var width = 1024;
                var height = 768;
                int.TryParse(this.tbox_Width.Text, out width);
                int.TryParse(this.tbox_Height.Text, out height);

                FilterImage(localFileList, width, height);
            }            
        }


        /// <summary>
        /// 先下载到本地再过滤
        /// 可以直接使用HttpWebResponse返回的流创建Bitmap进行判断，暂时不这么操作
        /// </summary>
        /// <param name="imageList"></param>
        /// <returns></returns>
        private async Task<List<string>> DownloadImage(ObservableCollection<UrlStruct> imageList)
        {
            List<string> localFileList = new List<string>();
            foreach (var item in imageList)
            {
                this.lbl_Download.Content = $"正在下载:{item.Url}";
                var file = await WebUtil.DownloadFileAsyncWithTimeout(item.Url,5000);

                if(!string.IsNullOrEmpty(file))
                    localFileList.Add(file);
            }

            this.lbl_Download.Content = "下载完成";
            return localFileList;
        }

        private void FilterImage(List<string> list,int width,int height)
        {
            List<string> toDelFileList = new List<string>();

            foreach (var item in list)
            {
                try
                {                   
                    using (var stream = new System.IO.FileStream(item, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.Read))
                    {
                        var bitmapFrame = BitmapFrame.Create(stream, BitmapCreateOptions.DelayCreation, BitmapCacheOption.None);
                        var imageWidth = bitmapFrame.PixelWidth;
                        var imageHheight = bitmapFrame.PixelHeight;

                        if (imageWidth < width || imageHheight < height)
                            toDelFileList.Add(item);
                    }                  
                }
                catch
                {
                    continue;
                }
            }

            foreach (var file in toDelFileList)
            {
                try
                {
                    System.IO.File.Delete(file);
                }
                catch
                {

                }
            }
        }

        #endregion

        #region Background-Image

        private void Btn_SurfingBackgroundImage_Click(object sender, RoutedEventArgs e)
        {
            string url = this.tbox_UrlBackgroundIimage.Text;

            if (string.IsNullOrEmpty(url))
            {
                ShowBackgroundImageStatusText("请输入Url");
                return;
            }

            //许多购物网站图像不会用img标签，而是用div的background-image属性来实现
            //有时候可能引入的css文件比较多，如果用正则可能会比较麻烦
            //我这里的做法是直接用CEF执行js获取结果 
            //getComputedStyle(document.getElementsByClassName('classname')[0]).backgroundImage

            ShowBackgroundImageStatusText($"正在从{url}抓取图像");
            globalData.Browser.GetHtmlSourceDynamic(url, ExtractBackgroundImageCallBack);
        }

        private void Listbox_BackgroundImage_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = this.listbox_BackgroundImage.SelectedIndex;
            if (index != -1)
            {
                try
                {
                    string imgUrl = backgroundImageList[index];
                    this.imgage_BackgroundImageThumbnail.Source = new BitmapImage(new Uri(imgUrl));
                }
                catch (Exception ex)
                {
                    ShowStatusText(ex.Message);
                }
            }
        }


        private void ExtractBackgroundImageCallBack(string html)
        {
            new Thread(ExtractBackgroundImage) { IsBackground = true }.Start(html);
        }

        private async void ExtractBackgroundImage(object html)
        {
            //我这里是写的div，可能页面上用来显示图片的不一定是div，是其它元素也说不定，如li ol ul
            var xpath = "//div";
            var result = HtmlAgilityPackUtil.XPathQuery(html.ToString(), xpath);

            foreach (var item in result)
            {
                var classAttribute = item.Attributes["class"];
                if (classAttribute == null)
                    continue;

                var className = classAttribute.Value;

                var script = $"getComputedStyle(document.getElementsByClassName('{className}')[0]).backgroundImage";
                //执行js
                var backgroundImage = await globalData.Browser.browser.EvaluateScriptAsync(script);

                if (backgroundImage.Result != null && backgroundImage.Result.ToString() != "none")
                {
                    var mathch = RegexUtil.Match(backgroundImage.Result.ToString(), RegexPattern.MatchImgPattern);
                    if (mathch.Success)
                    {
                        lock (obj)
                        {
                            Dispatcher.Invoke(() =>
                            {
                                backgroundImageList.Add(mathch.Value);
                                ShowBackgroundImageStatusText($"已抓取到{backgroundImageList.Count}个图像");
                            });
                        }
                    }
                }
            }         
        }

        public void ShowBackgroundImageStatusText(string content)
        {
            this.Dispatcher.Invoke(() => {
                this.lbl_BackgroundImageStatus.Content = content;
            });
        }

        #endregion
    }
}
