using System;
namespace Kaneko.Core.Configuration
{
    public class AppMetricsInfluxDbOptions
    {
        public bool? Enabled { get; set; }
        public string Database { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public string Password { get; set; } = null!;
        public Uri BaseUri { get; set; } = new Uri("http://127.0.0.1:8086");
        public bool CreateDataBaseIfNotExists { get; set; } = true;
        public TimeSpan? BackoffPeriod { get; set; } = null!;
        public int? FailuresBeforeBackoff { get; set; } = null!;
        public TimeSpan? Timeout { get; set; } = null!;
        public TimeSpan? FlushInterval { get; set; } = null!;
    }
}
