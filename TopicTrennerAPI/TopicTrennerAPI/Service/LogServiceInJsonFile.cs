using Microsoft.AspNetCore.Hosting;
using System;
using System.IO;
using TopicTrennerAPI.Models;
using TopicTrennerAPI.Interfaces;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

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
            //throw new NotImplementedException("Klasse nicht feritg");
            _mqttCon = mqttConnector;
            _config = config;
            applicationLifttime.ApplicationStopping.Register(OnStopApplication);
            Console.WriteLine("LogServiceInJsonFile Started");
            logPath = _config.GetConnectionString("LogFilePath");
            _mqttCon.AddTopicReceiver("#", this);
        }

        public bool IsLoggingActive()
        {
            return _active;
        }

        public void SetLoggingActive(int sessionRunId, bool active)
        {
            if(_active && writer != null)
            {
                writer.Close();
            }

            if (active)
            {
                
                _active = true;
                _sessionRunId = sessionRunId;
                try
                {
                    writer = File.AppendText(@logPath+"log_" + sessionRunId + ".log");
                }
                catch (Exception e)
                {
                    string s = e.ToString();
                    writer = null;
                }
                
            }
            else if(_sessionRunId == sessionRunId)
            {
                _active = false;
                _sessionRunId = int.MinValue;
                writer = null;
            }
            
        }

        private void OnStopApplication()
        {
            _active = false;
            Console.WriteLine("LogServiceInJsonFile finished: OnStopApplication");
        }

        public void OnReceivedMessage(string topic, byte[] message)
        {
            if (!_active || _sessionRunId == int.MinValue)
            {
                return;
            }
            if (writer != null)
            {

                Log l = new Log();
                l.ID = 1;
                l.SetMessageUft8Byte(message);
                l.Topic = topic;
                l.SessionRunID = _sessionRunId;

                JsonSerializer serializer = new JsonSerializer();
                //serialize object directly into file stream
                serializer.Serialize(writer, l);
            }
        }
    }
}
