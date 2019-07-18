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
        private TimeSpan _timeSpan;
        private int waitSecInTimeLoop;

        private HashSet<EventMessage> automaticEventStarts;
        private HashSet<EventMessage> activTimedEvents;
        private Dictionary<string, List<EventMessage>> activTopicBasedEvents;

        public EventService(IMqttConnector mqttConnector, IApplicationLifetime applicationLifttime, IConfiguration config)
        {
            automaticEventStarts = new HashSet<EventMessage>();
            activTimedEvents = new HashSet<EventMessage>();
            activTopicBasedEvents = new Dictionary<string, List<EventMessage>>();

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
            //TODO !!!
            throw new NotImplementedException();
        }

        public bool SetEventServiceActive(bool active, int sessionRunId = -1)
        {
            if ((sessionRunId == -1 && _sessionRunId != int.MinValue) || sessionRunId == _sessionRunId)
            {
                _active = active;
                return true;
            }
            return false;
        }

        public void SetTimeDiff(TimeSpan timeSpan)
        {
            _timeSpan = timeSpan;
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
                loopNow = DateTime.Now + _timeSpan;
                ActivateDeactivateEventsByTime(loopNow, loopLast);
                FireEventsByTime(loopNow, loopLast);
                Thread.Sleep(1000 * waitSecInTimeLoop);
                loopLast = loopNow;
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
                    SetActiveOrFireOnce(em);
                }
                else
                //Fall3
                if(timeLast < em.VonDate && timeNow < em.BisDate)
                {
                    SetActiveOrFireOnce(em);
                }
                else
                //Fall4
                if(timeLast > em.VonDate && timeNow > em.BisDate)
                {
                    em.Activ = false;
                    activTimedEvents.Remove(em);
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
                    activTimedEvents.Remove(em);
                    automaticEventStarts.Remove(em);
                    RemoveTopicBasedEvent(em);
                }
            }
        }

        private void SetActiveOrFireOnce(EventMessage em)
        {
            FireOnceEvent(em);

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
                    activTimedEvents.Add(em);
                }
            }
        }

        private void FireEventsByTime(DateTime timeNow, DateTime timeLast)
        {
            //TODO!!!
        }

        private void OnStopApplication()
        {
            _active = false;
        }

        public void OnReceivedMessage(string topic, byte[] message)
        {
            if(activTopicBasedEvents.TryGetValue(topic, out List<EventMessage> elist))
            {

            }
        }

        private void AddTopicBasedEvent(EventMessage em)
        {
            if (activTopicBasedEvents.TryGetValue(em.Topic, out List<EventMessage> list))
            {
                if(list == null)
                {
                    list = new List<EventMessage>();
                }
                list.Add(em);
                activTopicBasedEvents.Add(em.Topic, list);
            }
            else
            {
                list = new List<EventMessage>();
                list.Add(em);
                activTopicBasedEvents.Add(em.Topic, list);
            }
        }

        private void RemoveTopicBasedEvent(EventMessage em)
        {
            if(activTopicBasedEvents.TryGetValue(em.Topic, out List<EventMessage> list))
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
