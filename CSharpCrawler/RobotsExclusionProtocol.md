# Robots Exclusion Protocol(网络爬虫排除标准),也称为爬虫协议

网站通过Robots协议告诉搜索引擎哪些页面可以抓取，哪些页面不能抓取。
robots协议在网站的具体体现是**robots.txt**

如
https://www.taobao.com/robots.txt
https://www.baidu.com/robots.txt

在写爬虫时，这一点是必须注意的。不能违反爬虫协议，否则后果很严重→_→

**robots.txt**用法如下

* **User-agent**: * 这里的*代表的所有的搜索引擎种类，*是一个通配符
* **Disallow**: /admin/ 这里定义是禁止爬寻admin目录下面的目录
* **Disallow**: /require/ 这里定义是禁止爬寻require目录下面的目录
* **Disallow**: /ABC/ 这里定义是禁止爬寻ABC目录下面的目录
* **Disallow**: /cgi-bin/*.htm 禁止访问/cgi-bin/目录下的所有以".htm"为后缀的URL(包含子目录)。
* **Disallow**: /*?* 禁止访问网站中所有包含问号 (?) 的网址
* **Disallow**: /.jpg$ 禁止抓取网页所有的.jpg格式的图片
* **Disallow**:/ab/adc.html 禁止爬取ab文件夹下面的adc.html文件。
* **Allow**: /cgi-bin/　这里定义是允许爬寻cgi-bin目录下面的目录
* **Allow**: /tmp 这里定义是允许爬寻tmp的整个目录
* **Allow**: .htm$ 仅允许访问以".htm"为后缀的URL。
* **Allow**: .gif$ 允许抓取网页和gif格式图片
* **Sitemap**: 网站地图 告诉爬虫这个页面是网站地图
* #:   注释  


这里以https://www.taobao.com/robots.txt的一部分为例进行讲解  
*User-agent:  Baiduspider
Allow:  /article
Allow:  /oshtml
Allow:  /ershou
Allow: /$
Disallow:  /product/
Disallow:  /*

*User-agent:  Baiduspider*  
搜索引擎：百度蜘蛛

*Allow:  /article*  
允许抓取article目录下面的内容

*Allow:  /oshtml*  
允许抓取oshtml目录下面的内容

*Allow:  /ershou*
允许抓取ershou目录下面的内容

*Allow: /$*
使用 $ 匹配网址的结束字符，这里这句感觉没用啊，根目录下，不带任何字符，那就是没有允许访问任何页面啊

*Disallow:  /product/*
不允许抓取product目录下的内容

*Disallow:  /**
*是通配符，不允许抓取根目录下的所有内容。跟Disallow: /作用其实是一样的



