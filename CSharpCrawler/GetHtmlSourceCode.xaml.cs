using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net;
using System.IO;
using System.Threading;

namespace CSharpCrawler
{
    /// <summary>
    /// GetHtmlSourceCode.xaml 的交互逻辑
    /// </summary>
    public partial class GetHtmlSourceCode : Page
    {
        static readonly object lockobj = new object();

        public GetHtmlSourceCode()
        {
            InitializeComponent();
        }

        private void btn_Sync_Click(object sender, RoutedEventArgs e)
        {
            string url = tbx_Url.Text;
            if(string.IsNullOrEmpty(url))
            {
                MessageBox.Show("请输入网址");
                return;
            }

            //下面两种方法都可以
            //this.rtbx_Content.Document = new FlowDocument(new Paragraph(new Run(GetSourceCodeSync(url))));
            this.rtbx_Content.Document = new FlowDocument(new Paragraph(new Run(GetSourceCodeSync2(url))));
        }

        private void btn_Async_Click(object sender, RoutedEventArgs e)
        {
            string url = tbx_Url.Text;
            if (string.IsNullOrEmpty(url))
            {
                MessageBox.Show("请输入网址");
                return;
            }

            GetSourceCodeAsync(url);
        }

        /// <summary>
        /// 同步获取网页源码
        /// </summary>
        /// <param name="url">网址</param>
        /// <returns></returns>
        public string GetSourceCodeSync(string url)
        {
            try
            {
                WebClient client = new WebClient();
                Stream stream =  client.OpenRead(url);
                //在这里指定编码
                using(StreamReader sr = new StreamReader(stream,Encoding.UTF8))
                {
                    return sr.ReadToEnd();
                }
            }
            catch(Exception ex)
            {
                return ex.Message;
            }
            
        }

        /// <summary>
        /// 同步获取网页源码2
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public string GetSourceCodeSync2(string url)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream stream = response.GetResponseStream();
                //在这里指定编码
                using (StreamReader sr = new StreamReader(stream, Encoding.UTF8))
                {
                    return sr.ReadToEnd();
                }
            }
            catch(Exception ex)
            {
                return ex.Message;
            }
        }

        /// <summary>
        /// 异步获取网页源代码
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public void GetSourceCodeAsync(string url)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            RequestState state = new RequestState();
            state.request = request;

            lock(lockobj)
            {
                request.BeginGetResponse(GetResponseCallBack,state);                
                FlowDocument fd = new FlowDocument();                           
                Monitor.Wait(lockobj);
            }
        }

        /// <summary>
        /// 回调方法
        /// </summary>
        /// <param name="ar"></param>
        public  void GetResponseCallBack(IAsyncResult ar)
        {
            RequestState state = (RequestState)ar.AsyncState;
            state.response =(HttpWebResponse)state.request.EndGetResponse(ar);
            state.stream = state.response.GetResponseStream();
            state.stream.BeginRead(state.BufferRead, 0, state.BufferRead.Length, new AsyncCallback(ReadCallBack), state);
        }

        /// <summary>
        /// 读取内容回调方法
        /// </summary>
        /// <param name="ar"></param>
        public void ReadCallBack(IAsyncResult ar)
        {
            try
            {
                RequestState state = (RequestState)ar.AsyncState;
                Stream responseStream = state.stream;
                int read = responseStream.EndRead(ar);
                
                if (read > 0)
                {
                    state.requestData.Append(Encoding.ASCII.GetString(state.BufferRead, 0, read));
                    IAsyncResult asynchronousResult = responseStream.BeginRead(state.BufferRead, 0, state.BufferRead.Length, new AsyncCallback(ReadCallBack), state);
                    return;
                }
                else
                {                 
                    if (state.requestData.Length > 1)
                    {
                        string stringContent;
                        //这里是异步读取到的内容
                        stringContent = state.requestData.ToString();
                        this.Dispatcher.Invoke(new Action(() => {
                            this.rtbx_Content.Document = new FlowDocument(new Paragraph(new Run(stringContent)));
                        })); 
                    }                  

                    responseStream.Close();

                    lock(lockobj)
                    {
                        Monitor.Pulse(lockobj);
                    }
                }

            }
            catch (WebException e)
            {
                MessageBox.Show(e.Message);
            }

        }
    }

    public class RequestState
    {    
        const int BUFFER_SIZE = 1024;
        public byte[] BufferRead;

        public StringBuilder requestData;
        public HttpWebRequest request;
        public HttpWebResponse response;
        public Stream stream;

        public RequestState()
        {
            BufferRead = new byte[BUFFER_SIZE];
            requestData = new StringBuilder("");
            request = null;
            stream = null;
        }
    }
}
