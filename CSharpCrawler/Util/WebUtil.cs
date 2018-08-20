using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using CSharpCrawler.Model;
using System.IO;
using System.Text.RegularExpressions;

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
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            WebResponse response =await request.GetResponseAsync();

            string encodingStr = ((HttpWebResponse)response).CharacterSet;

            if (encodingStr == "ISO-8859-1")
                encodingStr = "utf-8";

            Encoding tempEncoding;

            if(encoding == null)
            {
                tempEncoding = Encoding.GetEncoding(encodingStr);
            }
            else
            {
                tempEncoding = encoding;
            }

            
            using (Stream stream = response.GetResponseStream())
            {
                using (StreamReader sr = new StreamReader(stream, tempEncoding))
                {
                    return sr.ReadToEnd();
                }
            }                       
        }

        public static string GetHtmlEncoding(string url)
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

        public static List<string> MatchResult(string html,string pattern)
        {
            List<string> list = new List<string>();
            MatchCollection mc = Regex.Matches(html, pattern);

            foreach (Match item in mc)
            {
                list.Add(item.Value);
            }

            return list;
        }
    }
}
