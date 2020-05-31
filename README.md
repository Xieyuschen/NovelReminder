# 本仓库的目的
看别人创作的作品是一种享受，但是看小说最烦的一点是小说作者更新的速度与我阅读速度严重不匹配，如果碰上不定期的拖更令人心烦了。不停的打开浏览器，打开界面，刷新，关掉手机。平白无故耗费时间。本应用的创建目的就是检索用户的阅读进度，并在小说更新时自动发送邮件到指定邮箱中。此方法既可以节约时间，又可以避免页面上的一大堆垃圾广告,提高阅读体验 :)

## 本仓库将会不断更新
# 2020.5.31 v.1.1更新：
将服务封装为nuget进行发布，若使用NovelReminder服务仅需完成一下几步配置即可轻松使用（其实还是有点麻烦，大量耦合还需要进行下面的麻烦配置，之后会慢慢升级)。  
1. 创建数据库：
在vs中点击`view->Sql Server Object Exploror`.然后在新弹出的窗口处点击`Sql server`的三角，选择数据库点击三角，展开后的文件夹的Database数据库右键点击`Add New Database`->将Database Name 设置为`NovalReminderDatabase`.之后右键点击新建的数据库，选择属性，在其中找到连接字符串，复制下来。 
2. 配指相关文件
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
使用qq的Smtp服务器进行邮件发送，[QQ邮箱开启Smtp](https://jingyan.baidu.com/article/6079ad0eb14aaa28fe86db5a.html)。
3. 调用服务：
首先添加nuget包`NovelReminder`，使用`using NovelReminder`指令。
```cs
static async System.Threading.Tasks.Task Main(string[] args)
{
    Reminder reminder = new Reminder("http://www.biquge.se/12809/", "收件人", "Smtp邮箱账号","Smtp邮件授权码");
    await reminder.StartAsync();
}
```
运行生成exe之后，可以设置为后台运行。这样只要电脑打开联网就可以定期扫描指定小说。

## 注：
暂时不支持多小说查询，该功能会很快被添加，敬请期待。小说网站暂时只支持[笔趣阁](http://www.biquge.se/).