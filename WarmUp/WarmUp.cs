using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace WarmUp
{
    public class Warmer
    {
        private readonly Action<string> log = s => { };
        private readonly HttpClient client;

        public Warmer(HttpClient client)
        {
            this.client = client;
        }

        public Warmer(HttpClient client, Action<string> log)
        {
            this.client = client;
            this.log = log;
        }

        public WarmupStatus Warmup(IEnumerable<Uri> requestUris, int retries = 1, TimeSpan? startDelay = null)
        {
            var uris = requestUris.ToList();
            for (int i = 0; i < retries; i++)
            {
                var tasks = StartMany(uris, startDelay);
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

        public Task StartOne(Uri requestUri)
        {
            return Task.Factory.StartNew(() =>
            {
                HttpResponseMessage result;
                try
                {
                    Log("Warmup of {0} started", requestUri);
                    var task = client.GetAsync(requestUri);
                    result = task.Result;
                }
                catch (AggregateException agex)
                {
                    foreach(var ex in agex.InnerExceptions)
                    {
                        Log("Warmup of {0} failed. Exception: {1}", requestUri, ex.Message);
                    }
                    throw;
                }

                if (result.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    Log("Warmup of {0} failed. Received status {1}", requestUri, result.StatusCode);
                    throw new ApplicationException(string.Format("Warmup of {0} failed. Received status {1}", requestUri, result.StatusCode));
                }

                Log("Warmup of {0} finished successfully.", requestUri);
            });
        }

        public Task[] StartMany(IEnumerable<Uri> requestUris, TimeSpan? startDelay = null)
        {
            var delay = startDelay ?? new TimeSpan();

            var tasks = new List<Task>();
            foreach(var uri in requestUris) 
            {
                tasks.Add(StartOne(uri));
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
