using IServicios.Articulo.DTOs;
using IServicios.Base;
using IServicios.BaseDto;
using System.Collections.Generic;

namespace IServicios.Articulo
{
    public interface IArticuloServicio : IServicio
    {
        bool VerificarSiExiste(string datoVerificar, long? entidadId = null);
        int ObtenerSiguienteNroCodigo();
        int ObtenerCantidadArticulos();
        void ActualizarStock(StockDepositoDto stock);
        void TransferirArticulo(StockDepositoDto depositoOrigen, StockDepositoDto depositoDestino, long articulo);
        IEnumerable<ArticuloVentaDto> ObtenerLookUp(string cadenaBuscar, long listraPrecioId);
        IEnumerable<StockDepositoDto> ObtenerStock(long id);
        ArticuloVentaDto ObtenerPorCodigo(string codigo, long listaPrecioId, long depositoId, bool compra = false);
    }
}
