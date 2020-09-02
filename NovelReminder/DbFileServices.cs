using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NovelReminder
{

    public class NovelInfo
    {
        string Url { get; set; }
    }
    /// <summary>
    /// Manage updating information by a txt file rather database
    /// </summary>
    class DbFileServices : IDataManager
    {

        public ValueTask<bool> GetIsInit(string url)
        {
            throw new NotImplementedException();
        }

        public ValueTask<int> GetLastChapterAsync(string url)
        {
            throw new NotImplementedException();
        }

        public ValueTask InsertOrUpdateOneAsync(string url, int lastUpdate)
        {
            throw new NotImplementedException();
        }

        public ValueTask UpdateAsync(string url, bool isinit)
        {
            throw new NotImplementedException();
        }

        public ValueTask UpdateAsync(string url, int lastUpdate)
        {
            throw new NotImplementedException();
        }
    }
}
