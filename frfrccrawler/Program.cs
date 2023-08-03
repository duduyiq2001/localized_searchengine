using System;
using Crawler;
using System.Threading.Tasks;
using pupcrawl;
using engine;
namespace maintask
{
    class Program
    {
        static async Task Main(string[] args)
        {
            /*
            webCrawler newcrawl = new webCrawler(2);
            await newcrawl.start();
            */
            //pupcrawler newcrawl = new pupcrawler(2);
            //await newcrawl.start();
            fetchfromengine e = new fetchfromengine();
            Console.WriteLine("wdnjnw");
            await e.start();

        }
    }
}
