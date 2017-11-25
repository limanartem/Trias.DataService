using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace Trias.DataService.Tests.Helpers
{
    public static class ConfigHelper
    {

        public static IConfigurationRoot Configuration { get; set; }

        static ConfigHelper()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appcofig.json");

            Configuration = builder.Build();
        }

        public static Uri TriasServiceUrl => new Uri(Configuration["triasServiceUrl"]);
        public static string TriasServiceRef => Configuration["triasServiceRef"];
    }
}
