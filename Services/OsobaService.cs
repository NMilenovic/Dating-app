using System.Collections.Generic;
using System.Threading.Tasks;
using Neo4j.Driver;

namespace Dating_app.Services
{
    public class OsobaService
    {
         private  readonly IDriver _driver;

        public OsobaService(IDriver driver)
        {
            _driver = driver;
        }

         public async Task<Dictionary<string,object>> SledecaOsoba(string userId)
         {
            await using var session = _driver.AsyncSession();
            return await session.ExecuteReadAsync(async tx =>{
                var query = @"
                MATCH (u:User {userId : $userId})-[[r:STANUJE_U]]->(m)
                MATCH (k:User WHERE k.userId != $userId)-[r1:STANUJE_U] -> r.mesto;
                
                RETURN k {.userId,.ime,.prezime,.godRodjenja,.opis} as k";
                var cursor = await tx.RunAsync(query,new {userId});

                if(!await cursor.FetchAsync())
                {
                    throw new System.Exception($"Nema korisnika iz istog mesta");
                }
                return cursor.Current["k"].As<Dictionary<string,object>>();
            });
         }
    }
}