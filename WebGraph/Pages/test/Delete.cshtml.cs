using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebGraph.Data;
using WebGraph.Models;

namespace WebGraph.Pages.test
{
    public class DeleteModel : PageModel
    {
        private readonly WebGraph.Data.RecordContext _context;

        public DeleteModel(WebGraph.Data.RecordContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Record Record { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
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

        public async Task<IActionResult> OnPostAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Record = await _context.Record.FindAsync(id);

            if (Record != null)
            {
                _context.Record.Remove(Record);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
