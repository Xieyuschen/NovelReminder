using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebGraph.Data;
using WebGraph.Models;

namespace WebGraph.Pages.test
{
    public class CreateModel : PageModel
    {
        private readonly WebGraph.Data.RecordContext _context;

        public CreateModel(WebGraph.Data.RecordContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Record Record { get; set; }

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Record.Add(Record);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
