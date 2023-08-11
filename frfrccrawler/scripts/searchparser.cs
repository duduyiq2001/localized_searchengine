using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using HtmlAgilityPack;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;
using pupcrawl;
using System.Linq;

namespace parser
{
    
    public struct siteinfo
    {
        public string title { get; set; }
        public string date { get; set; }
        public Uri uri { get; set; }
        public string abs
        {
            get; set; }
        public override string ToString()
        {
            return $"{title}, {date}\n Link: {uri}\n \t {abs}";
        }
    };
    public class misspellexception: Exception
    {

    }
    class searchparser
    {
        private JObject document;
        private static List<siteinfo> sites = new List<siteinfo>();
        private static pupcrawler pup = new pupcrawler(2);
        public searchparser(string doc)
        {
            document = JObject.Parse(doc);
        }
        //https://www.newtonsoft.com/json/help/html/SerializingJSONFragments.html
        public async Task start()
        {
            JArray results = (JArray)document["items"];
            if (results == null)
            {
                Console.WriteLine("misspelling");
                throw new misspellexception();
            }
            int length = results.Count;
            for (int i = 0; i < length; i++)
            {
                //filter down
                if ((string)results[i]["pagemap"]["metatags"][0]["dc.type"]!= null && (string)results[i]["pagemap"]["metatags"][0]["dc.type"] == "OriginalPaper")  
                {
                    siteinfo theinfo = new siteinfo { };
                    theinfo.title = (string)results[i]["title"];
                    theinfo.date = (string)results[i]["pagemap"]["metatags"][0]["dc.date"];
                    Uri theuri = new Uri((string)results[i]["link"]);
                    theinfo.uri = theuri;

                    // puppetteer site
                    string content = "";
                    try
                    {
                        content = await pup.GetContentwithbrowser(theuri);
                    }
                    catch(PuppeteerSharp.NavigationException e)
                    {
                        Console.WriteLine("timed out");
                        continue;
                    }
                    theinfo.abs = await fetchabs(content);
                    sites.Add(theinfo);



                }
            }

        } 

        private async Task<string> fetchabs(string content)
        {

            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(content);
            string abst = "";
            //var htmlNodes = htmlDoc.DocumentNode.SelectNodes("//body");

            var headings = htmlDoc.DocumentNode.Descendants().Where(n => n.Name.StartsWith("h") && n.Name.Length == 2&& (n.InnerHtml.Contains("abstract") || n.InnerHtml.Contains("Abstract")));

            foreach (var node in headings)
            {

                    var paragraphs = node.ParentNode.Descendants("p");
                    foreach (var pa in paragraphs)
                {
                    //Console.WriteLine(pa.InnerHtml);
                    abst += pa.InnerText + "\n";
                }
                    /*
                    using (StreamWriter outputFile = new StreamWriter("C://Windows//Temp//output1.txt"))
                    {
                        await outputFile.WriteAsync((string)node.InnerHtml);
                    }
                    */
              
                
            }
            
            

            return abst;
        }
        public void revealall()
        {
            foreach(var site in sites)
            {
                Console.WriteLine(site.ToString()+ "\n");
            }
        }
        public void clearall()
        {
            sites.Clear();
        }
        public List<siteinfo> getsites()
        {
            return sites;
        }
    }
}
