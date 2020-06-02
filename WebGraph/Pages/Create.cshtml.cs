using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using WebGraph.Models;
using System.Text.Json;
namespace WebGraph.Pages
{
    public class CreateModel : PageModel
    {
        [BindProperty]
        public Record Record { get; set; }
        public void OnGet()
        {

        }
        public async Task<IActionResult> OnPostAsync()
        {
            //写入时要先把所有的读出来，再一行行的读进去避免把原有的数据给覆盖掉
            FileStream fStream = new FileStream("wwwroot/DataFile.txt", FileMode.OpenOrCreate);
            StreamReader reader = new StreamReader(fStream);
            string contents = reader.ReadToEnd();
            reader.Close();

            FileStream fhelp = new FileStream("wwwroot/DataFile.txt", FileMode.OpenOrCreate,FileAccess.Write);
            StreamWriter writer = new StreamWriter(fhelp);
            var str = JsonSerializer.Serialize(Record);
            writer.Write(contents);
            await writer.WriteLineAsync(str);
            writer.Close();
            fStream.Close();
            return RedirectToPage("./Index");
        }
    }
}