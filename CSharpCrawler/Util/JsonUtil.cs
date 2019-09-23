using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using CSharpCrawler.Model;
using Newtonsoft.Json.Linq;
using System.Reflection;
using Newtonsoft.Json.Converters;
using System.IO;

namespace CSharpCrawler.Util
{
    public class JsonUtil
    {
        private string filePath = "";

        public JsonUtil()
        {

        }

        public void OpenFile(string filePath)
        {
            this.filePath = filePath;
        }

        public static WeatherInfo ResolveWeather(string source)
        {
            WeatherInfo weatherInfo = new WeatherInfo();
            JObject weatherObject = JObject.Parse(source);

            JToken root = weatherObject["weatherinfo"];

            foreach (JProperty item in root)
            {
                AttributeAssignment(weatherInfo, item.Name, item.Value.ToString());
            }
            return weatherInfo;
        }

        private static void AttributeAssignment(WeatherInfo weatherInfo,string property,string value)
        {
            foreach (var item in weatherInfo.GetType().GetProperties())
            {
                if(item.Name.ToUpper() == property.ToUpper())
                {
                    item.SetValue(weatherInfo, value);
                }
            }
        }

        public static T ConvertToObject<T>(string jsonStr)
        {
            T t = default(T);
            try
            {               
                JsonSerializer serializer = new JsonSerializer();
                serializer.Converters.Add(new JavaScriptDateTimeConverter());
                serializer.NullValueHandling = NullValueHandling.Ignore;             
                t = JsonConvert.DeserializeObject<T>(jsonStr);               
            }
            catch
            {
                return default(T);
            }
            return t;
        }
    }
}
