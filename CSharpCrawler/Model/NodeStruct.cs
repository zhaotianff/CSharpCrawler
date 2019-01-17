using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpCrawler.Model
{
    /// <summary>
    /// 结点结构(DOM树显示)
    /// </summary>
    public class NodeStruct
    {    
        public string DisplayName { get; set; }
        public string InnerText { get; set; }        
        public string InnerHtml { get; set; } 
        public string OuterHtml { get; set; }            
        public List<NodeStruct> Children { get; set; }
        public NodeStruct()
        {
            Children = new List<NodeStruct>();
        }
    
    }
}
