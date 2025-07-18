
/*
        private readonly ILogger<LogService> _logger;

        public LogService(ILogger<LogService> logger)
        {
            _logger = logger;
        }

burada kullandığım ILogger, logger için abstraction. Yaptığım Ilogger call ları kullandığım
logging providerını route ediliyor. FRAMEWORK-AGNOSTIC

ben şimdi iki farklı dosya yapıp kendi apilerini kullanıcam
*/

namespace TaskApi.Services
{

    public class LogService : ILogService
    {
        private readonly ILogger<LogService> _logger;

        public LogService(ILogger<LogService> logger)
        {
            _logger = logger;
        }

        public void LogInfo(string message, object? data = null)
        {
            if (data != null)
                _logger.LogInformation("{Message} {@Data}", message, data);
            else
                _logger.LogInformation("{Message}", message);
        }

        public void LogWarning(string message, object? data = null)
        {
            if (data != null)
                _logger.LogWarning("{Message} {@Data}", message, data);
            else
                _logger.LogWarning("{Message}", message);
        }

        public void LogError(string message, Exception? exception = null, object? data = null)
        {
            if (exception != null && data != null)
                _logger.LogError(exception, "{Message} {@Data}", message, data);
            else if (exception != null)
                _logger.LogError(exception, "{Message}", message);
            else if (data != null)
                _logger.LogError("{Message} {@Data}", message, data);
            else
                _logger.LogError("{Message}", message);
        }

    }
}