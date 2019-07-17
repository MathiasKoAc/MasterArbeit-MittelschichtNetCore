using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using TopicTrennerAPI.Models;
using TopicTrennerAPI.Data;
using TopicTrennerAPI.Interfaces;
using Microsoft.Extensions.Configuration;

namespace TopicTrennerAPI.Service
{
    public class LogServiceInJsonFile : IServeLogging, IMqttTopicReceiver
    {
        readonly IConfiguration _config;
        readonly IMqttConnector _mqttCon;
        private bool _active;
        private int _sessionRunId = int.MinValue;
        private string logPath;
        private StreamWriter writer;

        public LogServiceInJsonFile(IMqttConnector mqttConnector, IApplicationLifetime applicationLifttime, IConfiguration config)
        {
            throw new NotImplementedException("Klasse nicht feritg");
            _mqttCon = mqttConnector;
            _config = config;
            applicationLifttime.ApplicationStopping.Register(OnStopApplication);
            Console.WriteLine("LogServiceInDbContext Started");
            logPath = _config.GetConnectionString("LogFilePath");
            _mqttCon.AddTopicReceiver("#", this);
        }

        public bool IsLoggingActive()
        {
            return _active;
        }

        public void SetDbContext(DbTopicTrennerContext context)
        {
            //_context = context;
        }

        public void SetLoggingActive(int sessionRunId, bool active)
        {
            if (active)
            {
                _active = true;
                _sessionRunId = sessionRunId;
            }
            else if(_sessionRunId == sessionRunId)
            {
                _active = false;
                _sessionRunId = int.MinValue;
            }
            
        }

        private void OnStopApplication()
        {
            _active = false;
            Console.WriteLine("LogServiceInDbContext finished: OnStopApplication");
        }

        public void OnReceivedMessage(string topic, byte[] message)
        {
            if (!_active || _sessionRunId == int.MinValue)
            {
                return;
            }

            Log l = new Log();
            l.ID = 1;
            l.SetMessageUft8Byte(message);
            l.Topic = topic;
            l.SessionRunID = _sessionRunId;

            //TODO Problem beheben hier kann ich nicht auf die Datenbank zugreifen!
            //_context.Logs.Add(l);
            //_context.SaveChanges();
        }
    }
}
