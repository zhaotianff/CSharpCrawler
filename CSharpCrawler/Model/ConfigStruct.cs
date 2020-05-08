using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpCrawler.Model
{
    public class ConfigStruct
    {
        public FetchUrlConfig UrlConfig { get; set; }

        public FetchImageConfig ImageConfig { get; set; }

        public CommonConfig CommonConfig { get; set; }

        public List<Theme> ThemeList { get; set; }
    }

    public class CommonConfig
    {
        /// <summary>
        /// 是否检测Url合法性
        /// </summary>
        public bool UrlCheck { get; set; } = false;
    }

    public class FetchUrlConfig
    {
        public string Depth { get; set; }
        public bool IgnoreUrlCheck { get; set; }
        public bool DynamicGrab { get; set; }
    }

    public class FetchImageConfig
    {
        public string Depth { get; set; }
        public bool IgnoreUrlCheck { get; set; }

        /// <summary>
        /// 是否使用动态抓取
        /// </summary>
        public bool DynamicGrab { get; set; }  
        public int MinSize { get; set; }
        public int MaxSize { get; set; }
        public string MinResolution { get; set; }
        public string MaxResolution { get; set; }

        /// <summary>
        /// FetchMode 0-Mix 1-Regex 2-HtmlAgilityPack
        /// </summary>
        public int FetchMode { get; set; }

        /// <summary>
        /// 0-Manual 1-Auto
        /// </summary>
        public int PageDownRule { get; set; } = -1;

        /// <summary>
        /// 0-url 1-post
        /// </summary>
        public int ManualPageDownMethod { get; set; } = -1;

        public string PageDownUrl { get; set; }

        /// <summary>
        /// reserve
        /// </summary>
        public string PageDownPostData { get; set; }    
        
    }

    public class Theme
    {
        public string Background { get; set; }
        public BackgroundType BackgroundType { get; set; }
    }

    public enum BackgroundType
    {
        Static = 0,
        Dynamic = 1
    }
}
