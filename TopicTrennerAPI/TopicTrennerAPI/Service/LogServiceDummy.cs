using Microsoft.AspNetCore.Hosting;
using System;
using System.IO;
using TopicTrennerAPI.Models;
using TopicTrennerAPI.Interfaces;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace TopicTrennerAPI.Service
{
    public class LogServiceDummy : IServeLogging, IMqttTopicReceiver
    {
        private bool _active;

        public LogServiceDummy(IMqttConnector mqttConnector, IApplicationLifetime applicationLifttime, IConfiguration config)
        {
            //throw new NotImplementedException("Klasse nicht feritg");
            applicationLifttime.ApplicationStopping.Register(OnStopApplication);
            Console.WriteLine("LogServiceInJsonFile Started");
        }

        public bool IsLoggingActive()
        {
            return _active;
        }

        public void SetLoggingActive(int sessionRunId, bool active)
        {
            _active = active;
        }

        private void OnStopApplication()
        {
            _active = false;
            Console.WriteLine("LogServiceInJsonFile finished: OnStopApplication");
        }

        public void OnReceivedMessage(string topic, byte[] message)
        {

        }
    }
}
