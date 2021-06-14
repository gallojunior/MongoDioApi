using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using MongoDioApi.Data.Collections;
using MongoDioApi.Models;
using System.Linq;

namespace MongoDioApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class InfectadosController : ControllerBase
    {
        Data.MongoDB _mongoDB;
        IMongoCollection<Infectado> _infectadosCollection;

        public InfectadosController(Data.MongoDB mongoDB)
        {
            _mongoDB = mongoDB;
            _infectadosCollection = _mongoDB.DB.GetCollection<Infectado>(typeof(Infectado).Name.ToLower());
        }

        [HttpGet]
        public ActionResult ObterInfectados()
        {
            var infectados = _infectadosCollection.Find(Builders<Infectado>.Filter.Empty).ToList()
                .Select(i => new InfectadoDto
                {
                    DataNascimento = i.DataNascimento,
                    Sexo = i.Sexo,
                    Latitude = i.Localizacao.Latitude,
                    Longitude = i.Localizacao.Longitude
                });

            return Ok(infectados);
        }

        [HttpPost]
        public ActionResult SalvarInfectado([FromBody] InfectadoDto dto)
        {
            var infectado = new Infectado(dto.DataNascimento, dto.Sexo, dto.Latitude, dto.Longitude);

            _infectadosCollection.InsertOne(infectado);

            return StatusCode(201, "Infectado adicionado com sucesso");
        }
    }
}
