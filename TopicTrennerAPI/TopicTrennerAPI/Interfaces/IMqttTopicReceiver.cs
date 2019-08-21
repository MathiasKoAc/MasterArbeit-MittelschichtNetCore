
namespace TopicTrennerAPI.Interfaces
{
    public interface IMqttTopicReceiver
    {
        void OnReceivedMessage(string topic, byte[] message);
    }
}
