using System;
using Crawler;
using System.Threading.Tasks;
using pupcrawl;
using engine;
using parser;
using System.Collections.Generic;



namespace maintask
{
    class crawlstart
    {

        
        public static async Task<List<siteinfo>> start(string query, int request_number,string sort_method,int start=0)
        {
            /*
            webCrawler newcrawl = new webCrawler(2);
            await newcrawl.start();
            */
            //pupcrawler newcrawl = new pupcrawler(2);
            //await newcrawl.start();


            // Register other services here

            /*
            string query = Console.ReadLine();
            int request_number = Int32.Parse(Console.ReadLine());
            string sort_method = Console.ReadLine();
            */
            fetchfromengine e = new fetchfromengine();
            Console.WriteLine("wdnjnw");
            string content = await e.start(request_number,query,sort_method,start);
            searchparser.clear();
            searchparser prsr = new searchparser(content);
            await prsr.start();
            prsr.revealall();
            return prsr.getsites();




        }
        
    }
}
