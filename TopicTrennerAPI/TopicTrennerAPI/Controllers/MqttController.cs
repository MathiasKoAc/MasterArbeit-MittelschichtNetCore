using Microsoft.AspNetCore.Mvc;
using TopicTrennerAPI.Models;
using TopicTrennerAPI.Interfaces;

namespace TopicTrennerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MqttController : ControllerBase
    {
        readonly IMqttConnector _mqttConnector;

        public MqttController(IMqttConnector mqttConnector)
        {
            _mqttConnector = mqttConnector;
        }

        // posT: api/Mqtt
        [HttpPost]
        public void Post(MqttMessage mqtt)
        {
            _mqttConnector.PublishMessage(mqtt.Topic, mqtt.Message, mqtt.QosLevel, mqtt.Retain);
        }
    }
}
