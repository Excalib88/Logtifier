using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Logtifier.Core
{
    public class TelegramLoggerProcessor: ILoggerProcessor
    {
        // todo: need to do tread-safe queue
        //private readonly ConcurrentBag<LogMessageEntry> _messageQueue = new ConcurrentBag<LogMessageEntry>();
        private readonly List<LogMessageEntry> _messageQueue = new List<LogMessageEntry>();

        public async Task EnqueueMessage(LogMessageEntry message)
        {
            // Валидация и добавление логов в очередь
            try
            {
                _messageQueue.Add(message);
            }
            catch (Exception)
            {
                await WriteMessage(message);
            }
        }

        public Task DequeueMessage(LogMessageEntry message)
        {
            // Удаление сообщений из очереди
            throw new NotImplementedException();
        }

        private Task WriteMessage(LogMessageEntry message)
        {
            throw new NotImplementedException();
        }

        public Task ProcessLogQueue()
        {
            // Обход очереди и вызов WriteMessage
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
