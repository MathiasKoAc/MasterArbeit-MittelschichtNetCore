using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TopicTrennerAPI.Models
{
    public class Session
    {
        public int ID { get; set; }
        public List<SessionSimpleRule> SessionSimpleRules { get; set; }
        public DateTime DateCreated { get; set; } 
        public List<SessionRun> SessionRuns { get; set; }

        public bool IsActive()
        {
            foreach(SessionRun sr in SessionRuns)
            {
                if(sr.Active)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
