using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WarmUp
{
    public class Warmer
    {
        private Action<string> log = s => { };
        private HttpClient client;

        public Warmer(HttpClient client)
        {
            this.client = client;
        }

        public Warmer(HttpClient client, Action<string> log)
        {
            this.client = client;
            this.log = log;
        }

        public WarmupStatus Warmup(string[] requestUrls, int retries = 1, TimeSpan? startDelay = null)
        {
            for (int i = 0; i < retries; i++)
            {
                var tasks = StartMany(requestUrls, startDelay);
                try
                {
                    Task.WaitAll(tasks);
                    return WarmupStatus.Success;
                }
                catch (AggregateException agEx)
                {
                    foreach(var ex in agEx.InnerExceptions)
                    {
                        Log("Exception: {0}", ex.Message);
                    }
                }
            }
            return WarmupStatus.Failure;
        }

        public Task StartOne(string requestUrl)
        {
            return Task.Factory.StartNew(() =>
            {
                HttpResponseMessage result;
                try
                {
                    Log("Warmup of {0} started", requestUrl);
                    var task = client.GetAsync(requestUrl);
                    result = task.Result;
                }
                catch (AggregateException agex)
                {
                    foreach(var ex in agex.InnerExceptions)
                    {
                        Log("Warmup of {0} failed. Exception: {1}", requestUrl, ex.Message);
                    }
                    throw;
                }

                if (result.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    Log("Warmup of {0} failed. Received status {1}", requestUrl, result.StatusCode);
                    throw new ApplicationException("Status 200 doesn't received");
                }

                Log("Warmup of {0} finished successfully.", requestUrl);
            });
        }

        public Task[] StartMany(string[] requestUrls, TimeSpan? startDelay = null)
        {
            var delay = startDelay ?? new TimeSpan();

            var tasks = new List<Task>();
            foreach(var url in requestUrls) 
            {
                tasks.Add(StartOne(url));
                Thread.Sleep(delay);
            }
            return tasks.ToArray();
        }

        private void Log(string message, params object[] args)
        {
            log(string.Format(message, args));
        }
    }
}
