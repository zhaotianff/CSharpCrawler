using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpCrawler.Model.Dianpin
{
    public class Result
    {
        public int CityID { get; set; }

        public string Html { get; set; }

        public string RestaurentName { get; set; }

        public string AverageSpend { get; set; }

        public byte[] RestaurentImage { get; set; }
    }
}
