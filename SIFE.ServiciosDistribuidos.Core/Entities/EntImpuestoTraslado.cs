using Newtonsoft.Json;

namespace SIFE.ServiciosDistribuidos.Core.Entities
{
    public class EntImpuestoTraslado
    {
        [JsonProperty("tipo")]
        public string Tipo { get; set; }

        [JsonProperty("impuesto")]
        public string Impuesto { get; set; }

        [JsonProperty("factor")]
        public string TipoFactor { get; set; }

        [JsonProperty("tasa")]
        public decimal TasaOCuota { get; set; }

        [JsonProperty("importe")]
        public decimal Importe { get; set; }
    }
}
