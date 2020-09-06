using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.IO;
namespace NovelReminder
{

    //This class stores information about a piece of Novel to remind
    public class NovelInfo
    {
        public string Url { get; set; }
        public int LastUpdate { get; set;}
        public bool IsInit {get;set;}

        private static string split =  ";";
        public string Serialization()
        {
            return Url + split + LastUpdate.ToString() +split+ IsInit.ToString();
        }
        public static NovelInfo Deserialization(string str)
        {
            var info = str.Split(split);
            try
            {
                NovelInfo novelinfo = new NovelInfo
                {
                    Url = info[0],
                    LastUpdate = int.Parse(info[1]),
                    IsInit = bool.Parse(info[2])
                };
                return novelinfo;
            }
            catch 
            {
                throw new Exception("Incorrect piece of info!");
            }
        }
    }
    
    /// <summary>
    /// Manage updating information by a txt file rather a database
    /// </summary>
    class DbFileServices : IDataManager
    {
        private List<NovelInfo> Information {get;set;}
        private string filename = "./../../../NovelInfomation.txt";
        private void RewriteTxtFile()
        {
            //rewrite txt file when some records are updated
            using(StreamWriter file=new StreamWriter(filename,false))
            {
                foreach(var item in Information)
                {
                    file.WriteLine(item.Serialization());
                }
            }
        }
        public DbFileServices()
        {
            //Init infomation by file NovelInfomation.txt
            //Txt or Json?
            //Json can simplied Serialization!

            //how to Serilize NovelInfo and Deserilize it?
            //Implement it by myself first:)
            Information = new List<NovelInfo>();
            IEnumerable<string> lines = File.ReadLines(filename);
            foreach(var item in lines)
            {
                try
                {
                    Information.Add(NovelInfo.Deserialization(item));

                }
                catch { }
            }
        }

        //Lack of async will occurs that cannot convert bool to System.Threading.Task
        public async ValueTask<bool> GetIsInit(string url)
        {

                var ele = Information.Find(t => t.Url == url);
                if (ele == null)
                {
                    throw new Exception("Cannot find this Url infotmation");

                }
                else return ele.IsInit;
        }

        public async ValueTask<int> GetLastChapterAsync(string url)
        {
            var lastchapter= from item in Information
                            where item.Url==url
                            select item.LastUpdate;
                         
            return lastchapter.First();
        }

        public async ValueTask InsertOrUpdateOneAsync(string url, int lastUpdate)
        {
            bool IsUpdate=false;
            foreach(var item in Information)
            {
                if(item.Url==url)
                {
                    IsUpdate=true;
                    item.LastUpdate=lastUpdate;
                }

            }
            if(!IsUpdate)
            {
                Information.Add(new NovelInfo{
                    Url=url,
                    LastUpdate=lastUpdate
                    ,IsInit=false});
            }
            RewriteTxtFile();
        }

        public async ValueTask UpdateAsync(string url, bool isinit)
        {
            foreach(var item in Information)
            {
                if(item.Url==url)
                {
                    item.IsInit=isinit;
                }
            }
            RewriteTxtFile();
        }

        public async ValueTask UpdateAsync(string url, int lastUpdate)
        {
            foreach(var item in Information)
            {
                if(item.Url==url)
                {
                    item.LastUpdate=lastUpdate;
                }
            }
            RewriteTxtFile();
        }
    }
}
