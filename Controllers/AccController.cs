using System.Threading.Tasks;
using Dating_app.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dating_app.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AccController : ControllerBase
    {
        [HttpPost("interesovanje/{id}")]
        public async Task<IActionResult> DodajInteresovanje(string id)
        {
            var userId = HttpReqUtils.GetUserId(Request);

            var driver = Neo4j.Driver;
            var intService = new InteresovanjeService(driver);
            var interesovanje =await  intService.AddAsync(userId,id);

            return Ok(interesovanje);

        }
        [HttpDelete("interesovanje/{id}")]
        public async Task<IActionResult> ObrisiInteresovanje(string id)
        {
            var userId = HttpReqUtils.GetUserId(Request);
            var driver = Neo4j.Driver;
            var intService = new InteresovanjeService(driver);
            var interesovanje = await intService.RemoveAsync(userId, id);

            return Ok(interesovanje);
        }
        [HttpPut("mestoStanovanja/{id}")]
        public async Task<IActionResult> PromeniMestoStanovanja(string id)
        {
            var userId = HttpReqUtils.GetUserId(Request);

            var driver = Neo4j.Driver;
            var mestoService = new MestoService(driver);
            var mestoStanovanja =await  mestoService.AddAsync("1",id);

            return Ok(mestoService);

        }

        [HttpPut("opis/{noviOpis}")]
        public async Task<IActionResult> PromeniOpis(string noviOpis)
        {
            var userId = HttpReqUtils.GetUserId(Request);

            var driver = Neo4j.Driver;
            var infoservice = new InfoService(driver);
            var opis =await  infoservice.PromeniOpis(userId,noviOpis);

            return Ok(opis);

        }

        [HttpGet("osoba")]
        public async Task<IActionResult> SledecaOsoba()
        {
            var userId = HttpReqUtils.GetUserId(Request);
            var driver = Neo4j.Driver;
            var osobaService = new OsobaService(driver);
            var osoba = await osobaService.SledecaOsoba(userId);
            return Ok(osoba);
        }
    }
}