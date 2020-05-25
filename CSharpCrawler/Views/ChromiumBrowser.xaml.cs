using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using CefSharp;

namespace CSharpCrawler.Views
{
    /// <summary>
    /// ChromiumBrowser.xaml 的交互逻辑
    /// </summary>
    public partial class ChromiumBrowser : Window
    {
        private Action<string> loadEndCallBack;               
        
        public ChromiumBrowser()
        {
            InitializeComponent();
        }

        public Task<string> GetHtmlSource()
        {
            return browser.GetSourceAsync();
        }

        public void GetHtmlSourceDynamic(string url,Action<string> act)
        {           
            browser.Address = url;
            loadEndCallBack = act;                                     
        }

        public void ExecuteJavaScript(string method,params string[] args)
        {
            if (args.Length > 0)
                browser.ExecuteScriptAsync(method, args);
            else
                browser.ExecuteScriptAsync(method);
        }

        public async Task<string> EvaluateJavaScriptAsync(string method,params string[] args)
        {
            JavascriptResponse response;
            if (args.Length > 0)
                response = await browser.EvaluateScriptAsync(method, args);
            else
                response = await browser.EvaluateScriptAsync(method);
            return response.Result.ToString();
        }

        private async void browser_FrameLoadEnd(object sender, CefSharp.FrameLoadEndEventArgs e)
        {
            string source = await browser.GetSourceAsync();

            if (loadEndCallBack != null)
                loadEndCallBack(source);                   
        }
    }
}
