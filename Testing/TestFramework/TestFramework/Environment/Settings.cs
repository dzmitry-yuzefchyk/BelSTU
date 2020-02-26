using Microsoft.Extensions.Configuration;
using System;

namespace TestFramework.Environment
{
    public static class Settings
    {
        private static string _browser;
        public static string Browser
        {
            get
            {
                return _browser;
            }
            set
            {
                if (_browser == null)
                {
                    _browser = value;
                }
            }
        }

        private static string _env;
        public static string Env
        {
            get
            {
                return _env;
            }
            set
            {
                if (_env == null)
                {
                    _env = value switch
                    {
                        "dev" => Configurations.DevConfig,
                        "qa" => Configurations.QAConfig,
                        _ => Configurations.DevConfig
                    };
                }
            }
        }

        public static IConfiguration GetConfiguration() => new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile(_env)
            .Build();

        internal static class Configurations
        {
            public const string DevConfig = "env.dev.json";
            public const string QAConfig = "env.qa.json";
        }
    }
}
