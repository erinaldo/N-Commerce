using Dominio.UnidadDeTrabajo;
using IServicios.BaseDto;
using IServicios.ConceptoGasto;
using IServicios.ConceptoGasto.DTOs;
using Servicios.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Servicios.ConceptoGasto
{
    public class ConceptoGastoServicio : IConceptoGastoServicio
    {
        private readonly IUnidadDeTrabajo _unidadDeTrabajo;

        public ConceptoGastoServicio(IUnidadDeTrabajo unidadDeTrabajo)
        {
            _unidadDeTrabajo = unidadDeTrabajo;
        }

        public void Eliminar(long id)
        {
            _unidadDeTrabajo.ConceptoGastoRepositorio.Eliminar(id);
            _unidadDeTrabajo.Commit();
        }

        public void Insertar(DtoBase dtoEntidad)
        {
            var dto = (ConceptoGastoDTO)dtoEntidad;

            var entidad = new Dominio.Entidades.ConceptoGasto
            {
                Descripcion = dto.Descripcion,
                EstaEliminado = false
            };

            _unidadDeTrabajo.ConceptoGastoRepositorio.Insertar(entidad);
            _unidadDeTrabajo.Commit();
        }

        public void Modificar(DtoBase dtoEntidad)
        {
            var dto = (ConceptoGastoDTO)dtoEntidad;

            var entidad = _unidadDeTrabajo.ConceptoGastoRepositorio.Obtener(dto.Id);

            if (entidad == null) throw new Exception("Ocurrió un Error al Obtener el Concepto Gasto");

            entidad.Descripcion = dto.Descripcion;

            _unidadDeTrabajo.ConceptoGastoRepositorio.Modificar(entidad);
            _unidadDeTrabajo.Commit();
        }

        public DtoBase Obtener(long id)
        {
            var entidad = _unidadDeTrabajo.ConceptoGastoRepositorio.Obtener(id);

            return new ConceptoGastoDTO
            {
                Id = entidad.Id,
                Descripcion = entidad.Descripcion,
                Eliminado = entidad.EstaEliminado
            };
        }

        public IEnumerable<DtoBase> Obtener(string cadenaBuscar, bool mostrarTodos = true)
        {
            Expression<Func<Dominio.Entidades.Rubro, bool>> filtro =
                x => x.Descripcion.Contains(cadenaBuscar);

            if (!mostrarTodos)
            {
                filtro = filtro.And(x => !x.EstaEliminado);
            }

            return _unidadDeTrabajo.ConceptoGastoRepositorio.Obtener(x => x.Descripcion.Contains(cadenaBuscar))
                .Select(x => new ConceptoGastoDTO
                {
                    Id = x.Id,
                    Descripcion = x.Descripcion,
                    Eliminado = x.EstaEliminado
                })
                .OrderBy(x => x.Descripcion)
                .ToList();
        }

        public bool VerificarSiExiste(string datoVerificar, long? entidadId = null)
        {
            return entidadId.HasValue
                ? _unidadDeTrabajo.ConceptoGastoRepositorio.Obtener(x => !x.EstaEliminado
                                                                     && x.Id != entidadId.Value
                                                                     && x.Descripcion.Equals(datoVerificar,
                                                                         StringComparison.CurrentCultureIgnoreCase))
                    .Any()
                : _unidadDeTrabajo.ConceptoGastoRepositorio.Obtener(x => !x.EstaEliminado
                                                                 && x.Descripcion.Equals(datoVerificar,
                                                                     StringComparison.CurrentCultureIgnoreCase))
                    .Any();
        }
    }
}
