using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace NovelReminder
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
        public string Host { get; set; } = "smtp.163.com";
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
            client.EnableSsl = true;// options.EnableSsl;

            client.UseDefaultCredentials = true;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            account = options.Account;
            client.Host = options.Host;
            client.Port = options.Port;

            //client.Credentials = new NetworkCredential(options.Account, options.Token);
            client.Credentials = new NetworkCredential("xieyuschen@163.com", options.Token);

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
            try
            {
                client.Send(msg);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.GetType());
            }
        }
    }
}
