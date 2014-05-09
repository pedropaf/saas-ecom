namespace SaasEcom.Data.Infrastructure.Analytics
{
    public interface IEventLogger
    {
        void LogEvent<T>(T logEvent);
    }
}
