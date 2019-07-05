using System;
using TopicTrennerAPI.Interfaces;
using Microsoft.Extensions.Configuration;

namespace TopicTrennerAPI.Service
{
    public class ServerConfigConnectionString : IServerConfig
    {
        private readonly string _password;
        private readonly string _ip;
        private readonly string _username;

        public ServerConfigConnectionString (IConfiguration config) {
            _password = config.GetConnectionString("MqttDefaulConnectionPasswort");
            _ip = config.GetConnectionString("MqttDefaulConnectionServer");
            _username = config.GetConnectionString("MqttDefaulConnectionUser");
        }

        public string GetPassword()
        {
            return _password;
        }

        public string GetServerIp()
        {
            return _ip;
        }

        public string GetUsername()
        {
            return _username;
        }
    }
}
