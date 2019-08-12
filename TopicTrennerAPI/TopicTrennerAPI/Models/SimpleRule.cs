using System.Collections.Generic;
using TopicTrennerAPI.Interfaces;

namespace TopicTrennerAPI.Models
{
    public class SimpleRule : ISimpleRule
    {
        public int ID { get; set; }
        public string InTopic { get; set; }
        public string OutTopic { get; set; }
        public bool Active { get; set; }

        public List<SimpleRuleSubject> SimpleRuleSubjects { get; set; }
        public Session Session { get; set; }
        public int SessionID { get; set; }
        public EnumSimpleRuleTyp SimpleRuleTyp { get; set; }



        public string GetInTopic()
        {
            return InTopic;
        }

        public string GetOutTopic()
        {
            return OutTopic;
        }

        public bool IsActive()
        {
            return Active;
        }
    }
}
