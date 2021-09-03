using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using TechnicalTestMasivian.Data;
using TechnicalTestMasivian.Entities;

namespace TechnicalTestMasivian.Controllers
{
    [ApiController]
    public class RouletteController : ControllerBase
    {
        DBFunctions db = new DBFunctions();
        [Route("Roulettes")]
        [HttpGet]
        public List<Roulette> GetRoulettes()
        {
            List<Roulette> listRoulettes = db.GetAllRoulletes();

            return listRoulettes;
        }

        [Route("CreateRoulette")]
        [HttpGet]
        public string CreateRoulette()
        {
            string message = "";
            int resultRoulette = db.CreateRoullete();
            if (resultRoulette == 1)
                message = "La ruleta ha sido creada correctamente";
            else
                message = "Ha ocurrido un error en la cración de la ruleta";

            return message;
        }
    }
}
