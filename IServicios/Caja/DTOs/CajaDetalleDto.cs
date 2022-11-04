using Aplicacion.Constantes;
using IServicios.BaseDto;

namespace IServicios.Caja.DTOs
{
    public class CajaDetalleDto : DtoBase
    {
        public TipoPago TipoPago { get; set; }

        public decimal Monto { get; set; }
    }
}
