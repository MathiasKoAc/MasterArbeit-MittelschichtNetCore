using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TopicTrennerAPI.Interfaces
{
    interface IControlRules
    {
        void StartRuleService();
        void StopRuleService();
        void ReloadRules();
    }
}
