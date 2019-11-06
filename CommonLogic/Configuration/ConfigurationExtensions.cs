using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;

namespace CommonLogic.Configuration
{
    public static class ConfigurationExtensions
    {
        public static T GetValue<T>(this IConfiguration configuration, IWebHostEnvironment env,
            string configVariableFromEnv, string configVariableFromSettings)
        {
            if (env.IsDevelopment())
            {
                return configuration.GetValue<T>(configVariableFromSettings);
            }

            var value = Environment.GetEnvironmentVariable(configVariableFromEnv);
            return (T)Convert.ChangeType(value, typeof(T));
        }
    }
}
