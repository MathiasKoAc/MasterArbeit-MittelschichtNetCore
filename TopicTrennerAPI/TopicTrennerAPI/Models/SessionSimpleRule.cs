using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TopicTrennerAPI.Models
{
    public class SessionSimpleRule
    {
        public int ID { get; set; }
        public Session session { get; set; }
        public SimpleRule simpleRule { get; set; }
    }
}
