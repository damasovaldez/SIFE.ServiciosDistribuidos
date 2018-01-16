using System.Web.Http;
using SIFE.ServiciosDistribuidos.Core.Models;
using SIFE.ServiciosDistribuidos.Core.Services;
using System.Linq;
using System.Web.Http.Cors;

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

    }
}