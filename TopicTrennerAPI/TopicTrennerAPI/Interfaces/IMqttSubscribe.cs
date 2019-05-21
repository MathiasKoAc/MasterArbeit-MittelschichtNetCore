namespace TopicTrennerAPI.Interfaces
{
    interface IMqttSubscribe
    {
        void AddTopicReceiver(string topic, IMqttTopicReceiver receiver, EnumMqttQualityOfService MqttQOS_Level = EnumMqttQualityOfService.QOS_LEVEL_EXACTLY_ONCE);
        void AddTopicReceiver(string topic, IMqttTopicReceiver receiver, byte MqttQOS_Level);
    }
}
