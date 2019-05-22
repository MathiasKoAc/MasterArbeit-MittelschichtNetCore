﻿using System;
using TopicTrennerAPI.Interfaces;

namespace TopicTrennerAPI.Service
{
    public class ServerConfigDummycs : IServerConfig
    {
        public string GetPassword()
        {
            return "HomeSmart";
        }

        public string GetServerIp()
        {
            return "192.168.0.2";
        }

        public string GetUsername()
        {
            return "mqtt";
        }
    }
}