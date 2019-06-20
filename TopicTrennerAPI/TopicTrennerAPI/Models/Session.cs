using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TopicTrennerAPI.Models
{
    public class Session
    {
        public int ID { get; set; }
        public List<SessionSimpleRule> sessionSimpleRules { get; set; }
        public List<Log> logs { get; set; }
    }
}
