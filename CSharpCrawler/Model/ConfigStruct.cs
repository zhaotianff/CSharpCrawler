using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpCrawler.Model
{
    class ConfigStruct
    {
        public FetchUrlConfig UrlConfig { get; set; }

        public FetchImageConfig ImageConfig { get; set; }
    }



    struct FetchUrlConfig
    {
        public string Depth;
        public bool IgnoreUrlCheck;
    }

    struct FetchImageConfig
    {
        public string Depth;
        public bool IgnoreUrlCheck;
        public bool DynamicGrab;
        public int MinSize;  //KB
        public int MaxSize;  
        public string MinResolution;//e.g. 800,600
        public string MaxResolution;
    }


}
