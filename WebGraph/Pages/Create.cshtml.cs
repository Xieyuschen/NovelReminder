using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using WebGraph.Models;

namespace WebGraph.Pages
{
    public class CreateModel : PageModel
    {
        [BindProperty]
        public Record Record { get; set; }
        public void OnGet()
        {

        }
        public void OnPost()
        {
            FileStream aFile = new FileStream("wwwroot/DataFile.txt", FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
            StreamWriter writer = new StreamWriter(aFile);
            var str = Record.ToString();
            writer.WriteLine(str);
        }
    }
}