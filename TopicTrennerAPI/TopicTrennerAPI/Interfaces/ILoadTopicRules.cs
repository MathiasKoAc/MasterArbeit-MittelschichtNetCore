using System;
using System.Collections.Generic;
using TopicTrennerAPI.Models;

namespace TopicTrennerAPI.Interfaces
{
    interface ILoadTopicRules
    {
        Dictionary<string, TopicVertex> LoadRules();
    }
}
