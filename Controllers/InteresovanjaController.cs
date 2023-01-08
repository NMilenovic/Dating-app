using System.Threading.Tasks;
using Dating_app.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;
namespace Dating_app.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InteresovanjaController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> VratiSve()
        {
            var driver = Neo4j.Driver;
            var movieService = new InteresovanjeService(driver);

            var movies = await movieService.VratiSve();

            return Ok(movies);
        }
    }
}