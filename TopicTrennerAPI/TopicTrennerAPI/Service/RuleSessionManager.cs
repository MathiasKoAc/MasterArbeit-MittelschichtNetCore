using TopicTrennerAPI.Interfaces;
using TopicTrennerAPI.Models;

namespace TopicTrennerAPI.Service
{
    public class RuleSessionManager : IControlRuleSessions
    {
        readonly ILoadTopicRules TopicRuleLoader;
        readonly IRuleEvaluation RuleEvaluation;
        int _sessionId = int.MinValue;

        public RuleSessionManager(ILoadTopicRules topicRuleLoader, IRuleEvaluation setupEvaluationRules)
        {
            TopicRuleLoader = topicRuleLoader;
            RuleEvaluation = setupEvaluationRules;
        }

        public void StartRuleSession(int sessionId)
        {
            RuleEvaluation.SetTopicRulesAccess(TopicRuleLoader.LoadRules(sessionId, EnumSimpleRuleTyp.access));
            RuleEvaluation.SetTopicRulesDenyIn(TopicRuleLoader.LoadRules(sessionId, EnumSimpleRuleTyp.denyIn));
            RuleEvaluation.SetTopicRulesDenyOut(TopicRuleLoader.LoadRules(sessionId, EnumSimpleRuleTyp.denyOut));
            _sessionId = sessionId;
            RuleEvaluation.SetRuleEvaluationActive(true);
        }

        public bool StopRuleSession(int sessionId)
        {
            if (_sessionId != int.MinValue && _sessionId != sessionId)
            {
                return false;
            }
            RuleEvaluation.SetRuleEvaluationActive(false);
            _sessionId = int.MinValue;
            return true;
        }

        public bool ReloadRules(int sessionId)
        {
            if (_sessionId != int.MinValue && _sessionId != sessionId)
            {
                return false;
            }
            StartRuleSession(sessionId);
            return true;
        }

        public int GetSessionId()
        {
            return _sessionId;
        }
    }
}
