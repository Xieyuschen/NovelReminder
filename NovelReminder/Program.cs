using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
namespace NovelReminder
{
    class Program
    {
        static async System.Threading.Tasks.Task Main(string[] args)
        {
            DatabaseService dbService = new DatabaseService();
            var scanner = new Scanner();
            string jsonfile = @"C:\Users\DELL\AppData\Roaming\Microsoft\UserSecrets\ee6f7777-9738-4ddc-b287-7868412d3df1\secrets.json";
            StreamReader file = File.OpenText(jsonfile);
            JsonTextReader reader = new JsonTextReader(file);
            JObject o = (JObject)JToken.ReadFrom(reader);
            var value = o["password"].ToString();
            const string url = "http://www.biquge.se/12809/";
            //初始化时将当前信息加入到数据库中去

            var subject1 = await scanner.GetArticleAsync(url);
            var subject = subject1;
            Console.WriteLine(Encoding.UTF8.GetString(Encoding.Default.GetBytes(subject1)));
            Regex r1 = new Regex("(?<=第)\\d*");
            var co1 = r1.Matches(subject1);
            var index = co1.LastOrDefault();
            await dbService.InsertOrUpdateOneAsync(url, int.Parse(index.Value));

            //初始化成功，发送最新的一章表示成功订阅
            //筛选最新一篇文章的路径
            Regex r = new Regex($"(?<=<a href=\"/\\d*/).*?html(?=.*?{index.Value})");
            var co = r.Matches(subject);
            //获得最新一篇文章的内容
            string articleUrl = url + co.LastOrDefault().Value;
            var content = await scanner.GetArticleAsync(articleUrl);


            Regex rcontent = new Regex("(?<=<div\\sid=\"content\">)[\\S\\s]*?(?=</div>)");
            Regex rtitle = new Regex("(?<=<title>).*?(?=_)");
            var articleContent = rcontent.Match(content);
            var articleTitle = rtitle.Match(content);
            Console.WriteLine(articleContent);


            EmailService email = new EmailService(new SmtpClientOptions
            {
                Account = "1743432766@qq.com",
                Token = value,
            });
            email.SendEmail(new MailOptions
            {
                Recievers = new List<string> { "2016231075@qq.com" },
                Body = articleContent.Value,
                Subject = articleTitle.Value,
                From = "1743432766@qq.com"
            });

        }

    }


}

