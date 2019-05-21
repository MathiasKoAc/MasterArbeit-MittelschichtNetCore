using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TopicTrennerAPI.Interfaces
{
    public interface IMqttTopicReceiver
    {
        void OnReceivedMessage(string topic, byte[] message);
    }
}
