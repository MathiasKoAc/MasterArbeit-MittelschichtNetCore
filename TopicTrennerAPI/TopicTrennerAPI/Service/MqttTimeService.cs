using System;
using TopicTrennerAPI.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;

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


        public MqttTimeService(IMqttConnector mqttConnector, IApplicationLifetime applicationLifttime)
        {
            _mqttCon = mqttConnector;
            applicationLifttime.ApplicationStopping.Register(OnStopApplication);
            Console.WriteLine("MqttTimeService Started");

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
                _mqttCon.PublishMessage("simulation/fulldate", (DateTime.Now + _timeDiff).ToString("yyyy:MM:ddTHH:mm:ssZ"), MqttQualityOfService);
                _mqttCon.PublishMessage("simulation/date", (DateTime.Now + _timeDiff).ToString("yyyy:MM:dd"), MqttQualityOfService);
                _mqttCon.PublishMessage("simulation/time", (DateTime.Now + _timeDiff).ToLongTimeString(), MqttQualityOfService);

                for (int i = 0; i < waitSeconds && _acitve; i++)
                {
                    Thread.Sleep(1000);
                }
            }
        }

        private void OnStopApplication()
        {
            _acitve = false;
            Console.WriteLine("MqttTimeService finished: OnStopApplication");
        }
    }
}
