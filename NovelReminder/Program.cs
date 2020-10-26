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
        public static void SendMail()
        {
            var smtpClient = new SmtpClient("smtp.163.com")
            {
                //Port = ,
                Credentials = new NetworkCredential("xieyuschen@163.com", "IAEKCLPMNAFLQATX"),
                EnableSsl = true,
                Timeout = 1000,
                DeliveryMethod = SmtpDeliveryMethod.Network
            };
            var mailMessage = new MailMessage
            {
                From = new MailAddress("xieyuschen@163.com","xyc"),
                Subject = "subject",
                Body = "<h1>Hello</h1>",
                IsBodyHtml = true,
                Priority = MailPriority.High
            };
            mailMessage.To.Add("2016231075@qq.com");

            smtpClient.Send(mailMessage);
            
        }
        static async System.Threading.Tasks.Task Main(string[] args)
        {
            string emailToken=File.ReadAllText("../../../../Settings.txt");
            var option = new SmtpClientOptions
            {
                Account = "xieyuschen@163.com",
                Token = emailToken,
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

