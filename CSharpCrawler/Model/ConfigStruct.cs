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
        public string IgnoreUrlCheck;
    }

    struct FetchImageConfig
    {

    }


}
