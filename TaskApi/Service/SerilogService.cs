using Serilog;

namespace TaskApi.Services
{
    public class SerilogService : ILogService
    {
        private readonly Serilog.ILogger _logger;

        public SerilogService()
        {
            _logger = Log.Logger;
        }

        public void LogInfo(string message, object? data = null)
        {
            if (data != null)
                _logger.Information("{Message} {@Data}", message, data);
            else
                _logger.Information("{Message}", message);
        }

        public void LogWarning(string message, object? data = null)
        {
            if (data != null)
                _logger.Warning("{Message} {@Data}", message, data);
            else
                _logger.Warning("{Message}", message);
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