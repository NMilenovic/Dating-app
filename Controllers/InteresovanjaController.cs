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
    public class InteresovanjaController : ControllerBase
    {
        //Radi
       [HttpGet("VratiSvaInteresovanja")]
       public async Task<IActionResult> VratiSvaInteresovanja()
       {
            var driver = Neo4j.Driver;
            var iservice = new InteresovanjeService(driver);
            var lista = await iservice.VratiSva();

            return Ok(lista);
       }
    }
}