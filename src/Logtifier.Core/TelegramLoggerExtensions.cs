using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging.Configuration;
using Microsoft.Extensions.Logging.Console;

namespace Logtifier.Core
{
    public static class TelegramLoggerExtensions
    {
        public static ILoggingBuilder AddTelegram(this ILoggingBuilder builder)
        {
            builder.AddConfiguration();

            builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<ILoggerProvider, TelegramLoggerProvider>());
            LoggerProviderOptions.RegisterProviderOptions<TelegramLoggerOptions, TelegramLoggerProvider>(builder.Services);
            return builder;
        }

        public static ILoggingBuilder AddTelegram(this ILoggingBuilder builder, Action<ConsoleLoggerOptions> configure)
        {
            if (configure == null)
            {
                throw new ArgumentNullException(nameof(configure));
            }

            builder.AddTelegram();
            builder.Services.Configure(configure);

            return builder;
        }

        private static LogLevel GetMinimumLogLevel(IConfiguration configuration)
        {
            var minimumLevel = LogLevel.Information;
            var defaultLevel = configuration["LogLevel:Default"];
            if (!string.IsNullOrWhiteSpace(defaultLevel))
            {
                if (!Enum.TryParse(defaultLevel, out minimumLevel))
                {
                    Debug.WriteLine("The minimum level setting `{0}` is invalid", defaultLevel);
                    minimumLevel = LogLevel.Information;
                }
            }
            return minimumLevel;
        }

        private static Dictionary<string, LogLevel> GetLevelOverrides(IConfiguration configuration)
        {
            var levelOverrides = new Dictionary<string, LogLevel>();
            foreach (var overr in configuration.GetSection("LogLevel").GetChildren().Where(cfg => cfg.Key != "Default"))
            {
                if (!Enum.TryParse(overr.Value, out LogLevel value))
                {
                    Debug.WriteLine("The level override setting `{0}` for `{1}` is invalid", overr.Value, overr.Key);
                    continue;
                }

                levelOverrides[overr.Key] = value;
            }

            return levelOverrides;
        }
    }
}
