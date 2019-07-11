
namespace TopicTrennerAPI.Models
{
    public class Rule
    {
        private string _outTopic;
        private string[] _outTopicParts = null;
        private bool _outTopicHasWildcard = false;

        public TopicVertex InTopic { get; set; }
        
        public bool Active { get; set; }
        public bool OutTopicHasWildcard { get { return _outTopicHasWildcard; } }

        public string OutTopic
        {
            get { return _outTopic; }
            set
            {
                _outTopic = value;
                if(_outTopic.Contains('#') || _outTopic.Contains('+')) {
                    _outTopicParts = _outTopic.Split('/');
                    this._outTopicHasWildcard = true;
                }
            }
        }

        public Rule()
        {
        }

        public Rule(TopicVertex inTopic, string outTopic, bool active)
        {
            InTopic = inTopic;
            OutTopic = outTopic;
            Active = active;
        }

        public Rule(TopicVertex inTopic, SimpleRule simpleRule)
        {
            InTopic = inTopic;
            OutTopic = simpleRule.OutTopic;
            Active = simpleRule.Active;
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
