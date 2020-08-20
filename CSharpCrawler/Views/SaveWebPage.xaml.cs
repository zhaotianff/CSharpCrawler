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

            var fileName = Environment.CurrentDirectory + "\\download\\web.jpg";
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

            browser.Disconnect();
            browser.Dispose();
        }
    }
}
