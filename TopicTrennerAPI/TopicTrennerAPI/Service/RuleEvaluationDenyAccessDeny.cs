﻿using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;

using TopicTrennerAPI.Models;
using TopicTrennerAPI.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace TopicTrennerAPI.Service
{
    //alternativ Implementierung von RuleEvaluationService um DenyAccessDeny zu ermöglichen
    public class RuleEvaluationDenyAccessDeny : IMqttTopicReceiver, IControlRules
    {
        IMqttConnector mqttCon;
        ILoadTopicRules TopicRuleLoader;

        ///TopicRules key ist TopicVertex.TopicChain also die TopicPartsKette bis inkl diesem TopicPart
        Dictionary<string, TopicVertex> TopicRulesAccess;
        Dictionary<string, TopicVertex> TopicRulesDenyIn;
        Dictionary<string, TopicVertex> TopicRulesDenyOut;


        int _sessionId = int.MinValue;
        public bool active { get; private set; } = false;
        public EnumMqttQualityOfService MqttQualityOfService = EnumMqttQualityOfService.QOS_LEVEL_AT_LEAST_ONCE;


        public RuleEvaluationDenyAccessDeny(IMqttConnector mqttConnector, ILoadTopicRules TopicRuleLoader)
        {
            this.mqttCon = mqttConnector;
            this.TopicRuleLoader = TopicRuleLoader;
        }

        public void OnReceivedMessage(string topic, byte[] message)
        {
            if(!active)
            {
                return;
            }

            string[] topicParts = topic.Trim().ToLower().Split("/");

            //TODO topic check wie im Bsp von GITHUB
            if(topicParts[0] != null && TopicRulesAccess.TryGetValue(topicParts[0], out TopicVertex tv))
            {
                //TODO 01.07.2019 hier muss irgendwie TreeWalkDeny rein
                //if(!TreeWalkDeny(message, topicParts, 0, ))
                TreeWalk(message, topicParts, 0, tv);
            }
            
            //if there is no topic to match... do nothing
        }

        private void TreeWalk(byte[] message, string[] topicPartsMessageIn, int topicPartIndex, TopicVertex topicVertxRule)
        {
            if (topicVertxRule.Rules.Count > 0)
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

        //return true by deny
        private bool TreeWalkDeny(string[] topicPartsMessageIn, int topicPartIndex, TopicVertex topicVertxRule)
        {
            if (topicVertxRule.Rules.Count > 0)
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
                    if (tv.TopicPart == topicPartsMessageIn[topicPartIndex] || tv.TopicPart == "#" || tv.TopicPart == "+")
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
            if (topicVertxRule.Rules.Count == 0)
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
        private void CheckTheRule(byte[] message, string[] topicPartsMessageIn, int topicPartIndex, TopicVertex topicVertxRule)
        {
            // wenn keine Rul da, dann gibt es nix zu tuen
            if(topicVertxRule.Rules.Count == 0)
            {
                return;
            }

            // wenn # dann feuer
            if (topicVertxRule.TopicPart == "#")
            {
                SendTheMessagesByRules(message, topicPartsMessageIn, topicVertxRule.Rules);
            }

            // wenn der topicPart übereinstimmt und das der letzt Part ist dann feuer
            if(topicPartsMessageIn.Count() == (topicPartIndex + 1)
                && (topicPartsMessageIn[topicPartIndex] == topicVertxRule.TopicPart
                    || topicPartsMessageIn[topicPartIndex] == "+"))
            {
                SendTheMessagesByRules(message, topicPartsMessageIn, topicVertxRule.Rules);
            }
        }

        private void SendTheMessagesByRules(byte[] message, string[] topicPartsMessageIn, List<Rule> Rules)
        {
            foreach(Rule r in Rules)
            {
                if(!r.OutTopicHasWildcard)
                {
                    // normal senden
                    this.mqttCon.PublishMessage(r.OutTopic, message, (byte) this.MqttQualityOfService);
                } else
                {
                    SendMessageWithWildcardByRule(message, topicPartsMessageIn, r);
                }
            }
        }

        private void SendMessageWithWildcardByRule(byte[] message, string[] topicPartsMessageIn, Rule r)
        {
            // OutTopic zusammenbauen / Wildcard ersetzen
            string[] topicPartsRegel = r.GetOutTopicParts();

            // Die Message muss mindestens die Länge der Topics auf der Rechten-Regelseite haben
            if (topicPartsMessageIn.Count() >= topicPartsRegel.Count())
            {
                StringBuilder topicBuild = new StringBuilder();

                bool multiLevelWildModus = false;
                int i = 0;
                for (i = 0; i < topicPartsRegel.Count(); i++)
                {
                    if (topicPartsRegel[i].Equals("+"))
                    {
                        // + ersetzten mit Value von topic der Message
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
                        topicBuild.Append(topicPartsRegel[i]);
                    }
                }

                //Sonderfall wenn # gefunden wurde
                if(multiLevelWildModus)
                {
                    for(;i < topicPartsMessageIn.Count(); i++) {
                        // TODO go over the MessageParts and replace the #
                        topicBuild.Append(topicPartsMessageIn[i]);
                    }
                }
                mqttCon.PublishMessage(topicBuild.ToString(), message, (byte)MqttQualityOfService);
            }
        }

        public void StartRuleSession(int sessionId)
        {
            TopicRulesAccess = TopicRuleLoader.LoadRules(sessionId, EnumSimpleRuleTyp.access);
            TopicRulesDenyIn = TopicRuleLoader.LoadRules(sessionId, EnumSimpleRuleTyp.denyIn);
            TopicRulesDenyOut = TopicRuleLoader.LoadRules(sessionId, EnumSimpleRuleTyp.denyOut);
            _sessionId = sessionId;
            active = true;
        }

        public bool StopRuleSession(int sessionId)
        {
            if(_sessionId != int.MinValue && _sessionId != sessionId)
            {
                return false;
            }
            active = false;
            _sessionId = int.MinValue;
            return true;
        }

        public bool ReloadRules(int sessionId)
        {
            if (_sessionId != int.MinValue && _sessionId != sessionId)
            {
                return false;
            }
            TopicRulesAccess = TopicRuleLoader.LoadRules(sessionId, EnumSimpleRuleTyp.access);
            TopicRulesDenyIn = TopicRuleLoader.LoadRules(sessionId, EnumSimpleRuleTyp.denyIn);
            TopicRulesDenyOut = TopicRuleLoader.LoadRules(sessionId, EnumSimpleRuleTyp.denyOut);
            active = true;
            return true;
        }

        public int GetSessionId()
        {
            return _sessionId;
        }
    }
}