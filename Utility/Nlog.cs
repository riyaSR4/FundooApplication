﻿using NLog;
using System;

namespace Utility
{
    public class Nlog
    {
        Logger logger = LogManager.GetCurrentClassLogger();
        public void LogInfo(string message)
        {
            logger.Info(message);
        }

        public void LogError(string message)
        {
            logger.Error(message);
        }
    }
}
