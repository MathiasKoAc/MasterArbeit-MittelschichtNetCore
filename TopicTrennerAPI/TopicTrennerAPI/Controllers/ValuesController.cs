using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TopicTrennerAPI.Interfaces;
using TopicTrennerAPI.Models;

namespace TopicTrennerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private IMqttConnector _mqttConnector;

        public ValuesController(IMqttConnector mqttConnector)
        {
            _mqttConnector = mqttConnector;
        }

        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2", _mqttConnector.Hello() };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<Rule> Get(int id)
        {
            //return "value";
            //Testen...
            Rule r = new Rule();
            r.Active = true;
            r.OutTopic = "out/1";
            r.InTopic = TopicVertex.CreateTopicVertex("in/1");
            return r;
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
