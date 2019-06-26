using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TopicTrennerAPI.Interfaces
{
    public interface IControlRules
    {
        /// <summary>
        ///     Starts the Rule-Session with the id of session
        /// </summary>
        /// <param name="sessionId"></param>
        void StartRuleSession(int sessionId);
        
        /// <summary>
        ///     Stops the Rule-Session with the id of sessionId
        /// </summary>
        /// <param name="sessionId"></param>
        /// <returns>Returns false if sessionId is wrong</returns>
        bool StopRuleSession(int sessionId);

        /// <summary>
        ///     Reloads the Rules for the Session, sessionId musst be the same like the inner one
        /// </summary>
        /// <param name="sessionId"></param>
        /// <returns>Returns false if sessionId is wrong</returns>
        bool ReloadRules(int sessionId);
        int GetSessionId();
    }
}
