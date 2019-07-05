using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TopicTrennerAPI.Models;

namespace TopicTrennerAPI.Interfaces
{
    public interface IRuleEvaluation
    {
        void SetTopicRulesAccess(Dictionary<string, TopicVertex> accessRules);
        void SetTopicRulesDenyIn(Dictionary<string, TopicVertex> denyInRules);
        void SetTopicRulesDenyOut(Dictionary<string, TopicVertex> denyOutRules);

        bool IsRuleEvaluationActive();
        void SetRuleEvaluationActive(bool active);
    }
}
