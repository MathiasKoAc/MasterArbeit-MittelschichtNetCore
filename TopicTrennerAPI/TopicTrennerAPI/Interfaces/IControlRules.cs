using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TopicTrennerAPI.Interfaces
{
    interface IControlRules
    {
        void StartRuleService(int sessionId);
        void StopRuleService(int sessionId);
        void ReloadRules(int sessionId);
    }
}
