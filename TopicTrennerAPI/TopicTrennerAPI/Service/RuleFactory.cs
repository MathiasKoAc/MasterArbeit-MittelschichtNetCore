using TopicTrennerAPI.Interfaces;
using TopicTrennerAPI.Models;

namespace TopicTrennerAPI.Service
{
    public class AlgoRuleFactory : IAlgoRuleFactory
    {
        IAlgoRule IAlgoRuleFactory.CreateRule(TopicVertex inTopic, ISimpleRule rule)
        {
            return new Rule(inTopic, rule.GetOutTopic(), rule.IsActive());
        }
    }
}
