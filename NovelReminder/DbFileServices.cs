using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
namespace NovelReminder
{

    public class NovelInfo
    {
        string Url { get; set; }
        int LastUpdate {get;set;}
        bool IsInit {get;set;}
    }
    /// <summary>
    /// Manage updating information by a txt file rather a database
    /// </summary>
    class DbFileServices : IDataManager
    {
        private List<NovelInfo> Information {get;set;}
        
        private void RewriteTxtFile()
        {
            //rewrite txt file when some records are updated
        }
        public DbfileServices()
        {
            //Init infomation by file NovelInfomation.txt
            //do logical steps
        }
        public ValueTask<bool> GetIsInit(string url)
        {
            var IsInit=from s in Infomation
                       where s.Url==url
                       select s.IsInit;
            if(IsNullReference(IsInit))
            {
                throw new Exception();
            }
            return IsInit;
        }

        public ValueTask<int> GetLastChapterAsync(string url)
        {
            var lastchapter= from item in Information
                            where item.Url==url
                            select item.LastChapter;
                         
            return lastchapter;
        }

        public ValueTask InsertOrUpdateOneAsync(string url, int lastUpdate)
        {
            bool IsUpdate=false;
            foreach(var item in Information)
            {
                if(item.Url==url)
                {
                    IsUpdate=true;
                    item.LastChapter=lastUpdate;
                }

            }
            if(!IsUpdate)
            {
                Information.Add(new NovelInfo{
                    Url=url,LastChapter=lastUpdate,IsInit=false}
                    );
            }
        }

        public ValueTask UpdateAsync(string url, bool isinit)
        {
            foreach(var item in Information)
            {
                if(item.Url==url)
                {
                    item.IsInit=isinit;
                }
            }
        }

        public ValueTask UpdateAsync(string url, int lastUpdate)
        {
            foreach(var item in Information)
            {
                if(item.Url==url)
                {
                    item.LastUpdate=lastUpdate;
                }
            }
        }
    }
}
