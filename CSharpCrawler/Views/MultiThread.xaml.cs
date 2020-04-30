using CSharpCrawler.Util;
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
using ZT.Enhance;
using System.Collections.Concurrent;

namespace CSharpCrawler.Views
{
    /// <summary>
    /// MultiThread.xaml 的交互逻辑
    /// </summary>
    public partial class MultiThread : Page
    {
        ConcurrentQueue<string> queue = new ConcurrentQueue<string>();

        public MultiThread()
        {
            InitializeComponent();
        }

        private async void btn_Fetch_Click(object sender, RoutedEventArgs e)
        {
            var url = this.tbox_Url.Text.Trim();

            if(string.IsNullOrEmpty(url))
            {
                EMessageBox.Show("请输入网址");
                return;
            }

            try
            {
                var source = await WebUtil.HttpClientGetStringAsync(url);
                AppendText("开始获取链接");
                await ExtractLink(source);
                AppendText("获取链接完成，开始多线程抓取链接内容......");
                await ExtractTitleMultiThread();
                AppendText("获取链接标题完成");
            }
            catch(Exception ex)
            {
                AppendText(ex.Message);
            }


        }

        private Task ExtractLink(string source)
        {
            return Task.Run(()=> {
                AngleSharpHelper helper = new AngleSharpHelper();
                helper.Init(source);
                var tagAList = helper.CSSQueryAll("a");
                foreach (var item in tagAList)
                {
                    var url = item.Attributes["href"]?.Value;
                    if(string.IsNullOrEmpty(url) == false)
                    {
                        if (RegexUtil.IsUrl(url) == true)
                        {
                            //TODO 需要使用分组构造 排除文件路径 如http://abc.com/test.exe 今天太累了 想不动了
                            AppendText(url);
                            queue.Enqueue(url);
                        }
                    }
                    
                }
            });
        }

        private Task ExtractTitleMultiThread(int threadCount = 4)
        {
            var tasks = new List<Task>();
            for (int i = 0; i < threadCount; i++)
            {
                var task = Task.Factory.StartNew(ExtractTitle,TaskCreationOptions.LongRunning);
                tasks.Add(task);
            }
            return Task.WhenAll(tasks);
        }

        private async void ExtractTitle()
        {         
            var url = "";
            while(queue.TryDequeue(out url))
            {
                //防止太快被封IP 做一下等待吧
                await Task.Delay(1000);

                try
                {
                    var source = await WebUtil.HttpClientGetStringAsync(url);
                    var title = RegexUtil.ExtractTitle(source);
                    AppendText($"{url} 标题: {title}");
                }
                catch(Exception ex)
                {
                    AppendText(ex.Message);
                }
            }                                
        }

        private void AppendText(string text)
        {
            if (string.IsNullOrEmpty(text))
                return;

            this.Dispatcher.Invoke(()=> {
                this.rtbox_Content.Document.Blocks.Add(new Paragraph(new Run(text)));
            });
        }
    }
}
