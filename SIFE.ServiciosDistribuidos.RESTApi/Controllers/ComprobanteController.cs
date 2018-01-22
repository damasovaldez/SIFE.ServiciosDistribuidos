using System.Web.Http;
using SIFE.ServiciosDistribuidos.Core.Models;
using SIFE.ServiciosDistribuidos.Core.Services;
using System.Linq;
using System.Web.Http.Cors;
using System.Collections.Generic;

namespace SIFE.ServiciosDistribuidos.RESTApi.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ComprobanteController : ApiController
    {
        IComprobanteService _comprobanteService;

        public ComprobanteController(IComprobanteService comprobanteService)
        {
            this._comprobanteService = comprobanteService;
        }

        [HttpPost]
        [Route("comprobante/timbrar")]
        public IHttpActionResult Timbrar([FromBody]TimbradoRequest request)
        {
            var r = Request;
            var headers = r.Headers;

            if (!headers.Contains("x-api-key"))
            {
                return BadRequest();
            }

            string apiKeyValue = headers.GetValues("x-api-key").First();
            if (apiKeyValue != "12345")
            {
                return Unauthorized();
            }

            var response = _comprobanteService.EmitirComprobante(request.Comprobante, true);

            return Ok(response);
        }

        [HttpPost]
        [Route("comprobante/cancelar")]
        public IHttpActionResult Cancelar([FromBody]List<string> parametros)
        {
            var r = Request;
            var headers = r.Headers;

            if (!headers.Contains("x-api-key"))
            {
                return BadRequest();
            }

            string apiKeyValue = headers.GetValues("x-api-key").First();
            if (apiKeyValue != "12345")
            {
                return Unauthorized();
            }

            string rfc  = parametros[0];
            string uuid = parametros[1];

            var response = _comprobanteService.CancelarComprobante(rfc, uuid);

            return Ok(response);
        }
    }
}