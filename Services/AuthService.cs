
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Neo4j.Driver;
using BCryptNet = BCrypt.Net.BCrypt;
namespace Dating_app.Services
{
    public class AuthService
    {
        private readonly IDriver _driver;

        public AuthService(IDriver driver)
        {
            _driver = driver;
        }
        //Dodato ogranicenje za unique email (u bazi)
        //Dodaj ogranicenje za godine
          public async Task<Dictionary<string, object>> RegisterAsync(string email, string plainPassword, string ime,
          string prezime, int godRodjenja, string opis, string pol)
        {
            var rounds = Config.GetPasswordConfig();
            var encrypted = BCryptNet.HashPassword(plainPassword, rounds);

        try{
            //pravimo sesiju
            await using var session = _driver.AsyncSession();

            var user = await session.ExecuteWriteAsync(async tx =>{
                var query = @"
                CREATE(u:User {
                userId: randomUuid(),
                email: $email,
                password: $encrypted,
                ime: $ime,
                prezime: $prezime,
                godRodjenja: $godRodjenja,
                opis: $opis,
                pol:$pol
                })
                RETURN u {.userId,.email,.ime,.prezime,.godRodjenja,.opis,.pol} as u";

                var cursor = await tx.RunAsync(query,new{email,encrypted,ime,prezime,godRodjenja,opis,pol});
                //single async jer gornji query vraca samo jedan, tj novog usera
                var record = await cursor.SingleAsync();

                return record["u"].As<Dictionary<string, object>>();
            });
                var safeProperties = SafeProperties(user);
                safeProperties.Add("token", JwtHelper.CreateToken(GetUserClaims(safeProperties)));
                return safeProperties;
            }
            catch(ClientException exception) when (exception.Code == "Neo.ClientError.Schema.ConstraintValidationFailed")
            {
                throw new Neo4jException(exception.Message, email);
            }    
        }

        public async Task<Dictionary<string,object>> LoginAsync(string email, string plainPassword)
        {
            using var session = _driver.AsyncSession();
            var user = await session.ReadTransactionAsync(async tx =>
            {
                var cursor = await tx.RunAsync("MATCH (u:User {email:$email}) RETURN u", new {email});

                if(! await cursor.FetchAsync())
                {
                    return null;
                }
                var record = cursor.Current;
                var userProperties = record["u"].As<INode>().Properties;    
                return userProperties.ToDictionary(x => x.Key,x => x.Value);
            });

            if(user == null)
            {
                return null;
            }

            if(!BCryptNet.Verify(plainPassword,user["password"].As<string>()))
                return null;
            
            var safeProperties = SafeProperties(user);
            safeProperties.Add("token",JwtHelper.CreateToken(GetUserClaims(safeProperties)));
            return safeProperties;
        }

         private Dictionary<string, object> GetUserClaims(Dictionary<string, object> user)
        {
            return new Dictionary<string, object>
            {
                ["sub"] = user["userId"],
                ["userId"] = user["userId"],
                ["ime"] = user["ime"]
            };
        }
         private static Dictionary<string, object> SafeProperties(Dictionary<string, object> user)
        {
            return user
                .Where(x => x.Key != "password")
                .ToDictionary(x => x.Key, x => x.Value);
        }
        
    }
}