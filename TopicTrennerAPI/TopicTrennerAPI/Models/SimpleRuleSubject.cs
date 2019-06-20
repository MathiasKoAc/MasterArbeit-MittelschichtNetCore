using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TopicTrennerAPI.Models
{
    public class SimpleRuleSubject
    {
        public int ID { get; set; }
        public Subject Subject { get; set; }
        public SimpleRule SimpleRule { get; set; }
        public EnumBindingTyp BindingTyp {get; set; }
    }
}
