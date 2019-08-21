using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace TopicTrennerAPI.Models
{
    public class TimeDiff
    {
        [ForeignKey("SessionRun")]
        public int ID { get; set; }
        public virtual SessionRun SessionRun { get; set; }
        public TimeSpan Diff { get; set; }
    }
}
