using TopicTrennerAPI.Interfaces;

namespace TopicTrennerAPI.Models
{
    public class MqttMessage
    {
        public string Topic { get; set; }
        public string Message { get; set; }
        public byte QosLevel { get; set; }
        public bool Retain { get; set; }
    }
}
