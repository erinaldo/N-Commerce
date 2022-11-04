using IServicios.Base;
using IServicios.Localidad.DTOs;
using System.Collections.Generic;

namespace IServicios.Localidad
{
    public interface ILocalidadServicio : IServicio
    {
        bool VerificarSiExiste(string datoVerificar, long idRelacional, long? entidadId = null);

        IEnumerable<LocalidadDto> ObtenerPorDepartamento(long departamentoId, bool mostrarTodos = false);
    }
}
