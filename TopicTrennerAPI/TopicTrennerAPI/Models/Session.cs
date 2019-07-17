using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TopicTrennerAPI.Models
{
    public class Session
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Beschreibung { get; set; }
        public DateTime DateCreated { get; set; } 
        public List<SessionRun> SessionRuns { get; set; }
        public List<SimpleRule> SimpleRules { get; set; }
        public List<EventMessage> Events { get; set; }

        public bool IsActive()
        {
            if(SessionRuns != null)
            {
                foreach (SessionRun sr in SessionRuns)
                {
                    if (sr.Active)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
