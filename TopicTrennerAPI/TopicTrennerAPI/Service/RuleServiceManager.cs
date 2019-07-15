using TopicTrennerAPI.Interfaces;
using TopicTrennerAPI.Models;
using TopicTrennerAPI.Data;
using System.Collections.Generic;
using System.Linq;

namespace TopicTrennerAPI.Service
{
    public class RuleServiceManager : IManageRuleService
    {
        readonly ICreateTopicRulesFromSimpleRules topicRuleCreater;
        readonly DbTopicTrennerContext dbContext;
        readonly IServeRuleEvaluation ruleEvaluation;

        public RuleServiceManager(ICreateTopicRulesFromSimpleRules ruleCreater, DbTopicTrennerContext dbContexT, IServeRuleEvaluation setupEvaluationRules)
        {
            topicRuleCreater = ruleCreater;
            dbContext = dbContexT;
            ruleEvaluation = setupEvaluationRules;
        }

        public void StartRuleSession(int sessionRunId)
        {
            ruleEvaluation.SetTopicRulesAccess(LoadRules(sessionRunId, EnumSimpleRuleTyp.access));
            ruleEvaluation.SetTopicRulesDenyIn(LoadRules(sessionRunId, EnumSimpleRuleTyp.denyIn));
            ruleEvaluation.SetTopicRulesDenyOut(LoadRules(sessionRunId, EnumSimpleRuleTyp.denyOut));
            ruleEvaluation.SetRuleEvaluationActive(true);
        }

        public bool StopRuleSession(int sessionRunId)
        {
            ruleEvaluation.SetRuleEvaluationActive(false);
            return true; //TODO rückgaben lösen
        }

        public bool ReloadRules(int sessionRunId)
        {
            StartRuleSession(sessionRunId);
            return true; //TODO rückgaben lösen
        }

        private Dictionary<string, TopicVertex> LoadRules(int sessionRunId, EnumSimpleRuleTyp ruleTyp = EnumSimpleRuleTyp.access)
        {
            ///dicTv key ist TopicVertex.TopicChain also die TopicPartsKette bis inkl diesem TopicPart
            Dictionary<string, TopicVertex> topicRules = new Dictionary<string, TopicVertex>();

            var sessionId = dbContext.SessionRuns.Where(s => s.ID == sessionRunId).First().SessionID;

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
