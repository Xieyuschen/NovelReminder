using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization.Formatters;
namespace WebGraph.Models
{
    //为什么可以被序列化的类不能被继承？
    [Serializable]
    public class Record
    {
        //ID是为了生成一个CRUD，看一下如何组织数据库而写的东西
        public string ID { get; set; }
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
