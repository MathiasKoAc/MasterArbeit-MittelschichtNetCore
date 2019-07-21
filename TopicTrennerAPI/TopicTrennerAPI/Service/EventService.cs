using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using TopicTrennerAPI.Interfaces;
using TopicTrennerAPI.Models;

namespace TopicTrennerAPI.Service
{
    public class EventService : IServeEvents, IMqttTopicReceiver
    {
        readonly IMqttConnector _mqttCon;
        private volatile bool _active;
        private int _sessionRunId = int.MinValue;
        private TimeSpan _timeSpanDiff;
        private readonly int waitSecInTimeLoop;

        private HashSet<EventMessage> automaticEventStarts;
        private HashSet<TimedEventMessage> activTimedEvents;
        private Dictionary<string, HashSet<EventMessage>> activTopicBasedEvents;

        public EventService(IMqttConnector mqttConnector, IApplicationLifetime applicationLifttime, IConfiguration config)
        {
            automaticEventStarts = new HashSet<EventMessage>();
            activTimedEvents = new HashSet<TimedEventMessage>();
            activTopicBasedEvents = new Dictionary<string, HashSet<EventMessage>>();

            _mqttCon = mqttConnector;
            _mqttCon.AddTopicReceiver("#", this);
            applicationLifttime.ApplicationStopping.Register(OnStopApplication);
            StartEventLoop();
            waitSecInTimeLoop = config.GetValue<int>("WaitSecondsInEventLoop");
        }

        public bool FireOnceEvent(EventMessage eventM)
        {
            _mqttCon.PublishMessage(eventM.Topic, eventM.Message, eventM.QosLevel, eventM.Retain);
            return true;
        }

        public bool IsEventServiceActive(int sessionRunId = -1)
        {
            if(sessionRunId == -1 || sessionRunId == _sessionRunId)
            {
                return _active;
            }
            return false;
        }

        public void SetAllEvents(List<EventMessage> eventMessages)
        {
            //so weit DoublePuffer
            HashSet<EventMessage> autoEvents = new HashSet<EventMessage>();
            HashSet<TimedEventMessage> timedEvents = new HashSet<TimedEventMessage>();

            foreach(EventMessage em in eventMessages)
            {
                if(!em.Enabled)
                {
                    continue;
                }

                if(em.Automatic)
                {
                    autoEvents.Add(em);
                }

                if (em.Activ)
                {
                    if(em.ZeitAbstand == TimeSpan.Zero)
                    {
                        AddTopicBasedEvent(em);
                    }
                    else
                    {
                        timedEvents.Add(new TimedEventMessage(em));
                    }
                }
            }

            activTimedEvents = timedEvents;
            automaticEventStarts = autoEvents;
        }

        public bool SetEventServiceActive(bool active, int sessionRunId = -1)
        {
            //Service wurde schonmal gestartet und nun erneut gestartet / gestoppt
            if (sessionRunId == -1 && _sessionRunId != int.MinValue)
            {
                _active = active;
                return true;
            }

            //Service ist active und nun wird der status geändert
            if (_active && sessionRunId == _sessionRunId)
            {
                _active = active;
                return true;
            }

            //Service ist nicht activ und sessionRunId wir übermittelt
            if(!_active && sessionRunId != -1)
            {
                _sessionRunId = sessionRunId;
                _active = active;
                return true;
            }
            return false;
        }

        public void SetTimeDiff(TimeSpan timeSpan)
        {
            _timeSpanDiff = timeSpan;
        }

        private void StartEventLoop()
        {
            Task.Run(EventLoop);
        }

        private void EventLoop()
        {
            DateTime loopLast = DateTime.MinValue;
            DateTime loopNow;
            while(_active)
            {
                loopNow = DateTime.Now + _timeSpanDiff;
                ActivateDeactivateEventsByTime(loopNow, loopLast);
                FireEventsByTime(loopNow);
                loopLast = loopNow;
                Thread.Sleep(1000 * waitSecInTimeLoop);
            }
        }

        private void ActivateDeactivateEventsByTime(DateTime timeNow, DateTime timeLast)
        {
            foreach(EventMessage em in automaticEventStarts)
            {
                //Fall1
                if(timeLast < em.VonDate && timeNow > em.BisDate)
                {
                    FireOnceEvent(em);
                    automaticEventStarts.Remove(em);
                    RemoveTopicBasedEvent(em);
                }
                else 
                //Fall2
                if(timeLast > em.VonDate && timeNow < em.BisDate)
                {
                    SetEventActiveOrFireOnce(em);
                }
                else
                //Fall3
                if(timeLast < em.VonDate && timeNow < em.BisDate)
                {
                    SetEventActiveOrFireOnce(em);
                }
                else
                //Fall4
                if(timeLast > em.VonDate && timeNow > em.BisDate)
                {
                    em.Activ = false;
                    activTimedEvents.Remove((TimedEventMessage)em);
                    automaticEventStarts.Remove(em);
                    RemoveTopicBasedEvent(em);
                }
                else
                //Fall5
                if(timeNow < em.VonDate)
                {
                    em.Activ = false;
                }
                else
                //Fall6
                if(timeLast > em.BisDate)
                {
                    em.Activ = false;
                    activTimedEvents.Remove((TimedEventMessage)em);
                    automaticEventStarts.Remove(em);
                    RemoveTopicBasedEvent(em);
                }
            }
        }

        private void SetEventActiveOrFireOnce(EventMessage em)
        {
            FireOnceEvent(em);
            DateTime timeFired = DateTime.Now + _timeSpanDiff;

            if (!em.FireOnce)
            {
                em.Activ = true;

                //if ZeitAbstand 0000 dann kontinuierliches Update
                if(em.ZeitAbstand == TimeSpan.MinValue)
                {
                    //uber mqtt topics getimed
                    AddTopicBasedEvent(em);
                }
                else
                {
                    //normal im Loop getimed
                    TimedEventMessage tm = (TimedEventMessage)em;
                    tm.LastFired = timeFired;
                    activTimedEvents.Add(tm);
                }
            }
        }

        private void FireEventsByTime(DateTime timeNow)
        {
            foreach(TimedEventMessage tm in activTimedEvents)
            {
                if(tm.LastFired < timeNow)
                {
                    tm.LastFired = timeNow;
                    FireOnceEvent(tm);
                    //TODO check if LastFired ist changed in activTimedEvents
                }
            }
        }

        private void OnStopApplication()
        {
            _active = false;
        }

        public void OnReceivedMessage(string topic, byte[] message)
        {
            Task.Run(()=>FireTopicBasedEventsForTopic(topic, message));
        }

        private void FireTopicBasedEventsForTopic(string topic, byte[] message)
        {
            string strMessage = System.Text.Encoding.UTF8.GetString(message);
            if (activTopicBasedEvents.TryGetValue(topic, out HashSet<EventMessage> elist))
            {
                foreach (EventMessage em in elist)
                {
                    if (em.Activ && em.Message != strMessage)
                    {
                        FireOnceEvent(em);
                    }
                }
            }
        }

        private void AddTopicBasedEvent(EventMessage em)
        {
            if (activTopicBasedEvents.TryGetValue(em.Topic, out HashSet<EventMessage> list))
            {
                if(list == null)
                {
                    list = new HashSet<EventMessage>();
                }
                if(!list.TryGetValue(em, out EventMessage em2))
                {
                    list.Add(em);
                }
            }
            else
            {
                list = new HashSet<EventMessage>();
                list.Add(em);
                activTopicBasedEvents.Add(em.Topic, list);
            }
        }

        private void RemoveTopicBasedEvent(EventMessage em)
        {
            if(activTopicBasedEvents.TryGetValue(em.Topic, out HashSet<EventMessage> list))
            {
                if(list.Count <= 1)
                {
                    activTopicBasedEvents.Remove(em.Topic);
                }
                else
                {
                    list.Remove(em);
                    activTopicBasedEvents.Add(em.Topic, list);
                }
            }
        }
    }
}
