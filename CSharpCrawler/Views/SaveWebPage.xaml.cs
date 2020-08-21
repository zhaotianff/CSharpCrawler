using CSharpCrawler.Util;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace CSharpCrawler.Views
{
    /// <summary>
    /// SaveWebPage.xaml 的交互逻辑
    /// </summary>
    public partial class SaveWebPage : Page
    {
        public SaveWebPage()
        {
            InitializeComponent();
        }


        private async void btn_SaveAsImage_Click(object sender, RoutedEventArgs e)
        {
            //如果这一步发生异常了，可以参考 https://www.cnblogs.com/zhaotianff/p/13528507.html
            try
            {
                await new PuppeteerSharp.BrowserFetcher().DownloadAsync(PuppeteerSharp.BrowserFetcher.DefaultRevision);

                var browser = await PuppeteerSharp.Puppeteer.LaunchAsync(new PuppeteerSharp.LaunchOptions
                {
                    Headless = true
                });

                var page = await browser.NewPageAsync();  //打开一个新标签
                await page.GoToAsync(this.tbox_Url.Text); //访问页面

                //设置截图选项
                PuppeteerSharp.ScreenshotOptions screenshotOptions = new PuppeteerSharp.ScreenshotOptions();
                //screenshotOptions.Clip = new PuppeteerSharp.Media.Clip() { Height = 0, Width = 0, X = 0, Y = 0 };//设置截剪区域
                screenshotOptions.FullPage = true; //是否截取整个页面
                screenshotOptions.OmitBackground = false;//是否使用透明背景，而不是默认白色背景
                screenshotOptions.Quality = 100; //截图质量 0-100（png不可用）
                screenshotOptions.Type = PuppeteerSharp.ScreenshotType.Jpeg; //截图格式

                var fileName = Environment.CurrentDirectory + $"\\download\\{await page.GetTitleAsync()}.jpg";

                if (System.IO.File.Exists(fileName))
                {
                    fileName = fileName.Replace(".jpg", $"{DateTime.Now.ToString("ffff")}.jpg");
                }

                await page.ScreenshotAsync(fileName, screenshotOptions);

                if (System.IO.File.Exists(fileName))
                {
                    BitmapImage bi = new BitmapImage();
                    bi.BeginInit();
                    bi.UriSource = new Uri(fileName, UriKind.Absolute);
                    bi.EndInit();
                    this.image.Source = bi;
                }
                else
                {
                    EMessageBox.Show("保存网页截图失败");
                }

                //在最后记得关闭浏览器及释放资源
                browser.Disconnect();
                browser.Dispose();
            }
            catch(Exception ex)
            {
                EMessageBox.Show(ex.Message);
            }
        }

        private async void btn_SaveAsPDF_Click(object sender, RoutedEventArgs e)
        {
            //打开网页的操作跟上面是一样的
            try
            {
                await new PuppeteerSharp.BrowserFetcher().DownloadAsync(PuppeteerSharp.BrowserFetcher.DefaultRevision);
                var browser = await PuppeteerSharp.Puppeteer.LaunchAsync(new PuppeteerSharp.LaunchOptions
                {
                    Headless = true
                });
                var page = await browser.NewPageAsync();  //打开一个新标签
                await page.GoToAsync(this.tbox_Url.Text); //访问页面

                //设置PDF选项
                PuppeteerSharp.PdfOptions pdfOptions = new PuppeteerSharp.PdfOptions();
                pdfOptions.DisplayHeaderFooter = false; //是否显示页眉页脚
                pdfOptions.FooterTemplate = "";   //页脚文本

                var width = await page.EvaluateFunctionAsync<int>("function getWidth(){return document.body.scrollWidth}");
                var height = await page.EvaluateFunctionAsync<int>("function getHeight(){return document.body.scrollHeight}");

                pdfOptions.Width = $"{width}px";
                pdfOptions.Height = $"{height}px";

                pdfOptions.HeaderTemplate = "";   //页眉文本
                pdfOptions.Landscape = false;     //纸张方向 false-垂直 true-水平
                pdfOptions.MarginOptions = new PuppeteerSharp.Media.MarginOptions() { Bottom = "0px", Left = "0px", Right = "0px", Top = "0px" }; //纸张边距，需要设置带单位的值，默认值是None
                pdfOptions.Scale = 1m;            //PDF缩放，从0-1
                pdfOptions.PrintBackground = true;

                var fileName = Environment.CurrentDirectory + $"\\download\\{await page.GetTitleAsync()}.pdf";

                if (System.IO.File.Exists(fileName))
                {
                    fileName = fileName.Replace(".pdf", $"{DateTime.Now.ToString("ffff")}.pdf");
                }

                //保存PDF
                await page.PdfAsync(fileName, pdfOptions);
                EMessageBox.Show($"{fileName}保存成功");

                //在最后记得关闭浏览器及释放资源
                browser.Disconnect();
                browser.Dispose();
            }
            catch(Exception ex)
            {
                EMessageBox.Show(ex.Message);
            }
        }
    }
}
