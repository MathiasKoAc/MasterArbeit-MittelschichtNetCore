using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TopicTrennerAPI.Interfaces;

namespace TopicTrennerAPI.Service
{
    public class MqttConnectorService : IMqttConnector
    {

        public MqttConnectorService()
        {
        }

        public void AddTopicReceiver(string topic, IMqttTopicReceiver receiver, EnumMqttQualityOfService MqttQOS_Level = EnumMqttQualityOfService.QOS_LEVEL_EXACTLY_ONCE)
        {
            Console.WriteLine("AddTopicReceiver1");
        }

        public void AddTopicReceiver(string topic, IMqttTopicReceiver receiver, byte MqttQOS_Level)
        {
            Console.WriteLine("AddTopicReceiver2");
        }

        public void PublishMessage(string topic, string Message, EnumMqttQualityOfService MqttQOS_Level = EnumMqttQualityOfService.QOS_LEVEL_EXACTLY_ONCE, bool retain = false)
        {
            Console.WriteLine("PublishMessage1");
        }

        public void PublishMessage(string topic, string Message, byte MqttQOS_Level, bool retain = false)
        {
            Console.WriteLine("PublishMessage2");
        }

        public string Hello()
        {
            return "Hallo from MqttConnector";
        }
    }
}
