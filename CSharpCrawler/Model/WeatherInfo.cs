using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpCrawler.Model
{
    public class WeatherInfo
    {
        public string City { get; set; } = "";
        public string CityId { get; set; } = "";
        public string Temp { get; set; } = "";
        public string WD { get; set; } = "";
        public string WS { get; set; } = "";
        public string SD { get; set; } = "";
        public string AP { get; set; } = "";
        public string Njd { get; set; } = "";
        public string WSE { get; set; } = "";
        public string Time { get; set; } = "";
        public string SM { get; set; } = "";
        public string IsRadar { get; set; } = "";
        public string Radar { get; set; } = "";

        public override string ToString()
        {
            return string.Format($"City:{City}\r\n"
                +$"CityId:{CityId}\r\n"
                +$"Temp:{Temp}\r\n"
                +$"WD:{WD}\r\n"
                +$"WS:{WS}\r\n"
                +$"SD:{SD}\r\n"
                +$"AP:{AP}\r\n"
                +$"Njd:{Njd}\r\n"
                +$"WSD:{WSE}\r\n"
                +$"Time:{Time}\r\n"
                +$"SM:{SM}\r\n"
                +$"IsRadar:{IsRadar}\r\n"
                +$"Radar:{Radar}\r\n");
        }
    }
}
