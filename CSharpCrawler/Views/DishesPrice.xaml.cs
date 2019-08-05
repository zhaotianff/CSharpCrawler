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
using System.Net;

namespace CSharpCrawler.Views
{
    /// <summary>
    /// DishesPrice.xaml 的交互逻辑
    /// </summary>
    public partial class DishesPrice : Page
    {
        private string dbPath = Environment.CurrentDirectory + "\\User Data\\Dianping\\db\\" + "dianping";

        List<City> cityList = new List<City>();

        public DishesPrice()
        {
            InitializeComponent();
        }

        private async void btn_StartGetCity_Click(object sender, RoutedEventArgs e)
        {
            ShowStatusText($"正在从{UrlUtil.DianpingGetAllProvince}获取省份信息");
            List<Province> provinceList = await  GetProvinces();
            ShowProvincesInfo(provinceList);
            SaveProvinceToDB(provinceList);

            ShowStatusText($"正在从{UrlUtil.DianpingGetCityByProvince}获取城市信息");
            cityList = await GetAllCities(provinceList);
            SaveCityToDB(cityList);
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

        public async void SaveProvinceToDB(List<Province> provinceList)
        {
            await Task.Run(()=> {

                ShowStatusText($"开始保存省份信息");

                using (SQLiteUtil sqlite = new SQLiteUtil(dbPath))
                {
                    var sql = "Delete From Province";
                    sqlite.ExecuteNonQuery(sql);

                    foreach (var item in provinceList)
                    {
                        sql = "Insert Into Province (ProvinceID,ProvinceName) Values (@ProvinceID,@ProvinceName)";
                        System.Data.SQLite.SQLiteParameter[] parameters = new System.Data.SQLite.SQLiteParameter[]
                        {
                        new System.Data.SQLite.SQLiteParameter("@ProvinceID",item.ProvinceID),
                        new System.Data.SQLite.SQLiteParameter("@ProvinceName",item.ProvinceName)
                        };
                        sqlite.ExecuteNonQuery(sql, parameters);
                    }
                }
                ShowStatusText($"保存省份信息完成");
            }); 
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

                await Task.Delay(2000);
            }

            return cityList;
        }

        public async void SaveCityToDB(List<City> cityList)
        {
            await Task.Run(()=> {

                ShowStatusText($"开始保存城市数据");

                using (SQLiteUtil sqlite = new SQLiteUtil(dbPath))
                {
                    var sql = "Delete From City";
                    sqlite.ExecuteNonQuery(sql);

                    foreach (var item in cityList)
                    {
                        sql = "Insert Into City (ProvinceID,CityID,CityName,CityPinYin) Values (@ProvinceID,@CityID,@CityName,@CityPinYin)";
                        System.Data.SQLite.SQLiteParameter[] parameters = new System.Data.SQLite.SQLiteParameter[]
                        {
                        new System.Data.SQLite.SQLiteParameter("@ProvinceID",item.ProvinceID),
                        new System.Data.SQLite.SQLiteParameter("@CityID",item.CityID),
                        new System.Data.SQLite.SQLiteParameter("@CityName",item.CityName),
                        new System.Data.SQLite.SQLiteParameter("@CityPinYin",item.CityPinYinName)
                        };
                        sqlite.ExecuteNonQuery(sql, parameters);
                        ShowStatusText($"保存{item.CityName}数据完成");
                        Task.Delay(5);
                    }
                }
                ShowStatusText($"保存城市数据完成");
            }); 
        }

        private void ShowStatusText(string str)
        {
            this.Dispatcher.Invoke(()=> {
                this.paragraph.Inlines.Add(str + Environment.NewLine);
            });
        }

        private async void btn_StartDishPrice_Click(object sender, RoutedEventArgs e)
        {
            if(cityList.Count == 0)
            {
                this.paragraph_Step2.Inlines.Add("请先获取城市列表");
                //return;
            }

            //这里只是简单的示例，所以仅抓取第一页数据
            //需要添加以下Cookie信息，否则会出现验证码页面
            var url = "";
            var html = "";
            var userAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/63.0.3239.132 Safari/537.36";
            var accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8";

            CookieContainer cookieContainer = new CookieContainer();

            System.Net.Cookie cookie = new Cookie("_hc.v", "721d1647-b5e0-18b6-d41a-43453671c5f8.1563348000");
            cookie.Domain = "m.dianping.com";
            cookieContainer.Add(cookie);

            cookie = new Cookie("_lx_utm", "utm_source%3DBaidu%26utm_medium%3Dorganic");
            cookie.Domain = "m.dianping.com";
            cookieContainer.Add(cookie);

            cookie = new Cookie("_lxsdk", "16bfecd5b69c8-070d60ba7b365d-3c604504-1fa400-16bfecd5b69c8");
            cookie.Domain = "m.dianping.com";
            cookieContainer.Add(cookie);

            cookie = new Cookie("_lxsdk_cuid", "16bfecd5b69c8-070d60ba7b365d-3c604504-1fa400-16bfecd5b69c8");
            cookie.Domain = "m.dianping.com";
            cookieContainer.Add(cookie);

            //cookie = new Cookie("_lxsdk_s", "16c3b315b86-7bd-ed8-6a4%7C%7C21");
            //cookie.Domain = "www.dianping.com";
            //cookieContainer.Add(cookie);

            cookie = new Cookie("cityid", "2");
            cookie.Domain = "m.dianping.com";
            cookieContainer.Add(cookie);

            cookie = new Cookie("default_ab", "shopList%3AC%3A4");
            cookie.Domain = "m.dianping.com";
            cookieContainer.Add(cookie);

            cookie = new Cookie("cy", "7");
            cookie.Domain = "m.dianping.com";
            cookieContainer.Add(cookie);

            cookie = new Cookie("cye", "shenzhen");
            cookie.Domain = "m.dianping.com";
            cookieContainer.Add(cookie);

            cookie = new Cookie("s_ViewType", "10");
            cookie.Domain = "m.dianping.com";
            cookieContainer.Add(cookie);
              
            //PC页面价格加密了，访问移动端页面来获取价格
            foreach (var item in cityList)
            {
                //url = UrlUtil.DianpingHomeDishes.Replace("citypyname", item.CityPinYinName);
                //移动端
                //url = url.Replace("www", "m");

                url = "https://www.dianping.com/changsha/ch10/g1783";

                html = await WebUtil.GetHtmlSource(url, accept, userAgent, Encoding.UTF8, cookieContainer);


            }

            url = "https://m.dianping.com/hefei/ch10/g1783";

            html = await WebUtil.GetHtmlSource(url, accept, userAgent, Encoding.UTF8, cookieContainer);

            var pattern = "(?<=<ul class=\"list-search\">)[\\s\\S]*(?=</ul>)";

            var result = RegexUtil.Match(html, pattern);
        }



        private void SaveResult(Result result)
        {
            Task.Run(()=> {

            });
        }
    }
}
