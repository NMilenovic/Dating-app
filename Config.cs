using Microsoft.Extensions.Configuration;

namespace Dating_app
{
    public static class Config
    {
        private static readonly string NeoUri;
        private static readonly string NeoUsername;
        private static readonly string NeoPassword;
        private static readonly string JwtSecret;
        private static readonly int Rounds;

        static Config()
        {
            var config = new ConfigurationBuilder()
                        .AddJsonFile("appsettings.json")
                        .Build();

            var neo4j = config.GetSection("Neo4j");
            NeoUri = neo4j["uri"];
            NeoUsername = neo4j["username"];
            NeoPassword = neo4j["password"];

            JwtSecret = config.GetSection("Jwt")["secret"];
            Rounds = int.Parse(config.GetSection("Password")["rounds"]);
        }

         public static (string Uri, string Username, string Password) GetNeoConfig()
        {
            return (NeoUri, NeoUsername, NeoPassword);
        }

        public static string GetJwtConfig()
        {
            return JwtSecret;
        }

        public static int GetPasswordConfig()
        {
            return Rounds;
        }
    }
}