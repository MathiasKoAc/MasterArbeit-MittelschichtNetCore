using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TopicTrennerAPI.Models
{
    public class Subject
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public EnumSubjectTyp SubjectTyp { get; set; }
        public List<SimpleRuleSubject> SimpleRuleSubjects { get; set; }
    }
}
