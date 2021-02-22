using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace EngineOne.Models
{
    public class ConfigurationKeys
    {
        public class LogLevel
        {
            public string Default { get; set; }
            public string Microsoft { get; set; }
            [JsonProperty("Microsoft.Hosting.Lifetime")]
            public string MicrosoftHostingLifetime { get; set; }
        }

        public class Logging
        {
            public LogLevel LogLevel { get; set; }
        }

        public class PhotoApi
        {
            public string PhotoUri { get; set; }
            public string AuthUri { get; set; }
            public string Apikey { get; set; }
        }
    }
}
