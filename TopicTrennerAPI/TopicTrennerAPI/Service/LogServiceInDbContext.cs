using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using TopicTrennerAPI.Models;
using TopicTrennerAPI.Data;
using TopicTrennerAPI.Interfaces;

namespace TopicTrennerAPI.Service
{
    public class LogServiceInDbContext : IServeLogging, IMqttTopicReceiver
    {
        readonly IMqttConnector _mqttCon;
        private DbTopicTrennerContext _context;
        private bool _active;
        private int _sessionRunId = int.MinValue;

        public LogServiceInDbContext(IMqttConnector mqttConnector, IApplicationLifetime applicationLifttime)
        {
            _mqttCon = mqttConnector;
            applicationLifttime.ApplicationStopping.Register(OnStopApplication);
            Console.WriteLine("LogServiceInDbContext Started");
            _mqttCon.AddTopicReceiver("#", this);
        }

        public bool IsLoggingActive()
        {
            return _active;
        }

        public void SetDbContext(DbTopicTrennerContext context)
        {
            _context = context;
        }

        public void SetLoggingActive(int sessionRunId, bool active)
        {
            if (_context == null)
            {
                throw new Exception("No DbContext setup for LogServiceInDbContext, please setup Context to this");
            }

            
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
            l.SetMessageUft8Byte(message);
            l.Topic = topic;
            l.SessionRunID = _sessionRunId;

            _context.Logs.Add(l);
            _context.SaveChanges();
        }
    }
}
