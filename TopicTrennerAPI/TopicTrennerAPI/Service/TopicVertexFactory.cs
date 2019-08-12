using TopicTrennerAPI.Models;
using TopicTrennerAPI.Interfaces;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace TopicTrennerAPI.Service
{
    public class TopicVertexFactory : ICreateTopicVertexFromTopics
    {
        public void CreateTopicVertexFromTopic(ref Dictionary<string, TopicVertex> topicRules, ISimpleRule simpleRule, IAlgoRuleFactory ruleFactory)
        {
            if (simpleRule.GetInTopic() != null)
            {
                string[] inTopicParts = simpleRule.GetInTopic().Split("/");
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
                    lastVertex = aktualVertex;
                }
                aktualVertex.AlgoRule.Add(ruleFactory.CreateRule(aktualVertex, simpleRule));
            }
        }


        public void CreateTopicVertexFromTopics(ref Dictionary<string, TopicVertex> topicRules, List<ISimpleRule> simpleRules, IAlgoRuleFactory ruleFactory)
        {
            foreach(SimpleRule sRule in simpleRules)
            {
                CreateTopicVertexFromTopic(ref topicRules, sRule, ruleFactory);
            }
        }
    }
}
