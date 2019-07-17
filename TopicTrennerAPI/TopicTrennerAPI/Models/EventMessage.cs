using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TopicTrennerAPI.Models
{
    /// <summary>
    /// Events sind Sinmulations Happenings die Werte auf MQTT setzen.
    /// Events koennen per Hand oder Automatisch getriggert werden.
    /// </summary>
    public class EventMessage : MqttMessage
    {
        public int ID { get; set; }
        public bool FireOnce { get; set; }
        public DateTime VonDate { get; set; }
        public DateTime BisDate { get; set; }
        public TimeSpan ZeitAbstand { get; set; }
        public EnumActivityTyp ActivTyp { get; set; }
        public int SessionId { get; set; }
        public Session Session { get; set; }
    }
}
