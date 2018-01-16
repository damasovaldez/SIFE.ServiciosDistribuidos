namespace SIFE.ServiciosDistribuidos.Core.Models
{
    using Newtonsoft.Json;
    using Entities;

    public class TimbradoRequest
    {
        [JsonProperty("comprobante")]
        public EntComprobante Comprobante { get; set; }

        [JsonProperty("enviar_correo")]
        public bool EnviarCorreo { get; set; }

        [JsonProperty("cuenta_email")]
        public string CuentaEmail { get; set; }
    }
}
