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
    public class DetailsModel : PageModel
    {
        private readonly WebGraph.Data.WebGraphContext _context;

        public DetailsModel(WebGraph.Data.WebGraphContext context)
        {
            _context = context;
        }

        public Record Record { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Record = await _context.Record.FirstOrDefaultAsync(m => m.ID == id);

            if (Record == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
