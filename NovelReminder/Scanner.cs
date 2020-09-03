using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
namespace NovelReminder
{
    //Scanner 应该完成对页面的一些解析与处理，而不是只获得一个内容就完事了                      

    class Scanner : IScanner
    {

        public async ValueTask<string> GetArticleAsync(string url)
        {
            using (var client = new HttpClient())
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
