using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CSharpCrawler.Util
{
    public class RegexUtil
    {
        public static bool IsUrl(string urlStr, out bool isStartWithHttp)
        {
            bool result = false;
            isStartWithHttp = false;

            if (Regex.IsMatch(urlStr, RegexPattern.MatchUrlNoHttpPattern))
            {
                result = true;
                isStartWithHttp = false;
            }
            if (Regex.IsMatch(urlStr, RegexPattern.MatchUrlWithHttpPattern) || Regex.IsMatch(urlStr, RegexPattern.MatchUrlWithHttpsPattern))
            {
                result = true;
                isStartWithHttp = true;
            }
            return result;
        }

        public static bool IsUrl(string urlStr)
        {
            if (Regex.IsMatch(urlStr, RegexPattern.MatchUrlNoHttpPattern)
                || Regex.IsMatch(urlStr, RegexPattern.MatchUrlWithHttpPattern)
                || Regex.IsMatch(urlStr, RegexPattern.MatchUrlWithHttpsPattern))
                return true;
            return false;
        }

        public static MatchCollection Matches(string text, string pattern)
        {
            return Regex.Matches(text, pattern);
        }

        public static bool IsInvalidImgUrl(string url)
        {
            return RegexMatch(url, RegexPattern.MatchImgPattern).Success;
        }

        public static Match RegexMatch(string text,string pattern)
        {
            if (string.IsNullOrEmpty(text) || string.IsNullOrEmpty(pattern))
                return Match.Empty;

            return Regex.Match(text, pattern);
        }

        public static Tuple<bool,string> ExtractBingImage(string url)
        {
            //https://cn.bing.com/images/search?view=detailV2&amp;ccid=Bz0cmQBk&amp;id=7BD5328E1583E357007C079BB0719B3AA1EA4872&amp;thid=OIP.Bz0cmQBklh3rik4Kn8RthgHaE8&amp;mediaurl=http%3a%2f%2fi2.chinanews.com%2fsimg%2fhd%2f2018%2f04%2f19%2f27fdf774f284408f8a3e1299cefd18c3.jpg&amp;exph=620&amp;expw=930&amp;q=%e6%b5%b7%e5%86%9b%e7%ac%ac32%e6%89%b9%e6%8a%a4%e8%88%aa%e7%bc%96%e9%98%9f%e8%ae%ad%e7%bb%83&amp;simid=608051107410215424&amp;selectedIndex=0&amp;qft=+filterui%3aphoto-photo
            //http%3a%2f%2fi2.chinanews.com%2fsimg%2fhd%2f2018%2f04%2f19%2f27fdf774f284408f8a3e1299cefd18c3.jpg
            string pattern = "url=(?<url>\\S*(.jpg|.png|.bmp))";
            Match match = Regex.Match(url, pattern);
            //http%3a%2f%2fi2.chinanews.com%2fsimg%2fhd%2f2018%2f04%2f19%2f27fdf774f284408f8a3e1299cefd18c3.jpg
            if (match.Success)
                return new Tuple<bool, string>(true, match.Groups["url"].Value.Replace("%2f", "/").Replace("%3a", ":"));
            return new Tuple<bool, string>(false,url);
        }

        private static List<string> ExtractBackgroundImages(string html)
        {
            /*
             * <div id="sections" style="background-size: 100% 100%; background-image: url(&quot;http://image.fristlvy.com/hnxxxdsly/image/GetSingleImage?id=1150&amp;type=Original&amp;_sort=0&amp;_imgType=21&quot;); height: 400px; display: block;">
                <div class="div-b">
                    <div style="color: black;font-size: 16px;margin-top:10px;padding-left: 40px;"><span>更新时间:</span> <span id="newdate">7天前</span></div>
                    <div style="margin-top:5px;width: 100%;">
                        <span class="askQuestion" id="title" style="color: black;font-size: 25px;float:left;padding-left: 40px;line-height:35px;">厦门旅游攻略！玩几天合适，一般玩哪些地方？吃住行是怎么安排的，大概 需要多少费用？去过的朋友求解答！</span>
                    </div>
                </div>
            </div>
            *
            */
            List<string> list = new List<string>();

            //并不常见 TODO
            string pattern = "background-image:(\\s*url|url)(";
            return list;

        }

        public static string ExtractDianPingAveragePrice(string input)
        {
            var price = "0";
            Match match = RegexMatch(input, RegexPattern.DianPingAveragePricePattern);
            if(match.Success && !string.IsNullOrEmpty(match.Groups["price"].Value))
            {
                price = match.Groups["price"].Value;
            }
            return price;
        }

        public static string ExtractDigit(string input)
        {
            Match match = RegexMatch(input, RegexPattern.DigitPattern);
            if (match.Success)
                return match.Value;
            return "";
        }

        public static string ExtractTitle(string input)
        {
            Match match = RegexMatch(input, RegexPattern.TagTitlePattern);
            if (match.Success)
                return match.Groups["title"].Value;
            return "";
        }
    }
}
