# 爬虫相关知识点

## HTTP和HTTPS  
**HTTP:Hyper Text Transfer Protocol 超文件传输协议**  
HTTP协议是用于网络传输超文本数据到本地的传输协议  

**HTTPS:Hyper Text Transfer Protocol over Secure Socket Layer 超文本传输安全协议**  
HTTPS协议是HTTP的安全版，加入SSL层，数据传输经过SSL加密。使用HTTPS可以保证数据通过安全的通道来进行传输。还能确定网站的真实性，采用https的服务器必须从CA （Certificate Authority）申请一个用于证明服务器用途类型的证书。

>访问部分网页时，有时候会碰到提示不安全或HTTPS打叉的情况，这可能是由以下原因导致:  
>1.证书已过过期  
>2.证书跟域名不对应  
>3.证书不是由标准CA机构颁发  

## HTTP 请求过程
我们平常上网，在浏览器中输入网址，再回车，浏览器显示网页内容。
实际上，这个过程是浏览器向网站所在的服务器发送了一个请求，网站服务器接收到这个请求后进行处理和解析，然后返
回对应的响应，响应里包含了要显示的网页内容，浏览器在接收到这个响应后，对内容进行解析并显示。


## HTTP 请求分为四部分内容

* 请求方法（ Request Method ）  

  常用请求方法

方法|说明
:--:|:--:
GET|请求获取特定的资源 
HEAD|请求页面报头
POST|提交表单或上传文件等，数据包含在请求体中
PUT|替换服务器指定文档中的内容
DELETE|请求服务器删除指定的页面
CONNECT|把服务器当作跳板，让服务器代替客户端访问其它页面
OPTIONS|允许客户端查看服务器的性能
TRACE|回显服务器收到的请求，主要用于测试或诊断  

>**GET请求和POST请求的区别**  
>一、GET 请求中的参数包含在 URL 里面，数据可以在 URL 中看到，而 POST 请求的 URL 不会包
>含这些数据，数据都是通过表单形式传输的，会包含在请求体中 。  
>二、GET 请求提交的数据最多只有 1024 字节，而 POST 方式没有限制 。

* 请求的网址( Request URL ）  

  要访问的网址

* 请求头（ Request Headers ） 

  服务器使用的附加信息。  
  常用请求头信息如下：

请求头|说明
:--:|:--:
Accept|请求报头域，用于指定客户端可以接受哪些类型的信息，如：Accept:text/plain,text/html
Accept-Charset|浏览器可以接受的字符编码集。	如：Accept-Charset:utf-8
Accept-Language|指定客户端可以接受的语言类型 如：Accept-Language:en,zh
Accept-Encoding|指定客户端可以接受的内容编码 如：	Accept-Encoding:compress,gzip
Accept-Ranges|可以请求网页实体的一个或者多个子范围字段	如：Accept-Ranges:bytes
Authorization|HTTP授权的授权证书	如：Authorization:Basic QWxhZGRpbjpvcGVuIHNlc2FtZQ==
Cache-Control|指定请求和响应遵循的缓存机制	如：Cache-Control:no-cache
Connection|表示是否需要持久连接。（HTTP 1.1默认进行持久连接）	如：Connection:close
Content-Length|请求的内容长度	如：Content-Length:348
Host|用于指定请求资源的主机IP和端口号，其内容为请求URL的原始服务器或网关的位置。从HTTP1.1版本开始，请求必须包含此内容
Cookies|网站为了辨别用户进行会话跟踪而存储在用户本地的数据。它的主要功能是维持当前访问会话 。 例如，我们输入用户名和密码成功登录某个网站后，服务器会用会话保存登录状态信息，后面我们每次刷新或请求该站点的其他页面时，会发现都是登录状态，这就是 Cookies 的功劳 。 Cookies 里有信息标识了我们所对应的服务器的会话，每次浏览器在请求该站点的页面时，都会在请求头中加上 Cookies 并将其发送给服务器，服务器通过 Cookies 识别出是我们自己，并且查出当前状态是登录状态，所以返回结果就是登录之后才能看到的网页内容 。
Referer|标识这个请求是从哪个页面发过来的，服务器可以拿到这一信息并做相应的处理，如做来源统计、防盗链处理等
User-Agent|简单UA，它是一个特殊的字符串头，可以使服务器识别客户使用的操作系统及版本、浏览器及版本信息。使用爬虫抓取时，可以使用UA伪装成浏览器，否则容易被检测到是机器访问。
Content-Type|也叫互联网媒体类型（Internet Media Type）或都MIME类型，在HTTP协议消息头中，它用来表示具体请求中的媒体类型信息。如text/html代表HTML格式，image/gif代表gif图片。如果MIME类型指定不对，可能会导致POST提交后，无法正常响应，从而获取不到自己想要的内容。如：Content-Type:application/x-www-form-urlencoded

<p align="center">
 <img align="center" alt="Request Header" src="doc/RequestHeaders.png" />
</p>
<p align="center">请求头</p>

* 请求体（ Request Body ）  

  请求体一般承载的内容是POST请求中的表单数据，对于GET请求而言，请求体为空。  

  Content-Type和POST提交数据方式的关系

Content-Type|提交数据的方式
:--:|:--:
application/x-www-form-urlencoded|表单数据
multipart/form-data|表单文件上传
application/json|序列化json数据
text/xml|XML数据

## HTTP响应

* HTTP常用响应状态码  

状态码|说明
:--:|:--:
200|OK
304|NOT MODIFIED
401|UNAUTHORIZED
403|FORBIDDEN
404|NOT FOUND
405|METHOD NOT ALLOWED
501|NOT IMPLEMENTED

* 响应头  

  常用头信息如下

响应头|说明
:--:|:--:
Date|标识响应产生的时间
Last-Modified|指定资源的最后修改时间
Content-Encoding|指定响应内容的编码
Server|包含服务器的信息，比如名称、版本号等
Content-Type|文档类型，指定返回的数据类型是什么，如text/html代表返回的是HTML文档
Set-Cookie|设置Cookie。响应头中的Set-Cookie告诉浏览器需要将此内容放在Cookies中，下次请求携带Cookies请求
Expires|指定响应的过期时间，可以使代理服务器或浏览器将加载的内容更新到缓存中，如果再次访问时，就可以直接从缓存中加载，降低服务器的负载，缩短加载时间

<p align="center">
 <img align="center" alt="Response Header" src="doc/ResponseHeaders.png" />
</p>
<p align="center">响应头</p>

* 响应体  

  最重要的当属响应体的内容了。响应的正文数据都在响应体中，比如请求网页时，它的响应体就是网页的 HTML 代码；请求一张图片时，它的响应体就是图片的二进制数据。我们做爬虫请求网页后，要解析的 内容就是响应体。

<p align="center">
 <img align="center" alt="Response Body" src="doc/ResponseBody.png" />
</p>
<p align="center">响应体</p>

## HTTP无状态协议

HTTP无状态协议，是指HTTP协议对于事务处理没有记忆能力。缺少状态意味着如果后续处理需要前面的信息，则它必须重传，这样可能导致每次连接传送的数据量增大。另一方面，在服务器不需要先前信息时它的应答就较快。

## Session(会话)和Cookie

Session和 Cookies是为了保持 HTTP 连接状态。HTTP本身是一个无状态的连接协议，为了支持客户端与服务器之间的交互，我们就需要通过不同的技术为交互存储状态，而这些不同的技术就是Cookie和Session了。

* Session(会话)  
*会话位于服务器端*。在Web中，会话对象用来存储特定用户会话所需的属性及配置信息。这样，当用户在Web页之间跳转时，存储在会话对象中的变量将不会丢失。当用户请求访问Web页时，如果该用户没有会话，则Web服务器将自动创建一个会话对象。当会话过期或被放弃后，服务器将终止该会话。

* Cookie  
*Cookie位于客户端*。Cookie，有时也用其复数形式 Cookies，指某些网站为了辨别用户身份、进行Session跟踪而储存在用户本地终端上的数据（通常经过加密）。（Cookie可以叫做浏览器缓存）

当客户端第一次请求服务器时，服务器会返回一个请求头中带有Set-Cookie字段的响应给客户端，用于标记是哪一个用户，客户端浏览器会把Cookies保存起来。当浏览器下一次再请求该网站时，浏览器会把此Cookies放到请求头一起提交给服务器，Cookies携带了会话ID信息，服务器检查该Cookies即可找到对应的会话是什么，然后再判断会话来识别用户状态。在成功登录某个网站时，服务器会告诉客户端设置哪些Cookies信息，在后续访问该页面时，客户端会把Cookie发送给服务器，服务器再找到对应的会话加以判断。如果会话中的某些设置登录状态的变量有有效的，那就证明用户处于登录状态，可以看到登录之后的网页内容，反之，可能会要求重新登录，或者看到未登录前的页面。

所有Cookies和会话需要配合，一个处于客户端，一个处于服务器，二者共同协作，就实现了登录会话控制。

有很多网站有记住登录的功能。原理是把Cookie的Expires得比较长,而且Cookie里会记录会话ID信息。当再次访问记住登录的页面时，仍然能够通过本地的Cookie找以原来的会话ID，这就实现了保持登录状态的功能

## Cookie的结构  

<p align="center">
 <img align="center" alt="Cookies" src="doc/Cookies.png" />
</p>
<p align="center">Cookie结构</p>

* Name  
Cookie的名称，一旦创建，不可修改。
* Value  
该Cookie的值。如果值为Unicode字符，需要为字符编码。如果值为二进制数据，则需要使用BASE64编码  
* Domain  
可以访问该Cookie的域名.如.baidu.com。如果为N/A，则表示该值不适用
* Expires /Max Age  
表示Cookie失效的时间，单位为秒。如果为正数，则该Cookie在Max Age秒之后失效。如果为负数或N/A，则关闭浏览器Cookie就失效。
* Path  
该Cookie的使用路径。如果设置为/path/，则只有路径为/path/的页面可以访问该Cookie。如果为/，则本域名下的所有页面都可访问该Cookie
* Size  
该Cookie的大小  
* HTTP  
Cookie的httponly属性。或此属性为true，则只有在HTTP头中会带有此Cookie的信息。
* Secure  
该Cookie是否仅被使用安全协议传输

## 爬虫原理
爬虫其实就是获取网页，并提取网页信息的程序。

**爬虫的步骤**
* 获取网页  
使用代码抓取网页的源码内容。  
可以使用以下几种方法
1. HttpWebRequest/HttpWebResponse  
这是.Net类库提供的类，可以方便的抓取网页源码，优点是使用方便，缺点是不能抓取动态页面。  
2. [使用Selenium](https://www.cnblogs.com/zhaotianff/p/11330810.html)  
Selenium本身是一个自动化浏览器测试工具，但也可以被用于爬虫。通过使用不同的浏览器驱动，可以支持不同的浏览器，包括IE（7, 8, 9, 10, 11），Mozilla Firefox，Safari，Google Chrome，Opera等.  优点是可以获取动态页面，而且可以模拟鼠标点击，拖动等操作。缺点是会弹出浏览器窗体。·
3. [嵌入浏览器](https://www.cnblogs.com/zhaotianff/p/9556270.html)  
可以使用WebBrowser/[CEF](https://github.com/cefsharp/CefSharp)等浏览器控件。抓取原理是使用内嵌浏览器打开一个页面，再获取页面源码。这种方式的优点是可以抓取动态页面，缺点是抓取是被动的，不能主动控制。
4. [无头浏览器](https://www.cnblogs.com/zhaotianff/p/13528507.html)  
无头浏览器和嵌入浏览器获取网页源码的原理是一样的，都是使用浏览器打开一个页面，再获取源码。但它是以Headless方式运行，可以不用显示在界面上。在C#中常用的是Puppeteer-Sharp，这个库还支持主动等待抓取结果。

* 提取网页信息  

  网页源码获取到之后，我们需要对自己想要的内容进行提取，最常用的，肯定就是链接里的图片和视频等。
在C#中，可以使用以下方式来提取网页信息
1. [正则表达式](正则表达式.md)  
   这种方式虽然需要学习正则表达式的相关知识，但功能强大，几乎无所不能。这里我做了一些总结，可以参考一下
2. 使用[AngleSharp](https://github.com/AngleSharp/AngleSharp)/[HtmlAgilityPack](https://github.com/zzzprojects/html-agility-pack)等支持DOM的HTML解析库。  
   HtmlAgilityPack支持XPath查询，暂不支持CSS选择器。AngleSharp支持CSS选择器，暂不支持XPath查询

    附加：   
    1. 如果使用的是CEF/Puppeteer-Sharp，可以执行js来进行信息提取
    2. 如果使用的是Selenium，可直接使用Selenium自带的CSS选择器和XPath查询来进行信息提取

* 保存数据  
这里得看自己需求，推荐SQLite/Berkeley DB等嵌入式数据库，Redis和Mongodb也是不错的选择。

**如何抓取动态网页的内容**  

动态网页指的是采用 了Ajax 、前端模块化工具来构建，整个网页由 JavaScript 动态渲染出来的页面。    
对于这种情况，我们可以分析其后台 Ajax 接口，也可使用 Selenium/CEF/Puppeteer-Sharp这样的库来实现模拟 JavaScript渲染 。




## 编程相关

* 在.NET中发送 Xml Post请求和接受Xml的Post请求时，修改请求端Content-Type 为“text/xml”(httpclient.Headers.Add("Content-Type", "text/xml");)

* 下载网页的基本原理是和Web服务器建立Socket连接。发送请求后，再接收数据。在.Net中，可以使用封装好的WebRequest类进行操作

* 使用HttpWebRequest实现多线程下载的核心是：HttpWebRequest提供了一个AddRange方法，可以指定下载的文件数据范围



## 其它

### 深网和暗网
* 深网
深网是Web的一部分，与浅网对立。浅网是互联网上搜索引擎可以抓到的那部分网络。据估计，互联网中其实约90%的网络都是深网。
因为谷歌不能做像表单提交这类事情，也找不到那些没有直接链接到顶层域名上的网页，或者因为有robots.txt禁止而不能查看网站，
所以浅网的数量相对深网还是比较少的。
* 暗网  
暗网，也被称为darknet，则完全是另一种网络。它们也建立在已有的网络硬件基础上，但是使用Tor或者另一个客户端，带有运行在HTTP之上的应用协议，
提供了一个信息交换的安全渠道。(可以查看这篇文章来获取暗网的更多介绍：https://www.cnblogs.com/lsdb/p/10018830.html)


