using System;
using TopicTrennerAPI.Interfaces;

namespace TopicTrennerAPI.Service
{
    public class ServerConfigDummycs : IServerConfig
    {
        public string GetPassword()
        {
            //return "HomeSmart";
            return "";
        }

        public string GetServerIp()
        {
            //return "192.168.0.2";
            return "192.168.178.78";
        }

        public string GetUsername()
        {
            //return "mqtt";
            return "";
        }
    }
}
