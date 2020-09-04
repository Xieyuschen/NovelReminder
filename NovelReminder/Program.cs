using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using NovelReminder;
using HtmlAgilityPack;
namespace Try
{

    class Program
    {
        static async System.Threading.Tasks.Task Main(string[] args)
        {
            //string jsonfile = @"C:\Users\DELL\AppData\Roaming\Microsoft\UserSecrets\ee6f7777-9738-4ddc-b287-7868412d3df1\secrets.json";
            //StreamReader file = File.OpenText(jsonfile);
            //JsonTextReader reader = new JsonTextReader(file);
            //JObject o = (JObject)JToken.ReadFrom(reader);
            //var emailToken = o["password"].ToString();
            //var option = new SmtpClientOptions
            //{
            //    Account = "1743432766@qq.com",
            //    Token = emailToken
            //};
            //var em = new EmailService(option);
            //Reminder reminder = new Reminder(new DbFileServices(), em, new Scanner());
            //reminder.AddBooksUrl("http://www.biquge.se/23609/");
            //reminder.AddReceiver("2016231075@qq.com");
            //await reminder.StartAsync();

            Scanner scanner = new Scanner();
            var ContentInfos = await scanner.GetCatalogAsync("http://www.biquge.se/23609/");
            Regex latestChapter = new Regex("(?<=<a href=\"/\\d*/)(.*?html).*?(?<=>)(\\d*)(?=、.*?)");


            //<a href="/23609/72550175.html">735、青春、理想和现实</a>
            var results = latestChapter.Matches(ContentInfos.First());
            foreach (Match item in results)
            {

            }
        }
    }
}

