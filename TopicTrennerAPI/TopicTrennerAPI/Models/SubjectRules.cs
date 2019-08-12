using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TopicTrennerAPI.Models
{
    public class SubjectRules
    {
        public int SubjectID { get; set; }
        public EnumBindingTyp BindingTyp { get; set; }
        public List<int> SimpleRuleIDs { get; set; }

    }
}
