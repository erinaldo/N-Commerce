using IServicios.Base;
using IServicios.BaseDto;
using System.Collections.Generic;

namespace IServicios.Deposito
{
    public interface IDepositoServicio : IServicio
    {
        IEnumerable<DtoBase> Obtener(string cadenaBuscar, bool mostrarTodos = false, string exceptuar = "");
        bool VerificarSiExiste(string datoVerificar, long? entidadId = null);
    }
}
