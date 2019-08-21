
namespace TopicTrennerAPI.Interfaces
{
    public interface IServeLogging
    {
        bool IsLoggingActive();
        void SetLoggingActive(int sessionRunId, bool active);
    }
}
