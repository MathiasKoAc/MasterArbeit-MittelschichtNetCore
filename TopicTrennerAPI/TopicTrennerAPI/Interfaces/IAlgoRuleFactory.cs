using TopicTrennerAPI.Models;

namespace TopicTrennerAPI.Interfaces
{
    public interface IAlgoRuleFactory
    {
        IAlgoRule CreateRule(TopicVertex inTopic, ISimpleRule rule);
    }
}
