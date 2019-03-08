using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharpCrawler.Util
{
    class Urls
    {
        public const string BaiduUrl = "http://www.baidu.com/s?wd=%s";

        public const string WeatherQueryUrl = "http://www.weather.com.cn/data/sk/%s.html";

        public const string CnBingImageUrl = "https://cn.bing.com/images/async?q=%E9%A3%8E%E6%99%AF&first=250&count=35&relp=35&qft=+filterui%3aphoto-photo&scenario=ImageBasicHover&datsrc=N_I&layout=RowBased&mmasync=1&dgState=x*939_y*907_h*168_c*4_i*211_r*31&IG=765E054519674C8C861E4630A4BF2FC8&SFX=7&iid=images.5601";

        public static bool IsEmpty(string url)
        {
            return string.IsNullOrEmpty(url);
        }
    }
}
