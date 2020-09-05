using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Mail;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace NovelReminder
{
    //==============================================================
    //Reminder 要做以下几件事：
    //
    //1. 处理小说的目录列表
    //2. 检索小说是否更新
    //3. 当检查到小说更新的时候，给用户发送邮件
    //
    //===============================================================

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
        public async ValueTask InitializeReminderAsync(string url)
        {
            try
            {
                if (await DataService.GetIsInit(url))
                {
                    Console.WriteLine("The program is running successfully!");
                    //MessageBox.Show();
                }
                else
                {
                    await DataService.UpdateAsync(url, true);
                    Console.WriteLine("Initialize the new novel and the reminder is running successfully!");
                }
            }
            catch (Exception e)
            {
                await UpdateDicByCatalogHtmlAsync(url);
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
                    //如果监测到目前有不是最新的小说，会在CheckAnyNewAsync中进行发送邮件更新数据库的操作
                    //有一些对不住
                    if (!(await CheckAnyNewAsync(url)))
                    {
                        Console.WriteLine($"Scan {i++} times");
                        await Task.Delay(TimeSpan.FromSeconds(Interval));
                    }
                }
                catch (SmtpException e)
                {
                    Console.WriteLine("DetectRecycleErrorMessage:  " + e.Message+e.StatusCode);
                }
            }
        }        
        /// <summary>
        /// return false if there are nothing new
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public async ValueTask<bool> CheckAnyNewAsync(string url)
        {
            await UpdateDicByCatalogHtmlAsync(url);
            int numDb =await DataService.GetLastChapterAsync(url);

            //Dic中存储最新信息，numDb存储上次发送过邮件的信息。
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

        //9.4重构ReadHtmlAndUpdateDicAsync方法，部分工作由Scanner的解析框架完成
        private async ValueTask UpdateDicByCatalogHtmlAsync(string url)
        {

            var ContentInfos = await Scanner.GetCatalogAsync(url);
            Regex latestChapter = new Regex("(?<=<a href=\"/\\d*/)(.*?html).*?(?<=>)(\\d*)(?=、.*?)");

            
            //<a href="/23609/72550175.html">735、青春、理想和现实</a>
            var results = latestChapter.Matches(ContentInfos.First());
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

        public async ValueTask SenderForTesting()
        {
            await SendNovelDetailsAsync("http://www.biquge.se/23609/75704410.html");
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
