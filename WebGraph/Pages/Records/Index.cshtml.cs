using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebGraph.Data;
using WebGraph.Models;

namespace WebGraph.Pages.Records
{
    public class IndexModel : PageModel
    {
        private readonly WebGraph.Data.WebGraphContext _context;

        public IndexModel(WebGraph.Data.WebGraphContext context)
        {
            _context = context;
        }

        public IList<Record> Record { get;set; }

        public async Task OnGetAsync()
        {
            Record = await _context.Record.ToListAsync();
        }
    }
}
