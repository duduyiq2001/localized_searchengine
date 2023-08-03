using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace Crawler
{
    class webCrawler
    {
        private static Dictionary<string, Tuple<int, List<string>>> _pages = new Dictionary<string, Tuple<int, List<string>>>();
        private static HttpClient _client = new HttpClient();
        private static int maxDepth = 2;

        public webCrawler(int maxdepth)
        {
            maxDepth = maxdepth;
            _client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537");
        }
        public async Task start()
        {
            await ParsePages(new Uri("https://en.wikipedia.org/wiki/Main_Page"));
           

            foreach (var page in _pages)
            {
                Console.WriteLine("Visited Page: {0} ({1})", page.Key, page.Value.Item1);
                Console.WriteLine("------------------");
                Console.WriteLine(string.Join("\n", page.Value.Item2));
                Console.WriteLine();
            }
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
                var response = await _client.GetAsync(fullUrl);
                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine(content);

                return content;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Problem parsing: {fullUrl}\n{e}" + e);
                return null;
            }
        }
        public async Task<string> GetContentfromoutside(Uri fullUrl)
        {
            try
            {
                var response = await _client.GetAsync(fullUrl);

                // Ensure a successful response
                response.EnsureSuccessStatusCode();

                // Read the response content as a stream
                using (Stream stream = await response.Content.ReadAsStreamAsync())
                using (StreamReader reader = new StreamReader(stream))
                {
                    // Read the content as a string
                    string content = await reader.ReadToEndAsync();
                    Console.WriteLine(content);

                }

                return "oops";
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


