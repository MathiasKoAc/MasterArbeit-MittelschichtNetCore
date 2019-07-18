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
        public int SessionId { get; set; }
        public Session Session { get; set; }
        /// <summary>
        /// If it is Enabled the Event is albe to become activ
        /// If it is Disabled the Event is not albe to do anything
        /// </summary>
        public bool Enabled { get; set; }
        public bool Activ { get; set; }

        /// <summary>
        /// If it is deactiv and automatic, vonDate can starts it and make it activ
        /// If it is activ and automatic, bisDate can stops it and make it deactiv
        /// If it is not automatic, vonDate and bisDate can do nothing
        /// </summary>
        public bool Automatic { get; set; }

    }
}
