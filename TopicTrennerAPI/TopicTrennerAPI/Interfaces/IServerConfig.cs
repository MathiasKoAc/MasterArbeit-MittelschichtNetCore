namespace TopicTrennerAPI.Interfaces
{
    public interface IServerConfig
    {
        string GetServerIp();

        string GetPassword();

        string GetUsername();
    }
}
