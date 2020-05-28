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
        private List<string> NovalUrls;
        public async ValueTask<bool> ScanNovalUpdated(string url)
        {
            using (var client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    //这里查询数据库，检索现在的内容是否是最新的
                    //如果不是最新的将需要发送的url全部加入到数据库当中去，然后返回true
                    //如果是最新的就返回false
                    //do something..

                    //================================
                    //为了暂时测试就先返回true，以后再改
                    return true;
                    string responseBodyAsText = await response.Content.ReadAsStringAsync();
                    Console.WriteLine();
                    Regex r = new Regex(@"<dd>.*?</dd>");
                    var co = r.Matches(responseBodyAsText);
                    foreach (var item in co)
                    {
                        Console.WriteLine(item.ToString());
                    }
                }
            }
            return false;
        }
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
