using IServicios.Base;
using IServicios.BaseDto;
using System.Collections.Generic;

namespace IServicios.Rubro
{
    public interface IRubroServicio : IServicio
    {

        void Eliminar(long id);

        void Insertar(DtoBase dtoEntidad);

        void Modificar(DtoBase dtoEntidad);

        DtoBase Obtener(long id);

        IEnumerable<DtoBase> Obtener(string cadenaBuscar, bool mostrarTodos = true);

        bool VerificarSiExiste(string datoVerificar, long? entidadId = null);
    }
}
