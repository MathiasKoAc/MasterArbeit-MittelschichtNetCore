using System;
using System.Linq;
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

        public void ReloadTimeService(int sessionRunId)
        {
            var diffCall = _dbcontext.TimeDiffs.Where(t => t.ID == sessionRunId);

            if(diffCall.Count() > 0) {
                var diff = diffCall.First();
                TimeSpan d = (diff == null || diff.Diff == null) ? TimeSpan.Zero : diff.Diff;
                ReloadTimeService(sessionRunId, d);
            }
        }

        public void ReloadTimeService(int sessionRunId, TimeSpan timeDiff)
        {
            _serveTime.SetTimeDiff(timeDiff);

            if (!_serveTime.IsTimeServiceActive())
            {
                _serveTime.SetTimeServiceActive(true);
            }
        }

        public void StartTimeService(int sessionRunId)
        {
            var diff = _dbcontext.TimeDiffs.Find(sessionRunId);
            TimeSpan d = (diff == null || diff.Diff == null) ? TimeSpan.Zero : diff.Diff;
            _serveTime.SetTimeServiceActive(true, d);
        }

        public void StartTimeService(int sessionRunId, TimeSpan timeDiff)
        {
            _serveTime.SetTimeServiceActive(true, timeDiff);
        }

        public void StopTimeService(int sessionRunId)
        {
            _serveTime.SetTimeServiceActive(false);
        }
    }
}
