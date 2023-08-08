namespace TodoApp.Core.Constants
{
    public static class Const
    {
        private static readonly string DB_HOST = Environment.GetEnvironmentVariable("DB_HOST") ?? "localhost";
        private static readonly string DB_PORT = Environment.GetEnvironmentVariable("DB_PORT") ?? "1433";
        private static readonly string DB_USERNAME = Environment.GetEnvironmentVariable("DB_USERNAME") ?? "";
        private static readonly string DB_PASSWORD = Environment.GetEnvironmentVariable("DB_PASSWORD") ?? "";
        private static readonly string DB_NAME = Environment.GetEnvironmentVariable("DB_NAME") ?? "todo_db";
        private static readonly string DB_CONFIG = Environment.GetEnvironmentVariable("DB_CONFIG") ?? "";

        public static readonly string DB_CONN = $"Server={DB_HOST},{DB_PORT};Database={DB_NAME};User Id={DB_USERNAME};Password={DB_PASSWORD};{DB_CONFIG}";

        public static readonly string JWT_SECRET = Environment.GetEnvironmentVariable("JWT_SECRET") ?? "";
        public static readonly string JWT_ISSUER = Environment.GetEnvironmentVariable("JWT_ISSUER") ?? "";
        public static readonly string JWT_AUDIENCE = Environment.GetEnvironmentVariable("JWT_AUDIENCE") ?? "";
    }
}