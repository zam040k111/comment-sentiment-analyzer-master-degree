﻿using GameStore.BLL.Interfaces;
using NLog;

namespace GameStore.BLL.Logger
{
    public class NLogger : INLog
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        public void Information(string message)
        {
            Logger.Info(message);
        }

        public void Warning(string message)
        {
            Logger.Warn(message);
        }

        public void Debug(string message)
        {
            Logger.Debug(message);
        }

        public void Error(string message)
        {
            Logger.Error(message);
        }
    }
}
