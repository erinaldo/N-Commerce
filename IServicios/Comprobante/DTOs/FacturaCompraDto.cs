using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IServicios.Comprobante.DTOs
{
    public class FacturaCompraDto : FacturaDto
    {
        public decimal Iva27 { get; set; }
        public decimal ImpInterno { get; set; }
        public decimal PercTemp { get; set; }
        public decimal PercPyP { get; set; }
        public decimal PercIva { get; set; }
        public decimal PercIB { get; set; }
    }
}
