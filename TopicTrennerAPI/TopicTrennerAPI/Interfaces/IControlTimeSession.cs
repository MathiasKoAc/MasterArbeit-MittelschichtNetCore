using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TopicTrennerAPI.Interfaces
{
    interface IControlTimeSession
    {
        void StartTimeService(int SessionId);
        void StartTimeService(int SessionId, TimeSpan timeDiff);
        void StopTimeService(int SessionId);
        void ReloadTimeService(int SessionId);
        void ReloadTimeService(int SessionId, TimeSpan timeDiff);
    }
}
