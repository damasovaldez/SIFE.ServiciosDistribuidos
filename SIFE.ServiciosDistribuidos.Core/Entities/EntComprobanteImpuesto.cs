namespace SIFE.ServiciosDistribuidos.Core.Entities
{
    using Newtonsoft.Json;

    public class EntComprobanteImpuesto
    {
        [JsonProperty("id_impuesto")]
        public int IdImpuesto { get; set; }

        [JsonProperty("id_factura")]
        public int IdFactura { get; set; }

        [JsonProperty("descripcion")]
        public string Descripcion { get; set; }

        [JsonProperty("tasa")]
        public decimal Tasa { get; set; }

        [JsonProperty("importe")]
        public decimal Importe { get; set; }

        [JsonProperty("tipo")]
        public int Tipo { get; set; }

    }
}
