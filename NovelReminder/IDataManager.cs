using System.Threading.Tasks;

namespace NovelReminder
{
    interface IDataManager
    {
        ValueTask<bool> GetIsInit(string url);
        ValueTask<int> GetLastChapterAsync(string url);
        ValueTask InsertOrUpdateOneAsync(string url, int lastUpdate);
        ValueTask UpdateAsync(string url, bool isinit);
        ValueTask UpdateAsync(string url, int lastUpdate);
    }
}