
namespace TopicTrennerAPI.Interfaces
{
    public interface IManageRuleService
    {
        /// <summary>
        ///     Starts the Rule-Service with the id of sessionRun
        /// </summary>
        /// <param name="sessionRunId"></param>
        void StartRuleSession(int sessionRunId);
        
        /// <summary>
        ///     Stops the Rule-Service with the id of sessionIdRun
        /// </summary>
        /// <param name="sessionRunId"></param>
        /// <returns>Returns false if it it not started</returns>
        bool StopRuleSession(int sessionRunId);

        /// <summary>
        ///     Reloads the Rules for the Service, sessionRunId
        /// </summary>
        /// <param name="sessionRunId"></param>
        /// <returns>Returns false if it it not started</returns>
        bool ReloadRules(int sessionRunId);
    }
}
