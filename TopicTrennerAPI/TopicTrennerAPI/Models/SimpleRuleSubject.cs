using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TopicTrennerAPI.Models
{
    public class SimpleRuleSubject
    {
        public int ID { get; set; }
        public Subject subject { get; set; }
        public SimpleRule simpleRule { get; set; }
        public EnumBindingTyp bindingTyp {get; set; }
    }
}
