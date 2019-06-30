using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharpCrawler.Util
{
    class UrlUtil
    {
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

        public static string GetPageDownUrl(int page,string baseUrl,string pageDownUrl)
        {
            var suffix1 = "";
            var suffix2 = "";
            return "";
        }
    }
}
