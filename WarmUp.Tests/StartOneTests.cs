using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace WarmUp.Tests
{
    public class StartOneTests
    {
        [Fact]
        public void it_should_not_throw_if_status_code_200()
        {
            var message = new HttpResponseMessage(System.Net.HttpStatusCode.OK);
            var handler = new FakeHttpMessageHandler(message);
            var client = new HttpClient(handler);

            var sut = new WarmUp(client);

            Assert.DoesNotThrow(() => sut.StartOne("http://someurl").Wait());
        }

        [Fact]
        public void it_should_throw_if_status_code_not_200()
        {
            var message = new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest);
            var handler = new FakeHttpMessageHandler(message);
            var client = new HttpClient(handler);

            var sut = new WarmUp(client);

            Assert.Throws<AggregateException>(() => sut.StartOne("http://someurl").Wait());
        }

        [Fact]
        public void it_should_throw_if_client_throws()
        {
            var message = new HttpResponseMessage(System.Net.HttpStatusCode.OK);
            var handler = new FakeHttpMessageHandler(message, throwException: true);
            var client = new HttpClient(handler);

            var sut = new WarmUp(client);

            Assert.Throws<AggregateException>(() => sut.StartOne("http://someurl").Wait());
        }
    }
}
