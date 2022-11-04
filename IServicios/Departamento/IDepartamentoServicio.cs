using IServicios.Base;
using IServicios.Departamento.DTOs;
using System.Collections.Generic;

namespace IServicios.Departamento
{
    public interface IDepartamentoServicio : IServicio
    {
        bool VerificarSiExiste(string datoVerificar, long idRelacional, long? entidadId = null);

        IEnumerable<DepartamentoDto> ObtenerPorProvincia(long provinciaId, bool mostrarTodos = false);
    }
}
