using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TopicTrennerAPI.Interfaces
{
    public interface IServeTime
    {
        void SetTimeDiff(TimeSpan timeDiff);
        bool IsTimeServiceActive();
        void SetTimeServiceActive(bool active);
        void SetTimeServiceActive(bool active, TimeSpan timeDiff);
    }
}
