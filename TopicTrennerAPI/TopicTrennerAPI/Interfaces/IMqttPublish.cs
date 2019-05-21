namespace TopicTrennerAPI.Interfaces
{
    interface IMqttPublish
    {
        void PublishMessage(string topic, string Message, EnumMqttQualityOfService MqttQOS_Level = EnumMqttQualityOfService.QOS_LEVEL_EXACTLY_ONCE, bool retain = false);
        void PublishMessage(string topic, string Message, byte MqttQOS_Level, bool retain = false);
    }
}
