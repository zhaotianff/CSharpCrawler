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

namespace CSharpCrawler.Util
{
    class WebUtil
    {
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
        public static async Task<string> GetHtmlSource(string url,string accept,string userAgent,Encoding encoding=null)
        {
            try
            {
                //不受信任的HTTPS
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback((a, b, c, d) => { return true; });

                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
                request.Method = "GET"; //默认就是GET
                request.Accept = accept;
                request.UserAgent = userAgent;
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


        public static async Task<string> GetDynamicHtmlSourceWithIE(string url,Encoding encoding = null)
        {
            return "";
        }

        public static async Task<string> GetDynamicHtmlSourceWithChromium(string url,Encoding encoding = null)
        {
            return "";
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

                byte[] buffer = await Task.Run(() => { return DownloadPartFile(start, end, url); });


                using (System.IO.FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
                {
                    fs.Seek(start, SeekOrigin.Begin);
                    fs.Write(buffer, 0, buffer.Length);
                    //await fs.WriteAsync(buffer, 0, buffer.Length);
                }

                act($"Create thread {index+1},download range from {start} to {end} bytes");
                index++;
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

                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
                request.Referer = url;
                request.Method = "GET";
                request.UserAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1; SV1; .NET CLR 2.0.1124)";
                request.AllowAutoRedirect = false;
                request.ContentType = "application/octet-stream";
                request.Accept = "image/gif, image/jpeg, image/pjpeg, image/pjpeg, application/x-shockwave-flash, application/xaml+xml, application/vnd.ms-xpsdocument, application/x-ms-xbap, application/x-ms-application, application/vnd.ms-excel, application/vnd.ms-powerpoint, application/msword, */*";
                request.Timeout = 10 * 1000;
                request.AllowAutoRedirect = true;
                request.AddRange(start, end);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                using (Stream stream = response.GetResponseStream())
                {
                    byte[] receiveBytes = new byte[512];
                    int readBytes = stream.Read(receiveBytes, 0, receiveBytes.Length);
                    while (readBytes > 0)
                    {
                        Array.Copy(receiveBytes, 0, buffer, targetIndex, readBytes);
                        readBytes = stream.Read(receiveBytes, 0, receiveBytes.Length);
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
        public List<RobotsExclusionProtocol> ResolveRobotsProtocol(Stream stream)
        {
            List<RobotsExclusionProtocol> list = new List<RobotsExclusionProtocol>();
            using (StreamReader sr = new StreamReader(stream))
            {
                string line;
                while (string.IsNullOrEmpty( (line = sr.ReadLine() ).Trim())== false)
                {
                    
                }
            }
            return list;
        }
    }
}
