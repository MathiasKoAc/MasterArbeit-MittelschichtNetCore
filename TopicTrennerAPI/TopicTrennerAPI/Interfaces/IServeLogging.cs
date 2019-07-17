using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TopicTrennerAPI.Models;
using TopicTrennerAPI.Data;

namespace TopicTrennerAPI.Interfaces
{
    public interface IServeLogging
    {
        bool IsLoggingActive();
        void SetLoggingActive(int sessionRunId, bool active);
    }
}
