using IServicios.BaseDto;
using System.Collections.Generic;

namespace IServicios.Base
{
    public interface IServicioConsulta
    {
        DtoBase Obtener(long id);

        IEnumerable<DtoBase> Obtener(string cadenaBuscar, bool mostrarTodos = false);
    }
}
