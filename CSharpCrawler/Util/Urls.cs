using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharpCrawler.Util
{
    class Urls
    {
        public const string BaiduUrl = "http://www.baidu.com/s?wd=%s";

        public static bool IsEmpty(string url)
        {
            return string.IsNullOrEmpty(url);
        }
    }
}
