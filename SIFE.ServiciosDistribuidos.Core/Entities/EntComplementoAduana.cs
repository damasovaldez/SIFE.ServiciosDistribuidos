using Newtonsoft.Json;

namespace SIFE.ServiciosDistribuidos.Core.Entities
{
    public class EntComplementoAduana
    {
        [JsonProperty("numero_pedimento")]
        public string NumeroPedimento { get; set; }
    }
}
