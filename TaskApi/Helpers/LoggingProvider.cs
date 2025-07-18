using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;
using NLog;
using NLog.Extensions.Logging;

namespace TaskApi.Helpers
{
    public class LoggingProvider
    {
        public static void ConfigureLogging(IConfiguration config, IHostBuilder builder)
        {
            var provider = "Nlog";
            if (provider == "Serilog")
            {
                Log.Logger = new LoggerConfiguration()
                                .ReadFrom.Configuration(config)
                                .WriteTo.PostgreSQL(
                                connectionString: config.GetConnectionString("DefaultConnection"),
                                tableName: "logs",
                                needAutoCreateTable: true
                                ).CreateLogger();

                builder.UseSerilog();
            }
            else if (provider == "Nlog")
            {
                builder.ConfigureLogging(log =>
                {
                    log.ClearProviders();
                    log.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
                    log.AddNLog(config);

                });
            }
            else
            {
                throw new Exception("Invalid logger");
            }
        }
    }
}