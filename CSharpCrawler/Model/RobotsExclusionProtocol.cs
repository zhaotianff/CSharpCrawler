using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpCrawler.Model
{
    public class RobotsExclusionProtocol
    {
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

            return $"UserAgent:{UserAgent}\r\n" +
                   $"DisallowList:{disAllowStr}\r\n" +
                   $"AllowList:{allowStr}\r\n" +
                   $"Sitemap:{Sitemap}";
        }
    }
}
