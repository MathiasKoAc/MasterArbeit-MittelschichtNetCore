using TopicTrennerAPI.Models;

namespace TopicTrennerAPI.Interfaces
{
    public interface IAlgoRule
    {
        void Setup(TopicVertex startVertex, string outTopic, bool active);
        void SetStartVertex(TopicVertex vertex);
        void SetOutTopic(string topic);
        void SetActiv(bool active);
    }
}
