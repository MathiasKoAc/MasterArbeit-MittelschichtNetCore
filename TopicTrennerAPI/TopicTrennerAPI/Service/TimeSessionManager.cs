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
        DbTopicTrennerContext dbcontext;

        public TimeSessionManager(DbTopicTrennerContext dbContexT)
        {
            this.dbcontext = dbContexT;
        }

        public void ReloadTimeService(int SessionId)
        {
            throw new NotImplementedException();
        }

        public void ReloadTimeService(int SessionId, TimeSpan timeDiff)
        {
            throw new NotImplementedException();
        }

        public void StartTimeService(int SessionId)
        {
            throw new NotImplementedException();
        }

        public void StartTimeService(int SessionId, TimeSpan timeDiff)
        {
            throw new NotImplementedException();
        }

        public void StopTimeService(int SessionId)
        {
            throw new NotImplementedException();
        }
    }
}
