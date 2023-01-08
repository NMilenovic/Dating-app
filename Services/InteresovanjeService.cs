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

        public async Task<Dictionary<string,object>> AddAsync(string userId, string id)
        {
            await using var session = _driver.AsyncSession();

            return await session.ExecuteWriteAsync(async tx =>{
                var query = @"
                MATCH (u:User {userId:$userId})
                MATCH (i:Interesovanje {id: $id})
                
                MERGE (u) - [r:IMA_INTERESOVANJE] -> (i)
                
                RETURN i {.*} as interesovanje";
                var cursor = await tx.RunAsync(query,new {userId,id});

                if(!await cursor.FetchAsync())
                {
                    throw new System.Exception($"Ne moze da doda interesovanje");
                }
                return cursor.Current["interesovanje"].As<Dictionary<string,object>>();
            });
        }

        public async Task<Dictionary<string,object>> RemoveAsync(string userId, string id)
        {
            await using var session = _driver.AsyncSession();

            return await session.ExecuteWriteAsync(async tx =>{
                var query = @"
                MATCH(u:User {userId:$userId})-[r:IMA_INTRESEOVANJE]->(i:Interesovanje {id:$id}
                DELETE r
                RETURN i{
                    .*} as interesovanje";

                var cursor = await tx.RunAsync(query,new {userId,id});

                if(!await cursor.FetchAsync())
                {
                    throw new System.Exception($"Nije obrisao interesovanje korisnika");
                }

                return cursor.Current["interesovanje"].As<Dictionary<string,object>>();
            });
        }

        //Ne radi
        public async Task<Dictionary<string,object>[]> VratiSve()
        {
             await using var session = _driver.AsyncSession();
             return await session.ExecuteReadAsync(async tx =>{
                var cursor =await  tx.RunAsync("MATCH (i:Interesovanje) RETURN {ID(i),i.naziv} as int");
                var recorods = await cursor.ToListAsync();
                var interesovanja = recorods
                                    .Select(x => x["int"].As<Dictionary<string,object>>())
                                    .ToArray();
                return interesovanja;
             });
        }


    }
}