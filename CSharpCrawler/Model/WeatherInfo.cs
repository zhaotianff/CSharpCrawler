using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpCrawler.Model
{
    /// <summary>
    /// 天气信息
    /// </summary>
    /// <example>
    /// 获取到的json字符串如下
    /// {
    ///"weatherinfo":{
    ///    "city":"西安",
    ///    "cityid":"101110101",
    ///    "temp":"23.3",
    ///    "WD":"西南风",
    ///    "WS":"小于3级",
    ///    "SD":"52%",
    ///    "AP":"962.7hPa",
    ///    "njd":"暂无实况",
    ///    "WSE":"<3",
    ///    "time":"18:00",
    ///    "sm":"1.2",
    ///   "isRadar":"1",
    ///    "Radar":"JC_RADAR_AZ9290_JB"
    ///  }
    ///}
    /// </example>
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
