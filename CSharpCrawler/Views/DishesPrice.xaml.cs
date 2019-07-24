using CSharpCrawler.Model.Dianpin;
using CSharpCrawler.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CSharpCrawler.Views
{
    /// <summary>
    /// DishesPrice.xaml 的交互逻辑
    /// </summary>
    public partial class DishesPrice : Page
    {

        public DishesPrice()
        {
            InitializeComponent();
        }

        private void InitDB()
        {

        }

        private async void btn_StartGetCity_Click(object sender, RoutedEventArgs e)
        {
            ShowStatusText($"正在从{UrlUtil.DianpingGetAllProvince}获取省份信息");
            List<Province> provinceList = await  GetProvinces();
            ShowProvincesInfo(provinceList);
            ShowStatusText($"正在从{UrlUtil.DianpingGetCityByProvince}获取城市信息");
            List<City> cityList = await GetAllCities(provinceList);
        }

        public async Task<List<Province>> GetProvinces()
        {
            List<Province> list = new List<Province>();
            var provincesJsonStr = await WebUtil.PostData(UrlUtil.DianpingGetAllProvince, "");
            var provincesObj = JObject.Parse(provincesJsonStr);
            var provincesJToken = provincesObj["provinceList"];
            list =  provincesJToken.Select(x=>new Province {ProvinceID = (int)x["provinceId"],ProvinceName = (string)x["provinceName"] }).ToList();
            return list;
        }

        public void SaveProvinceToDB(List<Province> privinceList)
        {
            //TODO
        }

        public void ShowProvincesInfo(List<Province> list)
        {
            ShowStatusText($"获取到{list.Count}条记录");
            foreach (var item in list)
            {
                ShowStatusText(item.ToString());
            }
        }

        /// <summary>
        /// {"provinceId":id}
        /// </summary>
        /// <param name="provinceList"></param>
        /// <returns></returns>
        public async Task<List<City>> GetAllCities(List<Province> provinceList)
        {
            List<City> cityList = new List<City>();
            var postData = "{\"provinceId\":id}";

            foreach (var item in provinceList)
            {
                var tempPostData = postData.Replace("id", item.ProvinceID.ToString());
                var citiesJsonStr = await WebUtil.PostData(UrlUtil.DianpingGetCityByProvince, tempPostData, "application/json");

                var citiesObj = JObject.Parse(citiesJsonStr);
                var citiesJToken = citiesObj["cityList"];

                var tempCitiesList = citiesJToken.Select(x=>new City {
                    CityID = (string)x["cityId"],
                    CityName = (string)x["cityName"],
                    CityPinYinName = (string)x["cityPyName"],
                    ProvinceID = item.ProvinceID
                }).ToList();

                cityList.AddRange(tempCitiesList);

                ShowStatusText($"***********************************\r\n{item.ProvinceName}\r\n***********************************");
                tempCitiesList.ForEach(x => ShowStatusText(x.ToString()));
            }

            return cityList;
        }

        public void SaveCityToDB(List<City> cityList)
        {
            //TODO
        }

        private void ShowStatusText(string str)
        {
            this.Dispatcher.Invoke(()=> {
                this.paragraph.Inlines.Add(str + Environment.NewLine);
            });
        }

        private async void btn_StartDishPrice_Click(object sender, RoutedEventArgs e)
        {
            //这里只是简单的示例，所以仅抓取第一页数据
            //shenzhen

            var url = UrlUtil.DianpingHomeDishes.Replace("citypyname", "shenzhen");
            var htmlSourceCode = await WebUtil.GetHtmlSource(url,"text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8", "Mozilla/5.0 (Windows NT 6.3; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/55.0.2883.87 Safari/537.36");
            var pattern = "(?<=<li class=\"\")[\\s\\S]*?(?=</li>)";
            var matchCollection = RegexUtil.Match(htmlSourceCode, pattern);

            
        }
    }
}
