using System.Threading.Tasks;
using Neo4j.Driver;

namespace Dating_app
{
    public static class Neo4j
    {
        private static IDriver _driver = null;
        public static IDriver Driver => _driver;

        public static async Task InitDriverAsync(string uri, string username, string password)
        {
            _driver = GraphDatabase.Driver(uri,AuthTokens.Basic(username,password));
            await _driver.VerifyConnectivityAsync();
        }

        public static Task CloseDriver()
        {
            return _driver != null ? _driver.CloseAsync() : Task.CompletedTask;
        }
    }
}