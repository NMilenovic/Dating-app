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

         public async Task<Dictionary<string,object>> LikeOsobu(string userId,string userId2)
         {
            await using var session = _driver.AsyncSession();
            return await session.ExecuteWriteAsync(async tx =>{
                //za sad null mora nadjem neku smislenu vrednost
                var query = @"
                MATCH (u:User {userId:$userId})
                MATCH (k:User {userId:$userId2})
                MERGE (u)-[r1:LIKED]->(k)
                RETURN k as u
                LIMIT 1";
                 var cursor = await tx.RunAsync(query,new {userId,userId2});
                if(! await cursor.FetchAsync())
                {
                    return null;
                }
                var record = cursor.Current;
                var userProperties = record["u"].As<INode>().Properties;    
                return userProperties.ToDictionary(x => x.Key,x => x.Value);
            });
         }

         public async Task<Dictionary<string,object>> ProveriMatch(string userId,string userId2)
         {
            await using var session = _driver.AsyncSession();
            return await session.ExecuteReadAsync(async tx =>{
                //za sad null mora nadjem neku smislenu vrednost
                var query = @"
                MATCH (u:User {userId:$userId})
                MATCH (k:User {userId:$userId2})
                MATCH (k)-[r:LIKED]->(u)
                RETURN r as r
                LIMIT 1";
                var cursor = await tx.RunAsync(query,new {userId,userId2});
                if(!await cursor.FetchAsync())
                {
                    return null;
                }
                var record = cursor.Current;
                var userProperties = record["r"].As<IRelationship>().Properties;
                if(userProperties == null)
                        return null;
                return userProperties.ToDictionary(x => x.Key,x => x.Value);
            });
         }
    }
}