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

        //匹配有效图像路径
        public static string MatchImgPattern = "(ftp|http|https)://\\S*.(png|jpg|gif|jiff|jpeg|bmp)";


        //a标签
        public static string TagAPattern = "<a(\\s+(href=\"(?<url>([^\"])*)\"|'([^'])*'|\\w+=\"(([^\"])*)\"|'([^'])*'))+>(?<text>(.*?))</a>";
        //title标签
        public static string TagTitlePattern = "<title>(?<title>\\S+)</title>";
        //img标签
        //<img data-lazy-src="https://res6.vmallres.com/pimages//product/GB3102150044301/428_428_1534934426769mp.png" alt="摩飞（Morphyrichards）MR9500便携式榨汁机 （蓝色）"/>
        //<img hidefocus="true" src="//www.baidu.com/img/bd_logo1.png" width="270" height="129"></img>
        public static string TagImgPattern = @"<img\b[^<>]*?\bsrc[\s\t\r\n]*=[\s\t\r\n]*[""']?[\s\t\r\n]*(?<image>[^\s\t\r\n""'<>]*)[^<>]*?/?[\s\t\r\n]*>";
        public static string TagImgPattern2 = "<img\\s+src=\"(?<image>\\S+)\"(.*)/>";

        //charset
        public static string CharsetPattern = @"<meta[\s\S]+?charset=(?<charset>(.*?))""[\s\S]+?>";
    }
}
