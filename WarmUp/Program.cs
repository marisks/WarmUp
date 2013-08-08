using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace WarmUp
{
    class Program
    {
        static int Main(string[] args)
        {
            var siteUrls = args;
            var timeout = new TimeSpan(0, 10, 0);
            var startDelay = new TimeSpan(0, 0, 10);
            var retries = 5;

            var client = new HttpClient();
            client.Timeout = timeout;

            var warmer = new Warmer(client, msg => Console.WriteLine(msg));
            return (int)warmer.Warmup(siteUrls, retries, startDelay);
        }
    }
}
