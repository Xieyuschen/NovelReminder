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
using System.Net;
using System.Diagnostics;
using System.IO.Compression;
using HtmlAgilityPack;

namespace Try
{

    class Program
    {
        public class FeedSpecific
        {
            public int trend { get; set; }
            public int score { get; set; }
            public bool debut { get; set; }
            public int answerCount { get; set; }
        }
        public class Target
        {

        }


        public static string cookieString = " _xsrf=Q3P5AgNBNCbhjnzkLnEal1PjACw4Pw9a; d_c0=\"AFAitLALmA6PToKBxJalOxDadfLX78KZNK8 =| 1543501780\"; _zap=62f63c0b-b226-4813-83e7-68aa928a8ad8; _ga=GA1.2.1105581317.1573707192; z_c0=\"2 | 1:0 | 10:1590594054 | 4:z_c0 | 92:Mi4xbzFCNENnQUFBQUFBVUNLMHNBdVlEaVlBQUFCZ0FsVk5CdGk3WHdBRmFGX3lLd3cxLUFtTkhuT2ZTNE5MWnA3UmN3 | 8c18b4517b60111cd4f8bad74eff33fbb6e4d79b4ffe3740e9785036c354813c\"; q_c1=abeb677c895644ffb33f8134bbe6c36c|1593224476000|1543501782000; _gid=GA1.2.1292232640.1595151051; __utma=51854390.1105581317.1573707192.1595259397.1595259397.1; __utmz=51854390.1595259397.1.1.utmcsr=(direct)|utmccn=(direct)|utmcmd=(none); __utmv=51854390.100--|2=registration_date=20180622=1^3=entry_date=20180622=1; Hm_lvt_98beee57fd2ef70ccdd5ca52b9740c49=1595232157,1595259300,1595337557,1595419915; SESSIONID=EjhdSmUW0P4wu6szbm5WjJLSUS4GWedkWmML3d5dAoA; JOID=VVgVBEqQAJ_vHnmXNpDRxUiekBsg0mrwjUsTwmamV6ufbUnjTCZNhrAffpA34otgyQm26ZVc5J-hmtVTWLFVJmU=; osd=VF8RA0iRB5voHHiQMpfTxE-alxkh1W73j0oUxmGkVqybakviSyJKhLEYepc144xkzgu37pFb5p6mntJRWbZRIWc=; Hm_lpvt_98beee57fd2ef70ccdd5ca52b9740c49=1595427510; tst=h; tshl=digital; KLBRSID=ed2ad9934af8a1f80db52dcb08d13344|1595428187|1595424017";
        static readonly HttpClient client = new HttpClient();
        static async System.Threading.Tasks.Task Main(string[] args)
        {
            var request = (HttpWebRequest)WebRequest.Create("https://www.zhihu.com/hot");
            request.CookieContainer = new CookieContainer();
            using (var response = (HttpWebResponse)request.GetResponse())
            {
                // Print the properties of each cookie.
                foreach (Cookie cook in response.Cookies)
                {
                    Console.WriteLine("Cookie:");
                    Console.WriteLine($"{cook.Name} = {cook.Value}");
                    Console.WriteLine($"Domain: {cook.Domain}");
                    Console.WriteLine($"Path: {cook.Path}");
                    Console.WriteLine($"Port: {cook.Port}");
                    Console.WriteLine($"Secure: {cook.Secure}");

                    Console.WriteLine($"When issued: {cook.TimeStamp}");
                    Console.WriteLine($"Expires: {cook.Expires} (expired? {cook.Expired})");
                    Console.WriteLine($"Don't save: {cook.Discard}");
                    Console.WriteLine($"Comment: {cook.Comment}");
                    Console.WriteLine($"Uri for comments: {cook.CommentUri}");
                    Console.WriteLine($"Version: RFC {(cook.Version == 1 ? 2109 : 2965)}");
                    // Show the string representation of the cookie.
                    Console.WriteLine($"String: {cook}");
                }
            }
        }
    }
}

