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
    public class EventService : IServeEvents
    {
        readonly IMqttConnector _mqttCon;
        private volatile bool _active;
        private int _sessionRunId;
        private TimeSpan _timeSpan;
        private int waitSecInTimeLoop;

        public EventService(IMqttConnector mqttConnector, IApplicationLifetime applicationLifttime, IConfiguration config)
        {
            _mqttCon = mqttConnector;
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
            throw new NotImplementedException();
        }

        public bool SetEventServiceActive(bool active, int sessionRunId = -1)
        {
            throw new NotImplementedException();
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
            while(_active)
            {

                Thread.Sleep(1000 * waitSecInTimeLoop);
            }
        }

        private void StartEventsByTime()
        {

        }

        private void FireEventsByTime()
        {

        }

        private void StopEventsByTime()
        {

        }

        private void OnStopApplication()
        {
            throw new NotImplementedException();
        }
    }
}
