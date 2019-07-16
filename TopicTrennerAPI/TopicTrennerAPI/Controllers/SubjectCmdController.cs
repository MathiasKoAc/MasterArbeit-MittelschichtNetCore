using System;
using Microsoft.AspNetCore.Mvc;
using TopicTrennerAPI.Models;
using TopicTrennerAPI.Models.Command;
using TopicTrennerAPI.Interfaces;
using TopicTrennerAPI.Data;
using System.Linq;
using System.Collections.Generic;

namespace TopicTrennerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubjectCmdController : ControllerBase
    {
        readonly DbTopicTrennerContext _context;
        readonly IMqttConnector _mqttConnector;

        public SubjectCmdController(IMqttConnector mqttConnector, DbTopicTrennerContext dbTopicTrenner)
        {
            _mqttConnector = mqttConnector;
            _context = dbTopicTrenner;
        }

        // posT: api/Mqtt
        [HttpPost]
        public int Post(SubjectCmd subjectCmd)
        {
            List<PurposeMessage> messages = _context.PurposeMessages.Where(p => p.SubjectId == subjectCmd.SubjectId && p.MessageTyp == subjectCmd.CmdMessageTyp).ToList();
            return SendMessages(ref messages);
        }

        private int SendMessages(ref List<PurposeMessage> messages)
        {
            int i = 0;
            foreach(PurposeMessage m in messages)
            {
                _mqttConnector.PublishMessage(m.Topic, m.Message, m.QosLevel, m.Retain);
                i++;
            }
            return i;
        }
    }
}
