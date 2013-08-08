using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace WarmUp
{
    public class WarmUp
    {
        private Action<string> log = s => { };
        private HttpClient client;

        public WarmUp(HttpClient client)
        {
            this.client = client;
        }

        public WarmUp(HttpClient client, Action<string> log)
        {
            this.client = client;
            this.log = log;
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
                catch(Exception ex)
                {
                    Log("Warmup of {0} failed. Exception: {1}", requestUrl, ex.Message);
                    throw;
                }

                if (result.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    Log("Warmup of {0} failed. Received status {1}", requestUrl, result.StatusCode);
                    throw new ApplicationException("Status 200 doesn't received");
                }
            });
        }

        private void Log(string message, params object[] args)
        {
            log(string.Format(message, args));
        }
    }
}
