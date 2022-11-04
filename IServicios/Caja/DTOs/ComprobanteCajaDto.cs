using System;
using Aplicacion.Constantes;
using IServicios.BaseDto;

namespace IServicios.Caja.DTOs
{
    public class ComprobanteCajaDto : DtoBase
    {
        public string Vendedor { get; set; }
        public string Cliente { get; set; }
        public DateTime Fecha { get; set; }
        public int Numero { get; set; }
        public decimal Total { get; set; }
        public TipoComprobante TipoComprobante { get; set; }
    }
}
