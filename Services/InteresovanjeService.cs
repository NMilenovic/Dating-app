using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Neo4j.Driver;

namespace Dating_app.Services
{
    public class InteresovanjeService
    {
        private readonly IDriver _driver;
        
        public InteresovanjeService(IDriver driver)
        {
            _driver = driver;
        }

        public async Task<Dictionary<string,object>> AddAsync(string userId, string nazivInteresovanja)
        {
            await using var session = _driver.AsyncSession();

            return await session.ExecuteWriteAsync(async tx =>{
                var query = @"
                MATCH (u:User {userId:$userId}),
                    (i:Interesovanje {naziv:$nazivInteresovanja})
                MERGE (u)-[r:IMA_INTERESOVANJE]->(i)
                RETURN i {.*} as interesovanje";
                var cursor = await tx.RunAsync(query,new {userId,nazivInteresovanja});

                if(!await cursor.FetchAsync())
                {
                    throw new System.Exception($"Ne moze da doda interesovanje");
                }
                return cursor.Current["interesovanje"].As<Dictionary<string,object>>();
            });
        }

        public async Task<Dictionary<string,object>> RemoveAsync(string userId, string nazivInteresovanja)
        {
            await using var session = _driver.AsyncSession();

            return await session.ExecuteWriteAsync(async tx =>{
                var query = @"
                MATCH(u:User {userId:$userId})-[r:IMA_INTERESOVANJE]->(i:Interesovanje {naziv:$nazivInteresovanja})
                DELETE r
                RETURN i{
                    .*} as interesovanje";

                var cursor = await tx.RunAsync(query,new {userId,nazivInteresovanja});

                if(!await cursor.FetchAsync())
                {
                    throw new System.Exception($"Nije obrisao interesovanje korisnika");
                }

                return cursor.Current["interesovanje"].As<Dictionary<string,object>>();
            });
        }

    }
}