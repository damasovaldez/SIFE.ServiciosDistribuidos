namespace SIFE.ServiciosDistribuidos.Core.Entities
{
    using Newtonsoft.Json;
    using System.Runtime.Serialization;

    public class EntComprobanteDetalle
    {
        [JsonProperty("id_detalle")]
        public int IdDetalle { get; set; }

        [JsonProperty("id_factura")]
        public int IdFactura { get; set; }

        [JsonProperty("cantidad")]
        public decimal Cantidad { get; set; }

        [JsonProperty("unidad")]
        public string Unidad { get; set; }

        [JsonProperty("clave_unidad")]
        public string ClaveUnidad { get; set; }

        [JsonProperty("clave_prod_serv")]
        public string ClaveProdServ { get; set; }

        [JsonProperty("num_identificacion")]
        public string NoIdentificacion { get; set; }

        [JsonProperty("descripcion")]
        public string Descripcion { get; set; }

        [JsonProperty("valor_unitario")]
        public decimal ValorUnitario { get; set; }

        [JsonProperty("importe")]
        public decimal Importe { get; set; }

        [JsonProperty("descuento")]
        public decimal Descuento { get; set; }

        [JsonProperty("tasa_tra_iva")]
        public decimal? TasaTraIva { get; set; }

        [JsonProperty("importe_tra_iva")]
        public decimal? ImporteTraIva { get; set; }

        [JsonProperty("tasa_tra_ieps")]
        public decimal? TasaTraIeps { get; set; }

        [JsonProperty("importe_tra_ieps")]
        public decimal? ImporteTraIeps { get; set; }

        [JsonProperty("tasa_ret_isr")]
        public decimal? TasaRetIsr { get; set; }

        [JsonProperty("importe_ret_isr")]
        public decimal? ImporteRetIsr { get; set; }

        [JsonProperty("tasa_ret_iva")]
        public decimal? TasaRetIva { get; set; }

        [JsonProperty("importe_ret_iva")]
        public decimal? ImporteRetIva { get; set; }

        [JsonProperty("importe_tra")]
        public decimal? ImporteTra { get; set; }

        [JsonProperty("importe_ret")]
        public decimal? ImporteRet { get; set; }

        [JsonProperty("tiene_informacion_aduanera")]
        public bool TieneInformacionAduanera { get; set; }

        [JsonProperty("informacion_aduanera")]
        public EntComplementoAduana ComplementoAduana { get; set; }
    }
}