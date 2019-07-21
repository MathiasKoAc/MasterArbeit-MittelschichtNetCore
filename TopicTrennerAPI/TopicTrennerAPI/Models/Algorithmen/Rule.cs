using TopicTrennerAPI.Interfaces;

namespace TopicTrennerAPI.Models
{
    public class Rule : IAlgoRule
    {
        private string _outTopic;
        private string[] _outTopicParts = null;
        public TopicVertex InTopic { get; set; }
        public bool Active { get; set; }
        public bool OutTopicHasWildcard { get; private set; } = false;

        public string OutTopic
        {
            get { return _outTopic; }
            set
            {
                _outTopic = value;
                if(_outTopic.Contains('#') || _outTopic.Contains('+')) {
                    _outTopicParts = _outTopic.Split('/');
                    this.OutTopicHasWildcard = true;
                }
            }
        }

        public Rule(TopicVertex inTopic, string outTopic, bool active)
        {
            InTopic = inTopic;
            OutTopic = outTopic;
            Active = active;
        }

        public string[] GetOutTopicParts()
        {
            if(_outTopicParts == null)
            {
                _outTopicParts = _outTopic.Split('/');
            }
            return _outTopicParts;
        }

        public void Setup(TopicVertex startVertex, string outTopic, bool active)
        {
            this.InTopic = startVertex;
            this.OutTopic = OutTopic;
            this.Active = active;
        }

        public void SetStartVertex(TopicVertex vertex)
        {
            this.InTopic = vertex;
        }

        public void SetOutTopic(string topic)
        {
            this.OutTopic = topic;
        }

        public void SetActiv(bool active)
        {
            this.Active = active;
        }
    }
}
