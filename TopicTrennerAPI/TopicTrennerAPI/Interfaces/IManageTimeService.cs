using System;

namespace TopicTrennerAPI.Interfaces
{
    public interface IManageTimeService
    {
        void StartTimeService(int SessionRunId);
        void StartTimeService(int SessionRunId, TimeSpan timeDiff);
        void StopTimeService(int SessionRunId);
        void ReloadTimeService(int SessionRunId);
        void ReloadTimeService(int SessionRunId, TimeSpan timeDiff);
    }
}
