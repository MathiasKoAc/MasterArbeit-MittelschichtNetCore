using System;
using System.Collections.Generic;
using TopicTrennerAPI.Models;

namespace TopicTrennerAPI.Interfaces
{
    public interface ICreateTopicRulesFromSimpleRules
    {
        void CreateTopicRulesFromSimpleRule(ref Dictionary<string, TopicVertex> topicRules, SimpleRule simpleRule);
        void CreateTopicRulesFromSimpleRules(ref Dictionary<string, TopicVertex> topicRules, List<SimpleRule> simpleRules);
    }
}
