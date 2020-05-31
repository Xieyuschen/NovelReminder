using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebGraph.Models;

namespace WebGraph.Data
{
    public class WebGraphContext : DbContext
    {
        public WebGraphContext (DbContextOptions<WebGraphContext> options)
            : base(options)
        {
        }

        public DbSet<WebGraph.Models.Record> Record { get; set; }
    }
}
