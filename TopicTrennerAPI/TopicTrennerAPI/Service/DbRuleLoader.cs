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

        public DbRuleLoader(ICreateTopicRulesFromSimpleRules topicRuleCreaterSR, DbContext dbContexT)
        {
            topicRuleCreater = topicRuleCreaterSR;
            dbContext = (DbTopicTrennerContext) dbContexT;
        }

        public Dictionary<string, TopicVertex> LoadRules(int sessionId)
        {
            ///dicTv key ist TopicVertex.TopicChain also die TopicPartsKette bis inkl diesem TopicPart
            Dictionary<string, TopicVertex> topicRules = new Dictionary<string, TopicVertex>();

            topicRuleCreater.CreateTopicRulesFromSimpleRules(ref topicRules, ReadToSimpleRuleList(sessionId));
            return topicRules;
        }

        private List<SimpleRule> ReadToSimpleRuleList(int sessionId)
        {
            HashSet<SimpleRule> rules = new HashSet<SimpleRule>();

            var sessionSimpleRules = dbContext.SessionSimpleRules.Include(sr => sr.SimpleRule).Where(sr => sr.SessionID == sessionId);

            foreach(SessionSimpleRule ssr in sessionSimpleRules)
            {
                rules.Add(ssr.SimpleRule);
            }

            return rules.ToList<SimpleRule>();
        }
    }
}
