using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
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
            var client = new HttpClient();

            var sut = new WarmUp(client);

            var result = sut.StartMany(new[] {
                "http://first",
                "http://second",
                "http://third"
            });

            result.Length.ShouldEqual(3);
        }
    }
}
