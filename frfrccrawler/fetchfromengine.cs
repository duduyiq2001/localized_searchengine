using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Crawler;

namespace engine
{
    class fetchfromengine
    {
        
        public async Task start()
        {

            webCrawler newc = new webCrawler(2);
            await newc.GetContentfromoutside(new Uri("https://cse.google.com/cse/element/v1?rsz=filtered_cse&num=10&hl=en&source=gcsc&gss=.com&cselibv=827890a761694e44&cx=1178a0e82fb3b4593&q=fetus&safe=off&cse_tok=AB-tC_7Ah8aG-ZXCZSgKSb4cSrQ6:1691037104961&sort=&exp=csqr,cc&oq=fetus&gs_l=partner-web.12..0l29j0i512i433j0i512i433i131l2j0i512i433l3j0i512i433i131l3j0i512i433.75438.85728.1.92179.11.11.0.0.0.0.86.685.11.11.0.csems%2Cnrl%3D10...0....1.34.partner-web..0.17.1063.4C7U11eb6XE&cseclient=hosted-page-client&callback=google.search.cse.api19079"));

        }
    }
}
