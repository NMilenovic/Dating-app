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
        string testId = "4f4aa873-dc8f-469f-bac1-a35885ac7ec6";
        string testId2 ="a0ca45f0-da27-4d01-9a07-a8468adaea09";
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
            var mestoStanovanja =await  mestoService.AddAsync(userId,nazivMesta);

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
        [HttpGet("osoba/likeOsobu/{id1}")]
        public async Task<IActionResult> LikeOsobu(string id1)
        {
            var userId = HttpReqUtils.GetUserId(Request);
            var driver = Neo4j.Driver;
            var osobaService = new OsobaService(driver);
            var osoba = await osobaService.LikeOsobu(userId,id1);
            return Ok(osoba);
        }

        //Ako vrati status code 204 onda nije match, ako vrati bilo sta drugo match je
        [HttpGet("osoba/proveriMatch/{id1}")]
        public async Task<IActionResult> ProveriMatch(string id1)
        {
            var userId = HttpReqUtils.GetUserId(Request);
            var driver = Neo4j.Driver;
            var osobaService = new OsobaService(driver);
            var osoba = await osobaService.ProveriMatch(userId,id1);
            if(osoba == null)
                return Ok(null);
            return Ok(osoba);
        }

         [HttpPut("osoba/dodaj/{id1}")]
         public async Task<IActionResult> Dodaj(string id1)
        {
            var userId = HttpReqUtils.GetUserId(Request);
            var driver = Neo4j.Driver;
            var osobaService = new OsobaService(driver);
            var osoba = await osobaService.Dodaj(userId,id1);
            if(osoba == null)
                return Ok(null);
            return Ok($"Korisnik sa {userId} i korisnik {id1} su sada prijatelji.");
        }
    }
} 