
namespace TaskApi.Services
{
    public interface ILogService
{
    void LogInfo(string message, object? data = null);
    void LogWarning(string message, object? data = null);
    void LogError(string message, Exception? ex = null, object? data = null);
}
}