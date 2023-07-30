using System;
using Crawler;
using System.Threading.Tasks;
namespace maintask
{
    class Program
    {
        static async Task Main(string[] args)
        {
            webCrawler newcrawl = new webCrawler(2);
            await newcrawl.start();
        }
    }
}
