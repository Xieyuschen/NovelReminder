using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace WebGraph.NovelReminder
{
    public class MailOptions
    {

        public IEnumerable<string> Recievers { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public Encoding SubjectEncode { get; set; } = Encoding.UTF8;
        public Encoding BodyEncode { get; set; } = Encoding.UTF8;
        public string SenderName { get; set; }
    }
    public class SmtpClientOptions
    {
        public string Host { get; set; } = "smtp.qq.com";
        public int Port { get; set; } = 587;
        public bool EnableSsl { get; set; } = true;
        public string Account { get; set; }
        public string Token { get; set; }
    }

    public interface IEmailService
    {
        //account refers where the email from
        string account { get; set; }
        void SendEmail(MailOptions options);
    }
    public class EmailService : IEmailService
    {
        private SmtpClient client;
        public string account { get; set; }
        public EmailService(SmtpClientOptions options)
        {
            client = new SmtpClient();
            client.Host = options.Host;
            client.Port = options.Port;
            client.EnableSsl = options.EnableSsl;
            client.Credentials = new NetworkCredential(options.Account, options.Token);
            account = options.Account;
        }
        public void SendEmail(MailOptions options)
        {
            var msg = new MailMessage();
            foreach (var item in options.Recievers)
            {
                msg.To.Add(item);
            }
            msg.Subject = options.Subject;
            msg.Body = options.Body;
            msg.BodyEncoding = options.BodyEncode;
            msg.SubjectEncoding = options.SubjectEncode;
            msg.From = new MailAddress(account,options.SenderName);
            msg.IsBodyHtml = true;
            client.Send(msg);
        }
    }
}
