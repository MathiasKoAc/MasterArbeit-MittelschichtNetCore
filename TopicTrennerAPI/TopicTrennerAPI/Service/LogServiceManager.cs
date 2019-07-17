﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TopicTrennerAPI.Data;
using TopicTrennerAPI.Interfaces;

namespace TopicTrennerAPI.Service
{
    public class LogServiceManager : IManageLogService
    {
        readonly IServeLogging _logger;

        public LogServiceManager(IServeLogging logging)
        {
            _logger = logging;
        }

        public void StartLogService(int SessionRunId)
        {
            _logger.SetLoggingActive(SessionRunId, true);
        }

        public void StopLogService(int SessionRunId)
        {
            _logger.SetLoggingActive(SessionRunId, false);
        }
    }
}
