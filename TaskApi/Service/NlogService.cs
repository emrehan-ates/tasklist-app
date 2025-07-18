using NLog;
using Serilog;

namespace TaskApi.Services
{
    public class NlogService : ILogService
    {
        private readonly NLog.ILogger _logger;

        public NlogService()
        {
            _logger = LogManager.GetCurrentClassLogger();
        }

        public async void LogInfo(string message, object? data = null)
        {
            if (data != null)
                _logger.Info("{Message} {@Data}", message, data);
                
            else
                _logger.Info("{Message}", message);
        }

        public void LogWarning(string message, object? data = null)
        {
            if (data != null)
                _logger.Warn("{Message} {@Data}", message, data);
            else
                _logger.Warn("{Message}", message);
        }

        public void LogError(string message, Exception? exception = null, object? data = null)
        {
            if (exception != null && data != null)
                _logger.Error(exception, "{Message} {@Data}", message, data);
            else if (exception != null)
                _logger.Error(exception, "{Message}", message);
            else if (data != null)
                _logger.Error("{Message} {@Data}", message, data);
            else
                _logger.Error("{Message}", message);
        }
    }
}