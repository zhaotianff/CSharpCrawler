using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpCrawler.Util
{
    class RegexPattern
    {
        //匹配网址 RegexOptions.Multiline
        public static string MatchUrlWithHttpPattern = "^http://\\w+\\.\\w+\\.(cn|com|org|gov|net|top|club|xin|xyz|vip|cc|com\\.cn|gov\\.cn)$";
        public static string MatchUrlWithHttpsPattern = "^https://\\w+\\.\\w+\\.(cn|com|org|gov|net|top|club|xin|xyz|vip|cc|com\\.cn|gov\\.cn)$";
        public static string MatchUrlNoHttpPattern = "^\\w+\\.\\w+\\.(cn|com|org|gov|net|top|club|xin|xyz|vip|cc|com\\.cn|gov\\.cn)$";

        public static string TagAPattern = "<a(\\s+(href=\"(?<url>([^\"])*)\"|'([^'])*'|\\w+=\"(([^\"])*)\"|'([^'])*'))+>(?<text>(.*?))</a>";
    }
}
