using HtmlAgilityPack;
using lecture_14_crawler_ex.Models;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Security.AccessControl;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace lecture_14_crawler_ex
{
    public static class Scripts
    {

        private static StreamWriter swLogger = new StreamWriter("errors.txt");

        static Scripts()
        {
            swLogger.AutoFlush= true;
        }

        

        private static readonly HttpClient client = new HttpClient();
        public static async Task<string> returnSourceCode(string Url, string UrlHash)
        {

            // Call asynchronous network methods in a try/catch block to handle exceptions.
            bool crawled = true;
            Byte iraddCrawlCount = 0;
            string response_result = null;
            try
            {
                using HttpResponseMessage response = await client.GetAsync(Url);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                // Above three lines can be replaced with new helper method below
                // string responseBody = await client.GetStringAsync(uri);

                response_result = responseBody;
            }
            catch (Exception e)
            {
                lock(swLogger)
                {
                    swLogger.WriteLine(e.Message);
                }

                iraddCrawlCount++;
                 crawled = false;
            }


            using ExampleCrawlerContext _ExampleCrawlerContext = new ExampleCrawlerContext();
            var _url = await _ExampleCrawlerContext.Urls.FindAsync(UrlHash);
            _url.Crawled = crawled;
            _url.CrawlTryCount += iraddCrawlCount;
            await _ExampleCrawlerContext.SaveChangesAsync();

            lock (MainWindow.hsActiveCrawling)
            {
                MainWindow.hsActiveCrawling.Remove(Url);
            }

            return response_result;


        }

        public static async Task<List<string>> extractLinks(string CrawledUrl, string CrawlUrlHash)
        {
            var sourceCode = await returnSourceCode(CrawledUrl, CrawlUrlHash);

            if (string.IsNullOrEmpty(sourceCode))
                return null;

            HtmlDocument hdDoc = new HtmlDocument();
            hdDoc.LoadHtml(sourceCode);

            var vrlinks = hdDoc.DocumentNode.SelectNodes("//a");

            if (vrlinks == null)
                return null;

            List<string> lstFoundLinks = new List<string>();
            Uri baseUri = new Uri(CrawledUrl);

            foreach (var vrLinkNode in vrlinks)
            {
                if (vrLinkNode.Attributes.Contains("href"))
                {
                    Uri _uri = new Uri(baseUri, vrLinkNode.Attributes["href"].Value.ToString().Trim());
                    if (_uri.AbsoluteUri.ToString().IsValidUrl())
                        lstFoundLinks.Add(_uri.AbsoluteUri.ToString());
                }
            }
            return lstFoundLinks.Distinct().ToList();
        }
    }
}
