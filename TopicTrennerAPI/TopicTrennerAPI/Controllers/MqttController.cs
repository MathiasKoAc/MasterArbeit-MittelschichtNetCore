using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TopicTrennerAPI.Models;
using TopicTrennerAPI.Interfaces;

namespace TopicTrennerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MqttController : ControllerBase
    {
        private IMqttConnector _mqttConnector;

        public MqttController(IMqttConnector mqttConnector)
        {
            _mqttConnector = mqttConnector;
        }

        // GET: api/Mqtt
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Mqtt/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Mqtt
        [HttpPost]
        public void Post(MqttMessage mqtt)
        {
            _mqttConnector.PublishMessage(mqtt.Topic, mqtt.Message, mqtt.QosLevel, mqtt.Retain);
        }

        // PUT: api/Mqtt/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
