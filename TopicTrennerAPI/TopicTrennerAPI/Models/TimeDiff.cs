using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

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
