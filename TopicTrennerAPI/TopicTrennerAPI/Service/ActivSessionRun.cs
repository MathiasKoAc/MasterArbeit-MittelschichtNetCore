using Microsoft.AspNetCore.Hosting;
using System;
using System.Linq;
using TopicTrennerAPI.Data;
using TopicTrennerAPI.Interfaces;

namespace TopicTrennerAPI.Service
{
    public class ActivSessionRun : IFindSessionRun
    {
        readonly DbTopicTrennerContext _dbContext;

        public ActivSessionRun(DbTopicTrennerContext dbContexT, IApplicationLifetime applicationLifttime)
        {
            _dbContext = dbContexT;
            applicationLifttime.ApplicationStopping.Register(OnStopApplication);
            Console.WriteLine("ActivSessionRun Started");
        }

        public int GetActivSessionRun()
        {
            var sessionRun = _dbContext.SessionRuns.Where(sr => sr.Active == true).First();
            if(sessionRun != null)
            {
                return sessionRun.ID;
            }
            return int.MinValue;
        }

        private void OnStopApplication()
        {
            Console.WriteLine("ActivSessionRun Stopped");
        }
    }
}
