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
        //应该将EmailService独立出来，这样用户可以配置自己喜欢的邮件发送方式
        //需要将这里的内容进行重构，用户如何配置Email的发送我只需指定接口即可
        public List<string> Receivers { get; set; }
        public List<string> BookedUrls { get; set; }
        public double Interval { get; set; }
        private IDataManager DataService;
        private IScanner Scanner;
        public IEmailService EmailService { get; set; }
        private Dictionary<int, string> dic;

        //2020.06.02重构
        //接受者和订阅的url不应该放在构造函数里面去，因为均可能有1+个

        
        public Reminder(IDataManager dataManage,IEmailService emailService,IScanner scanner,double interval = 10)
        {
            dic = new Dictionary<int, string>();
            Interval = interval;
            Scanner = scanner;
            EmailService = emailService;
            DataService = dataManage;
            Receivers = new List<string>();
            BookedUrls = new List<string>();
        }
        public void AddReceiver(string emailAddress)
        {
            Receivers.Add(emailAddress);
        }
        public void AddBooksUrl(string url)
        {
            BookedUrls.Add(url);
        }
        public async ValueTask StartAsync()
        {
            var url = BookedUrls.FirstOrDefault();
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
                if(await DataService.GetIsInit(url))
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
                await DataService.InsertOrUpdateOneAsync(url, latestNum);

                //初始化成功，发送最新的一章表示成功订阅
                //获得最新一篇文章的内容
                string articleUrl = url + dic[latestNum];
                //获取最新一期小说的内容和标题
                await SendNovelDetailsAsync(articleUrl, true);
                await DataService.UpdateAsync(url, true);
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
            int numDb =await DataService.GetLastChapterAsync(url);
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
            await DataService.UpdateAsync(url, dic.Keys.Max());
            return true;
        }
        private async ValueTask ReadHtmlAndUpdateDicAsync(string url)
        {

            var ContentInfo = await Scanner.GetHtmlContentAsync(url);
            Regex latestChapter = new Regex("(?<=<a href=\"/\\d*/)(.*?html).*?(?<=第)(\\d*)(?=.*?)");
            var results = latestChapter.Matches(ContentInfo);
            foreach (Match item in results)
            {
                try
                {
                    dic.TryAdd(int.Parse(item.Groups[2].Value), item.Groups[1].Value);

                }
                catch (Exception e)
                {
                    Console.WriteLine("Some error In Dic.add:  " + e.Message);
                }

            }
        }

        
        private async ValueTask SendNovelDetailsAsync(string novalPageUrl,bool IsInitia=false)
        {
            var content = await Scanner.GetHtmlContentAsync(novalPageUrl);
            Regex rcontent = new Regex("(?<=<div\\sid=\"content\">)[\\S\\s]*?(?=</div>)");
            Regex rtitle = new Regex("(?<=<title>).*?(?=_)");
            var articleContent = rcontent.Match(content);
            var articleTitle = rtitle.Match(content);
            EmailSendAsync(articleContent, articleTitle, IsInitia);

        }
        private void EmailSendAsync(Match articleContent,Match articleTitle,bool IsInitia=false)
        {
            
            string initialStr = IsInitia
                ? "<h1>When you receive this email,it means you have booked the NovelUpdateReminder successfully!         </h1>"
                : "";

            EmailService.SendEmail(new MailOptions
            {
                Recievers = Receivers,
                Body = initialStr + articleContent.Value,
                Subject =articleTitle.Value,
            });
        }
    }
}
