using System.Collections.Generic;
using System.Linq;
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
            return await session.ExecuteWriteAsync(async tx =>{
                //query dobar
                var query = @"
                MATCH (u:User {userId :$userId})-[r:STANUJE_U]->(m:Mesto)
                MATCH (k:User WHERE k.userId <> u.userId)-[r1:STANUJE_U] ->(m1:Mesto {naziv:m.naziv})
                WHERE NOT (u) -[:VEC_VIDEO]->(k) 
                MERGE (u) -[v:VEC_VIDEO]->(k)
                RETURN k as k
                LIMIT 1";
                var cursor = await tx.RunAsync(query,new {userId});

                if(! await cursor.FetchAsync())
                {
                    return null;
                }
                var record = cursor.Current;
                var userProperties = record["k"].As<INode>().Properties;    
                return userProperties.ToDictionary(x => x.Key,x => x.Value);
            });
         }
    }
}