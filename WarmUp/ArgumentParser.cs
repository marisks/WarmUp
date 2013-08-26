using System;
using System.Collections.Generic;
using System.Linq;

namespace WarmUp
{
    public class ArgumentParser
    {
        private readonly string[] args;

        public ArgumentParser(string[] args)
        {
            if (args == null) throw new ArgumentNullException("args");
            this.args = args;
        }

        public Arguments GetArguments()
        {
            return new Arguments
            {
                Timeout = GetTimeSpan("-timeout", Arguments.DefaultTimeout),
                StartDelay = GetTimeSpan("-startDelay", Arguments.DefaultStartDelay),
                Retries = GetInt("-retries", Arguments.DefaultRetries),
                SiteUris = GetUris("-siteUrls")
            };
        }

        private IEnumerable<Uri> GetUris(string argName)
        {
            var urls = GetMany(argName);
            foreach (var url in urls)
            {
                Uri tmp;
                if (Uri.TryCreate(url, UriKind.Absolute, out tmp))
                {
                    yield return tmp;
                }
            }
        }

        private IEnumerable<string> GetMany(string argName)
        {
            if (!args.Contains(argName)) yield break;

            var argIdx = Array.IndexOf(args, argName);
            var nextArgIdx = Array.FindIndex(args, argIdx + 1, x => x.StartsWith("-"));
            var firstIdx = argIdx + 1;
            var lastIdx = (nextArgIdx != -1 ? nextArgIdx : args.Length) - 1;

            for (var i = firstIdx; i <= lastIdx; i++)
            {
                yield return args[i];
            }
        } 

        private int GetInt(string argName, int defaultValue)
        {
            var strValue = GetOne(argName);
            if (string.IsNullOrEmpty(strValue))
            {
                return defaultValue;
            }

            int val;
            int.TryParse(strValue, out val);
            return val;
        }

        private TimeSpan GetTimeSpan(string argName, TimeSpan defaultValue)
        {
            var strValue = GetOne(argName);
            if (string.IsNullOrEmpty(strValue))
            {
                return defaultValue;
            }

            int seconds;
            int.TryParse(strValue, out seconds);
            return new TimeSpan(0, 0, seconds);
        }

        private string GetOne(string argName)
        {
            if (!args.Contains(argName)) return string.Empty;
            var argIdx = Array.IndexOf(args, argName);
            return argIdx + 1 < args.Length ? args[argIdx + 1] : string.Empty;
        }
    }

    public class Arguments
    {
        public static TimeSpan DefaultTimeout
        {
            get { return new TimeSpan(0, 0, 300); }
        }

        public TimeSpan Timeout { get; set; }

        public static TimeSpan DefaultStartDelay
        {
            get { return new TimeSpan(0, 0, 10); }
        }

        public TimeSpan StartDelay { get; set; }

        public static int DefaultRetries
        {
            get { return 5; }
        }

        public int Retries { get; set; }

        public IEnumerable<Uri> SiteUris { get; set; }
    }
}