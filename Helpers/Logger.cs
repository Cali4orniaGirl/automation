namespace Helpers
{
    public static class Logger
    {
        private static readonly string logPath = Path.Combine(AppContext.BaseDirectory, "test-log.txt");

        public static void Info(string message)
        {
            var logEntry = $"[INFO]{DateTime.Now:HH:mm:ss} - {message}";
            Console.WriteLine(logEntry);
            File.AppendAllText(logPath, logEntry + Environment.NewLine);
        }
        
        public static void Error (string message, Exception ex = null)
        {
            var logEntry = $"[ERROR]{DateTime.Now:HH:mm:ss} - {message} {(ex != null ? ex.Message : "")}";
            Console.WriteLine(logEntry);
            File.AppendAllText(logPath, logEntry + Environment.NewLine);           
        }
    }
}