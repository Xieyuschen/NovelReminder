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

namespace WebGraph.NovelReminder
{

    class Program
    {
        static async System.Threading.Tasks.Task Main(string[] args)
        {
            string jsonfile = @"C:\Users\DELL\AppData\Roaming\Microsoft\UserSecrets\ee6f7777-9738-4ddc-b287-7868412d3df1\secrets.json";
            StreamReader file = File.OpenText(jsonfile);
            JsonTextReader reader = new JsonTextReader(file);
            JObject o = (JObject)JToken.ReadFrom(reader);
            var emailToken = o["password"].ToString();
            var option = new SmtpClientOptions
            {
                Account = "1743432766@qq.com",
                Token = emailToken
            };
            Reminder reminder = new Reminder(new DatabaseService(),new EmailService(option));
            reminder.AddBooksUrl("http://www.biquge.se/1591/");
            reminder.AddReceiver("2016231075@qq.com");
            await reminder.StartAsync();
        }
    }
}

