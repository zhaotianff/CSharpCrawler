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
        private string BaseUrl { get; set; }

        public FetchImage()
        {
            InitializeComponent();

            //直接绑定控件，后面只需要更新这两个ObservableCollection就可以了
            this.listview_Image.ItemsSource = imageCollection;
            this.listbox_BackgroundImage.ItemsSource = backgroundImageList;
        }

        #region Img标签

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            CheckBox cbx = sender as CheckBox;

            if (cbx.IsChecked == true)
            {
                var dockPanel = VisualTreeHelper.GetParent(cbx) as DockPanel;
                var index = dockPanel.Children.IndexOf(cbx);

                for (int i = 0; i < dockPanel.Children.Count; i++)
                {
                    if (i == index)
                        continue;

                    (dockPanel.Children[i] as CheckBox).IsChecked = false;
                }
            }
            else
            {
                cbx.IsChecked = true;
            }
        }

        private void btn_Surfing_Click(object sender, RoutedEventArgs e)
        {
            string url = this.tbox_Url.Text.Trim();

            if (string.IsNullOrEmpty(url))
            {
                ShowStatusText("请输入Url");
                return;
            }

            if (cbx_HttpWebRequest.IsChecked == true)
            {
                if (RegexUtil.IsUrl(url) == false)
                {
                    ShowStatusText("网址输入有误");
                    return;
                }
            }

            imageCollection.Clear();

            BaseUrl = UrlUtil.FixUrl(url);                 
            Surfing(url);
        }      

        public void Surfing(string url)
        {
            ShowStatusText($"正在从{url}抓取图像");

            if(cbx_CEF.IsChecked == true)
            {
                SurfingByCEF(url);
            }
            else
            {
                SurfingByHttpWebRequest(url);
            }
        }

        private void SurfingByCEF(string url)
        {
            if (cbx_HtmlAgilityPack.IsChecked == true)
            {
                globalData.Browser.GetHtmlSourceDynamic(url, ExtractImageWithHtmlAgilityPack);
            }
            else
            {
                globalData.Browser.GetHtmlSourceDynamic(url, ExtractImageWithRegex);
            }
        }

        private async void SurfingByHttpWebRequest(string url)
        {
            try
            {
                string html = await WebUtil.GetHtmlSource(url);
                if(cbx_HtmlAgilityPack.IsChecked == true)
                {
                    ExtractImageWithHtmlAgilityPack(html);
                }
                else
                {
                    ExtractImageWithRegex(html);
                }
            }
            catch (Exception ex)
            {
                ShowStatusText(ex.Message);
            }
        }

        private void ExtractImageWithRegex(string html)
        {
            try
            {
                string value = "";
                MatchCollection mc = RegexUtil.Matches(html, RegexPattern.TagImgPattern);

                for (int i = 0; i < mc.Count; i++)
                {
                    value = mc[i].Groups["image"].Value;
                    if (value.Contains("//") == false)
                    {
                        value = BaseUrl + value;
                    }
                    AddToCollection(new UrlStruct() { Id = i+1, Status = "", Title = "", Url = value });
                }
                ShowStatusText($"已抓取到{imageCollection.Count}个图像");
            }
            catch(Exception ex)
            {
                ShowStatusText(ex.Message);
            }
        }

        private async void ExtractImageWithHtmlAgilityPack(string html)
        {
            try
            {
                string value = "";

                var imageList = await HtmlAgilityPackUtil.GetImgFromHtmlAsync(html);
                for (int i = 0; i < imageList.Count; i++)
                {
                    value = imageList[i];
                    if(value.StartsWith("//"))
                    {
                        value = "http:" + value;
                    }

                    if (value.Contains(":") == false)
                    {
                        value = BaseUrl + value;
                    }
                    AddToCollection(new UrlStruct() { Id = i+1, Status = "", Title = "", Url = value });
                }
                ShowStatusText($"已抓取到{imageCollection.Count}个图像");
            }
            catch(Exception ex)
            {
                ShowStatusText(ex.Message);
            }
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
            }
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
                ShowStatusText("请输入Url");
                return;
            }

            backgroundImageList.Clear();

            //许多购物网站图像不会用img标签，而是用div的background-image属性来实现
            //有时候可能引入的css文件比较多，如果用正则可能会比较麻烦
            //我这里的做法是直接用CEF执行js获取结果 
            //getComputedStyle(document.getElementsByClassName('classname')[0]).backgroundImage

            ShowStatusText($"正在从{url}抓取图像");
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
                    var mathch = RegexUtil.RegexMatch(backgroundImage.Result.ToString(), RegexPattern.MatchImgPattern);
                    if (mathch.Success)
                    {
                        lock (obj)
                        {
                            Dispatcher.Invoke(() =>
                            {
                                backgroundImageList.Add(mathch.Value);
                                ShowStatusText($"已抓取到{backgroundImageList.Count}个图像");
                            });
                        }
                    }
                }
            }

            if (backgroundImageList.Count == 0)
                ShowStatusText("解析已完成，未抓取到任何图像");         
        }

        #endregion

        #region 动态页面
        /*
         * 像知乎，微博这种页面，页面需要滑动到最底部才会去加载动态内容。如果去分析接口，费时费力，还得每个网站都分析一次。
         * 个人觉得，可以控制浏览器一直滚动到最底部，待动态内容全部加载完成后，再进行抓取。这样就省了分析接口这一步，而且一劳永逸。
         * 如果需要登录的，可以进行模拟登录，再进行抓取（一些网站只需关闭弹出的登录对话框，就可以直接抓取）
         */


        private void btn_SurfingDynamic_Click(object sender, RoutedEventArgs e)
        {
            //下面开始操作
            var url = this.tbox_UrlDynamic.Text.Trim();

            if(string.IsNullOrEmpty(url))
            {
                EMessageBox.Show("请输入网址");
                return;
            }

            globalData.Browser.GetHtmlSourceDynamic(url, StartScroll);
        }

        private async void StartScroll(string html)
        {
            //第一次抓取内容完成，开始滚动页面

            //获取高度 document.body.clientHeight
            var getHeightJs = "document.body.clientHeight";

            //用js控制滚动
            //这里也可以直接用Selenium去驱动浏览器滚动
            var scrollJs = "window.scroll(0,{0})";
            var height = await globalData.Browser.EvaluateJavaScriptAsync(getHeightJs);

            //无限循环滚动
            while (true)
            {
                globalData.Browser.ExecuteJavaScript(string.Format(scrollJs,height));
                var oldHeight = height;
                height = await globalData.Browser.EvaluateJavaScriptAsync(getHeightJs);

                if (height == oldHeight)
                    break;

                //todo 登录操作
                //使用js填入登录框内容 模拟点击登录
                //由于这里仅做示例不针对任何网站
                await Task.Delay(1000);
            }

            //到这里可以提取页面上的图片了
            html = await globalData.Browser.GetHtmlSource();
            var list = await HtmlAgilityPackUtil.GetImgFromHtmlAsync(html);
            this.Dispatcher.Invoke(()=> {
                this.listbox_ImageDynamic.ItemsSource = list;
            }); 
        }

        private void listbox_ImageDynamic_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void listbox_ImageDynamic_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }
        #endregion

        #region 公共
        public void ShowStatusText(string content)
        {
            this.Dispatcher.Invoke(() => {
                this.lbl_Status.Content = content;
            });
        }
        #endregion
    }
}
