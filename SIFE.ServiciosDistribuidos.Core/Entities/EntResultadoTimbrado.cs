namespace SIFE.ServiciosDistribuidos.Core.Entities
{
    using Newtonsoft.Json;

    public class EntResultadoTimbrado
    {
        [JsonProperty("exitoso")]
        public bool Exitoso { get; set; }

        [JsonProperty("mensaje")]
        public string Mensaje { get; set; }

        [JsonProperty("uuid")]
        public string FolioUuid { get; set; }

        [JsonProperty("num_certificado_emisor")]
        public string NoCertificado { get; set; }

        [JsonProperty("num_certificado_sat")]
        public string NoCertificadoSat { get; set; }

        [JsonProperty("fecha_certificacion")]
        public string FechaCertificacion { get; set; }

        [JsonProperty("sello_cfdi")]
        public string SelloCfdi { get; set; }

        [JsonProperty("sello_sat")]
        public string SelloSat { get; set; }

        [JsonProperty("cadena_timbre")]
        public string CadenaTimbre { get; set; }

        [JsonProperty("xml")]
        public string Xml { get; set; }

        [JsonProperty("cbb")]
        public byte[] CbbBytes { get; set; }
    }
}
