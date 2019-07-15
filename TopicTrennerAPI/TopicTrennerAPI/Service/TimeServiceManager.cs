using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TopicTrennerAPI.Interfaces;
using TopicTrennerAPI.Data;


namespace TopicTrennerAPI.Service
{
    public class TimeServiceManager : IManageTimeService
    {
        readonly DbTopicTrennerContext _dbcontext;
        readonly IServeTime _serveTime;


        public TimeServiceManager(DbTopicTrennerContext dbContexT, IServeTime serveTime)
        {
            _dbcontext = dbContexT;
            _serveTime = serveTime;
        }

        public void ReloadTimeService(int SessionRunId)
        {
            var diff = _dbcontext.TimeDiffs.Where(t => t.ID == SessionRunId).First();
            ReloadTimeService(SessionRunId, diff.Diff);
        }

        public void ReloadTimeService(int SessionRunId, TimeSpan timeDiff)
        {
            _serveTime.SetTimeDiff(timeDiff);

            if (!_serveTime.IsTimeServiceActive())
            {
                _serveTime.SetTimeServiceActive(true);
            }
        }

        public void StartTimeService(int SessionRunId)
        {
            var diff = _dbcontext.TimeDiffs.Where(t => t.ID == SessionRunId).First();
            _serveTime.SetTimeServiceActive(true, diff.Diff);
        }

        public void StartTimeService(int SessionRunId, TimeSpan timeDiff)
        {
            _serveTime.SetTimeServiceActive(true, timeDiff);
        }

        public void StopTimeService(int SessionRunId)
        {
            _serveTime.SetTimeServiceActive(false);
        }
    }
}
