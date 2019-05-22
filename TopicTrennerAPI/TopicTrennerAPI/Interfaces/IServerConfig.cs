using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TopicTrennerAPI.Interfaces
{
    public interface IServerConfig
    {
        string GetServerIp();

        string GetPassword();

        string GetUsername();
    }
}
