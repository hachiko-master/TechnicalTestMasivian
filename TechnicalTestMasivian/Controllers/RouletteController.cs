using Microsoft.AspNetCore.Mvc;
using System;
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
        public IActionResult CreateBetRoulette([FromHeader(Name = "userId")] int userId, [FromRoute(Name = "rouletteId")] int rouletteId, [FromBody] BetRequest request)
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

        [Route("close/{rouletteId}")]
        [HttpPut]
        public IActionResult CloseRoulette([FromRoute(Name = "rouletteId")] int rouletteId)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            try
            {
                List<Winners> listWinners = db.CloseRoulette(rouletteId);
                listWinners = CalculateWinMoney(listWinners);

                return Ok(listWinners);
            }
            catch (System.Exception)
            {
                return StatusCode(500);
            }
        }

        public List<Winners> CalculateWinMoney(List<Winners> listWinners)
        {
            Random random = new Random();
            List<Winners> resultListWinners = new List<Winners>();
            int numRandomRoulette = random.Next(0, 37);
            double winMoney = 0;
            foreach (Winners item in listWinners)
            {
                if (item.BetOption >= 0 && item.BetOption <= 36)
                    winMoney = item.BetMoney * 5;
                else if(item.BetOption == -1 && numRandomRoulette%2==0)
                    winMoney = item.BetMoney * 1.8;
                else if (item.BetOption == -2 && numRandomRoulette % 2 != 0)
                    winMoney = item.BetMoney * 1.8;
                item.WinMoney = winMoney;
                resultListWinners.Add(item);
            }

            return resultListWinners;
        }
    }
}
