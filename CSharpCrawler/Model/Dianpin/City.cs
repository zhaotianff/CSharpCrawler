using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpCrawler.Model.Dianpin
{
    public class City
    {
       public int ProvinceID { get; set; }
       public int CityID { get; set; }
       public string CityName { get; set; }
       public string CityPinYinName { get; set; }

        public override string ToString()
        {
            return $"City ID: {CityID}  City Name: {CityName}({CityPinYinName})";
        }
    }
}
