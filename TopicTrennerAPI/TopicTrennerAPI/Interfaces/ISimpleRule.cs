
namespace TopicTrennerAPI.Interfaces
{
    public interface ISimpleRule
    {
        string GetInTopic();
        string GetOutTopic();
        bool IsActive();
    }
}
