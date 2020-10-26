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
using System.Net.Mail;
using System.Net;

namespace Try
{

    public class Program
    {
        static async System.Threading.Tasks.Task Main(string[] args)
        {
            var option = new SmtpClientOptions
            {
                Account = "xieyuschen@163.com",
                Token = "IAEKCLPMNAFLQATX",
                Host = "smtp.163.com",
                Port = 465,
                EnableSsl = false
            };
            var em = new EmailService(option);

            Reminder reminder = new Reminder(new DbFileServices(), em, new Scanner());
            reminder.AddBooksUrl("http://www.biquge.se/23609/");
            reminder.AddReceiver("2016231075@qq.com");
            await reminder.StartAsync();
        }
    }
}

