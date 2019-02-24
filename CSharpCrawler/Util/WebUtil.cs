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
                    string encodingStr = GetHtmlEncoding(url);

                    Encoding tempEncoding;

                    if (encoding == null)
                    {
                        tempEncoding = Encoding.GetEncoding(encodingStr);
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

        public static async Task<string> GetDynamicHtmlSourceWithIE(string url,Encoding encoding = null)
        {
            return "";
        }

        public static async Task<string> GetDynamicHtmlSourceWithChromium(string url,Encoding encoding = null)
        {
            return "";
        }

        /// <summary>
        /// 获取网页编码集
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string GetHtmlEncoding(string url)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
                request.Timeout = 5000;
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                using (Stream stream = response.GetResponseStream())
                {
                    using (StreamReader sr = new StreamReader(stream, Encoding.Default))
                    {
                        string str = sr.ReadToEnd();
                        Regex regex = new Regex(RegexPattern.CharsetPattern, RegexOptions.Multiline | RegexOptions.IgnoreCase);
                        Match match = regex.Match(str);
                        if(match.Success)
                        {
                            string charsestStr = match.Groups["charset"].Value;
                            if(string.IsNullOrEmpty(charsestStr))
                            {
                                //<meta charset="utf-8">
                                int startIndex = match.Value.ToLower().IndexOf("\"");
                                int endIndex = match.Value.ToLower().LastIndexOf("\"");
                                return match.Value.Substring(startIndex + 1, endIndex - startIndex -1);
                            }
                            else
                            {
                                return charsestStr;
                            }
                        }
                        return "utf-8";
                    }
                }                  
            }
            catch
            {
                return "utf-8";
            }
            
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

        public static string FetchResourceWithSocket(string hostName)
        {
            Socket socket = new Socket(AddressFamily.InterNetwork,SocketType.Stream,ProtocolType.Tcp);
            IPHostEntry host = Dns.GetHostEntry(hostName);
            IPEndPoint iPEndPoint = new IPEndPoint(host.AddressList[0], 443);
            socket.Connect(iPEndPoint);
            string requestHeader = "Get / HTTP/1.1\r\n"
                                    + "User-Agent:Windows NT 10.0\r\n"
                                    + "Host:" + hostName + "\r\n"
                                    + "Connection:Close\r\n";
            socket.Send( Encoding.UTF8.GetBytes(requestHeader));
            byte[] buffer = new byte[1024];
            //SocketAsyncEventArgs  //TODO
            int length = socket.Receive(buffer);

            //reference
            //string h1 = "GET " + _path + " HTTP/1.1\r\n";
            //string h2 = "Accept: */*\r\n";
            //string h3 = "Accept-Language: zh-cn\r\n";
            //string h4 = "Host: " + _host + "\r\n";
            //string h5 = "User-Agent: Mozilla/5.0 (Windows NT 6.1) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/27.0.1453.116 Safari/537.36\r\n";
            //string h7 = "Connection: close\r\n\r\n";
            return "";
        }
    }
}
