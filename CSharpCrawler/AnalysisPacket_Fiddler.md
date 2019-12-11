# 抓包工具Fiddler使用

## 下载和安装
* 下载   
https://www.telerik.com/download/fiddler   
输入邮箱和选择国家即可下载
* 安装   
直接下一步，没有什么问题。装完是英文界面，官方没有汉化包，网上有汉化的版本，如果需要可以自己去找一下。
* 首次使用配置  
1. 开启HTTPS抓取  
Tools > Fiddler Options > HTTPS > Decrypt HTTPS Traffic box.

<p align="center">
 <img align="center" alt="Decrypt HTTPS" src="https://github.com/zhaotianff/CSharpCrawler/blob/master/CSharpCrawler/doc/DecryptHTTPSTrafficOption.png" />
</p>

2. 开启Win8+系统WinRT(UWP)应用抓取  

<p align="center">
 <img align="center" alt="Win8Config" src="https://github.com/zhaotianff/CSharpCrawler/blob/master/CSharpCrawler/doc/Win8Config.png" />
</p>

<p align="center">
 <img align="center" alt="Win8Config" src="https://github.com/zhaotianff/CSharpCrawler/blob/master/CSharpCrawler/doc/Win8Config_2.png" />
</p>

<p align="center">
 <img align="center" alt="Win8Config" src="https://github.com/zhaotianff/CSharpCrawler/blob/master/CSharpCrawler/doc/Win8Config_3.png" />
</p>

## 使用
* 开始和停止抓取  
  软件启动后，会自动启动抓取。如果需要停止抓取，可以单击状态栏上的Capturing  
<p align="center">
 <img align="center" alt="Stop Capturing" src="https://github.com/zhaotianff/CSharpCrawler/blob/master/CSharpCrawler/doc/StopCapturing.png" />
</p>

* 指定进程抓取  
在状态栏上单击**All Processes**，可以筛选要抓取的程序。可选项为全部进程(All Processes)，只抓取浏览器(Web Browsers)，只抓取非浏览器(Non-Browser)，隐藏所有(Hide-All)  
![Filter Process](https://github.com/zhaotianff/CSharpCrawler/blob/master/CSharpCrawler/doc/FilterProcess.png)  
在工具栏单击**Any Processes**，再将光标拖动到需要抓取的程序窗体，可以对指定程序进行抓取。我这里选取了360极速浏览器，可以看到整个浏览器窗体会变色，代表选中。  
![Select Process](https://github.com/zhaotianff/CSharpCrawler/blob/master/CSharpCrawler/doc/SelectProcess.png)  
  
* 查看会话列表  
运行fiddler，再打开浏览器。可以在左侧列表中看到会话情况
<p align="center">
 <img align="center" alt="Web Traffic" src="https://github.com/zhaotianff/CSharpCrawler/blob/master/CSharpCrawler/doc/WebTraffic.png" />
</p>

* 查看会话详情  
双击左边列表，可以在右侧看到会话详情。  
点击Statistics，查看会话统计。点击Inspectors，可以查看会话的详细。点击TimeLine，可以查看时间线。
<p align="center">
 <img align="center" alt="View Web Traffic Statistics" src="https://github.com/zhaotianff/CSharpCrawler/blob/master/CSharpCrawler/doc/WebTrafficStatistics.png" />
</p>
