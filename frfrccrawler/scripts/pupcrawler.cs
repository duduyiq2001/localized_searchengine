using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using PuppeteerSharp;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace pupcrawl
{
    class pupcrawler

    {
        private static Dictionary<string, Tuple<int, List<string>>> _pages = new Dictionary<string, Tuple<int, List<string>>>();
        private static int maxDepth = 2;
        private static IBrowser browser;
        public pupcrawler(int maxdepth)
        {
            maxDepth = maxdepth;
        }
        public async Task start()
        {
            await new BrowserFetcher().DownloadAsync(BrowserFetcher.DefaultChromiumRevision);
            browser = await Puppeteer.LaunchAsync(new LaunchOptions
            {
                Headless = true
            });

            await ParsePages(new Uri("https://www.arxiv-sanity-lite.com/"), maxDepth);




        }
        private async Task ParsePages(Uri site, int depth = 0)
        {
            Console.WriteLine("wdnjwdn");
            if (_pages.ContainsKey(site.ToString()))
                return;

            depth++;

            var content = await GetContent(site);
            Console.WriteLine(content);

            if (content == null) return;

            var links = ParseContent(content, site);
            if (links == null) Console.WriteLine("link_null");
            _pages.Add(site.ToString(), new Tuple<int, List<string>>(depth, links));

            if (depth >= maxDepth)
                return;

            foreach (var link in links)
            {
                if (!_pages.ContainsKey(link))
                    await ParsePages(new Uri(link), depth);
            }

        }


        private async Task<string> GetContent(Uri fullUrl)
        {
            try

            {
                using var page = await browser.NewPageAsync();
                await page.GoToAsync(fullUrl.ToString(), WaitUntilNavigation.Networkidle0);
                //await page.GoToAsync(fullUrl.ToString(), 4500,null);
                string content = await page.GetContentAsync();
               Console.WriteLine(content);
                Console.WriteLine("djbejd");
                return content;

            }
            catch (Exception e)
            {
                Console.WriteLine($"Problem parsing: {fullUrl}\n{e}" + e);
                return null;
            }
        }

        public async Task<string> GetContentwithbrowser(Uri fullUrl)
        {
            try

            {
                await new BrowserFetcher().DownloadAsync(BrowserFetcher.DefaultChromiumRevision);
                browser = await Puppeteer.LaunchAsync(new LaunchOptions
                {
                    Headless = true
                });
                using var page = await browser.NewPageAsync();
                await page.GoToAsync(fullUrl.ToString(), WaitUntilNavigation.Networkidle0);
                //await page.GoToAsync(fullUrl.ToString(), 4500,null);
                string content = await page.GetContentAsync();
               // Console.WriteLine(content);
                //Console.WriteLine("djbejd");
                return content;

            }
            catch (Exception e)
            {
                Console.WriteLine($"Problem parsing: {fullUrl}\n{e}" + e);
                return null;
            }
        }

        private List<string> ParseContent(string content, Uri uri)
        {
            var regex = new Regex("^http(s)?://" + uri.Host, RegexOptions.IgnoreCase);
            var doc = new HtmlDocument();
            doc.LoadHtml(content);


            var nodes = doc.DocumentNode
                .SelectNodes("//a");
            var alist = new List<string> { };
            if (nodes != null)
            {
                alist = nodes
                   .Select(a =>
                   {
                       var val = a.GetAttributeValue("href", string.Empty);
                       Console.WriteLine(val);
                       return val.StartsWith("/") ? uri.GetLeftPart(UriPartial.Authority) + val : val;
                   })
                   .Distinct()
                   .Where(u => !string.IsNullOrEmpty(u) && regex.IsMatch(u))
                   .ToList();
            }
            else
            {
                Console.WriteLine("no links");
            }


            return alist;


        }
    

}
}
