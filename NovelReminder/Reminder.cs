using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace NovelReminder
{
    class Reminder
    {
        public IEnumerable<string> Receivers { get; set; }
        public double Interval { get; set; }

        private IEnumerable<string> BookedUrl;
        private DatabaseService dbService;
        private Scanner scanner;
        private string EmailToken;
        private string Sender;
        private Dictionary<int, string> dic;
        public Reminder(IEnumerable<string> urls,IEnumerable<string> receivers, string sender, string emailToken,double scanInterval=10)
        {
            Interval = scanInterval;
            BookedUrl = urls;
            dbService = new DatabaseService();
            scanner = new Scanner();
            if (sender == null || emailToken == null)
                throw new Exception("Sender or Token cannot be null!");
            Sender = sender;
            EmailToken = emailToken;
            dic = new Dictionary<int, string>();
            Receivers = receivers;
        }
        public Reminder(string url,string receivers, string sender, string emailToken, double scanInterval = 10)
        {
            Interval = scanInterval;
            BookedUrl = new List<string>{ url };
            dbService = new DatabaseService();
            scanner = new Scanner();
            if (sender == null || emailToken == null)
                throw new Exception("Sender or Token cannot be null!");
            Sender = sender;
            EmailToken = emailToken;
            dic = new Dictionary<int, string>();
            Receivers =new List<string> { receivers };
        }

        public async ValueTask StartAsync()
        {
            var url = BookedUrl.FirstOrDefault();
            await InitializeReminderAsync(url);
            int i = 1;
            while (true)
            {
                try
                {
                    if (!(await CheckAnyNewAsync(url)))
                    {
                        Console.WriteLine($"Scan {i++} times");
                        await Task.Delay(TimeSpan.FromSeconds(Interval));
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("DetectRecycleErrorMessage:  " + e.Message);
                }
            }
        }
        
        public async ValueTask InitializeReminderAsync(string url)
        {
            try
            {
                if(await dbService.GetIsInit(url))
                {
                    Console.WriteLine("The program is running successfully!");
                    //MessageBox.Show();
                }
            }
            catch(Exception e)
            {
                await ReadHtmlAndUpdateDicAsync(url);
                //初始化时将当前信息加入到数据库中去
                var latestNum = dic.Keys.Max();
                await dbService.InsertOrUpdateOneAsync(url, latestNum);

                //初始化成功，发送最新的一章表示成功订阅
                //获得最新一篇文章的内容
                string articleUrl = url + dic[latestNum];
                //获取最新一期小说的内容和标题
                await SendNovelDetailsAsync(articleUrl, true);
                await dbService.UpdateAsync(url, true);
            }
        }
        /// <summary>
        /// return false if there are nothing new
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public async ValueTask<bool> CheckAnyNewAsync(string url)
        {
            await ReadHtmlAndUpdateDicAsync(url);
            int numDb =await dbService.GetLastChapterAsync(url);
            if (numDb == dic.Keys.Max())
            {
                return false;
            }
            int i = 0;
            for (i = numDb+1; i <= dic.Keys.Max(); i++)
            {
                await SendNovelDetailsAsync(url + dic[i]);
            }
            Console.WriteLine(i-1);
            await dbService.UpdateAsync(url, dic.Keys.Max());
            return true;
        }
        private async ValueTask ReadHtmlAndUpdateDicAsync(string url)
        {
            var ContentInfo = await scanner.GetArticleAsync(url);
            Regex latestChapter = new Regex("(?<=<a href=\"/\\d*/)(.*?html).*?(?<=第)(\\d*)(?=.*?)");
            var results= latestChapter.Matches(ContentInfo);
            foreach(Match item in results)
            {
                dic.TryAdd(int.Parse(item.Groups[2].Value), item.Groups[1].Value);
            }
        }
        private async ValueTask SendNovelDetailsAsync(string novalPageUrl,bool IsInitia=false)
        {
            var content = await scanner.GetArticleAsync(novalPageUrl);
            Regex rcontent = new Regex("(?<=<div\\sid=\"content\">)[\\S\\s]*?(?=</div>)");
            Regex rtitle = new Regex("(?<=<title>).*?(?=_)");
            var articleContent = rcontent.Match(content);
            var articleTitle = rtitle.Match(content);
            EmailSendAsync(articleContent, articleTitle, IsInitia);

        }
        private void EmailSendAsync(Match articleContent,Match articleTitle,bool IsInitia=false)
        {
            EmailService email = new EmailService(new SmtpClientOptions
            {
                Account = Sender,
                Token = EmailToken,
            });
            string initialStr = IsInitia
                ? "<h1>When you receive this email,it means you have booked the NovelUpdateReminder successfully!         </h1>"
                : "";

            email.SendEmail(new MailOptions
            {
                Recievers = Receivers,
                Body = initialStr + articleContent.Value,
                Subject =articleTitle.Value,
                From = Sender
            });
        }
    }
}
