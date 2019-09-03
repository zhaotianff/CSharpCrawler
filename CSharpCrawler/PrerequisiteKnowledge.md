# HTTP相关知识点

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
我们平常上网，在浏览器中输出网址，浏览器显示网页内容。
实际上，这个过程是浏览器向网站所在的服务器发送了一个请求，网站服务器接收到这个请求后进行处理和解析，然后返
回对应的响应，响应里包含了要显示的网页内容，浏览器在接收到这个响应后，对内容进行解析并显示


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

GET请求和POST请求的区别
一、GET 请求中的参数包含在 URL 里面，数据可以在 URL 中看到，而 POST 请求的 URL 不会包
含这些数据，数据都是通过表单形式传输的，会包含在请求体中 。
二、GET 请求提交的数据最多只有 1024 字节，而 POST 方式没有限制 。

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
 <img align="center" alt="Request Header" src="ScreenShots/RequestHeaders.png" />
</p>

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

* HTTP常用状态码  

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
 <img align="center" alt="Response Header" src="ScreenShots/ResponseHeaders.png" />
</p>

* 响应体
最重要的当属响应体的内容了。响应的正文数据都在响应体中，比如请求网页时，它的响应体就是网页的 HTML 代码；请求一张图片时 ， 它的响应体就是图片的二进制数据。我们做爬虫请求网页后，要解析的 内容就是响应体。

<p align="center">
 <img align="center" alt="Response Body" src="ScreenShots/ResponseBody.png" />
</p>


## 编程相关

* 在.NET中发送 Xml Post请求和接受Xml的Post请求时，修改请求端Content-Type 为“text/xml”(httpclient.Headers.Add("Content-Type", "text/xml");)

* 下载网页的基本原理是和Web服务器建立Socket连接。发送请求后，再接收数据。在.Net中，可以使用封装好的WebRequest类进行操作

* 使用HttpWebRequest实现多线程下载的核心是：HttpWebRequest提供了一个AddRange方法，可以指定下载的文件数据范围






