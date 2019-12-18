using System;
using System.Threading.Tasks;

namespace Logtifier.Core
{
    public interface ILoggerProcessor: IDisposable
    {
        Task EnqueueMessage(LogMessageEntry message);
        Task DequeueMessage(LogMessageEntry message);
        Task ProcessLogQueue();
    }
}