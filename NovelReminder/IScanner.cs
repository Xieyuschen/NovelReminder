using System.Threading.Tasks;

namespace NovelReminder
{
    interface IScanner
    {
        /// <summary>
        /// Return html content or throw an exceotion when failed to get response
        /// </summary>
        ValueTask<string> GetArticleAsync(string url);
    }
}