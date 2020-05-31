using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace RazorPage.Models
{
    public class Record
    {
        public int ID { get; set; }
        public string Url { get; set; }
        public bool IsInit { get; set; }
    }
}
