namespace SIFE.ServiciosDistribuidos.Core.Entities
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;

    public class EntComprobante
    {
        [JsonProperty("id_factura")]
        public int IdFactura { get; set; }

        [JsonProperty("id_sucursal")]
        public int IdSucursal { get; set; }

        [JsonProperty("id_empresa")]
        public int? IdEmpresa { get; set; }

        [JsonProperty("tipo_comprobante")]
        public string TipoComprobante { get; set; }

        [JsonProperty("serie")]
        public string Serie { get; set; }

        [JsonProperty("folio")]
        public int Folio { get; set; }

        [JsonProperty("uso_cfdi")]
        public string UsoCFDi { get; set; }

        [JsonProperty("fecha_emision")]
        public DateTime FechaEmision { get; set; }

        [JsonProperty("sello_digital")]
        public string SelloDigital { get; set; }

        [JsonProperty("forma_pago")]
        public string FormaPago { get; set; }

        [JsonProperty("num_certificado")]
        public string NoCertificado { get; set; }

        [JsonProperty("condiciones_pago")]
        public string CondicionesPago { get; set; }

        [JsonProperty("subtotal")]
        public decimal Subtotal { get; set; }

        [JsonProperty("descuento")]
        public decimal? Descuento { get; set; }

        [JsonProperty("tipo_cambio")]
        public decimal? TipoCambio { get; set; }

        [JsonProperty("moneda")]
        public string Moneda { get; set; }

        [JsonProperty("total")]
        public decimal Total { get; set; }

        [JsonProperty("tipo_documento")]
        public string TipoDocumento { get; set; }

        [JsonProperty("metodo_pago")]
        public string MetodoPago { get; set; }

        [JsonProperty("metodo_pago_descripcion")]
        public string MetodoPagoDescripcion { get; set; }

        [JsonProperty("lugar_expedicion")]
        public string LugarExpedicion { get; set; }

        [JsonProperty("num_cuenta_pago")]
        public string NumCuentaPago { get; set; }

        [JsonProperty("folio_fiscal_orig")]
        public string FolioFiscalOrig { get; set; }

        [JsonProperty("serie_folio_fiscal_orig")]
        public string SerieFolioFiscalOrig { get; set; }

        [JsonProperty("fecha_folio_fiscal_orig")]
        public string FechaFolioFiscalOrig { get; set; }

        [JsonProperty("monto_folio_fiscal_orig")]
        public decimal? MontoFolioFiscalOrig { get; set; }

        [JsonProperty("emisor_rfc")]
        public string EmisorRfc { get; set; }

        [JsonProperty("emisor_nombre")]
        public string EmisorNombre { get; set; }

        [JsonProperty("regimen_fiscal")]
        public string RegimenFiscal { get; set; }

        [JsonProperty("receptor_rfc")]
        public string ReceptorRfc { get; set; }

        [JsonProperty("receptor_nombre")]
        public string ReceptorNombre { get; set; }

        [JsonProperty("importe_neto")]
        public decimal ImporteNeto { get; set; }

        [JsonProperty("cantidad_letras")]
        public string CantidadLetras { get; set; }

        [JsonProperty("cadena_original")]
        public string CadenaOriginal { get; set; }

        [JsonProperty("certificado_sat")]
        public string CertificadoSat { get; set; }

        [JsonProperty("sello_sat")]
        public string SelloSat { get; set; }

        [JsonProperty("uuid")]
        public string Uuid { get; set; }

        [JsonProperty("cbb")]
        public byte[] Cbb { get; set; }

        [JsonProperty("archivo_xml")]
        public string ArchivoXml { get; set; }

        [JsonProperty("estatus")]
        public int Estatus { get; set; }

        [JsonProperty("id_plantilla")]
        public int? IdPlantilla { get; set; }

        [JsonProperty("tiene_donataria")]
        public bool TieneDonataria { get; set; }

        [JsonProperty("donataria_leyenda")]
        public string DonatariaLeyenda { get; set; }

        [JsonProperty("donataria_no_autorizacion")]
        public string DonatariaNoAutorizacion { get; set; }

        [JsonProperty("donataria_fecha_autorizacion")]
        public DateTime? DonatariaFechaAutorizacion { get; set; }

        [JsonProperty("observaciones")]
        public string Observaciones { get; set; }

        [JsonProperty("conceptos")]
        public List<EntComprobanteDetalle> ComprobanteDetalle { get; set; }

        [JsonProperty("impuestos")]
        public List<EntComprobanteImpuesto> ComprobantesImpuestos { get; set; }

        [JsonProperty("conceptos_incluyen_impuestos")]
        public bool ConceptosIncluyenImpuestos { get; set; }

        [JsonProperty("impuestos_traslado")]
        public List<EntImpuestoTraslado> ImpuestosTraslado { get; set; }

        [JsonProperty("impuestos_retencion")]
        public List<EntImpuestoRetencion> ImpuestosRetencion { get; set; }

        [JsonProperty("esPago")]
        public bool EsPago { get; set; }

        [JsonProperty("pago_detalle")]
        public List<EntComprobantePago> ComprobantesPagos { get; set; }
    }
}