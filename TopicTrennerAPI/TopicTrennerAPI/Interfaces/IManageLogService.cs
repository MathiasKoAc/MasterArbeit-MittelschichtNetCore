using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TopicTrennerAPI.Interfaces
{
    public interface IManageLogService
    {
        void StartLogService(int SessionRunId);
        void StopLogService(int SessionRunId);
    }
}
