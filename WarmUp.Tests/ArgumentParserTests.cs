using System;
using System.Linq;
using Xunit;
using Should;

namespace WarmUp.Tests
{
    public class ArgumentParserTests
    {
        [Fact]
        public void it_should_return_timeout_following_timout_key()
        {
            var args = new[]
            {
                "-timeout",
                "200"
            };

            var sut = new ArgumentParser(args);

            var result = sut.GetArguments().Timeout;

            result.ShouldEqual(new TimeSpan(0, 0, 200));
        }

        [Fact]
        public void it_should_return_default_timeout_when_no_timeout_provided()
        {
            var args = new string[] {};

            var sut = new ArgumentParser(args);

            var result = sut.GetArguments().Timeout;

            result.ShouldEqual(Arguments.DefaultTimeout);
        }

        [Fact]
        public void it_should_return_default_timeout_when_key_provided_but_no_value()
        {
            var args = new[]
            {
                "-timeout"
            };

            var sut = new ArgumentParser(args);

            var result = sut.GetArguments().Timeout;

            result.ShouldEqual(Arguments.DefaultTimeout);
        }

        [Fact]
        public void it_should_return_startDelay_following_startDelay_key()
        {
            var args = new[]
            {
                "-startDelay",
                "15"
            };

            var sut = new ArgumentParser(args);

            var result = sut.GetArguments().StartDelay;

            result.ShouldEqual(new TimeSpan(0, 0, 15));
        }

        [Fact]
        public void it_should_return_default_startDelay_when_no_startDelay_provided()
        {
            var args = new string[] {};

            var sut = new ArgumentParser(args);

            var result = sut.GetArguments().StartDelay;

            result.ShouldEqual(Arguments.DefaultStartDelay);
        }

        [Fact]
        public void it_should_return_retries_following_retries_key()
        {
            var args = new[]
            {
                "-retries",
                "10"
            };

            var sut = new ArgumentParser(args);

            var result = sut.GetArguments().Retries;

            result.ShouldEqual(10);
        }

        [Fact]
        public void it_should_return_default_retries_when_no_retries_provided()
        {
            var args = new string[] {};

            var sut = new ArgumentParser(args);

            var result = sut.GetArguments().Retries;

            result.ShouldEqual(Arguments.DefaultRetries);
        }

        [Fact]
        public void it_should_return_siteUris_following_siteUrls_key()
        {
            var args = new[]
            {
                "-siteUrls",
                "http://mysite.com",
                "http://localhost",
                "http://othersite.no"
            };

            var sut = new ArgumentParser(args);

            var result = sut.GetArguments().SiteUris.ToList();

            result.ShouldContain(new Uri(args[1]));
            result.ShouldContain(new Uri(args[2]));
            result.ShouldContain(new Uri(args[3]));
        }

        [Fact]
        public void it_should_return_emptyList_when_no_urls_provided()
        {
            var args = new string[] {};

            var sut = new ArgumentParser(args);

            var result = sut.GetArguments().SiteUris;

            result.ShouldBeEmpty();
        }

        [Fact]
        public void it_should_return_emptyList_when_key_provided_but_no_urls()
        {
            var args = new[]
            {
                "-siteUrls"
            };

            var sut = new ArgumentParser(args);

            var result = sut.GetArguments().SiteUris;

            result.ShouldBeEmpty();
        }

        [Fact]
        public void it_should_return_siteUris_when_there_are_multiple_args()
        {
            var args = new[]
            {
                "-timeout",
                "400",
                "-siteUrls",
                "http://mysite.com",
                "http://localhost",
                "http://othersite.no",
                "-startDelay",
                "10"
            };

            var sut = new ArgumentParser(args);

            var result = sut.GetArguments().SiteUris.ToList();

            result.ShouldContain(new Uri(args[3]));
            result.ShouldContain(new Uri(args[4]));
            result.ShouldContain(new Uri(args[5]));
        }
    }
}