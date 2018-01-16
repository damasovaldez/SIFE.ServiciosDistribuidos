using SIFE.ServiciosDistribuidos.Core.Entities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using SIFE.ServiciosDistribuidos.Core.Extensions;
using Profact.TimbraCFDI33;

namespace SIFE.ServiciosDistribuidos.Core.Services
{
    public interface IComprobanteService
    {
        EntResultadoTimbrado EmitirComprobante(EntComprobante comprobante, bool emitidoEnMatriz);
    }

    public class ComprobanteService : IComprobanteService
    {
        private readonly bool _ambienteProductivo;
        private readonly string _usuarioIntegrador;

        public ComprobanteService()
        {
            // Obtenemos la configuración para el componente TimbraCFDi
            _ambienteProductivo = ConfigurationManager.AppSettings["ambienteProductivo"].ToBoolean();
            _usuarioIntegrador  = ConfigurationManager.AppSettings["usuarioIntegrador"].ToString();
        }

        public EntResultadoTimbrado EmitirComprobante(EntComprobante comprobante, bool emitidoEnMatriz)
        {
            try
            {
                // Se valida que el componente no sea null
                if (comprobante == null)
                    throw new ArgumentNullException(nameof(comprobante));

                // El comprobante debe tener al menos un concepto 
                var totalProductos = comprobante.ComprobanteDetalle.Count;
                if (totalProductos == 0)
                    throw new Exception("El comprobante no tiene conceptos definidos, agregue un por lo menos un concepto para emitir el comprobante");

                // Inicializamos el conector y establecemos las credenciales para el permiso de conexión
                var conector = new Conector(_ambienteProductivo);

                conector.EstableceCredenciales(_usuarioIntegrador);

                //Creamos un comprobante por medio de la entidad Comprobante
                var CFDi = new Comprobante();

                // Llenamos datos del comprobante
                switch (comprobante.TipoComprobante.ToLower())
                {
                    case "ingreso":
                        CFDi.TipoDeComprobante = "I";
                        break;
                    case "egreso":
                        CFDi.TipoDeComprobante = "E";
                        break;
                    case "traslado:":
                        CFDi.TipoDeComprobante = "T";
                        break;
                    case "nomina":
                        CFDi.TipoDeComprobante = "N";
                        break;
                    case "pago":
                        CFDi.TipoDeComprobante = "P";
                        break;
                }
                CFDi.Serie = string.IsNullOrEmpty(comprobante.Serie.Trim()) ? null : comprobante.Serie.Trim();
                CFDi.Folio = comprobante.Folio.ToString();
                CFDi.Version = "3.3";
                CFDi.Fecha = comprobante.FechaEmision;
                CFDi.FormaPago = comprobante.FormaPago;
                CFDi.FormaPagoSpecified = true;
                CFDi.MetodoPago = comprobante.MetodoPago;
                CFDi.MetodoPagoSpecified = true;
                CFDi.Total = comprobante.Total.ToDecimal();
                CFDi.SubTotal = comprobante.Subtotal.ToDecimal();
                CFDi.LugarExpedicion = comprobante.LugarExpedicion;
                CFDi.CondicionesDePago = string.IsNullOrEmpty(comprobante.CondicionesPago.Trim()) ? null : comprobante.CondicionesPago.Trim();
                if (comprobante.Descuento != null && comprobante.Descuento.Value > 0)
                {
                    CFDi.Descuento = comprobante.Descuento.Value.ToDecimal();
                    CFDi.DescuentoSpecified = true;
                }
                CFDi.Moneda = comprobante.Moneda;
                CFDi.TipoCambio = comprobante.TipoCambio.Value;
                CFDi.TipoCambioSpecified = true;

                // Folio del comprobante original (para parcialidades)
                // Serie del comprobante original (para parcialidades)
                // Fecha del comprobante original (para parcialidades)
                // Monto del comprobante original (para parcialidades)

                //Llenamos datos del emisor
                CFDi.Emisor = new ComprobanteEmisor
                {
                    Rfc = comprobante.EmisorRfc,
                    Nombre = comprobante.EmisorNombre,
                    RegimenFiscal = comprobante.RegimenFiscal
                };

                //Llena datos del receptor
                CFDi.Receptor = new ComprobanteReceptor
                {
                    Rfc = comprobante.ReceptorRfc,
                    Nombre = comprobante.ReceptorNombre,
                    UsoCFDI = comprobante.UsoCFDi
                };

                //Llenamos los conceptos
                CFDi.Conceptos = new ComprobanteConcepto[totalProductos];
                var indx = 0;
                foreach (var detalle in comprobante.ComprobanteDetalle.ToList())
                {
                    CFDi.Conceptos[indx] = new ComprobanteConcepto();
                    CFDi.Conceptos[indx].ClaveProdServ = detalle.ClaveProdServ;
                    CFDi.Conceptos[indx].ClaveUnidad = detalle.ClaveUnidad;
                    CFDi.Conceptos[indx].Cantidad = detalle.Cantidad;
                    CFDi.Conceptos[indx].Unidad = detalle.Unidad;
                    CFDi.Conceptos[indx].NoIdentificacion = !string.IsNullOrEmpty(detalle.NoIdentificacion) ? detalle.NoIdentificacion : null;
                    CFDi.Conceptos[indx].Descripcion = detalle.Descripcion;
                    CFDi.Conceptos[indx].ValorUnitario = Math.Round(detalle.ValorUnitario, 2);
                    CFDi.Conceptos[indx].Importe = Math.Round(detalle.Importe, 2);

                    if(detalle.TieneInformacionAduanera)
                    {
                        var informacionAduanera = new ComprobanteConceptoInformacionAduanera();
                        informacionAduanera.NumeroPedimento = detalle.ComplementoAduana.NumeroPedimento;
                        CFDi.Conceptos[indx].InformacionAduanera = new ComprobanteConceptoInformacionAduanera[1];
                        CFDi.Conceptos[indx].InformacionAduanera[0] = informacionAduanera;
                    }

                    if (detalle.Descuento != 0)
                    {
                        CFDi.Conceptos[indx].Descuento = detalle.Descuento;
                        CFDi.Conceptos[indx].DescuentoSpecified = true;
                    }

                    if (detalle.TasaTraIva.HasValue)
                    {
                        CFDi.Conceptos[indx].Impuestos = new ComprobanteConceptoImpuestos();
                        CFDi.Conceptos[indx].Impuestos.Traslados = new ComprobanteConceptoImpuestosTraslado[1];
                        CFDi.Conceptos[indx].Impuestos.Traslados[0] = new ComprobanteConceptoImpuestosTraslado();
                        CFDi.Conceptos[indx].Impuestos.Traslados[0].Base = detalle.Importe - detalle.Descuento;
                        CFDi.Conceptos[indx].Impuestos.Traslados[0].Impuesto = "002";
                        CFDi.Conceptos[indx].Impuestos.Traslados[0].TipoFactor = "Tasa";
                        CFDi.Conceptos[indx].Impuestos.Traslados[0].Importe = detalle.ImporteTraIva.Value;
                        CFDi.Conceptos[indx].Impuestos.Traslados[0].ImporteSpecified = true;
                        CFDi.Conceptos[indx].Impuestos.Traslados[0].TasaOCuota = detalle.TasaTraIva.Value.ToString();
                        CFDi.Conceptos[indx].Impuestos.Traslados[0].TasaOCuotaSpecified = true;
                    }
                    if (detalle.TasaTraIeps.HasValue)
                    {
                        CFDi.Conceptos[indx].Impuestos = new ComprobanteConceptoImpuestos();
                        CFDi.Conceptos[indx].Impuestos.Traslados = new ComprobanteConceptoImpuestosTraslado[1];
                        CFDi.Conceptos[indx].Impuestos.Traslados[0] = new ComprobanteConceptoImpuestosTraslado();
                        CFDi.Conceptos[indx].Impuestos.Traslados[0].Base = detalle.Importe - detalle.Descuento;
                        CFDi.Conceptos[indx].Impuestos.Traslados[0].Impuesto = "003";
                        CFDi.Conceptos[indx].Impuestos.Traslados[0].TipoFactor = "Tasa";
                        CFDi.Conceptos[indx].Impuestos.Traslados[0].Importe = detalle.ImporteTraIeps.Value;
                        CFDi.Conceptos[indx].Impuestos.Traslados[0].ImporteSpecified = true;
                        CFDi.Conceptos[indx].Impuestos.Traslados[0].TasaOCuota = detalle.TasaTraIeps.Value.ToString();
                        CFDi.Conceptos[indx].Impuestos.Traslados[0].TasaOCuotaSpecified = true;
                    }
                    if (detalle.TasaRetIva.HasValue)
                    {
                        CFDi.Conceptos[indx].Impuestos = new ComprobanteConceptoImpuestos();
                        CFDi.Conceptos[indx].Impuestos.Retenciones = new ComprobanteConceptoImpuestosRetencion[1];
                        CFDi.Conceptos[indx].Impuestos.Retenciones[0] = new ComprobanteConceptoImpuestosRetencion();
                        CFDi.Conceptos[indx].Impuestos.Retenciones[0].Base = detalle.Importe - detalle.Descuento;
                        CFDi.Conceptos[indx].Impuestos.Retenciones[0].Impuesto = "002";
                        CFDi.Conceptos[indx].Impuestos.Retenciones[0].TipoFactor = "Tasa";
                        CFDi.Conceptos[indx].Impuestos.Retenciones[0].Importe = detalle.ImporteRetIva.Value;
                        CFDi.Conceptos[indx].Impuestos.Retenciones[0].TasaOCuota = detalle.TasaRetIva.Value;
                    }
                    if (detalle.TasaRetIsr.HasValue)
                    {
                        CFDi.Conceptos[indx].Impuestos = new ComprobanteConceptoImpuestos();
                        CFDi.Conceptos[indx].Impuestos.Retenciones = new ComprobanteConceptoImpuestosRetencion[1];
                        CFDi.Conceptos[indx].Impuestos.Retenciones[0] = new ComprobanteConceptoImpuestosRetencion();
                        CFDi.Conceptos[indx].Impuestos.Retenciones[0].Base = detalle.Importe - detalle.Descuento;
                        CFDi.Conceptos[indx].Impuestos.Retenciones[0].Impuesto = "001";
                        CFDi.Conceptos[indx].Impuestos.Retenciones[0].TipoFactor = "Tasa";
                        CFDi.Conceptos[indx].Impuestos.Retenciones[0].Importe = detalle.ImporteRetIsr.Value;
                        CFDi.Conceptos[indx].Impuestos.Retenciones[0].TasaOCuota = detalle.TasaRetIsr.Value;
                    }

                    //Llenamos los complementos si existiera alguno                    
                    //if (detalle.Complementos.Count() > 0)
                    //{
                    //    CFDi.Conceptos[indx].Items = new Object[detalle.Complementos.Count()];
                    //    int compIndx = 0;
                    //    foreach (IComplementoConcepto complemento in detalle.Complementos)
                    //    {
                    //        var type = complemento.GetType();
                    //        switch (type.Name)
                    //        {
                    //            case "EntComplementoAduana":
                    //                var complementoAduana = complemento as EntComplementoAduana;

                    //                CFDi.Conceptos[indx].Items[compIndx] = new t_InformacionAduanera()
                    //                {
                    //                    aduana = complementoAduana.Nombre,
                    //                    numero = complementoAduana.Numero,
                    //                    fecha = complementoAduana.Fecha
                    //                };
                    //                break;
                    //            case "EntComplementoPredial":
                    //                var complementoPredial = complemento as EntComplementoPredial;

                    //                CFDi.Conceptos[indx].Items[compIndx] = new ComprobanteConceptoCuentaPredial()
                    //                {
                    //                    numero = complementoPredial.Numero
                    //                };
                    //                break;
                    //            case "EntComplementoParte":
                    //                var complementoParte = complemento as EntComplementoParte;

                    //                CFDi.Conceptos[indx].Items[compIndx] = new ComprobanteConceptoParte()
                    //                {
                    //                    cantidad = complementoParte.Cantidad,
                    //                    unidad = complementoParte.Unidad,
                    //                    noIdentificacion = complementoParte.NoIdentificacion,
                    //                    descripcion = complementoParte.Descripcion,
                    //                    valorUnitario = complementoParte.ValorUnitario,
                    //                    importe = complementoParte.Importe
                    //                };
                    //                break;
                    //        }
                    //        compIndx++;
                    //    }
                    //}

                    indx++;
                }

                //Se integran los impuestos
                var detalles = comprobante.ComprobanteDetalle.ToList() as List<EntComprobanteDetalle>;
                var impuestosTraslado = new List<EntImpuestoTraslado>();
                var impuestosRetencion = new List<EntImpuestoRetencion>();

                if (comprobante.ConceptosIncluyenImpuestos)
                {
                    foreach (EntComprobanteDetalle detalle in detalles)
                    {
                        if (detalle.TasaTraIva.HasValue)
                        {
                            impuestosTraslado.Add(new EntImpuestoTraslado() { Tipo = "IVA", TasaOCuota = detalle.TasaTraIva.Value, Importe = detalle.ImporteTraIva.Value });
                        }
                        if (detalle.TasaTraIeps.HasValue)
                        {
                            impuestosTraslado.Add(new EntImpuestoTraslado() { Tipo = "IEPS", TasaOCuota = detalle.TasaTraIeps.Value, Importe = detalle.ImporteTraIeps.Value });
                        }
                        if (detalle.TasaRetIva.HasValue)
                        {
                            impuestosRetencion.Add(new EntImpuestoRetencion() { Tipo = "IVA", TasaOCuota = detalle.TasaRetIva.Value, Importe = detalle.ImporteRetIva.Value });
                        }
                        if (detalle.TasaRetIsr.HasValue)
                        {
                            impuestosRetencion.Add(new EntImpuestoRetencion() { Tipo = "ISR", TasaOCuota = detalle.TasaRetIsr.Value, Importe = detalle.ImporteRetIsr.Value });
                        }
                    }
                }

                var impuestosTrasladoGroup = (from p in impuestosTraslado
                                              group p by new { p.Tipo, p.TasaOCuota }
                                                  into grupo
                                              select new
                                              {
                                                  Tipo = grupo.Key.Tipo,
                                                  Tasa = grupo.Key.TasaOCuota,
                                                  Importe = grupo.Sum(x => x.Importe)
                                              }).ToList();

                var impuestosRetencionGroup = (from p in impuestosRetencion
                                               group p by new { p.Tipo, p.TasaOCuota }
                                                   into grupo
                                               select new
                                               {
                                                   Tipo = grupo.Key.Tipo,
                                                   Tasa = grupo.Key.TasaOCuota,
                                                   Importe = grupo.Sum(x => x.Importe)
                                               }).ToList();

                //Se registran los impuestos federales
                CFDi.Impuestos = new ComprobanteImpuestos();

                if (impuestosTrasladoGroup.Count > 0)
                {
                    decimal totalImpuestosTrasladados = 0;
                    CFDi.Impuestos.Traslados = new ComprobanteImpuestosTraslado[impuestosTrasladoGroup.Count];

                    var i = 0;
                    foreach (var impuesto in impuestosTrasladoGroup)
                    {
                        CFDi.Impuestos.Traslados[i] = new ComprobanteImpuestosTraslado();
                        if (impuesto.Tipo == "IVA")
                        {
                            CFDi.Impuestos.Traslados[i].Impuesto = "002";
                            CFDi.Impuestos.Traslados[i].TipoFactor = "Tasa";
                            CFDi.Impuestos.Traslados[i].TasaOCuota = impuesto.Tasa.ToString();
                            CFDi.Impuestos.Traslados[i].Importe = impuesto.Importe.ToString("##0.00").ToDecimal();
                        }
                        if (impuesto.Tipo == "IEPS")
                        {
                            CFDi.Impuestos.Traslados[i].Impuesto = "003";
                            CFDi.Impuestos.Traslados[i].TipoFactor = "Tasa";
                            CFDi.Impuestos.Traslados[i].TasaOCuota = impuesto.Tasa.ToString();
                            CFDi.Impuestos.Traslados[i].Importe = impuesto.Importe.ToString("##0.00").ToDecimal();                            
                        }

                        totalImpuestosTrasladados += impuesto.Importe;
                        i++;
                    }

                    CFDi.Impuestos.TotalImpuestosTrasladados = totalImpuestosTrasladados;
                    CFDi.Impuestos.TotalImpuestosTrasladadosSpecified = true;
                }

                if (impuestosRetencionGroup.Count > 0)
                {
                    decimal totalImpuestosRetenidos = 0;
                    CFDi.Impuestos.Retenciones = new ComprobanteImpuestosRetencion[impuestosRetencionGroup.Count];

                    var i = 0;
                    foreach (var impuesto in impuestosRetencionGroup)
                    {
                        CFDi.Impuestos.Retenciones[i] = new ComprobanteImpuestosRetencion();
                        if (impuesto.Tipo == "IVA")
                        {
                            CFDi.Impuestos.Retenciones[i].Impuesto = "002";
                            CFDi.Impuestos.Retenciones[i].Importe = impuesto.Importe.ToString("##0.00").ToDecimal();
                        }
                        if (impuesto.Tipo == "ISR")
                        {
                            CFDi.Impuestos.Retenciones[i].Impuesto = "001";
                            CFDi.Impuestos.Retenciones[i].Importe = impuesto.Importe.ToString("##0.00").ToDecimal();
                        }

                        totalImpuestosRetenidos += impuesto.Importe;
                        i++;
                    }

                    CFDi.Impuestos.TotalImpuestosRetenidos = totalImpuestosRetenidos;
                    CFDi.Impuestos.TotalImpuestosRetenidosSpecified = true;
                }

                // Se registran los complementos de la factura
                //var xmlComplementos = new List<XmlElement>();
                // Impuestos locales
                //if (comprobante.ImpuestosLocales != null)
                //{
                //if (comprobante.ImpuestosLocales.Count > 0)
                //{
                //XmlDocument impLocal = RegistrarImpuestosLocales(comprobante.ImpuestosLocales);
                //xmlComplementos.Add(impLocal.DocumentElement);
                //CFDi.ImpLocalSpecified = true;
                //}
                //}

                //Timbramos el comprobante a traves de una petición al PAC
                var resultadoTimbre = conector.TimbraCFDI(CFDi);

                //Se mapean los resultados para ser devueltos por el servicio
                var resultado = new EntResultadoTimbrado
                {
                    Exitoso = resultadoTimbre.Exitoso,
                    Mensaje = resultadoTimbre.Descripcion
                };

                if (resultadoTimbre.Exitoso)
                {
                    resultado.FolioUuid = resultadoTimbre.FolioUUID;                    //2.- Folio fiscal
                    resultado.NoCertificado = resultadoTimbre.No_Certificado;           //3.- No. Certificado del Emisor
                    resultado.NoCertificadoSat = resultadoTimbre.No_Certificado_SAT;    //4.- No. Certificado del SAT
                    resultado.FechaCertificacion = resultadoTimbre.FechaCertificacion;  //5.- Fecha y Hora de certificación
                    resultado.SelloCfdi = resultadoTimbre.SelloCFDI;                    //6.- Sello del cfdi
                    resultado.SelloSat = resultadoTimbre.SelloSAT;                      //7.- Sello del SAT
                    resultado.CadenaTimbre = resultadoTimbre.CadenaTimbre;              //8.- Cadena original de complemento de certificación
                    resultado.Xml = resultadoTimbre.Xml;
                    resultado.CbbBytes = resultadoTimbre.CodigoBidimensional;
                }

                return resultado;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw e.GetBaseException();
            }
        }
    }
}