using System;
using System.Collections.Generic;
using TopicTrennerAPI.Interfaces;

namespace TopicTrennerAPI.Models
{
    /// <summary>
    /// Events sind Sinmulations Happenings die Werte auf MQTT setzen.
    /// Events koennen per Hand oder Automatisch getriggert werden.
    /// </summary>
    public class EventMessage : MqttMessage, ISimpleRule, IEquatable<EventMessage>
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

        public bool Equals(EventMessage other)
        {
            if(other.ID == this.ID)
            {
                return true;
            }
            else if(other.ID <= 0 || this.ID <= 0)
            {
                return SessionId == other.SessionId && Topic == other.Topic && VonDate == other.VonDate && BisDate == other.BisDate && ZeitAbstand == other.ZeitAbstand;
            }
            return false;
        }

        public string GetInTopic()
        {
            return Topic;
        }

        public string GetOutTopic()
        {
            return Topic;
        }

        public bool IsActive()
        {
            return Activ;
        }
    }
}
