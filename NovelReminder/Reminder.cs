using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NovelReminder
{
    class Reminder
    {
        private DatabaseService dbService;
        private Scanner scanner;
        private string EmailToken;
        public IEnumerable<string> receivers { get; set; }
        private Dictionary<int,string> dic;
        public Reminder()
        {
            dbService = new DatabaseService();
            scanner = new Scanner();
            string jsonfile = @"C:\Users\DELL\AppData\Roaming\Microsoft\UserSecrets\ee6f7777-9738-4ddc-b287-7868412d3df1\secrets.json";
            StreamReader file = File.OpenText(jsonfile);
            JsonTextReader reader = new JsonTextReader(file);
            JObject o = (JObject)JToken.ReadFrom(reader);
            EmailToken = o["password"].ToString();
            dic = new Dictionary<int, string>();
        }
        public async ValueTask ReadHtmlAsync(string url)
        {
            var ContentInfo = await scanner.GetArticleAsync(url);
            Regex latestChapter = new Regex("(?<=<a href=\"/\\d*/)(.*?html).*?(?<=第)(\\d*)(?=.*?)");
            var results= latestChapter.Matches(ContentInfo);
            foreach(Match item in results)
            {
                dic.TryAdd(int.Parse(item.Groups[2].Value), item.Groups[1].Value);
            }
        }

        public async ValueTask SendNovalDetailsAsync(string novalPageUrl,bool IsInitia=false)
        {
            var content = await scanner.GetArticleAsync(novalPageUrl);
            Regex rcontent = new Regex("(?<=<div\\sid=\"content\">)[\\S\\s]*?(?=</div>)");
            Regex rtitle = new Regex("(?<=<title>).*?(?=_)");
            var articleContent = rcontent.Match(content);
            var articleTitle = rtitle.Match(content);
            EmailSendAsync(articleContent, articleTitle, IsInitia);

        }
        public async ValueTask InitializeReminder(string url)
        {

            //初始化时将当前信息加入到数据库中去
            var latestNum = dic.Keys.Max();
            await dbService.InsertOrUpdateOneAsync(url, latestNum);

            //初始化成功，发送最新的一章表示成功订阅
            //获得最新一篇文章的内容
            string articleUrl = url + dic[latestNum];

            //获取最新一期小说的内容和标题
            await SendNovalDetailsAsync(articleUrl, true);

        }
        public async ValueTask AvailableOrNotAsync(string url)
        {

        }

        private void EmailSendAsync(Match articleContent,Match articleTitle,bool IsInitia=false)
        {
            EmailService email = new EmailService(new SmtpClientOptions
            {
                Account = "1743432766@qq.com",
                Token = EmailToken,
            });
            string initialStr = IsInitia
                ? "<h1>When you receive this email,it means your have booked the NovalUpdateRemind successfully!</h1>"
                : "";

            email.SendEmail(new MailOptions
            {
                Recievers = receivers,
                Body = articleContent.Value,
                Subject =initialStr + articleTitle.Value,
                From = "1743432766@qq.com"
            });
        }
    }
}
