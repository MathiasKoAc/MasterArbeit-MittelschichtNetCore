using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TopicTrennerAPI.Models;

namespace TopicTrennerAPI.Interfaces
{
    public interface IServeEvents
    {
        void SetTimeDiff(TimeSpan timeSpan);
        void SetAllEvents(List<EventMessage> eventMessages);
        bool IsEventServiceActive(int sessionRunId = -1);
        bool SetEventServiceActive(bool active, int sessionRunId = -1);
        bool FireOnceEvent(EventMessage eventM);
    }
}
