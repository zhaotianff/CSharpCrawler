# 如何绕开反爬虫机制

大部分网站会有反爬虫机制，通俗点讲，就是会区分人和机器访问。下面列出了一些常用的方法可以绕开网站的反爬虫机制。

* 修改请求头   
这个方法仅仅适用于使用代码直接访问的场景，如*HttpWebRequest*类（示例程序中已提供相关调用的代码）。  
平常我们在使用浏览器访问网页时，浏览器会生成如下的请求头   

请求头|值
:--:|:--:
Accept|text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8
Accept-Encoding|gzip, deflate, br
Accept-Language|zh-CN,zh;q=0.9
Cache-Control|max-age=0
Connection|keep-alive
Host|www.baidu.com
Upgrade-Insecure-Requests|1
User-Agent|Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/69.0.3464.0 Safari/537.36  

所以我们在使用代码去获取网页源码时，也尽量为这些请求头信息赋值。一般来说Accept和UA影响会比较大，没有赋值可能抓取不到想要的结果。  

UA还可以改变页面的布局样式。例如，使用移动设备访问时，可以将UA改成如下内容(iPhoneX)  
*Mozilla/5.0 (iPhone; CPU iPhone OS 11_0 like Mac OS X) AppleWebKit/604.1.38 (KHTML, like Gecko) Version/11.0 Mobile/15A372 Safari/604.1*

* 延时操作  
大部分网站会判断你两次请求之间的时间间隔。如果在很短的时候内，进行了多次请求，就会被判定为机器人。此时再获取到的页面可能会要求你输入验证码。
```
System.Threading.Thread.Sleep(3000);
```

* Cookie  

