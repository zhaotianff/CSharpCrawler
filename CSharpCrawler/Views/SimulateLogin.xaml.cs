using CSharpCrawler.Util;
using QA =  OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
using ZT.Enhance;

namespace CSharpCrawler.Views
{
    /// <summary>
    /// SimulateLogin.xaml 的交互逻辑
    /// </summary>
    public partial class SimulateLogin : Page
    {
        /// <summary>
        /// 使用Cookie登录360，分析了一段时间没分析出来还缺了什么，一直没有模拟登录成功，脑壳痛。
        /// 可能需要换个网站做示例了，哈哈
        /// </summary>
        public SimulateLogin()
        {
            InitializeComponent();
        }

        #region 使用Cookies

        private async void btn_Login_Click(object sender, RoutedEventArgs e)
        {
            var userName = this.tbox_UserName.Text.Trim();
            var password = this.tbox_Password.Text.Trim();

            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
            {
                EMessageBox.Show("请输入用户名或密码");
                return;
            }
            try
            {
                var sourceBeforeLogin = await WebUtil.GetHtmlSource(UrlUtil._360UserHome);
                this.rtbox_BeforeLoginContent.Document = new FlowDocument(new Paragraph(new Run(sourceBeforeLogin)));

                //获取登录Token
                var getTokenUrl = $"https://login.360.cn/?func=jQuery112406519812780967824_1569420309197&src=pcw_i360&from=pcw_i360&charset=UTF-8&requestScema=https&quc_sdk_version=6.8.3&quc_sdk_name=jssdk&o=sso&m=getToken&userName={userName}&_=1569420309199";

                //请求结果如下：
                /*
                 *  jQuery112406519812780967824_1569420309197({"errno":0,"errmsg":"","token":"916e4fcbfa1c06e5"})
                 *  就不转成对象了，直接使用正则
                 */

                //需要设置Cookie
                CookieContainer cookieContainer = new CookieContainer();
                Cookie _DC_gidCookie = new Cookie("__DC_gid", "59612149.266039479.1569828952751.1569829055668.4");
                _DC_gidCookie.Domain = "login.360.cn";
                cookieContainer.Add(_DC_gidCookie);
                Cookie _guidCookie = new Cookie("__guid", "59612149.2764161016782091300.1569828952751.616");
                _guidCookie.Domain = "login.360.cn";
                cookieContainer.Add(_guidCookie);

                //向服务器请求登录
                var requesttLoginUrl = $"http://s.360.cn/i360/qhpass.htm?src=pcw_i360&version=6.8.3&guid={_guidCookie.Value}&action=submit&module=signin";
                await WebUtil.GetHtmlSource(requesttLoginUrl,cookieContainer:cookieContainer);

                var sourceGetToken = await WebUtil.GetHtmlSource(getTokenUrl,cookieContainer:cookieContainer);
                var token = RegexUtil.Match(sourceGetToken.Item1, RegexPattern.Get360TokenPattern).Groups["token"].Value;

                password = EncryptionUtil.MD5_32(password);
                var postData = $"src=pcw_i360&from=pcw_i360&charset=UTF-8&requestScema=https&quc_sdk_version=6.8.3&quc_sdk_name=jssdk&o=sso&m=login&lm=0&captFlag=1&rtype=data&validatelm=0&isKeepAlive=1&captchaApp=i360&userName={userName}&smDeviceId=&type=normal&account={userName}&password={password}&captcha=&token={token}&proxy=http%3A%2F%2Fi.360.cn%2Fpsp_jump.html&callback=QiUserJsonp331446237&func=QiUserJsonp331446237";
                var contentType = "application/x-www-form-urlencoded";
                var sourceAfterLogin = await WebUtil.PostData(UrlUtil._360LoginUrl, postData, contentType,cookieContainer);
                this.rtbox_AfterLoginContent.Document = new FlowDocument(new Paragraph(new Run(sourceAfterLogin)));
                ShowStatusMessage("登录成功");
            }
            catch (Exception ex)
            {
                ShowStatusMessage("登录失败-" + ex.Message);
            }
        }

        private async void tbox_UserName_LostFocus(object sender, RoutedEventArgs e)
        {
            var userName = tbox_UserName.Text.Trim();
            if (string.IsNullOrEmpty(userName))
                return;

            //会请求是否需要验证码
            var captchaCheckUrl = $"http://login.360.cn/?callback=jQuery112409864490515138251_1569225916432&src=pcw_i360&from=pcw_i360&charset=UTF-8&requestScema=http&quc_sdk_version=6.8.3&quc_sdk_name=jssdk&o=sso&m=checkNeedCaptcha&account={userName}&captchaApp=i360&_=1569225916434";
            //返回结果如下
            // jQuery112409864490515138251_1569225916432({"errno":"0","errmsg":"OK","errinfo":{"en":"OK"},"captchaFlag":false,"captchaUrl":"http:\/\/passport.360.cn\/captcha.php?m=create&app=i360&scene=login&userip=L1rs2jzkX5XAvVNJLg3ipg%3D%3D&level=default&sign=9723cf&r=1569226349"})
            //格式化结果如下
            /*
             * {
            "errno":"0",
            "errmsg":"OK",
            "errinfo":{
                "en":"OK"
                 },
            "captchaFlag":false,
            "captchaUrl":"http://passport.360.cn/captcha.php?m=create&app=i360&scene=login&userip=L1rs2jzkX5XAvVNJLg3ipg%3D%3D&level=default&sign=9723cf&r=1569226349"
            }*/

            //如果captchaFlag = false，就不用输入验证码
            //如果captchaFlag = true,访问captchaUrl来获取验证码

            var source = await WebUtil.GetHtmlSource(captchaCheckUrl);
            var jsonStr = RegexUtil.Match(source, RegexPattern.Get360CaptchaCheckJsonPattern).Value;
            var json = JsonUtil.ConvertToObject<Root>(jsonStr);

            if (json.captchaFlag == true)
            {
                var tempCaptchaImgPath = Environment.GetEnvironmentVariable("TMP") + "\\";
                var tempCpatchaImgName = Guid.NewGuid().ToString() + ".jpg";
                var fullCaptchaPath = tempCaptchaImgPath + tempCpatchaImgName;

                await WebUtil.DownloadFileAsync(json.captchaUrl, fullCaptchaPath);

                if (System.IO.File.Exists(fullCaptchaPath))
                {
                    grid_Captcha.IsEnabled = true;
                    this.img_Captcha.Source = new BitmapImage(new Uri(fullCaptchaPath, UriKind.Absolute));
                }
            }
            else
            {
                grid_Captcha.IsEnabled = false;
            }
        }

        private void ShowStatusMessage(string str)
        {
            this.Dispatcher.Invoke(() =>
            {
                this.lbl_Status.Content = str;
            });
        }

        #endregion

        #region 使用Selenium登录 

        private async void btn_Login_Selenium_Click(object sender, RoutedEventArgs e)
        {
            var userName = this.tbox_UserName_Selenium.Text.Trim();
            var password = this.tbox_Password_Selenium.Text.Trim();

            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
            {
                EMessageBox.Show("请输入用户名或密码");
                return;
            }

            using (OpenQA.Selenium.IWebDriver driver = new OpenQA.Selenium.Edge.EdgeDriver())
            {
               
                driver.Navigate().GoToUrl("http://i.360.cn");  //driver.Url = "http://i.360.cn"是一样的

                var source = driver.PageSource;

                this.rtbox_BeforeLoginContent_Selenium.Document = new FlowDocument(new Paragraph(new Run(source)));
              
                //这个等待是无效的，只是测试代码，可以直接启用下载获取username的代码
                QA.Support.UI.WebDriverWait wait = new QA.Support.UI.WebDriverWait(driver, TimeSpan.FromSeconds(2));
                QA.IWebElement userNameEle = wait.Until<QA.IWebElement>(d => d.FindElement(QA.By.Name("userName")));

                //QA.IWebElement userNameEle = driver.FindElement(QA.By.Name("userName"));
                QA.IWebElement passwordEle = driver.FindElement(QA.By.Name("password"));
                QA.IWebElement loginEle = driver.FindElement(QA.By.ClassName("quc-button-submit quc-button quc-button-primary"));

                //填写用户名
                userNameEle.SendKeys(QA.Keys.Tab);
                userNameEle.Clear();
                userNameEle.SendKeys(userName);
                //主动等待2秒
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(2);

                //填写密码
                passwordEle.SendKeys(QA.Keys.Tab);
                passwordEle.Clear();
                passwordEle.SendKeys(password);
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(2);

                //点击登录按钮
                loginEle.Click();

                //主动等待5秒
                //使用driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(2);这种方式等待无效
                //登录需要时间，如果直接去获取Cookie，获取的还是未登录前的Cookie
                System.Threading.Thread.Sleep(5000);

                //Cookies
                var cookies = driver.Manage().Cookies.AllCookies;

                var cookieContainer = SeleniumUtil.CookieConvert(cookies);

                var html = await WebUtil.GetHtmlSource("http://i.360.cn", cookieContainer: cookieContainer);

                this.rtbox_AfterLoginContent_Selenium.Document = new FlowDocument(new Paragraph(new Run(html.Item1)));
            }
        }

        #endregion
    }

    /// <summary>
    /// 自动化生成
    /// </summary>
    public class Errinfo
    {
        /// <summary>
        /// 
        /// </summary>
        public string en { get; set; }
    }

    /// <summary>
    /// 自动化生成
    /// </summary>
    public class Root
    {
        /// <summary>
        /// 
        /// </summary>
        public string errno { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string errmsg { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Errinfo errinfo { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool captchaFlag { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string captchaUrl { get; set; }
    }
}
