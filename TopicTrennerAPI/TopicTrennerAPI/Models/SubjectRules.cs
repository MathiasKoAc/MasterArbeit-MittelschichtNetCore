using System.Collections.Generic;

namespace TopicTrennerAPI.Models
{
    public class SubjectRules
    {
        public int SubjectID { get; set; }
        public EnumBindingTyp BindingTyp { get; set; }
        public List<int> SimpleRuleIDs { get; set; }

    }
}
