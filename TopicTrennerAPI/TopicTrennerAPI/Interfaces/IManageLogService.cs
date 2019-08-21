
namespace TopicTrennerAPI.Interfaces
{
    public interface IManageLogService
    {
        void StartLogService(int SessionRunId);
        void StopLogService(int SessionRunId);
    }
}
