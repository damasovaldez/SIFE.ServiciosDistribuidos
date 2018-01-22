using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace SIFE.ServiciosDistribuidos.Core.Entities
{
    public class EntComprobantePago
    {
        [JsonProperty("fecha")]
        public DateTime Fecha { get; set; }

        [JsonProperty("forma_pago")]
        public string FormaPago { get; set; }

        [JsonProperty("moneda")]
        public string Moneda { get; set; }

        [JsonProperty("tipo_cambio")]
        public int? TipoCambio { get; set; }

        [JsonProperty("monto")]
        public decimal Monto { get; set; }

        [JsonProperty("numero_operacion")]
        public string NumeroOperacion { get; set; }

        [JsonProperty("emisor_cuenta_origen")]
        public string EmisorCuentaOrigen { get; set; }

        [JsonProperty("banco")]
        public string Banco { get; set; }

        [JsonProperty("cuenta_ordenante")]
        public string CuentaOrdenante { get; set; }

        [JsonProperty("emisor_cuenta_benef")]
        public string EmisorCuentaBenef { get; set; }

        [JsonProperty("cuenta_beneficiaria")]
        public string CuentaBeneficiaria { get; set; }

        [JsonProperty("tipo_cadena_pago")]
        public string TipoCadenaPago { get; set; }

        [JsonProperty("certificado_pago")]
        public string CertificadoPago { get; set; }

        [JsonProperty("cadena_pago")]
        public string CadenaPago { get; set; }

        [JsonProperty("sello_pago")]
        public string SelloPago { get; set; }

        [JsonProperty("documentos_relacionados")]
        public List<EntDocRelacionado> DocumentosRelacionados { get; set; }
    }
}
