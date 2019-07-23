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

        public void SaveProvince()
        {

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

        public void GetCitiesByProvinceID(int provinceID)
        {

        }

        private void ShowStatusText(string str)
        {
            this.Dispatcher.Invoke(()=> {
                this.paragraph.Inlines.Add(str + Environment.NewLine);
            });
        }
    }
}
