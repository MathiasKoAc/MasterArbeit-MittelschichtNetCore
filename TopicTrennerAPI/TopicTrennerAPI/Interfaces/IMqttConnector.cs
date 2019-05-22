using TopicTrennerAPI.Models;

namespace TopicTrennerAPI.Interfaces
{
    public interface IMqttConnector
    {
        void PublishMessage(string topic, string Message, EnumMqttQualityOfService MqttQOS_Level = EnumMqttQualityOfService.QOS_LEVEL_EXACTLY_ONCE, bool retain = false);
        void PublishMessage(string topic, string Message, byte MqttQOS_Level, bool retain = false);
        void AddTopicReceiver(string topic, IMqttTopicReceiver receiver, EnumMqttQualityOfService MqttQOS_Level = EnumMqttQualityOfService.QOS_LEVEL_EXACTLY_ONCE);
        void AddTopicReceiver(string topic, IMqttTopicReceiver receiver, byte MqttQOS_Level);
        string Hello();
    }
}
