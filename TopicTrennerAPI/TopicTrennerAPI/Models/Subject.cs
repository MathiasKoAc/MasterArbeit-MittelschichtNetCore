using System.Collections.Generic;

namespace TopicTrennerAPI.Models
{
    public class Subject
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public EnumSubjectTyp SubjectTyp { get; set; }
        public List<SimpleRuleSubject> SimpleRuleSubjects { get; set; }
        public List<PurposeMessage> PurposeMessages { get; set; }
    }
}
