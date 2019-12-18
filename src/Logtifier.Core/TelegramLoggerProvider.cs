using System;
using System.Collections.Concurrent;
using System.Runtime.InteropServices;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Logtifier.Core
{
    public class TelegramLoggerProvider: ILoggerProvider, ISupportExternalScope
    {
        private readonly IOptionsMonitor<TelegramLoggerOptions> _options;
        private readonly ConcurrentDictionary<string, TelegramLogger> _loggers;
        private readonly TelegramLoggerProcessor _messageQueue;

        private IDisposable _optionsReloadToken;
        private IExternalScopeProvider _scopeProvider = NullExternalScopeProvider.Instance;

        [ProviderAlias("Telegram")]
        public TelegramLoggerProvider(IOptionsMonitor<TelegramLoggerOptions> telegramLoggerOptions)
        {
            _options = options;
            _loggers = new ConcurrentDictionary<string, TelegramLogger>();

            ReloadLoggerOptions(options.CurrentValue);
            _optionsReloadToken = _options.OnChange(ReloadLoggerOptions);

            _messageQueue = new TelegramLoggerProcessor();
            //if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            //{
            //    _messageQueue.Console = new WindowsLogConsole();
            //    _messageQueue.ErrorConsole = new WindowsLogConsole(stdErr: true);
            //}
            //else
            //{
            //    _messageQueue.Console = new AnsiLogConsole(new AnsiSystemConsole());
            //    _messageQueue.ErrorConsole = new AnsiLogConsole(new AnsiSystemConsole(stdErr: true));
            //}
        }

        public ILogger CreateLogger(string name)
        {
            return _loggers.GetOrAdd(name, loggerName => new TelegramLogger(name, _messageQueue)
            {
                Options = _options.CurrentValue,
                ScopeProvider = _scopeProvider
            });
        }

        public void Dispose()
        {
            throw new System.NotImplementedException();
        }

        public void SetScopeProvider(IExternalScopeProvider scopeProvider)
        {
            _scopeProvider = scopeProvider;

            foreach (var logger in _loggers)
            {
                logger.Value.ScopeProvider = _scopeProvider;
            }
        }
    }
}
