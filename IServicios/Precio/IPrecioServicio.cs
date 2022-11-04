using IServicios.Base;
using System;

namespace IServicios.Precio
{
    public interface IPrecioServicio : IServicio
    {
        void ActualizarPrecio(decimal valor, bool esPorcentaje, DateTime fechaDeActualizacion, long? marcaId = null, long? rubroId = null, long? listaPrecioId = null, int? codigoInicial = null, int? codigoFinal = null);
    }
}
