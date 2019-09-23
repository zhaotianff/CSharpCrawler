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
        public static string MatchUrlWithHttpPattern = "^http://\\S+\\.(com|cn|top|vip|ltd|shop|wang|club|online|store|site|tech|fun|biz|info|com.cn|org|org.cn|gov.cn|gov)$";
        public static string MatchUrlWithHttpsPattern = "^https://\\S+\\.(com|cn|top|vip|ltd|shop|wang|club|online|store|site|tech|fun|biz|info|com.cn|org|org.cn|gov.cn|gov)$";
        public static string MatchUrlNoHttpPattern = "^\\S+\\.(com|cn|top|vip|ltd|shop|wang|club|online|store|site|tech|fun|biz|info|com.cn|org|org.cn|gov.cn|gov)$";

        //匹配有效图像路径
        public static string MatchImgPattern = "(ftp|http|https)://\\S*.(png|jpg|gif|jiff|jpeg|bmp)";

        //标签通用 替换TagName和PropertyName即可
        public static string TagCommon = "(?<=<TagName)[\\s\\S]*?href=\"(?<PropertyName>\\S*)\"[\\s\\S]*?(?=</TagName>)";

        //a标签
        public static string TagAPattern = "<a(\\s+(href=\"(?<url>([^\"])*)\"|'([^'])*'|\\w+=\"(([^\"])*)\"|'([^'])*'))+>(?<text>(.*?))</a>";
        public static string TagAPatternExtractUrl = "(?<=<a)[\\s\\S]*?href=\"(?<href>\\S*)\"[\\s\\S]*?(?=</a>)";

        //title标签
        public static string TagTitlePattern = "<title>(?<title>\\S+)</title>";
        //img标签
        //<img data-lazy-src="https://res6.vmallres.com/pimages//product/GB3102150044301/428_428_1534934426769mp.png" alt="摩飞（Morphyrichards）MR9500便携式榨汁机 （蓝色）"/>
        //<img hidefocus="true" src="//www.baidu.com/img/bd_logo1.png" width="270" height="129"></img>
        public static string TagImgPattern = @"<img\b[^<>]*?\bsrc[\s\t\r\n]*=[\s\t\r\n]*[""']?[\s\t\r\n]*(?<image>[^\s\t\r\n""'<>]*)[^<>]*?/?[\s\t\r\n]*>";
        public static string TagImgPattern2 = "<img\\s+src=\"(?<image>\\S+)\"(.*)/>";

        //charset
        public static string CharsetPattern = @"<meta[\s\S]+?charset=(?<charset>(.*?))""[\s\S]+?>";

        public const string EndWithHtmlPattern = @"\S*(.html|.shtml)$";

        //Dianping
        public const string DianPingAveragePricePattern = @"(?<price>\d+)/";
        public const string DianPingContentArea = "(?<=<ul class=\"list-search\">)[\\s\\S]*(?=</ul>)";

        //360
        public const string Get360CaptchaCheckJsonPattern = "(?<=\\()[\\s\\S]+(?=\\))";
    }
}
