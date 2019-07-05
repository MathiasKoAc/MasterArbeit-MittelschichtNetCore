using TopicTrennerAPI.Interfaces;
using TopicTrennerAPI.Models;

namespace TopicTrennerAPI.Service
{
    public class RuleSessionManager : IControlRuleSessions
    {
        ILoadTopicRules TopicRuleLoader;
        ISetupEvaluationRules RuleEvaluation;
        int _sessionId = int.MinValue;

        public RuleSessionManager(ILoadTopicRules topicRuleLoader, ISetupEvaluationRules setupEvaluationRules)
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
