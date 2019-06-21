using System;
using TopicTrennerAPI.Interfaces;
using Microsoft.Extensions.Configuration;

namespace TopicTrennerAPI.Service
{
    public class ServerConfigDummycs : IServerConfig
    {
        private string _password;
        private string _ip;
        private string _username;

        public ServerConfigDummycs (IConfiguration config) {
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
