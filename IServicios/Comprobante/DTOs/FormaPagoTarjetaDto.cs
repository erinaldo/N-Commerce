using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IServicios.Comprobante.DTOs
{
    public class FormaPagoTarjetaDto : FormaPagoDto
    {
        public long TarjetaId { get; set; }
        public string NumeroTarjeta { get; set; }
        public string CuponPago { get; set; }
        public int CantidadCuotas { get; set; }

    }
}
