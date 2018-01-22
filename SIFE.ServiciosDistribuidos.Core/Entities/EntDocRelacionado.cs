using Newtonsoft.Json;

namespace SIFE.ServiciosDistribuidos.Core.Entities
{
    public class EntDocRelacionado
    {
        [JsonProperty("id_documento")]
        public string IdDocumento { get; set; }

        [JsonProperty("serie")]
        public string Serie { get; set; }

        [JsonProperty("folio")]
        public string Folio { get; set; }

        [JsonProperty("moneda")]
        public string Moneda { get; set; }

        [JsonProperty("tipo_cambio")]
        public decimal? TipoCambio { get; set; }

        [JsonProperty("metodo_pago")]
        public string MetodoPago { get; set; }

        [JsonProperty("num_parcialidad")]
        public int? NumParcialidad { get; set; }

        [JsonProperty("importe_saldo_anterior")]
        public decimal? ImpSaldoAnt { get; set; }

        [JsonProperty("importe_saldo_insoluto")]
        public decimal? ImpSaldoInsoluto { get; set; }

        [JsonProperty("importe_saldo_pagado")]
        public decimal? ImpPagado { get; set; }
    }
}
