using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TopicTrennerAPI.Models;
using TopicTrennerAPI.Interfaces;
using System.IO;
using System.Text;
using System.Runtime.Serialization.Json;

namespace TopicTrennerAPI.Service
{
    public class TextRuleImporter : ILoadTopicRules
    {
        string a = "[ {\"InTopic\":\"in/VR/Haus/Etage1\", \"OutTopic\":\"in/AR/Haus/Etage1\", \"Active\":true}, {\"InTopic\":\"in/VR/Haus/Etage2\", \"OutTopic\":\"in/AR/Haus/Etage2\", \"Active\":true}, {\"InTopic\":\"in/VR/Haus/Etage3\", \"OutTopic\":\"in/AR/Haus/Etage3\", \"Active\":true} ]";

        public Dictionary<string, TopicVertex> LoadRules()
        {
            ///dicTv key ist TopicVertex.TopicChain also die TopicPartsKette bis inkl diesem TopicPart
            Dictionary<string, TopicVertex> dicTv = new Dictionary<string, TopicVertex>();
            List<SimpleRule> Rules = ReadToSimpleRuleList(a);

            foreach(SimpleRule sr in Rules)
            {
                if(sr.InTopic != null)
                {
                    string[] inTopicParts = sr.InTopic.Split("/");
                    StringBuilder strBuilder = new StringBuilder();

                    for(int i = 0; i < inTopicParts.Count(); i++)
                    {
                        strBuilder.Append(inTopicParts[i]);
                    }
                }
            }
            return dicTv;
        }

        // Deserialize a JSON stream to a List<SimpleRule> object.  
        private List<SimpleRule> ReadToSimpleRuleList(string json)
        {
            List<SimpleRule> deserializedRuleList = new List<SimpleRule>();
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(json));
            DataContractJsonSerializer ser = new DataContractJsonSerializer(deserializedRuleList.GetType());
            deserializedRuleList = ser.ReadObject(ms) as List<SimpleRule>;
            ms.Close();
            return deserializedRuleList;
        }
    }
}
