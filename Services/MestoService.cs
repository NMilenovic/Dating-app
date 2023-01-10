using System.Collections.Generic;
using System.Linq;
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
        //Treba se doda za if
        public async Task<Dictionary<string,object>> AddAsync(string userId, string nazivMesta)
        {
            await using var session = _driver.AsyncSession();

            return await session.ExecuteWriteAsync(async tx =>{
                var query = @"
                MATCH (u:User {userId : $userId}),
                    (m:Mesto {naziv:$nazivMesta})
                OPTIONAL MATCH (u) - [k:STANUJE_U] -> (n:Mesto)
                DELETE k
                MERGE (u) - [r:STANUJE_U] -> (m)
                
                RETURN m{
                    .*} as mesto";
                var cursor = await tx.RunAsync(query,new {userId,nazivMesta});
                if(!await cursor.FetchAsync())
                {
                    throw new System.Exception($"Ne moze da se promeni mesto stanovanja");
                }
                return cursor.Current["mesto"].As<Dictionary<string,object>>();
            });
        }
        public async Task<IEnumerable<string>> VratiSva()
        {
            await using var session = _driver.AsyncSession();
            return await session.ExecuteReadAsync(async tx =>{
                var query = "MATCH (m:Mesto) RETURN m.naziv AS naziv";
                var cursor = await tx.RunAsync(query);
                var records = await cursor.ToListAsync();
                var lista = records.Select(x => x["naziv"].As<string>());
                return lista;
            });
        }
        }
}