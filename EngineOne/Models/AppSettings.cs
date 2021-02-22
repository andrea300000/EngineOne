using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static EngineOne.Models.ConfigurationKeys;

namespace EngineOne.Models
{
    public class AppSettings
    {
        public Logging Logging { get; set; }
        public string AllowedHosts { get; set; }
        public PhotoApi PhotoApi { get; set; }
    }
}
