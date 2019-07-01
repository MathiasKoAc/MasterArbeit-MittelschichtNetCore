using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TopicTrennerAPI.Data;
using TopicTrennerAPI.Interfaces;
using TopicTrennerAPI.Models;

namespace TopicTrennerAPI.Service
{
    public class DbRuleLoader : ILoadTopicRules
    {
        ICreateTopicRulesFromSimpleRules topicRuleCreater;
        DbTopicTrennerContext dbContext;

        public DbRuleLoader(ICreateTopicRulesFromSimpleRules topicRuleCreaterSR, DbTopicTrennerContext dbContexT)
        {
            topicRuleCreater = topicRuleCreaterSR;
            dbContext = dbContexT;
        }

        public Dictionary<string, TopicVertex> LoadRules(int sessionId, EnumSimpleRuleTyp ruleTyp = EnumSimpleRuleTyp.access)
        {
            ///dicTv key ist TopicVertex.TopicChain also die TopicPartsKette bis inkl diesem TopicPart
            Dictionary<string, TopicVertex> topicRules = new Dictionary<string, TopicVertex>();

            topicRuleCreater.CreateTopicRulesFromSimpleRules(ref topicRules, ReadToSimpleRuleList(sessionId, ruleTyp));
            return topicRules;
        }

        private List<SimpleRule> ReadToSimpleRuleList(int sessionId, EnumSimpleRuleTyp ruleTyp = EnumSimpleRuleTyp.access)
        {
            var rules = dbContext.Rules.Where(r => r.SessionID == sessionId && r.SimpleRuleTyp.Equals(ruleTyp));
            return rules.ToList();
        }
    }
}
