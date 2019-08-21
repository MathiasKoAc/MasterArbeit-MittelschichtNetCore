using System.Collections.Generic;
using TopicTrennerAPI.Models;

namespace TopicTrennerAPI.Interfaces
{
    public interface IServeRuleEvaluation
    {
        void SetTopicRulesAccess(Dictionary<string, TopicVertex> accessRules);
        void SetTopicRulesDenyIn(Dictionary<string, TopicVertex> denyInRules);
        void SetTopicRulesDenyOut(Dictionary<string, TopicVertex> denyOutRules);

        bool IsRuleEvaluationActive();
        void SetRuleEvaluationActive(bool active);
    }
}
