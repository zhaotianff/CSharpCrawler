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
部分网站会用Cookie跟踪用户的访问过程，如果发现了行为异常的访问，就会中断它的访问，比如特别快速地填写表单，或者浏览大量页面。
虽然这些行为可以通过关闭并重新连接网站或改变IP地址等方式来伪装，但是如果Cookie暴露了访问者的身份，再多努力也是白费。  
部分静态网站访问时是不需要Cookie的，直接使用HttpWebRequest/HttpClient类就可以获取。很多网站是需要处理Cookie信息的，所以在抓取目标网站前，
可以使用抓包工具分析网站的Cookie，确定哪一个Cookie是爬虫需要处理的。我们也可以直接使用CEF/Selenium这样的框架去访问网站，浏览器能生成正常访问所需的Cookie，就不用我们手动去处理Cookie。  

* 处理表单隐含字段  
在HTML表单中，“隐含”字段的值对浏览器可见，但对用户不可见（除非查看网页源代码）。
用隐含字段阻止网页抓取的方式主要有两种  
  1. 表单页面上的一个字段可以用服务器生成的随机变量填充。如果提交时这个值不在表单上，服务器就有理由认为它不是从原始表单页面提交的，而是由爬虫直接提交的。
  绕开这个问题的最佳方法是首先抓取表单所在页面上生成的随机数，然后再提交到表单处理页面。  
  2. 如果表单里包含一个具有普通名称的隐含字段，比如“用户名”(username)或“邮箱地址”(email address)，设计不太好的爬虫往往不管这个字段是不是对用户可见，直接填写这个字段并向服务器提交，这样就会中服务器圈套。  
  **Selenium可以区分页面上的可见元素与隐含元素，利用QA.IWebElement.Displayed(C#)属性可以判断元素在页面上是否可见** 
  **这个问题的一个解决办法是使用抓包工具分析正常浏览时提交的表单数据，确定提交了哪些值，使用爬虫抓取时也提交同样的值**