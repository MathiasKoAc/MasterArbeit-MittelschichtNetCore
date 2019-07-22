using System;

namespace TopicTrennerAPI.Models
{
    public class TimedEventMessage : EventMessage, IEquatable<TimedEventMessage>, IComparable<TimedEventMessage>
    {
        public DateTime LastFired { get; set; }

        public TimedEventMessage(EventMessage em)
        {
            this.Activ = em.Activ;
            this.Automatic = em.Automatic;
            this.BisDate = em.BisDate;
            this.Enabled = em.Enabled;
            this.FireOnce = em.FireOnce;
            this.ID = em.ID;
            this.Message = em.Message;
            this.QosLevel = em.QosLevel;
            this.Retain = em.Retain;
            this.SessionId = em.SessionId;
            this.Topic = em.Topic;
            this.VonDate = em.VonDate;
            this.ZeitAbstand = em.ZeitAbstand;

            this.LastFired = DateTime.MinValue;
        }

        public bool Equals(TimedEventMessage other)
        {
            if (other.ID == this.ID)
            {
                return true;
            }
            else if (other.ID <= 0 || this.ID <= 0)
            {
                return SessionId == other.SessionId && Topic == other.Topic && VonDate == other.VonDate && BisDate == other.BisDate && ZeitAbstand == other.ZeitAbstand;
            }
            return false;
        }

        public int CompareTo(TimedEventMessage other)
        {
            return this.ID - other.ID;
        }
    }
}
