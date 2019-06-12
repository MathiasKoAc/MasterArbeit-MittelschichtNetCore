using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TopicTrennerAPI.Models;
using TopicTrennerAPI.Interfaces;

namespace TopicTrennerAPI.Service.RuleService
{
    public class RuleService : IMqttTopicReceiver
    {
        IMqttConnector mqttCon;
        Dictionary<string, List<TopicVertex>> TopicRules;
        List<Rule> Rules;

        public void LoadRules()
        {

        }

        public void OnReceivedMessage(string topic, byte[] message)
        {
            //TODO topic check wie im Bsp von GITHUB

            //TODO sonderfall #-Regeln definiert...
            if(TopicRules.TryGetValue("#", out List<TopicVertex> value))
            {

            }

            //TODO walk throu the tree
            string[] topicParts = topic.Split("/");
            if(topicParts[0] != null && TopicRules.TryGetValue(topicParts[0], out List<TopicVertex> topicVertexes))
            {
                
            }
            
            //else if there is no, topic to match... do nothing
        }

        private void treeWalk(byte[] message, string[] topicParts, int index, TopicVertex topicVertx)
        {
            if(topicVertx.Rules.Count > 0)
            {
                //TODO kümmer dich um die Regel...
            }

            

        }
    }
}
