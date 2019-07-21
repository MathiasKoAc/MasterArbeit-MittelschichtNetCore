using System;

namespace TopicTrennerAPI.Models
{
    public class TimedEventMessage : EventMessage
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
    }
}
