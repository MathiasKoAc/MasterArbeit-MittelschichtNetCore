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

        private void TreeWalk(byte[] message, string[] topicPartsMessageIn, int topicPartIndex, TopicVertex topicVertxRule)
        {
            if(topicVertxRule.Rules.Count > 0)
            {
                CheckTheRule(message, topicPartsMessageIn, topicPartIndex, topicVertxRule);
            }

            // checking for new TopicVertex
            List<TopicVertex> tvList = topicVertxRule.GetChildVertexList();
            if(tvList.Count > 0)
            {
                topicPartIndex++;
                foreach (TopicVertex tv in tvList)
                {
                    if(tv.TopicPart == topicPartsMessageIn[topicPartIndex] || tv.TopicPart == "#" || tv.TopicPart == "+")
                    {
                        TreeWalk(message, topicPartsMessageIn, topicPartIndex, tv);
                    }
                }
            }
        }

        private void CheckTheRule(byte[] message, string[] topicPartsMessageIn, int topicPartIndex, TopicVertex topicVertxRule)
        {
            //wenn keine Rul da, dann gibt es nix zu tuen
            if(topicVertxRule.Rules.Count == 0)
            {
                return;
            }

            //wenn # dann feuer
            if (topicVertxRule.TopicPart == "#")
            {
                SendTheMessageByRule(message, topicPartsMessageIn, topicPartIndex, topicVertxRule);
            }

            //wenn der topicPart übereinstimmt und das der letzt Part ist dann feuer
            if(topicPartsMessageIn.Count() == (topicPartIndex + 1)
                && (topicPartsMessageIn[topicPartIndex] == topicVertxRule.TopicPart
                    || topicPartsMessageIn[topicPartIndex] == "+"))
            {
                SendTheMessageByRule(message, topicPartsMessageIn, topicPartIndex, topicVertxRule);
            }
        }

        private void SendTheMessageByRule(byte[] message, string[] topicPartsMessageIn, int topicPartIndex, TopicVertex topicVertxRule)
        {

        }
    }
}
