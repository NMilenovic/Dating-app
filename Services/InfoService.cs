using System.Collections.Generic;
using System.Threading.Tasks;
using Neo4j.Driver;

namespace Dating_app.Services
{
    public class InfoService
    {
        private  readonly IDriver _driver;

        public InfoService(IDriver driver)
        {
            _driver = driver;
        }

        public async Task<Dictionary<string,object>> PromeniOpis(string userId,string noviOpis)
        {
             await using var session = _driver.AsyncSession();
            return await session.ExecuteWriteAsync(async tx =>{
                var query = @"
                MATCH (u:User {userId : $userId})
                SET u.opis = $noviOpis
                
                RETURN u {.userId,.email,.ime,.prezime,.godRodjenja,.opis} as u";
                var cursor = await tx.RunAsync(query,new {userId,noviOpis});

                if(!await cursor.FetchAsync())
                {
                    throw new System.Exception($"Ne moze da se promeni opis");
                }
                return cursor.Current["u"].As<Dictionary<string,object>>();
            });
        }
    }
}