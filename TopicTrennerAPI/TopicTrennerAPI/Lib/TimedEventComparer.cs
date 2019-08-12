using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TopicTrennerAPI.Models;

namespace TopicTrennerAPI.Lib
{
    public class TimedEventComparer : IEqualityComparer<TimedEventMessage>
    {
        public bool Equals(TimedEventMessage x, TimedEventMessage y)
        {
            return x.Equals(y);
        }

        public int GetHashCode(TimedEventMessage obj)
        {
            return obj.ID;
        }
    }
}
