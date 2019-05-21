using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TopicTrennerAPI.Interfaces;

namespace TopicTrennerAPI.Service
{
    public class MqttConnectorService : IMqttPublish, IMqttSubscribe
    {
        public void AddTopicReceiver(string topic, IMqttTopicReceiver receiver, EnumMqttQualityOfService MqttQOS_Level = EnumMqttQualityOfService.QOS_LEVEL_EXACTLY_ONCE)
        {
            throw new NotImplementedException();
        }

        public void AddTopicReceiver(string topic, IMqttTopicReceiver receiver, byte MqttQOS_Level)
        {
            throw new NotImplementedException();
        }

        public void PublishMessage(string topic, string Message, EnumMqttQualityOfService MqttQOS_Level = EnumMqttQualityOfService.QOS_LEVEL_EXACTLY_ONCE, bool retain = false)
        {
            throw new NotImplementedException();
        }

        public void PublishMessage(string topic, string Message, byte MqttQOS_Level, bool retain = false)
        {
            throw new NotImplementedException();
        }
    }
}
