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
using CefSharp.Handler;
using CSharpCrawler.Model;

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
            //如果不清除Address，网页不会再次加载，就不会触发browser_FrameLoadEnd事件
            if (browser.Address == url)
                browser.Address = "";
            browser.Address = url;
            loadEndCallBack = act;                                     
        }

        public void GetNetworkResponseDynamic(string url,Action<NetworkResponse> act)
        {
            this.browser.RequestHandler = new CustomRequestHandler(act);

            if (browser.Address == url)
                browser.Address = "";
            browser.Address = url;
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

    public class NetworkCapturingResourceRequestHandler : ResourceRequestHandler
    {
        private Action<NetworkResponse> getNetworkResponseCallBack = null;

        public NetworkCapturingResourceRequestHandler(Action<NetworkResponse> getNetworkResponseCallBack)
        {
            this.getNetworkResponseCallBack = getNetworkResponseCallBack;
        }

        protected override bool OnResourceResponse(IWebBrowser chromiumWebBrowser, IBrowser browser,
            IFrame frame, IRequest request, IResponse response)
        {
            var requestUrl = request.Url;
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("Request");
            stringBuilder.AppendLine("[Url] : " + request.Url);
            stringBuilder.AppendLine("[Headers] : ");
            foreach (var item in request.Headers.AllKeys)
            {
                stringBuilder.AppendLine("\t\t" + item + ":" + request.Headers[item]);
            }
            stringBuilder.AppendLine("[ReferrerUrl] : " + request.ReferrerUrl);
            stringBuilder.AppendLine("[Method] : " + request.Method);
            stringBuilder.AppendLine("[ResourceType] : " + request.ResourceType);
            stringBuilder.AppendLine();
            stringBuilder.AppendLine("-----------------------------");
            stringBuilder.AppendLine();
            stringBuilder.AppendLine("Response");
            stringBuilder.AppendLine("[MIME] : " + response.MimeType);
            stringBuilder.AppendLine("[StatusCode] : " + response.StatusCode);
            stringBuilder.AppendLine("[Charset] : " + response.Charset);
            stringBuilder.AppendLine("[Headers] : ");
            foreach (var item in response.Headers.AllKeys)
            {
                stringBuilder.AppendLine("\t\t" + item + ":" + response.Headers[item]);
            }
            getNetworkResponseCallBack?.Invoke(new NetworkResponse() { Url = requestUrl,Detail = stringBuilder.ToString() });
            
            return false;
        }
    }

    public class CustomRequestHandler : CefSharp.Handler.RequestHandler
    {
        private Action<NetworkResponse> getNetworkResponseCallBack = null;

        public CustomRequestHandler(Action<NetworkResponse> getNetworkResponseCallBack)
        {
            this.getNetworkResponseCallBack = getNetworkResponseCallBack;
        }

        protected override IResourceRequestHandler GetResourceRequestHandler(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request, bool isNavigation, bool isDownload, string requestInitiator, ref bool disableDefaultHandling)
        {
            return new NetworkCapturingResourceRequestHandler(this.getNetworkResponseCallBack);
        }
    }
}
