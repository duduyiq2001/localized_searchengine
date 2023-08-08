using System;
using Crawler;
using System.Threading.Tasks;
using pupcrawl;
using engine;
using parser;



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


            // Register other services here

        
            string query = Console.ReadLine();
            int request_number = Int32.Parse(Console.ReadLine());
            string sort_method = Console.ReadLine();
        
            fetchfromengine e = new fetchfromengine();
            Console.WriteLine("wdnjnw");
            string content = await e.start(request_number,query,sort_method);
            searchparser prsr = new searchparser(content);
            await prsr.start();



        }
    }
}
