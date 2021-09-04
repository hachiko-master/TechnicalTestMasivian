using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using TechnicalTestMasivian.Data;
using TechnicalTestMasivian.DTO;
using TechnicalTestMasivian.Models;

namespace TechnicalTestMasivian.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RouletteController : ControllerBase
    {
        DBFunctions db = new DBFunctions();
        [Route("all")]
        [HttpGet]
        public IActionResult GetRoulettes()
        {
            List<Roulette> listRoulettes = db.GetAllRoulettes();

            return Ok(listRoulettes);
        }

        [Route("create")]
        [HttpPost]
        public IActionResult CreateRoulette()
        {
            int rouletteId = db.CreateRoulette();

            return Ok(rouletteId);
        }
        
        [Route("open/{rouletteId}")]
        [HttpPut]
        public IActionResult OpenRoulette([FromRoute(Name = "rouletteId")] int rouletteId)
        {
            try
            {
                int statusRoulette = db.OpenRoulette(rouletteId: rouletteId);

                return Ok(statusRoulette);
            }
            catch (System.Exception)
            {
                return StatusCode(500);
            }
        }

        [Route("bet/{rouletteId}")]
        [HttpPost]
        public IActionResult BetRoulette([FromHeader(Name = "userId")] int userId, [FromRoute(Name = "rouletteId")] int rouletteId, [FromBody] BetRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            if(request.money < 0 || request.money>10000)
                return BadRequest(new { error = true, msg = "The field money must be between 1 and 10000", code = 1 });
            if (request.option < -2 && request.option > 36)
                return BadRequest(new { error = true, msg = "The field option must be between -2 and 36", code = 2 });
            try
            {
                int statusRoulette = db.CreateBet(rouletteId: rouletteId, userId: userId, betOption: request.option, betMoney: request.money);

                return Ok(statusRoulette);
            }
            catch (System.Exception)
            {
                return StatusCode(500);
            }
        }
    }
}
