using System;

namespace TopicTrennerAPI.Models
{
    public class TimedEventMessage : EventMessage
    {
        public DateTime LastFired { get; set; }
    }
}
