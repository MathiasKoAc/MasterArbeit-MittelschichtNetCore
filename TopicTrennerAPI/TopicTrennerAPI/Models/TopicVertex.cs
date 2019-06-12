using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TopicTrennerAPI.Models
{
    public class TopicVertex
    {
        public TopicVertex FromVertex { get; set; }
        private List<TopicVertex> ToVertexes;
        public string Value { get; set; }    //topicPart
        public List<Rule> Rules { get; set; }

        public TopicVertex()
        {
            this.ToVertexes = new List<TopicVertex>();
            this.Rules = new List<Rule>();
        }


        public string TopicChain
        {
            get
            {
                if (this.FromVertex != null)
                {
                    return this.FromVertex.TopicChain +"/"+ Value;
                }
                return Value;
            }
        }
        /*
        public override string ToString()
        {
            return this.TopicChain;
        }
        */
        public TopicVertex FindFirstTopicVertex()
        {
            TopicVertex mover = this;
            while(this.FromVertex != null)
            {
                mover = this.FromVertex;
            }
            return mover;
        }

        public static TopicVertex CreateTopicVertex(string topicChainStr)
        {
            string[] topicParts = topicChainStr.Split('/');

            TopicVertex tv = null;
            TopicVertex last = null;
            foreach (string part in topicParts)
            {
                tv = new TopicVertex();
                if(last != null)
                {
                    tv.FromVertex = last;
                    last.ToVertexes.Add(tv);
                }
                tv.Value = part;
                last = tv;
            }
            return tv;
        }
    }
}
