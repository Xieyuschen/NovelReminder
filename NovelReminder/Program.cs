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
            
            string emailToken=File.ReadAllText("Settings.txt");
            var option = new SmtpClientOptions
            {
                Account = "1743432766@qq.com",
                Token = emailToken
            };
            var em = new EmailService(option);
            Reminder reminder = new Reminder(new DbFileServices(), em, new Scanner());
            reminder.AddBooksUrl("http://www.biquge.se/23609/");
            reminder.AddReceiver("2016231075@qq.com");
            await reminder.StartAsync();

        }
    }
}

