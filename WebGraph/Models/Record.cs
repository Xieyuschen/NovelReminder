using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace WebGraph.Models
{
    public class Record
    {
        public string Url { get; set; }
        public string Name { get; set; }
        public int LastChapter { get; set; }
        public bool IsInit { get; set; } = false;
        public override string ToString()
        {
            return Name + "\t" + Url + "\t" + LastChapter.ToString() + "\t" + IsInit.ToString();
        }
    }
}
