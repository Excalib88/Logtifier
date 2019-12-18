using System;
using System.Collections.Generic;
using System.Text;

namespace Logtifier.Core
{
    public class LogMessageEntry
    {
        public LogMessageEntry(string message, string timeStamp = null, string levelString = null, bool logAsError = false)
        {
            TimeStamp = timeStamp;
            LevelString = levelString;
            Message = message;
            LogAsError = logAsError;
        }

        public readonly string TimeStamp;
        public readonly string LevelString;
        public readonly string Message;
        public readonly bool LogAsError;
    }
}
