using System;
using TopicTrennerAPI.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace TopicTrennerAPI.Service
{
    public class MqttTimeService : IServeTime
    {
        readonly IMqttConnector _mqttCon;
        TimeSpan _timeDiff;
        volatile bool _acitve = false;
        Task runningTask;

        //TODO in SETUP CLASS / INTERFACE
        public static int waitSeconds = 30;//300;
        public EnumMqttQualityOfService MqttQualityOfService = EnumMqttQualityOfService.QOS_LEVEL_AT_LEAST_ONCE;


        public MqttTimeService(IMqttConnector mqttConnector)
        {
            _mqttCon = mqttConnector;
        }

        public bool IsTimeServiceActive()
        {
            return _acitve;
        }

        public void SetTimeDiff(TimeSpan timeDiff)
        {
            _timeDiff = timeDiff;
        }

        public void SetTimeServiceActive(bool active)
        {
            _acitve = active;
            if(active && (runningTask == null || runningTask.Status != TaskStatus.Running))
            {
                runningTask = Task.Run(SendTimeLoop);
            }
        }

        public void SetTimeServiceActive(bool active, TimeSpan timeDiff)
        {
            SetTimeDiff(timeDiff);
            SetTimeServiceActive(active);
        }

        private void SendTimeLoop()
        {
            while(_acitve)
            {
                _mqttCon.PublishMessage("simulation/fulldate", (DateTime.Now + _timeDiff).ToLongTimeString(), MqttQualityOfService);
                Thread.Sleep(waitSeconds * 1000);
            }
        }
    }
}
