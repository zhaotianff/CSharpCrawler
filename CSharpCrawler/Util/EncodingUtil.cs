using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CSharpCrawler.Util
{
    public class EncodingUtil
    {
        /// <summary>
        /// 获取网页编码集
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static Encoding GetEncoding(string url)
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
                        return GetEncodingFromStr(str);
                    }
                }
            }
            catch
            {
                return Encoding.Default;
            }

        }

        private static Encoding GetEncodingFromStr(string source)
        {
            Regex regex = new Regex(RegexPattern.CharsetPattern, RegexOptions.Multiline | RegexOptions.IgnoreCase);
            Match match = regex.Match(source);
            if (match.Success)
            {
                string charsestStr = match.Groups["charset"].Value;
                if (string.IsNullOrEmpty(charsestStr))
                {
                    //<meta charset="utf-8">
                    int startIndex = match.Value.ToLower().IndexOf("\"");
                    int endIndex = match.Value.ToLower().LastIndexOf("\"");
                    charsestStr =  match.Value.Substring(startIndex + 1, endIndex - startIndex - 1);
                }
                return Encoding.GetEncoding(charsestStr);               
            }
            return Encoding.Default;
        }

        /// <summary>
        /// 从字节流获取编码集
        /// </summary>
        /// <param name="buffer"></param>
        /// <remarks>
        /// </remarks>
        /// <returns></returns>
        public static Encoding GetEncoding(byte[] buffer)
        {
            string str = Encoding.Default.GetString(buffer);
            return GetEncodingFromStr(str);
        }     
    }
}
