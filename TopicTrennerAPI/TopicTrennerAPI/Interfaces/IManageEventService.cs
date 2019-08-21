
namespace TopicTrennerAPI.Interfaces
{
    public interface IManageEventService
    {

        void StartEventService(int sessionRunId);

        bool StopEventService(int sessionRunId);

        bool ReloadEventService(int sessionRunId);

        bool FireOnceEvent(int eventId);
    }
}
