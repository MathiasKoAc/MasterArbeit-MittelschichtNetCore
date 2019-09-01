using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TopicTrennerAPI.Models;
using TopicTrennerAPI.Interfaces;
using Microsoft.AspNetCore.Hosting;

namespace TopicTrennerAPI.Service
{
    //alternativ Implementierung von RuleEvaluationService um DenyAccessDeny zu ermöglichen
    public class RuleEvaluationDenyAccessDeny : IMqttTopicReceiver, IServeRuleEvaluation
    {
        private readonly IMqttConnector mqttCon;

        ///TopicRules key ist TopicVertex.TopicChain also die TopicPartsKette bis inkl diesem TopicPart
        private Dictionary<string, TopicVertex> topicRulesAccess;
        private Dictionary<string, TopicVertex> topicRulesDenyIn;
        private Dictionary<string, TopicVertex> topicRulesDenyOut;


        public EnumMqttQualityOfService MqttQualityOfService = EnumMqttQualityOfService.QOS_LEVEL_AT_LEAST_ONCE;

        private object _locker = new object();
        private volatile bool _active;


        public RuleEvaluationDenyAccessDeny(IMqttConnector mqttConnector, IApplicationLifetime applicationLifttime)
        {
            this.mqttCon = mqttConnector;

            //TODO checken ob nicht zu häftig:
            this.mqttCon.AddTopicReceiver("#", this);
            applicationLifttime.ApplicationStopping.Register(OnStopApplication);
            Console.WriteLine("RuleEvaluationDenyAccessDeny Started");
        }

        public void OnReceivedMessage(string topic, byte[] message)
        {
            if(!_active)
            {
                return;
            }

            string cleanedTopic = topic.Trim().ToLower();
            cleanedTopic = cleanedTopic.StartsWith('/') ? cleanedTopic.Substring(1) : cleanedTopic;
            string[] topicParts = cleanedTopic.Split("/");

            if (topicParts[0] == null)
            {
                return;
            }

            Task.Run(() => CompleteCheckAndSend(topicParts, message));
        }

        private void CompleteCheckAndSend(string[] topicParts, byte[] message)
        {
            if (CheckDeny(topicParts, topicRulesDenyIn))
            {
                //Topic Denied by DenyIn-Rule
                return;
            }

            //TODO topic check wie im Bsp von GITHUB
            if (topicRulesAccess.TryGetValue(topicParts[0], out TopicVertex tvAccess))
            {
                TreeWalk(message, topicParts, 0, tvAccess);
            }
            
            //if there is no topic to match... do nothing
        }
        
        private void TreeWalk(byte[] message, string[] topicPartsMessageIn, int topicPartIndex, TopicVertex topicVertxRule)
        {
            if (topicVertxRule.AlgoRule.Count > 0)
            {
                CheckTheRuleAndSend(message, topicPartsMessageIn, topicPartIndex, topicVertxRule);
            }

            // checking for new TopicVertex
            List<TopicVertex> tvList = topicVertxRule.GetChildVertexList();
            if(tvList.Count > 0)
            {
                topicPartIndex++;
                if (topicPartsMessageIn.Length > topicPartIndex)
                {
                    foreach (TopicVertex tv in tvList)
                    {
                        if(tv.TopicPart.Equals(topicPartsMessageIn[topicPartIndex], StringComparison.InvariantCultureIgnoreCase) 
                                || tv.TopicPart.Equals("#", StringComparison.InvariantCultureIgnoreCase) 
                                || tv.TopicPart.Equals("+", StringComparison.InvariantCultureIgnoreCase))
                        {
                            TreeWalk(message, topicPartsMessageIn, topicPartIndex, tv);
                        }
                    }    
                }
            }
        }

        //return true by deny
        private bool TreeWalkDeny(string[] topicPartsMessageIn, int topicPartIndex, TopicVertex topicVertxRule)
        {
            if (topicVertxRule.AlgoRule.Count > 0)
            {
                return CheckTheRuleMatch(topicPartsMessageIn, topicPartIndex, topicVertxRule);
            }

            // checking for new TopicVertex
            List<TopicVertex> tvList = topicVertxRule.GetChildVertexList();
            if (tvList.Count > 0)
            {
                bool stopLoop = false;
                topicPartIndex++;
                foreach (TopicVertex tv in tvList)
                {
                    if (tv.TopicPart.Equals(topicPartsMessageIn[topicPartIndex], StringComparison.InvariantCultureIgnoreCase) 
                        || tv.TopicPart.Equals("#", StringComparison.InvariantCultureIgnoreCase) 
                        || tv.TopicPart.Equals("+", StringComparison.InvariantCultureIgnoreCase))
                    {
                        stopLoop = TreeWalkDeny(topicPartsMessageIn, topicPartIndex, tv);
                    }

                    if(stopLoop)
                    {
                        //stop  weil ein TreeWalk zurück kommt aus dem Stack mit einem Match
                        return true;
                    }
                }
            }
            // no DenyRule found
            return false;
        }

        //prüft ob die Rule Matched
        //return true by Match
        private bool CheckTheRuleMatch(string[] topicPartsMessageIn, int topicPartIndex, TopicVertex topicVertxRule)
        {
            // wenn keine Rul da, dann gibt es nix zu tuen
            if (topicVertxRule.AlgoRule.Count == 0)
            {
                return false;
            }

            // wenn # dann feuer
            if (topicVertxRule.TopicPart == "#")
            {
                return true;
            }

            // wenn der topicPart übereinstimmt und das der letzt Part ist dann feuer
            if (topicPartsMessageIn.Count() == (topicPartIndex + 1)
                && (topicPartsMessageIn[topicPartIndex] == topicVertxRule.TopicPart
                    || topicPartsMessageIn[topicPartIndex] == "+"))
            {
                return true;
            }

            return false;
        }

        //prüft ob die Rule ausgeführt werden kann
        private void CheckTheRuleAndSend(byte[] message, string[] topicPartsMessageIn, int topicPartIndex, TopicVertex topicVertxRule)
        {
            // wenn keine Rul da, dann gibt es nix zu tuen
            if(topicVertxRule.AlgoRule.Count == 0)
            {
                return;
            }

            // wenn # dann feuer
            if (topicVertxRule.TopicPart == "#")
            {
                SendTheMessagesIfNotDenyOut(message, topicPartsMessageIn, topicVertxRule.AlgoRule);
            }

            // wenn der topicPart übereinstimmt und das der letzt Part ist dann feuer
            if(topicPartsMessageIn.Count() == (topicPartIndex + 1)
                && (topicPartsMessageIn[topicPartIndex] == topicVertxRule.TopicPart
                    || topicPartsMessageIn[topicPartIndex] == "+"))
            {
                SendTheMessagesIfNotDenyOut(message, topicPartsMessageIn, topicVertxRule.AlgoRule);
            }
        }

        private void SendTheMessagesIfNotDenyOut(byte[] message, string[] topicPartsMessageIn, List<IAlgoRule> rules)
        {
            foreach(IAlgoRule ar in rules)
            {
                Rule r = (Rule)ar;

                if(!r.OutTopicHasWildcard && !CheckDeny(r.OutTopic, topicRulesDenyOut)) //wenn keine Wildcard drin und topicOut erlaubt
                {
                    // normal senden
                    this.mqttCon.PublishMessage(r.OutTopic, message, (byte) this.MqttQualityOfService);
                }
                else if (TryBuildTopicOutByWildcardRule(topicPartsMessageIn, r, out string topicOut) && !CheckDeny(topicOut, topicRulesDenyOut))
                {
                    //senden, wenn Topic gebuilded werden konnte und topicOut erlaubt
                    this.mqttCon.PublishMessage(topicOut, message, (byte)this.MqttQualityOfService);
                }
            }
        }

        //deny = true
        private bool CheckDeny(string[] topicParts, Dictionary<string, TopicVertex> topicRulesDeny)
        {
            if (topicRulesDeny.TryGetValue(topicParts[0], out TopicVertex tvDenyIn) && TreeWalkDeny(topicParts, 0, tvDenyIn))
            {
                return true;
            }
            return false;
        }

        private bool CheckDeny(string topicFull, Dictionary<string, TopicVertex> topicRulesDeny)
        {
            string[] topicParts = topicFull.Trim().ToLower().Split("/");
            return CheckDeny(topicParts, topicRulesDeny);
        }

        //return true if Message can build
        private bool TryBuildTopicOutByWildcardRule(string[] topicPartsMessageIn, Rule r, out string topicOut)
        {
            // OutTopic zusammenbauen / Wildcard ersetzen
            string[] topicPartsRegel = r.GetOutTopicParts();

            // Die Message muss mindestens die Länge der Topics auf der Rechten-Regelseite haben
            if (topicPartsMessageIn.Count() >= topicPartsRegel.Count())
            {
                StringBuilder topicBuild = new StringBuilder();

                bool multiLevelWildModus = false;
                int i;
                for (i = 0; i < topicPartsRegel.Count(); i++)
                {
                    if (topicPartsRegel[i].Equals("+"))
                    {
                        // + ersetzten mit Value von topic der Message
                        topicBuild.Append("/");
                        topicBuild.Append(topicPartsMessageIn[i]);
                    }
                    else if (topicPartsRegel[i].Equals("#"))
                    {
                        // Break aus der Loop
                        multiLevelWildModus = true;
                        break;
                    }
                    else
                    {
                        // weiter anhängen...
                        topicBuild.Append("/");
                        topicBuild.Append(topicPartsRegel[i]);
                    }
                }

                //Sonderfall wenn # gefunden wurde
                if(multiLevelWildModus)
                {
                    for(;i < topicPartsMessageIn.Count(); i++) {
                        //go over the MessageParts and replace the #
                        topicBuild.Append("/");
                        topicBuild.Append(topicPartsMessageIn[i]);
                    }
                }
                topicOut = topicBuild.ToString();
                return true;
            }
            topicOut = "";
            return false;
        }

        public void SetTopicRulesAccess(Dictionary<string, TopicVertex> accessRules)
        {
            topicRulesAccess = accessRules;
        }

        public void SetTopicRulesDenyIn(Dictionary<string, TopicVertex> denyInRules)
        {
            topicRulesDenyIn = denyInRules;
        }

        public void SetTopicRulesDenyOut(Dictionary<string, TopicVertex> denyOutRules)
        {
            topicRulesDenyOut = denyOutRules;
        }

        public bool IsRuleEvaluationActive()
        {
            return _active;
        }

        public void SetRuleEvaluationActive(bool active)
        {
            lock (_locker)
            {
                _active = active;
            }
        }

        private void OnStopApplication()
        {
            _active = false;
            Console.WriteLine("RuleEvaluationDenyAccessDeny finished: OnStopApplication");
        }
    }
}
