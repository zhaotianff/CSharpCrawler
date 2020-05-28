using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CSharpCrawler.Util
{
    class UrlUtil
    {
        public const string UrlPathSeparator = "/";

        public const string BaiduUrl = "http://www.baidu.com/s?wd=%s";
        public const string WeatherQueryUrl = "http://www.weather.com.cn/data/sk/%s.html";
        //public const string CNBingImageDetailUrl = "https://cn.bing.com/images/async?q=%E9%A3%8E%E6%99%AF&first=250&count=35&relp=35&qft=+filterui%3aphoto-photo&scenario=ImageBasicHover&datsrc=N_I&layout=RowBased&mmasync=1&dgState=x*939_y*907_h*168_c*4_i*211_r*31&IG=765E054519674C8C861E4630A4BF2FC8&SFX=7&iid=images.5601";
        public const string CNBingImageDetailUrl = "https://cn.bing.com/images/async?q=[keyword]&first=[start]&count=35&relp=35&qft=+filterui%3aphoto-photo&scenario=ImageBasicHover&datsrc=N_I&layout=RowBased&mmasync=1&dgState=x*939_y*907_h*168_c*4_i*211_r*31&IG=765E054519674C8C861E4630A4BF2FC8&SFX=7&iid=images.5601";
        public const string CNBingImageUrl = "https://cn.bing.com/images/trending?form=Z9LH";  

        /// <summary>
        /// Bing每日图片获取地址
        /// </summary>
        /// <remarks>
        /// format:
        ///        不指定或xml 显示为xml
        ///        js          显示为json
        /// idx:
        ///        不存在或0， 当天的图像
        ///        -1          明天的图像
        ///        1           昨天的图像
        /// n:
        ///        要显示的结果数量 从指定的idx往回推 最多8䅇
        /// 
        /// </remarks>
        public const string CNBingDailyImageUrl = "https://cn.bing.com/HPImageArchive.aspx?format=xml&idx=0&n=1";

        public const string CNBingDailyImageBasicUrl = "https://cn.bing.com";

        #region Dianping Urls
        public const string DianpingBaseUrl = "https://www.dianping.com";
        public const string DianpingCityListUrl = "https://www.dianping.com/citylist";
        public const string DianpingGetAllProvince = "https://www.dianping.com/ajax/citylist/getAllDomesticProvince";
        public const string DianpingGetCityByProvince = "https://www.dianping.com/ajax/citylist/getDomesticCityByProvince";
        /// <summary>
        /// replace citypyname
        /// </summary>
        public const string DianpingHomeDishes = "https://www.dianping.com/citypyname/ch10/g1783";
        public const string DianpingCityIndex = "https://www.dianping.com/citypyname";
        #endregion

        #region 360
        public const string _360UserHome = "https://i.360.cn";
        public const string _360LoginUrl = "https://login.360.cn/";
        #endregion

        public static bool IsEmpty(string url)
        {
            return string.IsNullOrEmpty(url);
        }

        public static string FixUrl(string url)
        {
            if (url.LastIndexOf("/") == url.Length -1)
            {
                url = url.Replace("//", "@");
                url = url.Substring(0, url.IndexOf("/"));
                url = url.Replace("@", "//");
            }

            if (url.Contains(":") == false)
            {
                url = "http://" + url;
            }

            return url;
        }

        public static bool IsAvailableFileUrl(string url)
        {
            //常见的文件url
            //https://qd.myapp.com/myapp/qqteam/pcqq/PCQQ2019.exe
            //http://qzonestyle.gtimg.cn/qzone/qzactStatics/imgs/20190626150615_2860ae.png
            //https://res.vmallres.com/pimages//common/config/logo/SXppnESYv4K11DBxDFc2.png
            //文件类型太多，这里只写几个需要的
            // http\S*\.(jpg|png|bmp|mp4|exe|rar|zip)


            //另一种文件url
            // /img/2019/flower.jpg            
            // /\S*\.(jpg|png|bmp|mp4|exe|rar|zip)

            var tempUrl = url.ToUpper();
            if (tempUrl.StartsWith("HTTP") || tempUrl.StartsWith("HTTPS"))
            {
                var match = RegexUtil.RegexMatch(url, RegexPattern.MatchFileUrlWithHttpPattern);
                return match.Success;

            }
            else if(tempUrl.StartsWith("/"))
            {
                var match = RegexUtil.RegexMatch(url, RegexPattern.MatchFileUrlWithForwardSlash);
                return match.Success;
            }
            else
            {
                return false;
            }
        }

        public static string FixFileUrl(string url,string baseUrl)
        {
            var tempUrl = url.ToUpper();

            if (tempUrl.StartsWith("HTTP") || tempUrl.StartsWith("HTTPS"))
                return url;
            
            if(tempUrl.StartsWith("/"))
            {
                return baseUrl + url;
            }
            return url;
        }

        public static bool IsEndWithHtml(string url)
        {
            return Regex.IsMatch(url, RegexPattern.EndWithHtmlPattern);
        }

        public static string ExtractBaseUrl(string url)
        {
            if (url.Contains("/") == false)
            {
                return url;
            }
            else
            {
                var index = url.IndexOf("/");
                return url.Substring(0, index -1);
            }
        }

        public static string GetPageDownUrlManual(string baseUrl, string pageDownUrl)
        {
            //目前只考虑简单的翻页，太复杂的翻页还是具体情况具体分析
            var url = "";
            int i;
            for (i = 0; i < baseUrl.Length; i++)
            {
                if (baseUrl[i] == pageDownUrl[i])
                    continue;

                break;
            }

            url = pageDownUrl.Substring(i);

            //补齐
            baseUrl = baseUrl.PadLeft(pageDownUrl.Length, ' ');

            for (i = pageDownUrl.Length - 1; i >= 0; i--)
            {
                if (pageDownUrl[i] == baseUrl[i])
                    continue;

                break;
            }

            url = pageDownUrl.Substring(i);

            var pageStr = RegexUtil.ExtractDigit(url);

            if (string.IsNullOrEmpty(pageStr))
                return pageDownUrl;

            var page = 0;
            int.TryParse(pageStr, out page);

            url = url.Replace(pageStr, (++page).ToString());

            //防止前面有数字重合的，再获取最后一段url
            var subUrl = pageDownUrl.Substring(pageDownUrl.LastIndexOf("/") + 1);
            url = pageDownUrl.Replace(subUrl, subUrl.Replace(pageStr, url));
            return url;
        }

        public static List<string> GetPageDownUrlAuto(string baseUrl)
        {
            //自动获取翻页

            //1、直接在url后面加 _page
            //2、将url最后一段转换成数字加1

            List<string> list = new List<string>();
            var urlSuffix = baseUrl.Substring(baseUrl.LastIndexOf("."));
            var subUrl = baseUrl.Substring(baseUrl.LastIndexOf("/") + 1).Replace(urlSuffix, "");
            var resultUrl = "";
            var pageStr = "";
            var page = 0;

            //获取最后一段数字区域
            pageStr = RegexUtil.ExtractDigit(subUrl);

            if (int.TryParse(pageStr, out page) == true)
                resultUrl = subUrl.Replace(pageStr, (++page).ToString());

            resultUrl = baseUrl.Replace(subUrl, resultUrl);
            //添加到url列表中
            list.Add(resultUrl);

            resultUrl = subUrl + "_2";
            resultUrl = baseUrl.Replace(subUrl, resultUrl);
            //添加到url列表中
            list.Add(resultUrl);
            return list;
        }
    }
}
