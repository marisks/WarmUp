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
            var siteUrls = new[] { "http://coop", "http://littditt", "http://m.coop", "http://blah" };
            var timeout = new TimeSpan(0, 10, 0);
            var retries = 5;

            var client = new HttpClient();
            client.Timeout = timeout;

            for (int i = 0; i < retries; i++)
            {
                if (WarmUpSites(client, siteUrls))
                {
                    return 0;
                }
            }

            Console.WriteLine("Warmup failed");
            Console.WriteLine("Press any key");
            Console.ReadKey();

            return 1;
        }

        private static bool  WarmUpSites(HttpClient client, string[] siteUrls)
        {
            var taskList = new List<Task>();
            foreach (var siteUrl in siteUrls)
            {
                var task = new WarmUp(client).StartOne(siteUrl);
                taskList.Add(task);
            }

            try
            {
                Task.WaitAll(taskList.ToArray());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }

            return true;
        }
    }
}
