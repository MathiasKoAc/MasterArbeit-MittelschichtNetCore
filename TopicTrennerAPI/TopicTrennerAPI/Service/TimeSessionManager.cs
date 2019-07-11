using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TopicTrennerAPI.Interfaces;
using TopicTrennerAPI.Data;


namespace TopicTrennerAPI.Service
{
    public class TimeSessionManager : IControlTimeSession
    {
        readonly DbTopicTrennerContext _dbcontext;
        readonly IServeTime _serveTime;


        public TimeSessionManager(DbTopicTrennerContext dbContexT, IServeTime serveTime)
        {
            _dbcontext = dbContexT;
            _serveTime = serveTime;
        }

        public void ReloadTimeService(int SessionId)
        {
            var diff = _dbcontext.TimeDiffs.Where(t => t.ID == SessionId).First();
            ReloadTimeService(SessionId, diff.Diff);
        }

        public void ReloadTimeService(int SessionId, TimeSpan timeDiff)
        {
            _serveTime.SetTimeDiff(timeDiff);

            if (!_serveTime.IsTimeServiceActive())
            {
                _serveTime.SetTimeServiceActive(true);
            }
        }

        public void StartTimeService(int SessionId)
        {
            var diff = _dbcontext.TimeDiffs.Where(t => t.ID == SessionId).First();
            _serveTime.SetTimeServiceActive(true, diff.Diff);
        }

        public void StartTimeService(int SessionId, TimeSpan timeDiff)
        {
            _serveTime.SetTimeServiceActive(true, timeDiff);
        }

        public void StopTimeService(int SessionId)
        {
            _serveTime.SetTimeServiceActive(false);
        }
    }
}
