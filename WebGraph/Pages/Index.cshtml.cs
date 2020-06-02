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

        public void OnGet()
        {

        }
    }
}
