using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RazorPage.Models;

namespace RazorPage.Data
{
    public class RazorPageContext : DbContext
    {
        public RazorPageContext (DbContextOptions<RazorPageContext> options)
            : base(options)
        {
        }

        public DbSet<RazorPage.Models.Record> Record { get; set; }
    }
}
