using IServicios.BaseDto;

namespace IServicios.Articulo.DTOs
{
    public class StockDepositoDto : DtoBase
    {
        public string Deposito { get; set; }
        public decimal Cantidad { get; set; }
    }
}
