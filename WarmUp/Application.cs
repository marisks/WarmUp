using System;
using System.Net.Http;

namespace WarmUp
{
    public static class Application
    {
        public static int Start(string[] args)
        {
            var parser = new ArgumentParser(args);
            var arguments = parser.GetArguments();

            using (var client = new HttpClient())
            {
                client.Timeout = arguments.Timeout;

                var warmer = new Warmer(client, Console.WriteLine);
                return (int) warmer.Warmup(arguments.SiteUris, arguments.Retries, arguments.StartDelay);
            }
        }
    }
}