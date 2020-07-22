using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebGraph.Models;

namespace WebGraph.Data
{
    public class RecordContext : DbContext
    {

        //services.AddDbContext<RecordContext>(options =>
        //            options.UseSqlServer(Configuration.GetConnectionString("RecordContext")));

        //使用Option Builder从appsettings中读取数据
      //        "ConnectionStrings": {
      //  "WebGraphContext": "Server=(localdb)\\mssqllocaldb;Database=WebGraphContext-b641a2bc-f882-4d12-988e-df1a477d02be;Trusted_Connection=True;MultipleActiveResultSets=true",
      //  "RecordContext": "Server=(localdb)\\mssqllocaldb;Database=RecordContext-34b41c55-f4ff-41c8-a9e2-828f01671c03;Trusted_Connection=True;MultipleActiveResultSets=true"
      //}

        public RecordContext (DbContextOptions<RecordContext> options)
            : base(options)
        {
            
        }

        public DbSet<WebGraph.Models.Record> Record { get; set; }
    }
}
