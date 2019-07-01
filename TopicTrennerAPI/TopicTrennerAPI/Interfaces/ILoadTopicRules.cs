using System;
using System.Collections.Generic;
using TopicTrennerAPI.Models;

namespace TopicTrennerAPI.Interfaces
{
    public interface ILoadTopicRules
    {
        Dictionary<string, TopicVertex> LoadRules(int sessionId, EnumSimpleRuleTyp ruleTyp = EnumSimpleRuleTyp.access);
    }
}
