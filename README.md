# C\#爬虫项目(更新中)
### 关于项目
C#开发爬虫的知识总结，目前还在更新中。这并不是一个完整的爬虫程序，只是一些示例。

### 功能介绍
* 网页抓取原理
<p>使用套接字来获取网页源码</p>

* 爬虫协议
<p>爬虫协议介绍，以及它的语法规则;</p>
<p>C#如何获取网站的爬虫协议;</p>
<p>C#中如何解析爬虫协议</p>

* 正则表达式的使用
<p>正则表达式的基础知识和基本使用；</p>
<p>正则表达式中的分组构造；</p>
<p>常用匹配模式</p>

* WebAPI调用
    * 获取实时天气
      <p>调用中国天气网公开API接口来获取天气</p>
      
    * 获取Bing每日图片
      <p>调用cn bing API接口来获取Bing每日图片</p>
      
* 抓取网页
<p>HttpWebRequest类的使用</p>
<p>获取指定url的IP地址</p>
<p>获取指定url的网页头信息</p>
<p>如何从网页源码中提取页面的编码</p>

* 抓取动态网页
<p>使用[CEFSharp](https://github.com/cefsharp/CefSharp)来抓取动态网页</p>

* 获取网页DOM
<p>使用[HtmlAgilityPack](https://github.com/zzzprojects/html-agility-pack)来获取网页的DOM结构</p>

* Url抓取(待更新)
    * 抓取指定网址的全部链接
    * 通过指定深度，抓取子网页的全部链接
    * 动态网页链接抓取
    * 链接可用性判断
    * 链接标题抓取
    
* 图片抓取(更新中)
<p>抓取指定url页面中的图片，通过配置url的页码规则，来进行翻页。
<p>自动获取下一页</p>

* 文件下载
<p>使用WebClient类下载文件</p>
<p>多线程下载文件</p>
<p>从文件加载批量下载(待更新)</p>

* 必应图片搜索(仅供学习，请勿用途其它用途)
<p>实现必应图片搜索的功能</p>
<p>翻页及优化(待更新)</p>

* 爬虫数据存储
    * Berkeley DB
    <p>介绍BerkeleyDB以及使用方式</p>
    
    * SQLite
    <p>介绍SQLite以及使用方式</p>
    
### 开发环境
Visual Studio 2013 + .Net 4.5<br/>

**如果没有安装Blend，GAC中没有System.Windows.Interactivity.dll，需要自己引用bin/x64/Debug目录下的System.Windows.Interactivity.dll**

**编译时可能会显示各种库找不到，Nuget还原下包就可以正常编译了**

**Berkeley DB需要引用bin/x64/Debug目录下的libdb_dotnet181.dll，运行时还需要libdb_csharp181.dll和libdb181.dll，已置于bin/x64/Debug目录下**

### 使用的三方组件
* [CefSharp](https://github.com/cefsharp/CefSharp)
* [HtmlAgilityPack](https://github.com/zzzprojects/html-agility-pack)
* [Oracle Berkeley DB](https://www.oracle.com/database/technologies/related/berkeleydb.html)
* [SQLite](https://www.sqlite.org/index.html)

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


### License

[MIT License](LICENSE).
