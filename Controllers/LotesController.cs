using API_CAFETAL.Clases;
using API_CAFETAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace API_CAFETAL.Controllers
{
    [RoutePrefix("api/Lotes")]
    public class LotesController : ApiController
    {
        [HttpGet]
        [Route("ConsultarTodos")]

        public List<LOTE> ConsultarTodos()
        {
            clsLote Lote = new clsLote();
            return Lote.ConsultarTodos();
        }

        [HttpPost]
        [Route("Insertar")]

        public string Insertar([FromBody] LOTE lote)
        {
            clsLote Lote = new clsLote();

            Lote.lote = lote;

            return Lote.Insertar();
        }
    }
}