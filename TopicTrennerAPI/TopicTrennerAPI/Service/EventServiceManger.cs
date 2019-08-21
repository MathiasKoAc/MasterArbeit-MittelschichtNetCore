using System.Collections.Generic;
using System.Linq;
using TopicTrennerAPI.Interfaces;
using TopicTrennerAPI.Models;
using TopicTrennerAPI.Data;


namespace TopicTrennerAPI.Service
{
    public class EventServiceManger : IManageEventService
    {
        readonly DbTopicTrennerContext _dbContext;
        readonly IServeEvents _eventService;

        public EventServiceManger(IServeEvents eventService, DbTopicTrennerContext dbContexT)
        {
            _dbContext = dbContexT;
            _eventService = eventService;
        }

        public void StartEventService(int sessionRunId)
        {
            SessionRun session = _dbContext.SessionRuns.Where(s => s.ID == sessionRunId).First();
            List <EventMessage> events = _dbContext.Events.Where(e => e.SessionId == session.SessionID && e.Enabled).ToList();

            _eventService.SetAllEvents(events);
            _eventService.SetEventServiceActive(true, sessionRunId);
        }

        public bool StopEventService(int sessionRunId)
        {
            return _eventService.SetEventServiceActive(false, sessionRunId);
        }

        public bool ReloadEventService(int sessionRunId)
        {
            _eventService.SetEventServiceActive(false);
            StartEventService(sessionRunId);
            return true;
        }

        public bool FireOnceEvent(int eventId)
        {
            EventMessage eventM = _dbContext.Events.Where(eM => eM.ID == eventId).First();
            return _eventService.FireOnceEvent(eventM);
        }
    }
}
