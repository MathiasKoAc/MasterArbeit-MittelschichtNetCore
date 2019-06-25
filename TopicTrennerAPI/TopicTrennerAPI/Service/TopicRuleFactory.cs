using TopicTrennerAPI.Models;
using TopicTrennerAPI.Interfaces;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace TopicTrennerAPI.Service
{
    public class TopicRuleFactory : ICreateTopicRulesFromSimpleRules
    {
        public void CreateTopicRulesFromSimpleRule(ref Dictionary<string, TopicVertex> topicRules, SimpleRule simpleRule)
        {
            if (simpleRule.InTopic != null)
            {
                string[] inTopicParts = simpleRule.InTopic.Split("/");
                StringBuilder strBuilder = new StringBuilder();
                TopicVertex lastVertex = null;
                TopicVertex aktualVertex = null;

                for (int i = 0; i < inTopicParts.Count(); i++)
                {
                    strBuilder.Append(inTopicParts[i]);
                    // wenn das aktuelle Vertex nicht existiert (identifiert bei TopicChain), dann erstelle ein neues
                    if (!topicRules.TryGetValue(strBuilder.ToString(), out aktualVertex))
                    {
                        // erstelle Vertex
                        aktualVertex = new TopicVertex(lastVertex, inTopicParts[i]);
                        if (lastVertex != null)
                        {
                            lastVertex.AddChildVertex(aktualVertex);
                        }
                        topicRules.Add(aktualVertex.TopicChain, aktualVertex);
                    }
                    strBuilder.Append("/");
                }
                aktualVertex.Rules.Add(new Rule(aktualVertex, simpleRule));
            }
        }

        public void CreateTopicRulesFromSimpleRules(ref Dictionary<string, TopicVertex> topicRules, List<SimpleRule> simpleRules)
        {
            foreach(SimpleRule sRule in simpleRules)
            {
                CreateTopicRulesFromSimpleRule(ref topicRules, sRule);
            }
        }
    }
}
