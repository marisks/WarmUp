using System;
using System.Net.Http;
using Xunit;
using Should;

namespace WarmUp.Tests
{
    public class StartManyTests
    {
        [Fact]
        public void it_should_start_3_tasks_for_3_urls()
        {
            var response = new HttpResponseMessage(System.Net.HttpStatusCode.OK);
            var handler = new FakeHttpMessageHandler(response);
            var client = new HttpClient(handler);

            var sut = new Warmer(client);

            var result = sut.StartMany(new[]
            {
                new Uri("http://first"),
                new Uri("http://second"),
                new Uri("http://third")
            });

            result.Length.ShouldEqual(3);
        }
    }
}