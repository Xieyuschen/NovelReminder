using System.Collections.Generic;
using System.Threading.Tasks;

namespace NovelReminder
{
    interface IScanner
    {
        /// <summary>
        /// Return html content or throw an exceotion when failed to get response
        /// </summary>
        ValueTask<string> GetHtmlContentAsync(string url);

        ValueTask<IEnumerable<string>> GetCatalogAsync(string url);


    }
}