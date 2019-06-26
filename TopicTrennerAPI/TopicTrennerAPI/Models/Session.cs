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
        public List<Log> Logs { get; set; }
        public DateTime DateCreated { get; set; } 
        //public DateTime DateUsed { get; set; }
    }
}
