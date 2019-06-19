using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpCrawler.Model
{
    public class RobotsExclusionProtocol
    {
        public RobotsExclusionProtocol()
        {
            DisallowList = new List<string>();
            AllowList = new List<string>();
        }
        /// <summary>
        /// 搜索引擎种类，*是一个通配符，代表所有搜索引擎
        /// </summary>
        public string UserAgent { get; set; }

        /// <summary>
        /// 不允许访问的目录
        /// </summary>
        public List<string> DisallowList { get; set; }

        /// <summary>
        /// 允许访问的目录
        /// 
        /// </summary>
        /// <remarks>
        /// /        是一个通配符，代表全部
        /// .htm$    以".htm"为后缀的URL
        /// /*?*     所有包含问号 (?) 的网址
        /// </remarks>
        public List<string> AllowList { get; set; }

        /// <summary>
        /// 网站地图
        /// </summary>
        public string Sitemap { get; set; }

        public override string ToString()
        {
            var disAllowStr = "";
            var allowStr = "";

            DisallowList.ForEach(x => disAllowStr += x + ";");
            AllowList.ForEach(x => allowStr += x + ";");

            return $"搜索引擎:{UserAgent}\r\n" +
                   $"禁止抓取的目录:{disAllowStr}\r\n" +
                   $"允许抓取的目录:{allowStr}\r\n" +
                   $"网站地图:{Sitemap}\r\n\n";
        }
    }
}
