using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TopicTrennerAPI.Models
{
    public class Rule
    {
        private string _outTopic;
        private string[] _outTopicParts = null;

        public TopicVertex InTopic { get; set; }
        
        public bool Active { get; set; }
        public bool HasWildcard { get; }

        public string OutTopic
        {
            get { return _outTopic; }
            set
            {
                _outTopic = value;
                if(_outTopic.Contains('#') || _outTopic.Contains('+')) {
                    _outTopicParts = _outTopic.Split('/');
                }
            }
        }

        public string[] GetOutTopicParts()
        {
            if(_outTopicParts == null)
            {
                _outTopicParts = _outTopic.Split('/');
            }
            return _outTopicParts;
        }
    }
}
