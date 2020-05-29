using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;

namespace NovelReminder
{
    class Program
    {
        static async System.Threading.Tasks.Task Main(string[] args)
        {
            DatabaseService service = new DatabaseService();
            await service.InsertOrUpdateOneAsync("/w123456", 12);
            var re = service.GetLastChapter("w/123456");
            Console.WriteLine(re);
            //const string url = @"http://www.biquge.se/12809/";
            //var scanner = new Scanner();
            //string jsonfile = @"C:\Users\DELL\AppData\Roaming\Microsoft\UserSecrets\ee6f7777-9738-4ddc-b287-7868412d3df1\secrets.json";
            //StreamReader file = File.OpenText(jsonfile);
            //JsonTextReader reader = new JsonTextReader(file);

            //JObject o = (JObject)JToken.ReadFrom(reader);
            //var value = o["password"].ToString();
            //if (await scanner.ScanNovalUpdated(url))
            //{

            //    using (var client = new HttpClient())
            //    {
            //        var subject = await scanner.GetArticleAsync(url);
            //        //直接查询第一条即可
            //        Regex r = new Regex("(?<=<a href=\"/\\d*/).*?html(?=\">)");
            //        var co = r.Matches(subject);
            //        //foreach(var item in co)
            //        //{
            //        //    Console.WriteLine(item);
            //        //}
            //        string articleUrl = url + co.LastOrDefault().Value;
            //        var content = await scanner.GetArticleAsync(articleUrl);
            //        //Console.WriteLine(content);
            //        Regex rcontent = new Regex("(?<=<div\\sid=\"content\">)[\\S\\s]*?(?=</div>)");
            //        Regex rtitle = new Regex("(?<=<title>).*?(?=_)");
            //        var articleContent = rcontent.Match(content);
            //        var articleTitle = rtitle.Match(content);
            //        //Console.WriteLine(articleContent);
            //        EmailService email = new EmailService(new SmtpClientOptions
            //        {
            //            Account = "1743432766@qq.com",
            //            Token = value,
            //        });

            //        email.SendEmail(new MailOptions
            //        {
            //            Recievers = new List<string> { "xxxx@qq.com" },
            //            Body = articleContent.Value,
            //            Subject = articleTitle.Value,
            //            From = "xxxx@qq.com"
            //        });

            //    }
            //}
        }
    }
}
