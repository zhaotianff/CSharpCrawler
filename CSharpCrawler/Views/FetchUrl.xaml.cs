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

namespace CSharpCrawler.Views
{
    /// <summary>
    /// FetchUrl.xaml 的交互逻辑
    /// </summary>
    public partial class FetchUrl : Page
    {
        ObservableCollection<UrlStruct> urlCollection = new ObservableCollection<UrlStruct>();
        object obj = new object();
        int globalIndex = 1;

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

            if(RegexUtil.IsUrl(url,out isStartWithHttp) == false)
            {
                ShowStatusText("网址输入有误");
                return;
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

            string html = await WebUtil.GetHtmlSource(url);

            MatchCollection mc = RegexUtil.Match(html, RegexPattern.TagAPattern);
            foreach (Match item in mc)
            {
                AddToCollection(new UrlStruct() { Description = "", Id = globalIndex, Status = "", Url = item.Value });
                System.Threading.Interlocked.Increment(ref globalIndex);
            }

            mc = Regex.Matches(html, RegexPattern.MatchUrlWithHttpsPattern,RegexOptions.Multiline);
            foreach (Match item in mc)
            {
                AddToCollection(new UrlStruct() { Description = "", Id = globalIndex, Status = "", Url = item.Value });
                System.Threading.Interlocked.Increment(ref globalIndex);
            }

            ShowStatusText("");
        }


        public void AddToCollection(UrlStruct urlStruct)
        {
            lock(obj)
            {
                urlCollection.Add(urlStruct);
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
        }
    }
}
