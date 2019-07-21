using System;
using System.Collections.Generic;
using TopicTrennerAPI.Models;

namespace TopicTrennerAPI.Interfaces
{
    public interface ICreateTopicVertexFromTopics
    {
        void CreateTopicVertexFromTopic(ref Dictionary<string, TopicVertex> topicRules, ISimpleRule simpleRule, IAlgoRuleFactory ruleFactory);
        void CreateTopicVertexFromTopics(ref Dictionary<string, TopicVertex> topicRules, List<ISimpleRule> simpleRules, IAlgoRuleFactory ruleFactory);
    }
}
