using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpCrawler.Model
{
    /// <summary>
    /// 浏览器网络请求/响应
    /// </summary>
    public class NetworkResponse
    {
        public string Url { get; set; }

        /// <summary>
        /// 请求/响应详细
        /// </summary>
        /// <remarks>这里只是方便演示，正常使用时，可以定义多个属性，如RequestUrl/Method/Status/...</remarks>
        public string Detail { get; set; }

    }
}
