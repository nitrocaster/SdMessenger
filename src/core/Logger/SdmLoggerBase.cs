﻿using System;

namespace Sdm.Core
{
    public abstract class SdmLoggerBase : ILogger
    {
        public event Action<string> MessageLogged;
        public event Action LogCleared;

        protected void OnMessageLogged(string msg)
        {
            if (MessageLogged != null)
                MessageLogged(msg);
        }

        protected void OnLogCleaned()
        {
            if (LogCleared != null)
                LogCleared();
        }

        protected static readonly string DateTimeFormat = "dd.MM.yy-HH:mm:ss";
        protected LogLevel MinLogLevel;

        protected SdmLoggerBase(LogLevel minLogLevel)
        {
            MinLogLevel = minLogLevel;
        }

        public abstract int LineCount { get; }
        public abstract void Log(LogLevel logLevel, string message);
        public abstract void Clear();
        public abstract void Flush();
        public abstract void Dispose();
    }
}