# 本仓库的目的
看别人创作的作品是一种享受，但是看小说最烦的一点是小说作者更新的速度与我阅读速度严重不匹配，如果碰上不定期的拖更令人心烦了。不停的打开浏览器，打开界面，刷新，关掉手机。平白无故耗费时间。本应用的创建目的就是检索用户的阅读进度，并在小说更新时自动发送邮件到指定邮箱中。此方法既可以节约时间，又可以避免页面上的一大堆垃圾广告,提高阅读体验 :)

## 本仓库将会不断更新
# 2020.5.20第一版使用说明：
本版功能较为简单，支持对一个小说的更新推送。暂时只支持开源(/doge)小说网站[笔趣阁](http://www.biquge.se/)里面的小说。
## 如何使用？
1. 使用vs从github上clone下来。
2. 创建数据库：
在vs中点击`view->Sql Server Object Exploror`.然后在新弹出的窗口处点击`Sql server`的三角，选择数据库点击三角，展开后的文件夹的Database数据库右键点击`Add New Database`->将Database Name 设置为`NovalReminderDatabase`.之后右键点击新建的数据库，选择属性，在其中找到连接字符串，复制下来。 

3. 配指相关文件
- 添加`app.config`文件，需要在本文中放置数据库的连接字符串。  
```xml
<?xml version='1.0' encoding='utf-8'?>  
<configuration>  
<connectionStrings>  
    <clear />  
    <!--这里name指本地数据库的名称,不推荐修改，如果修改请修改DatabaseService.cs文件19行的字符参数为所设置的名字-->
    <add name="Database"
    providerName="System.Data.ProviderName"
    connectionString="输入创建数据库后得到的连接字符串" />  
</connectionStrings>  
</configuration>
```
- 使用Smtp发送邮件  
在这里我们使用qq的Smtp服务器进行邮件发送，[QQ邮箱开启Smtp](https://jingyan.baidu.com/article/6079ad0eb14aaa28fe86db5a.html)。之后在`Reminder.cs`中把`EmailToken`的值设为所得的授权码。

4. 指定要进行更新推送的小说url  
[点击这里搜索想要的小说](http://www.biquge.se/).  
输入小说名称获取对应的小说之后，将`Program.cs`程序中url的值设置为该小说的url。在下一行的list中输入接受小说的邮箱地址，最终运行该程序即可在小说更新时第一时间给您发送包含文章内容的邮件，方便快捷。