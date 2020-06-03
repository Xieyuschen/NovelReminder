using System.Threading.Tasks;

namespace NovelReminder
{
    interface IScanner
    {
        ValueTask<string> GetArticleAsync(string url);
    }
}