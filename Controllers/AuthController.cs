using System.Threading.Tasks;
using Dating_app.Model;
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
            if (registerDto.GodinaRodjenja <1950 || registerDto.GodinaRodjenja> System.DateTime.Now.Year-17)
            {
                return BadRequest("Nevalidna godina rodjenja.");
            }
            if(registerDto.Password.Length < 8)
                return BadRequest("Lozinka mora biti 8 ili vise karaktera!");
            var driver = Neo4j.Driver;
            var authService = new AuthService(driver);
            var user = await authService.RegisterAsync(registerDto.Email, registerDto.Password, 
                registerDto.Ime,registerDto.Prezime,registerDto.GodinaRodjenja,registerDto.Opis,registerDto.Pol);

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
    }
}