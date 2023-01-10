using System.Collections.Generic;
using System.Threading.Tasks;
using Dating_app.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;
namespace Dating_app.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MestaController : ControllerBase
    {
       [HttpGet("VratiSvaMesta")]
       public async Task<IActionResult> VratiSvaMesta()
       {
            var driver = Neo4j.Driver;
            var mservice = new MestoService(driver);
            var lista = await mservice.VratiSva();

            return Ok(lista);
       }
    }
}