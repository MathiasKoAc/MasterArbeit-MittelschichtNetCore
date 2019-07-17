using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace TopicTrennerAPI.Models
{
    public class PurposeMessage : MqttMessage
    {
        public int ID { get; set; }
        public EnumPurposeMessageTyp MessageTyp { get; set; }
        public int SubjectId { get; set; }
        public Subject Subject { get; set; }
    }
}
