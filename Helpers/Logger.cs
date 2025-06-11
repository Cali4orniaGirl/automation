using Serilog;

namespace Helpers
{
    public static class Logger
    {
        static Logger()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}")
                .WriteTo.File("logs\\tests-.log", rollingInterval: RollingInterval.Day)
                .CreateLogger();
        }

        public static void Debug(string msg) => Log.Debug(msg);
        public static void Info(string msg) => Log.Information(msg);
        public static void Warn(string msg) => Log.Warning(msg);
        public static void Error(string msg) => Log.Error(msg);
        public static void Error(Exception ex, string msg) => Log.Error(ex, msg);
    }
}