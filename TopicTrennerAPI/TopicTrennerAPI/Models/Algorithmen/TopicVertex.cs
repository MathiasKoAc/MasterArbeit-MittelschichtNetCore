﻿using System.Collections.Generic;
using TopicTrennerAPI.Interfaces;

namespace TopicTrennerAPI.Models
{
    public class TopicVertex
    {
        public TopicVertex ParentVertex { get; set; }
        List<TopicVertex> ChildVertexes;
        public string TopicPart { get; set; }    //topicPart
        public List<IAlgoRule> AlgoRule { get; set; }

        public TopicVertex()
        {
            this.ChildVertexes = new List<TopicVertex>();
            this.AlgoRule = new List<IAlgoRule>();
        }

        public TopicVertex(TopicVertex parenVertex, string topicPart) : this()
        {
            this.ParentVertex = parenVertex;
            this.TopicPart = topicPart;
        }
        public string TopicChain
        {
            get
            {
                if (this.ParentVertex != null)
                {
                    return this.ParentVertex.TopicChain +"/"+ TopicPart;
                }
                return TopicPart;
            }
        }

        public TopicVertex FindFirstTopicVertex()
        {
            TopicVertex mover = this;
            while(this.ParentVertex != null)
            {
                mover = this.ParentVertex;
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
                    tv.ParentVertex = last;
                    last.ChildVertexes.Add(tv);
                }
                tv.TopicPart = part;
                last = tv;
            }
            return tv;
        }

        public void AddChildVertex(TopicVertex tv)
        {
            this.ChildVertexes.Add(tv);
        }

        public void RemoveChildVertex(TopicVertex tv)
        {
            this.ChildVertexes.Remove(tv);
        }

        public List<TopicVertex> GetChildVertexList()
        {
            return this.ChildVertexes;
        }
    }
}
