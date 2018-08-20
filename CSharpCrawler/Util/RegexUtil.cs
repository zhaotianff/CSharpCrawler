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

        public static MatchCollection Match(string text,string pattern)
        {
            return Regex.Matches(text, pattern);
        }
    }
}
