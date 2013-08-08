/* code taken from http://nocture.dk/2013/05/21/csharp-unit-testing-classes-with-httpclient-dependence-using-autofixture/ */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace WarmUp.Tests
{
    public class FakeHttpMessageHandler: HttpMessageHandler
    {
        private HttpResponseMessage response;
        private bool throwException;
        public FakeHttpMessageHandler(HttpResponseMessage response, bool throwException = false)
        {
            this.response = response;
            this.throwException = throwException;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
        {
            if (throwException)
            {
                throw new Exception();
            }

            var responseTask = new TaskCompletionSource<HttpResponseMessage>();
            responseTask.SetResult(response);

            return responseTask.Task;
        }
    }
}
