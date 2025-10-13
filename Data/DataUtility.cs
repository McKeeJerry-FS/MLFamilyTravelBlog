namespace MLFamilyTravelBlog.Data
{
    public class DataUtility
    {
        public static string GetConnectionString(IConfiguration configuration)
        {
            // Use this method to get the connection string for your database
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            var databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL");
            return string.IsNullOrEmpty(databaseUrl) ? connectionString! : BuildConnectionString(databaseUrl);
        }

        private static string BuildConnectionString(string databaseUrl)
        {
            // Implement your logic to build the connection string from the database URL
            var databaseUri = new Uri(databaseUrl);
            var userInfo = databaseUri.UserInfo.Split(':');

            var builder = new Npgsql.NpgsqlConnectionStringBuilder
            {
                Host = databaseUri.Host,
                Port = databaseUri.Port,
                Username = userInfo[0],
                Password = userInfo[1],
                Database = databaseUri.AbsolutePath.TrimStart('/'),
                SslMode = Npgsql.SslMode.Prefer
            };
            return builder.ConnectionString;
        }
    }
}
