using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HtmlAgilityPack;
namespace NovelReminder
{
    //Scanner 应该完成对页面的一些解析与处理，而不是只获得一个内容就完事了                      

    class Scanner : IScanner
    {
        HtmlDocument HtmlDocument;
        public Scanner()
        {
            HtmlDocument = new HtmlDocument();
        }
        public async ValueTask<string> GetHtmlContentAsync(string url)
        {
            
            using (var client = new HttpClient())
            {

                HttpResponseMessage response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var htmlStr= await response.Content.ReadAsStringAsync();
                    HtmlDocument.LoadHtml(htmlStr);
                    return htmlStr;
                }
                else
                {
                    throw new Exception("The Request failed!");
                }
            }
        }


        /// <summary>
        /// 返回每条目录的字符串，格式型为：a href="xxx" blabla(含有tag)
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public async ValueTask<IEnumerable<string>> GetCatalogAsync(string url)
        {
            HtmlDocument htmlDocument = new HtmlDocument();
            var htmlpage = await GetHtmlContentAsync(url);
            htmlDocument.LoadHtml(htmlpage);
            var Catalog = htmlDocument.GetElementbyId("list");
            htmlDocument.LoadHtml(Catalog.OuterHtml);
            List<string> list = new List<string>();
            var pieces = htmlDocument.DocumentNode.SelectNodes("//a");

            foreach(var item in pieces)
            {
                list.Add(item.OuterHtml);
            }
            return list;
        }

    }
}
