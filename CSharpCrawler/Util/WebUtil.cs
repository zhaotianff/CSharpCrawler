using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using CSharpCrawler.Model;
using System.IO;
using System.Text.RegularExpressions;
using System.Net.Security;
using System.IO.Compression;
using System.Net.Sockets;

using CefSharp;

namespace CSharpCrawler.Util
{
    class WebUtil
    {
        private static System.Threading.AutoResetEvent ieAutoResetEvent = new System.Threading.AutoResetEvent(false);
        private static System.Threading.AutoResetEvent chromeAutoResetEvent = new System.Threading.AutoResetEvent(false);

        private static System.Net.Http.HttpClient httpClient;

        public static HttpHeader GetHeader(string url)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "GET";
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    return new HttpHeader()
                    {
                        CharSet = response.CharacterSet,
                        ContentEncoding = response.ContentEncoding,
                        ContentLength = response.ContentLength,
                        ContentType = response.ContentType,
                        LastModified = response.LastModified,
                        Server = response.Server,
                        StatusCode = response.StatusCode
                    };
                }                
            }
            catch
            {
                return new HttpHeader();
            }
        }

        public static bool IsResourceAvailable(string url)
        {
            return GetHeader(url).StatusCode == HttpStatusCode.OK;
        }

        public static async Task<string> GetHtmlSource(string url,Encoding encoding = null)
        {
            try
            {
                //不受信任的HTTPS
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback((a,b,c,d)=> { return true; });

                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
                request.Method = "GET"; //默认就是GET
                using (WebResponse response = await request.GetResponseAsync())
                {
                    Encoding tempEncoding = Encoding.Default;

                    if (encoding == null)
                    {
                        tempEncoding = EncodingUtil.GetEncoding(url);
                    }
                    else
                    {
                        tempEncoding = encoding;
                    }

                    Stream stream = response.GetResponseStream();

                    //GZIP流
                    if (((HttpWebResponse)response).ContentEncoding.ToLower().Contains("gzip"))
                    {
                        stream = new GZipStream(stream, CompressionMode.Decompress);
                    }

                    using (StreamReader sr = new StreamReader(stream, tempEncoding))
                    {
                        return sr.ReadToEnd();
                    }

                }
            }
            catch(Exception ex)
            {
                throw ex;
            }                   
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="accept"></param>
        /// <param name="userAgent"></param>
        /// <param name="encoding"></param>
        /// <remarks>以后再改</remarks>
        /// <returns></returns>
        public static async Task<string> GetHtmlSource(string url,string accept,string userAgent,Encoding encoding=null,CookieContainer cookieContainer = null)
        {
            try
            {
                //不受信任的HTTPS
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback((a, b, c, d) => { return true; });

                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
                request.Method = "GET"; //默认就是GET
                request.Accept = accept;
                request.UserAgent = userAgent;
                if (cookieContainer != null)
                    request.CookieContainer = cookieContainer;

                using (WebResponse response = await request.GetResponseAsync())
                {
                    Encoding tempEncoding = Encoding.Default;

                    if (encoding == null)
                    {
                        tempEncoding = EncodingUtil.GetEncoding(url);
                    }
                    else
                    {
                        tempEncoding = encoding;
                    }

                    Stream stream = response.GetResponseStream();

                    //GZIP流
                    if (((HttpWebResponse)response).ContentEncoding.ToLower().Contains("gzip"))
                    {
                        stream = new GZipStream(stream, CompressionMode.Decompress);
                    }

                    using (StreamReader sr = new StreamReader(stream, tempEncoding))
                    {
                        return sr.ReadToEnd();
                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static async Task<Tuple<string,CookieContainer>> GetHtmlSource(string url,CookieContainer cookieContainer, string accept = "", string userAgent = "", Encoding encoding = null)
        {
            //现在很多是共用的，直接复制过来了，以后再统一整理吧。
            try
            {
                //不受信任的HTTPS
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback((a, b, c, d) => { return true; });

                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
                request.Method = "GET"; //默认就是GET

                if(!string.IsNullOrEmpty(accept))
                    request.Accept = accept;

                if(!string.IsNullOrEmpty(userAgent))
                    request.UserAgent = userAgent;

                 request.CookieContainer = cookieContainer;

                using (WebResponse response = await request.GetResponseAsync())
                {
                    Encoding tempEncoding = Encoding.Default;

                    if (encoding == null)
                    {
                        tempEncoding = EncodingUtil.GetEncoding(url);
                    }
                    else
                    {
                        tempEncoding = encoding;
                    }

                    Stream stream = response.GetResponseStream();

                    //GZIP流
                    if (((HttpWebResponse)response).ContentEncoding.ToLower().Contains("gzip"))
                    {
                        stream = new GZipStream(stream, CompressionMode.Decompress);
                    }

                    using (StreamReader sr = new StreamReader(stream, tempEncoding))
                    {
                        var html =  sr.ReadToEnd();
                        CookieCollection responseCookieContainer = ((HttpWebResponse)response).Cookies;
                        stream.Close();
                        return new Tuple<string, CookieContainer>(html,cookieContainer);
                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static async Task<Stream> GetHtmlStreamAsync(string url)
        {
            try
            {
                //不受信任的HTTPS
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback((a, b, c, d) => { return true; });

                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
                request.Method = "GET"; //默认就是GET
                WebResponse response = await request.GetResponseAsync();              
                return response.GetResponseStream();               
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public static async Task<CookieContainer> GetCookies(string url,string postData,CookieContainer cookieContainer = null)
        {          
            HttpWebRequest request = null;
            WebResponse response = null;

            if (cookieContainer == null)
                cookieContainer = new CookieContainer();

            try
            {
                using (request = (HttpWebRequest)WebRequest.Create(url))
                {
                    request.Method = "POST";
                    byte[] postdatabyte = Encoding.UTF8.GetBytes(postData);
                    request.ContentLength = postdatabyte.Length;
                    request.AllowAutoRedirect = true;
                    request.CookieContainer = cookieContainer;
                    request.KeepAlive = true;
                    request.ContentType = "application/x-www-form-urlencoded";

                    Stream stream = await request.GetRequestStreamAsync();
                    stream.Write(postdatabyte, 0, postdatabyte.Length);
                    stream.Close();

                    response = await request.GetResponseAsync();
                    var cookies = ((HttpWebResponse)response).Cookies;
                    cookieContainer.Add(cookies);
                    return cookieContainer;
                }                  
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static async Task<string> PostData(string url, string postData, string contentType = "",CookieContainer cookieContainer = null)
        {
            HttpWebRequest request = null;
            WebResponse response = null;        

            try
            {
                request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";
                byte[] postdatabyte = Encoding.UTF8.GetBytes(postData);
                request.ContentLength = postdatabyte.Length;
                request.AllowAutoRedirect = true;
                request.KeepAlive = true;
                request.ContentType = "application/x-www-form-urlencoded";
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/63.0.3239.132 Safari/537.36";
                if (!string.IsNullOrEmpty(contentType))
                    request.ContentType = "application/json";

                if (cookieContainer != null)
                    request.CookieContainer = cookieContainer;

                Stream stream = await request.GetRequestStreamAsync();
                stream.Write(postdatabyte, 0, postdatabyte.Length);
                stream.Close();

                response = await request.GetResponseAsync();
                Stream responseStream = response.GetResponseStream();
                StreamReader sr = new StreamReader(responseStream);
                var responseStr = sr.ReadToEnd();
                response.Close();
                responseStream.Close();

                return responseStr;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// 使用System.Windows.Forms.WebBrowser来抓取（支持动态跳转）
        /// 需要连续且快速抓取不是动态跳转的页面，推荐使用HttpWebRequest类
        /// </summary>
        /// <param name="uIElement"></param>
        /// <param name="url"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public static async Task<string> GetDynamicHtmlSourceWithIE(System.Windows.UIElement uIElement, string url,int timeout = 6000)
        {
            System.Windows.Forms.WebBrowser webBrowser = new System.Windows.Forms.WebBrowser();
            webBrowser.ScriptErrorsSuppressed = true;
            var source = "";

            //这里注意几点
            //System.Windows.Forms.WebBrowser需要UI线程调用，所以传入UIElement
            //动态网站会跳转多次，取最后一次DocumentText

            try
            {
                await Task.Run(()=> {
                    uIElement.Dispatcher.Invoke(()=> {                      
                        webBrowser.Navigated += WebBrowser_Navigated;
                        webBrowser.Navigate(url);
                    });      
                    
                    while(true)
                    {
                        var waitResult = ieAutoResetEvent.WaitOne(timeout);

                        if (waitResult == true)
                        {
                            uIElement.Dispatcher.Invoke(() =>
                            {
                                source = webBrowser.DocumentText;
                            });
                        }
                        else
                        {
                            //这里目前只能借助等待超时来做，因为不知道动态页面会跳转多少次
                            //正常访问时，跳转会比较快，最后一次需要等待设置的超时时间
                            //超时后直接返回
                            //如果不是需要多次跳转的页面，可以把while去掉，只等待一次
                            break;
                        }
                    }                    
                });
                return source;
            }
            catch
            {
                return source;
            }            
        }

        private static void WebBrowser_Navigated(object sender, System.Windows.Forms.WebBrowserNavigatedEventArgs e)
        {
            ieAutoResetEvent.Set();
        }

        /// <summary>
        /// 使用CEFSharp来抓取（支持多次跳转的页面）
        /// 需要连续且快速抓取不是动态跳转的页面，推荐使用HttpWebRequest类
        /// </summary>
        /// <param name="uIElement"></param>
        /// <param name="url"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public static async Task<string> GetDynamicHtmlSourceWithChromium(System.Windows.UIElement uIElement, string url,int timeout = 10000)
        {
            //CEF使用参见 https://github.com/cefsharp/CefSharp/wiki
            //这里不对ChromiumWebBrowser做任何设置
            //真实生产环境需要对ChromiumWebBrowser做相关配置
            //CEF初次打开页面会慢，给10秒超时时间
            var source = "";

            try
            {
                await Task.Run(() => {
                    uIElement.Dispatcher.Invoke(()=> {
                        GlobalDataUtil.GetInstance().Browser.GetHtmlSourceDynamic(url, ChromiumWebBrowserLoadEnd);
                    });
                    
                    while (true)
                    {
                        var waitResult = chromeAutoResetEvent.WaitOne(timeout);
                        if (waitResult == true)
                        {
                            uIElement.Dispatcher.Invoke(async () =>
                            {
                                source = await GlobalDataUtil.GetInstance().Browser.GetHtmlSource();
                            });
                        }
                        else
                        {
                            //这里目前只能借助等待超时来做，因为不知道动态页面会跳转多少次
                            //正常访问时，跳转会比较快，最后一次需要等待设置的超时时间
                            //超时后直接返回
                            //如果不是需要多次跳转的页面，可以把while去掉，只等待一次
                            break;
                        }
                    }
                });
                return source;
            }
            catch
            {
                return source;
            }
        }

        private static void ChromiumWebBrowserLoadEnd(string source)
        {
            //以前封装的函数是在回调中直接返回网页源码
            //这里仅取通知，再获取一次
            chromeAutoResetEvent.Set();
        }


        public static void InitializeHttpClient(int timeout, params KeyValuePair<string,string> [] headers)
        {
            if (httpClient == null)
                httpClient = new System.Net.Http.HttpClient();

            //超时时间
            httpClient.Timeout = TimeSpan.FromMilliseconds(timeout);
            
            foreach (var item in headers)
            {
                //直接移除header
                //不存在不会异常
                httpClient.DefaultRequestHeaders.Remove(item.Key);
                httpClient.DefaultRequestHeaders.Add(item.Key, item.Value);
            }
        }

        public static void DisposeHttpClient()
        {
            if (httpClient != null)
                httpClient.Dispose();
        }

        public static async Task<System.Net.Http.HttpResponseMessage> HttpClientGetAsync(string url)
        {
            if (httpClient == null)
                return null;

            var message =  await httpClient.GetAsync(url);
            return message;
        }

        public static async Task<string> HttpClientGetStringAsync(string url,Encoding encoding = null,CookieCollection cookies = null)
        {
            if(cookies != null)
            {
                var cookiesStr = "";
                foreach (System.Net.Cookie item in cookies)
                {
                    cookiesStr += $"{item.Name}={item.Value};";
                }
                var message = new System.Net.Http.HttpRequestMessage(System.Net.Http.HttpMethod.Get, url);
                message.Headers.Add("Cookie", cookiesStr);
                await httpClient.SendAsync(message);
            }

            var source = "";           

            var buffer = await httpClient.GetByteArrayAsync(url);

            if (encoding == null)
                encoding = EncodingUtil.GetEncoding(buffer);

            source = encoding.GetString(buffer);
            return source;
        }

        public async Task<List<string>> FetchImage(string source)
        {
            List<string> list = new List<string>();

            await Task.Run(()=> {

            });

            return list;
        }

        /// <summary>
        /// 获取服务器IP
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static IPAddress[] GetHostIP(string url,bool showIPV6Flag = false)
        {           
            if (showIPV6Flag == true)
            {
                return Dns.GetHostEntry(url).AddressList;
            }
            else
            {
                return Dns.GetHostEntry(url).AddressList.ToList().Where(x=>x.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork).ToArray();
            }
        }

        public static string RemoveUrlParam(string url)
        {
            return url;
        }

        public static string[] ExtractPageKeyword(string html)
        {
            return new string[] { };
        }

        public static string GetHtmlSourceWithSocket(string hostName,int port = 80)
        {

            Socket socket = ConnectSocket(hostName, port);

            if (socket != null)
            {
                string request = "GET / HTTP/1.1\r\n" + 
                                "Host:" + hostName + "\r\n" + 
                                "Connection:Close\r\n\r\n";
                byte[] sentBuffer = Encoding.ASCII.GetBytes(request);
                byte[] receiveBuffer = new byte[1024];

                socket.Send(sentBuffer);

                int bufferLength = 0;
                string htmlSource = "";
                Encoding encoding = Encoding.Default;
                int times = 0;

                do
                {
                    bufferLength = socket.Receive(receiveBuffer);

                    if (times == 0)
                        encoding = EncodingUtil.GetEncoding(receiveBuffer);
                    times++;

                    htmlSource += encoding.GetString(receiveBuffer, 0, bufferLength);
                } while (bufferLength > 0);
                return htmlSource;
            }
            else
            {
                return "";
            }                        
        }

        private static Socket ConnectSocket(string hostName, int port)
        {
            Socket socket = null;

            IPHostEntry hostEntry =  Dns.GetHostEntry(hostName);

            foreach (var item in hostEntry.AddressList)
            {
                Socket tempSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                tempSocket.Connect(new IPEndPoint(item,port));
                if (tempSocket.Connected)
                    socket = tempSocket;
            }
            return socket;
        }

        /// <summary>
        /// https://dldir1.qq.com/qqfile/qq/QQ9.1.0/24712/QQ9.1.0.24712.exe
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private static string ExtractFileName(string url)
        {
            return url.Substring(url.LastIndexOf("/") + 1);
        }

        public static async Task<string> DownloadFileAsync(string url,string fileName = "")
        {
            WebClient client = new WebClient();
            if (string.IsNullOrEmpty(fileName))
                fileName = "./download/" + ExtractFileName(url);
            //TODO
            //File exist
            await client.DownloadFileTaskAsync(new Uri(url), fileName);
            return fileName;
        }

        public static async Task<string> DownloadFileAsyncWithTimeout(string url,int millisecondsTimeout, string fileName = "")
        {
            //判断url指向是否可用
            if (IsResourceAvailable(url) == false)
                return "";

            WebClient client = new WebClient();
            if (string.IsNullOrEmpty(fileName))
                fileName = "./download/" + ExtractFileName(url);
 
            var task =  client.DownloadFileTaskAsync(new Uri(url), fileName);

            var waitResult = false;
            Task subTask = Task.Run(()=> {
                //这个操作是阻塞的
                //再增加一个线程跑
                 task.Wait(millisecondsTimeout);
            });
            await subTask;

            if (waitResult == true)
                return fileName;
            else
                return "";
        }

        public async static void DownloadFileWithProgress(string url,Action<string> act,string fileName = "")
        {
            //TODO thread count
            int threadCount = 3;
            long blockSize = 0;
            int index = 0;
            long[] rangeArray = new long[threadCount*2];
            byte[] fileBuffer;
            long fileSize = 0;
            List<Task<byte[]>> tasks = new List<Task<byte[]>>();

            if (string.IsNullOrEmpty(fileName))
                fileName ="./download/" +  ExtractFileName(url);

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            fileSize = response.ContentLength;
            fileBuffer = new byte[fileSize];
            act($"FileSize:{fileSize} bytes");

            blockSize = fileSize / threadCount;

            for (int i = 0; i < threadCount*2; i+=2)
            {
                if (i == 0)
                {
                    rangeArray[i] = 0;
                    rangeArray[i + 1] = blockSize;
                }
                else if(i == threadCount*2 -2)
                {
                    rangeArray[i] = rangeArray[i - 1] + 1;
                    rangeArray[i + 1] = fileSize;
                }
                else
                {
                    rangeArray[i] = rangeArray[i - 1] + 1;
                    rangeArray[i + 1] = rangeArray[i] + blockSize;
                }
            }           

            for (int i = 0; i < threadCount*2; i+=2)
            {
                long start = rangeArray[i];
                long end = rangeArray[i + 1];

                tasks.Add(Task.Run(() => { return DownloadPartFile(start, end, url); }));

                act($"Create thread {index + 1},download range from {start} to {end} bytes");
                index++;
            }

            await Task.WhenAll(tasks);

            index = 0;

            for (int i = 0; i < threadCount * 2; i += 2)
            {
                long start = rangeArray[i];
                long end = rangeArray[i + 1];
                byte[] buffer =await tasks[index];

                using (System.IO.FileStream fs = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite))
                {
                    fs.Seek(start, SeekOrigin.Begin);
                    fs.Write(buffer, 0, buffer.Length);
                    //await fs.WriteAsync(buffer, 0, buffer.Length);
                }              
            }

            act($"Download {url} success");
        }

        private static byte[] DownloadPartFile(long start,long end,string url)
        {
            try
            {
                long bufferSize = end - start;
                byte[] buffer = new byte[bufferSize + 1];
                int targetIndex = 0;
                int readBytes = 0;
                byte[] receiveBytes = new byte[512];

                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
                request.Referer = url;
                request.Method = "GET";
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/63.0.3239.132 Safari/537.36";
                request.AllowAutoRedirect = false;
                request.ContentType = "application/octet-stream";
                request.Accept = "image/gif, image/jpeg, image/pjpeg, image/pjpeg, application/x-shockwave-flash, application/xaml+xml, application/vnd.ms-xpsdocument, application/x-ms-xbap, application/x-ms-application, application/vnd.ms-excel, application/vnd.ms-powerpoint, application/msword, */*";
                request.Timeout = 10 * 1000;
                request.AllowAutoRedirect = true;
                request.AddRange(start, end);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                using (Stream stream = response.GetResponseStream())
                {                                 
                    while ((readBytes  = stream.Read(receiveBytes, 0, receiveBytes.Length)) > 0)
                    {                       
                        Array.Copy(receiveBytes, 0, buffer, targetIndex, readBytes);
                        targetIndex += readBytes;
                    }
                }
                return buffer;
            }
            catch
            {
                return DownloadPartFile(start, end, url);
            }
            
        }

        /// <summary>
        /// 可以使用RobotsTxt库，Nuget可以搜索得到
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static List<RobotsExclusionProtocol> ResolveRobotsProtocol(Stream stream)
        {
            List<RobotsExclusionProtocol> list = new List<RobotsExclusionProtocol>();
            List<string> strList = new List<string>();
            RobotsExclusionProtocol robotsProtocol = new RobotsExclusionProtocol();

            string userAgent = "USER-AGENT";
            string allow = "ALLOW";
            string disallow = "DISALLOW";
            string sitemap = "SITEMAP";

            using (StreamReader sr = new StreamReader(stream))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    strList.Add(line);
                }

                foreach (var item in strList)
                {
                    if(string.IsNullOrEmpty(item.Trim()))
                    {
                        list.Add(robotsProtocol);
                        robotsProtocol = new RobotsExclusionProtocol();
                    }
                    else
                    {
                        if(item.ToUpper().StartsWith(userAgent))
                        {
                            robotsProtocol.UserAgent = item.Substring(userAgent.Length).Replace(":", "").Trim();
                        }
                        else if(item.ToUpper().StartsWith(allow))
                        {
                            robotsProtocol.AllowList.Add(item.Substring(allow.Length).Replace(":", "").Trim());
                        }
                        else if(item.ToUpper().StartsWith(disallow))
                        {
                            robotsProtocol.DisallowList.Add(item.Substring(disallow.Length).Replace(":", "").Trim());
                        }
                        else if(item.ToUpper().StartsWith(sitemap))
                        {
                            robotsProtocol.Sitemap = item.Substring(sitemap.Length).Replace(":", "").Trim();
                        }
                    }
                }

                list.Add(robotsProtocol);

            }
            return list;
        }
    }
}
