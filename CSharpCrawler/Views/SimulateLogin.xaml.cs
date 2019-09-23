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

namespace CSharpCrawler.Views
{
    /// <summary>
    /// SimulateLogin.xaml 的交互逻辑
    /// </summary>
    public partial class SimulateLogin : Page
    {
        public SimulateLogin()
        {
            InitializeComponent();
        }

        private void btn_Login_Click(object sender, RoutedEventArgs e)
        {

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

            var source =await WebUtil.GetHtmlSource(captchaCheckUrl);
            var jsonStr = RegexUtil.Match(source, RegexPattern.Get360CaptchaCheckJsonPattern).Value;
            var json = JsonUtil.ConvertToObject<Root>(jsonStr);

            if(json.captchaFlag == true)
            {
                var tempCaptchaImgPath = Environment.GetEnvironmentVariable("TMP") + "\\";
                var tempCpatchaImgName = Guid.NewGuid().ToString() + ".jpg";
                var fullCaptchaPath = tempCaptchaImgPath + tempCpatchaImgName;

                await WebUtil.DownloadFileAsync(json.captchaUrl,fullCaptchaPath);

                if(System.IO.File.Exists(fullCaptchaPath))
                {
                    this.img_Captcha.Source = new BitmapImage(new Uri(fullCaptchaPath,UriKind.Absolute));
                }
            }
        }
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
