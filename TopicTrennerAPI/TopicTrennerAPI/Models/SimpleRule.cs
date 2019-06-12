using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TopicTrennerAPI.Models
{
    public class SimpleRule
    {
        public string InTopic { get; set; }
        public string OutTopic { get; set; }
        public bool Active { get; set; }
    }
}
