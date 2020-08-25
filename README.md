# C\#爬虫项目

<p align="center">
<a href="https://github.com/zhaotianff/CSharpCrawler" target="_blank">
<img align="center" alt="CSharpCrawler" src="CSharpCrawler/crawler.png" />
</a>
</p>
<p align="center">
<a href="https://github.com/zhaotianff/CSharpCrawler/stargazers" target="_blank">
 <img alt="GitHub stars" src="https://img.shields.io/github/stars/zhaotianff/CSharpCrawler.svg" />
</a>
<a href="https://github.com/zhaotianff/CSharpCrawler/releases" target="_blank">
 <img alt="All releases" src="https://img.shields.io/github/downloads/zhaotianff/CSharpCrawler/total.svg" />
</a>
<a href="https://github.com/zhaotianff/CSharpCrawler/network/members" target="_blank">
 <img alt="Github forks" src="https://img.shields.io/github/forks/zhaotianff/CSharpCrawler.svg" />
</a>
<a href="https://github.com/zhaotianff/CSharpCrawler/issues" target="_blank">
 <img alt="All issues" src="https://img.shields.io/github/issues/zhaotianff/CSharpCrawler.svg" />
</a>
</p>
<h1 align="center">CSharpCrawler :spider: </h1>

### 关于项目
C#开发爬虫的知识总结，目前还在更新中。这并不是一个完整的爬虫程序，只是一些示例。  
> 为什么要拿C#开发爬虫项目，因为个人还是比较喜欢C#。C#虽然库少一点，但想要的功能基本还是能实现的。  
> 总结的知识点如果什么错误之处，还恳请大家提个issue指正，一起学习进步♂（￣▽￣）/  

### 功能介绍

* 基础知识
  * [爬虫基础知识](CSharpCrawler/PrerequisiteKnowledge.md)
  * [如何绕开反爬虫机制](CSharpCrawler/AvoidAnti-CrawlingMechanisms.md)
  
* 网页抓取原理
  * 使用套接字来获取网页源码

* 法律与道德约束 
  * 爬虫协议
    * [爬虫协议介绍，以及它的语法规则;](https://github.com/zhaotianff/CSharpCrawler/blob/master/CSharpCrawler/RobotsExclusionProtocol.md)
    * C#如何获取网站的爬虫协议;
    * C#中如何解析爬虫协议;
  * [法律相关](CSharpCrawler/CrawlerLaw.md)
  
* 抓取网页
  * HttpWebRequest类的使用
  * HttpClient类的使用
  * 获取指定url的IP地址
  * 获取指定url的网页头信息
  * 如何从网页源码中提取页面的编码
   
* 抓取动态网页
  * 使用[CEFSharp](https://github.com/cefsharp/CefSharp)来抓取动态网页
  * 使用WebBrowser(IE)来抓取动态网页
  * 使用[Selenium](https://github.com/SeleniumHQ/selenium)来抓取动态网页

* WebAPI调用
  * 获取实时天气
    * 调用中国天气网公开API接口来获取天气
      
  * 获取Bing每日图片
    * 调用cn bing API接口来获取Bing每日图片

* 获取网页DOM
  * 使用[HtmlAgilityPack](https://github.com/zzzprojects/html-agility-pack)来获取网页的DOM结构
  
* 使用CSS选择器和XPath选取元素
  * CSS选择器
  * XPath
  
* [正则表达式的使用](https://github.com/zhaotianff/CSharpCrawler/blob/master/CSharpCrawler/%E6%AD%A3%E5%88%99%E8%A1%A8%E8%BE%BE%E5%BC%8F.md)
  * 正则表达式的基础知识和基本使用;
  * 正则表达式中的分组构造;
  * 常用匹配模式;

* Url抓取(当Url太多时，UI会卡)
  * 抓取指定网址的全部链接
  * 通过指定深度，抓取子网页的全部链接
  * 动态网页链接抓取
  * 限定抓取当前页面的子链接
    
* 图片抓取
  * 抓取指定url页面中的图片，通过配置url的页码规则，来进行翻页。
  * 自动获取下一页

* 文件下载
  * 使用WebClient类下载文件
  * 多线程下载文件
  * 从文件加载批量下载
  
* 多线程抓取
  
* 抓包工具使用
  * [Fiddler](https://github.com/zhaotianff/CSharpCrawler/blob/master/CSharpCrawler/AnalysisPacket_Fiddler.md)
	
* 模拟登录并获取登录后的内容
  * 使用Cookie(实现中)
  * [使用Selenium](https://github.com/zhaotianff/CSharpCrawler/blob/master/CSharpCrawler/Selenium.md)(实现中)
	* 说明：示例程序使用的是EdgeDriver，所以需要Windows10系统，如果需要其它浏览器Driver,可自行修改。
    * 测试系统：Windows 10 1703 Edge 15.15063.0，如果Edge驱动版本不一致，需要手动更新至对应的版本。 	

* 必应图片搜索(*仅供交流学习使用，请勿用作商业用途*)
  * 实现必应图片搜索的功能
  * 翻页及优化(待更新)

* 爬虫数据存储
  * Berkeley DB
    * 介绍BerkeleyDB以及使用方式
    
  * SQLite
    * 介绍SQLite以及使用方式
	 
* 小例子-全国家常菜价格统计(*仅供交流学习使用，请勿用作商业用途*)
  * 获取全国城市，以及城市代码
  * 抓取家常菜价格
  * 生成统计图表	  
	  	
* 小例子-通用抓取
  * 电商类网站
  * 新闻类网站(待更新)
  
* 将网页保存为图片/PDF
	
### Roadmap
* Url Encode/Decode 原理
* 抓包工具Charles的使用
* 使用抓包工具分析网站接口
* 使用抓包工具分析APP接口
* 验证码识别(字符验证码，滑块验证码)
* BloomFilter算法
* NLP基础
* 中文分词
* Lucene.net使用
* 优先级队列实现
* 基本爬虫架构
* 分布式爬虫架构
* 抓取豆瓣书评
* 当抓取的数据到非常大的的数量级时该怎么处理
* 使用代理
	
    
### 开发环境
~~Visual Studio 2013 + .Net 4.5<br/>~~
~~Visual Studio 2015 + .Net 4.5.2<br/>~~
Visual Studio 2017 + .Net 4.7.2

**如果没有安装Blend SDK，GAC中没有System.Windows.Interactivity.dll，需要自己引用bin/x64/Debug目录下的System.Windows.Interactivity.dll**

**编译时可能会显示各种库找不到，Nuget还原下包就可以正常编译了**

**Berkeley DB需要引用bin/x64/Debug目录下的libdb_dotnet181.dll，运行时还需要libdb_csharp181.dll和libdb181.dll，已置于bin/x64/Debug目录下**

### 使用的三方组件
* [CefSharp](https://github.com/cefsharp/CefSharp)
* [HtmlAgilityPack](https://github.com/zzzprojects/html-agility-pack)
* [Oracle Berkeley DB](https://www.oracle.com/database/technologies/related/berkeleydb.html)
* [SQLite](https://www.sqlite.org/index.html)
* [Json.NET](https://github.com/JamesNK/Newtonsoft.Json)
* [Selenium](https://github.com/SeleniumHQ/selenium)
* [AngleSharp](https://github.com/AngleSharp/AngleSharp)
* [Puppeteer Sharp](https://github.com/hardkoded/puppeteer-sharp)

### 软件截图
<p align="center">
 <img align="center" alt="start up" src="https://github.com/zhaotianff/CSharpCrawler/blob/master/CSharpCrawler/ScreenShots/1.png" />
</p>


<p align="center">
 <img align="center" alt="start up" src="https://github.com/zhaotianff/CSharpCrawler/blob/master/CSharpCrawler/ScreenShots/2.png" />
</p>


<p align="center">
 <img align="center" alt="start up" src="https://github.com/zhaotianff/CSharpCrawler/blob/master/CSharpCrawler/ScreenShots/3.png" />
</p>


<p align="center">
 <img align="center" alt="file download" src="https://github.com/zhaotianff/CSharpCrawler/blob/master/CSharpCrawler/ScreenShots/4.png" />
</p>


<p align="center">
 <img align="center" alt="file download" src="https://github.com/zhaotianff/CSharpCrawler/blob/master/CSharpCrawler/ScreenShots/5.png" />
</p>


<p align="center">
 <img align="center" alt="file download" src="https://github.com/zhaotianff/CSharpCrawler/blob/master/CSharpCrawler/ScreenShots/6.png" />
</p>

### License

[MIT License](LICENSE).
