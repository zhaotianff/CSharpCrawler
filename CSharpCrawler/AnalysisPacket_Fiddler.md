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
* 查看会话列表  
运行fiddler，再打开浏览器。可以在左侧列表中看到会话情况
<p align="center">
 <img align="center" alt="Web Traffic" src="https://github.com/zhaotianff/CSharpCrawler/blob/master/CSharpCrawler/doc/WebTraffic.png" />
</p>

* 查看会话详情  
双击左边列表，可以在右侧看到会话详情。  
点击Statistics，查看会话统计
<p align="center">
 <img align="center" alt="View Web Traffic Statistics" src="https://github.com/zhaotianff/CSharpCrawler/blob/master/CSharpCrawler/doc/WebTrafficStatistics.png" />
</p>
