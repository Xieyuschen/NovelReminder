using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using WebGraph.Models;
using System.Text.Json;
using WebGraph.NovelReminder;

namespace WebGraph.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        public List<Record> Records;
        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
            Records = new List<Record>();
            FileStream fStream = new FileStream("wwwroot/DataFile.txt", FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            StreamReader reader = new StreamReader(fStream);
            while (!reader.EndOfStream)
            {
                var str = reader.ReadLine();
                Records.Add(JsonSerializer.Deserialize<Record>(str));
            }
        }

        public async ValueTask OnGet()
        {
            Reminder reminder = new Reminder("http://www.biquge.se/12809/", "2016231075@qq.com", "1743432766@qq.com", "1234");
            await reminder.StartAsync();
        }
    }
}
