using CefSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CSharpCrawler.Util
{
    public class SeleniumUtil
    {
        public static System.Net.Cookie CookieConvert(OpenQA.Selenium.Cookie cookie)
        {
            System.Net.Cookie tempCookie = new System.Net.Cookie();
            tempCookie.Domain = cookie.Domain;
            tempCookie.Expires = cookie.Expiry ?? DateTime.Today;
            tempCookie.HttpOnly = cookie.IsHttpOnly;
            tempCookie.Name = cookie.Name;
            tempCookie.Path = cookie.Path;
            tempCookie.Secure = cookie.Secure;
            tempCookie.Value = cookie.Value;
            return tempCookie; 
        }

        public static CookieContainer CookieConvert(System.Collections.ObjectModel.ReadOnlyCollection<OpenQA.Selenium.Cookie> cookies)
        {
            CookieContainer container = new CookieContainer();
            foreach (var item in cookies)
            {
                var cookie = CookieConvert(item);
                container.Add(cookie);
            }
            return container;         
        }
    }
}
