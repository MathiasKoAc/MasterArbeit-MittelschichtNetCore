using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TopicTrennerAPI.Models;

namespace TopicTrennerAPI.Interfaces
{
    public interface IManageEventService
    {

        void StartEventService(int sessionRunId);

        bool StopEventService(int sessionRunId);

        bool ReloadEventService(int sessionRunId);

        bool FireOnceEvent(int eventId);
    }
}
