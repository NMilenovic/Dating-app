using System.Collections.Generic;
using System.Threading.Tasks;
using Neo4j.Driver;

namespace Dating_app.Services
{
    public class MestoService
    {
        private readonly IDriver _driver;
        
        public MestoService(IDriver driver)
        {
            _driver = driver;
        }

        public async Task<Dictionary<string,object>> AddAsync(string userId, string id)
        {
            await using var session = _driver.AsyncSession();

            return await session.ExecuteWriteAsync(async tx =>{
                var query = @"
                MATCH (u:User {userId : $userId})
                MATCH (m:Mesto {id: $id})
                
                MERGE (u) - [r:STANUJE_U] -> (m)
                
                RETURN m{
                    .*} as mesto";
                var cursor = await tx.RunAsync(query,new {userId,id});
                if(!await cursor.FetchAsync())
                {
                    throw new System.Exception($"Ne moze da se promeni mesto stanovanja");
                }
                return cursor.Current["mesto"].As<Dictionary<string,object>>();
            });
        }
        }
}