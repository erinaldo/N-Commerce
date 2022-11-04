using IServicios.BaseDto;
using System;

namespace IServicios.Gasto.DTOs
{
    public class GastoDTO : DtoBase
    {
        public long ConceptoGastoId { get; set; }
        public DateTime Fecha { get; set; }
        public String Descripcion { get; set; }
        public decimal Monto { get; set; }
    }
}
