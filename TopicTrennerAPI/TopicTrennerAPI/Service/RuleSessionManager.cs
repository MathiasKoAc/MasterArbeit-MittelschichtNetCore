using TopicTrennerAPI.Interfaces;
using TopicTrennerAPI.Models;
using TopicTrennerAPI.Data;
using System.Collections.Generic;
using System.Linq;

namespace TopicTrennerAPI.Service
{
    public class RuleSessionManager : IControlRuleSessions
    {
        readonly ICreateTopicRulesFromSimpleRules topicRuleCreater;
        readonly DbTopicTrennerContext dbContext;
        readonly IRuleEvaluation ruleEvaluation;
        int _sessionId = int.MinValue;

        public RuleSessionManager(ICreateTopicRulesFromSimpleRules ruleCreater, DbTopicTrennerContext dbContexT, IRuleEvaluation setupEvaluationRules)
        {
            topicRuleCreater = ruleCreater;
            dbContext = dbContexT;
            ruleEvaluation = setupEvaluationRules;
        }

        public void StartRuleSession(int sessionId)
        {
            ruleEvaluation.SetTopicRulesAccess(LoadRules(sessionId, EnumSimpleRuleTyp.access));
            ruleEvaluation.SetTopicRulesDenyIn(LoadRules(sessionId, EnumSimpleRuleTyp.denyIn));
            ruleEvaluation.SetTopicRulesDenyOut(LoadRules(sessionId, EnumSimpleRuleTyp.denyOut));
            _sessionId = sessionId;
            ruleEvaluation.SetRuleEvaluationActive(true);
        }

        public bool StopRuleSession(int sessionId)
        {
            if (_sessionId != int.MinValue && _sessionId != sessionId)
            {
                return false;
            }
            ruleEvaluation.SetRuleEvaluationActive(false);
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

        private Dictionary<string, TopicVertex> LoadRules(int sessionId, EnumSimpleRuleTyp ruleTyp = EnumSimpleRuleTyp.access)
        {
            ///dicTv key ist TopicVertex.TopicChain also die TopicPartsKette bis inkl diesem TopicPart
            Dictionary<string, TopicVertex> topicRules = new Dictionary<string, TopicVertex>();

            topicRuleCreater.CreateTopicRulesFromSimpleRules(ref topicRules, ReadToSimpleRuleList(sessionId, ruleTyp));
            return topicRules;
        }

        private List<SimpleRule> ReadToSimpleRuleList(int sessionId, EnumSimpleRuleTyp ruleTyp = EnumSimpleRuleTyp.access)
        {
            var rules = dbContext.Rules.Where(r => r.SessionID == sessionId && r.SimpleRuleTyp.Equals(ruleTyp) && r.Active == true);
            return rules.ToList();
        }
    }
}
