using System;

namespace libNitroStream
{
    public static class Logger
    {
        public delegate void LoggedEventHandler(object sender, LogEventArgs e);
        public static event LoggedEventHandler Logged;

        public static void Log(string message)
        {
            Logged(null, new LogEventArgs(message));
        }

    }

    public class LogEventArgs : EventArgs
    {
        public string Message { get; private set; }
        public LogEventArgs(string message)
        {
            Message = message;
        }
    }
}

