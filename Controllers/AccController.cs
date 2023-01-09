using System.Threading.Tasks;
using Dating_app.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dating_app.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccController : ControllerBase
    {
        string testId = "5faabeec-1b28-4dd8-8d17-7af578f09183";
        //Radi
        [HttpPost("interesovanje/{nazivInteresovanja}")]
        public async Task<IActionResult> DodajInteresovanje(string nazivInteresovanja)
        {
            var userId = HttpReqUtils.GetUserId(Request);

            var driver = Neo4j.Driver;
            var intService = new InteresovanjeService(driver);
            var interesovanje =await  intService.AddAsync(userId,nazivInteresovanja);

            return Ok(interesovanje);

        }
         //Radi
        [HttpDelete("interesovanje/{nazivInteresovanja}")]
        public async Task<IActionResult> ObrisiInteresovanje(string nazivInteresovanja)
        {
            var userId = HttpReqUtils.GetUserId(Request);
            var driver = Neo4j.Driver;
            var intService = new InteresovanjeService(driver);
            var interesovanje = await intService.RemoveAsync(userId, nazivInteresovanja);

            return Ok(interesovanje);
        }
        //Puca kad prvi put stavlja iz koda
        [HttpPut("mestoStanovanja/{nazivMesta}")]
        public async Task<IActionResult> PromeniMestoStanovanja(string nazivMesta)
        {
            var userId = HttpReqUtils.GetUserId(Request);

            var driver = Neo4j.Driver;
            var mestoService = new MestoService(driver);
            var mestoStanovanja =await  mestoService.AddAsync(testId,nazivMesta);

            return Ok(mestoService);

        }
        //Radi
        [HttpPut("opis/{noviOpis}")]
        public async Task<IActionResult> PromeniOpis(string noviOpis)
        {
            var userId = HttpReqUtils.GetUserId(Request);

            var driver = Neo4j.Driver;
            var infoservice = new InfoService(driver);
            var opis =await  infoservice.PromeniOpis(testId,noviOpis);

            return Ok(opis);

        }

        [HttpGet("osoba")]
        public async Task<IActionResult> SledecaOsoba()
        {
            var userId = HttpReqUtils.GetUserId(Request);
            var driver = Neo4j.Driver;
            var osobaService = new OsobaService(driver);
            var osoba = await osobaService.SledecaOsoba(testId);
            return Ok(osoba);
        }
    }
}