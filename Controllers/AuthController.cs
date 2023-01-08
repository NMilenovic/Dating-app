using System.Threading.Tasks;
using Dating_app.Services;
using Microsoft.AspNetCore.Mvc;
namespace Dating_app.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
         [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterDto registerDto)
        {
            var driver = Neo4j.Driver;
            var authService = new AuthService(driver);

            var user = await authService.RegisterAsync(registerDto.Email, registerDto.Password, 
                registerDto.Ime,registerDto.Prezime,registerDto.GodinaRodjenja,registerDto.Opis);

            return Ok(user);
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody]LoginDto loginDto)
        {
            var driver = Neo4j.Driver;
            var authService = new AuthService(driver);

            var user = await authService.LoginAsync(loginDto.Email,loginDto.Password);

            return Ok(user);
        }
         public class RegisterDto
        {
            public string Email { get; init; }
            public string Password { get; init; }
            public string Ime { get; init; }
            public string Prezime {get; init;}
            public int GodinaRodjenja { get; init; }
            public string Opis {get;init;}

            //Fali avatar
            //Interesovanja
        }

        public class LoginDto
        {
            public string Email { get; init; }
            public string Password { get; init; }
        }
    }
}