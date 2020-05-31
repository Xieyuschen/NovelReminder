using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NovelReminder
{
    class Scanner
    {
        //private List<string> NovalUrls;
        public async ValueTask<string> GetArticleAsync(string url)
        {
            using(var client=new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStringAsync();
                }
                else
                {
                    throw new Exception("The Request failed!");
                }
            }
              
            
        }
        
    }
}
