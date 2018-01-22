using Newtonsoft.Json;

namespace SIFE.ServiciosDistribuidos.Core.Entities
{
    public class EntResultadoCancelacion
    {
        [JsonProperty("exitoso")]
        public bool Exitoso { get; set; }

        [JsonProperty("mensaje")]
        public string Mensaje { get; set; }
    }
}
