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

        public static MatchCollection Match(string text, string pattern)
        {
            return Regex.Matches(text, pattern);
        }

        public static bool IsInvalidImgUrl(string url)
        {
            return Match(url, RegexPattern.MatchImgPattern)[0].Success;
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
    }
}
