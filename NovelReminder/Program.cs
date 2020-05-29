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

namespace NovelReminder
{
    class Program
    {
        static async System.Threading.Tasks.Task Main(string[] args)
        {
            const string url = "http://www.biquge.se/12809/";
            Reminder reminder = new Reminder(new List<string> { "2016231075@qq.com" });
            await reminder.InitializeReminder(url);
            while (true)
            {
                try
                {
                    if(!(await reminder.CheckAnyNewAsync(url)))
                    {
                        await Task.Delay(TimeSpan.FromSeconds(10));
                    }
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
    }
}

