using Dominio.UnidadDeTrabajo;
using IServicios.BaseDto;
using IServicios.Gasto;
using IServicios.Gasto.DTOs;
using Servicios.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Servicios.Gasto
{
    public class GastoServicio : IGastoServicio
    {
        private readonly IUnidadDeTrabajo _unidadDeTrabajo;

        public GastoServicio(IUnidadDeTrabajo unidadDeTrabajo)
        {
            _unidadDeTrabajo = unidadDeTrabajo;
        }

        public void Eliminar(long id)
        {
            _unidadDeTrabajo.GastoRepositorio.Eliminar(id);
            _unidadDeTrabajo.Commit();
        }

        public void Insertar(DtoBase dtoEntidad)
        {
            var dto = (GastoDTO)dtoEntidad;

            var entidad = new Dominio.Entidades.Gasto
            {
                Fecha = dto.Fecha,
                ConceptoGastoId = dto.ConceptoGastoId,
                Descripcion = dto.Descripcion,
                Monto = dto.Monto,
                EstaEliminado = false
            };

            _unidadDeTrabajo.GastoRepositorio.Insertar(entidad);
            _unidadDeTrabajo.Commit();
        }

        public void Modificar(DtoBase dtoEntidad)
        {
            var dto = (GastoDTO)dtoEntidad;

            var entidad = _unidadDeTrabajo.GastoRepositorio.Obtener(dto.Id);

            if (entidad == null) throw new Exception("Ocurrio un Error al Obtener el Gasto");

            entidad.Descripcion = dto.Descripcion;
            entidad.ConceptoGastoId = dto.ConceptoGastoId;
            entidad.Fecha = dto.Fecha;
            entidad.Monto = dto.Monto;

            _unidadDeTrabajo.GastoRepositorio.Modificar(entidad);
            _unidadDeTrabajo.Commit();
        }

        public DtoBase Obtener(long id)
        {
            var entidad = _unidadDeTrabajo.GastoRepositorio.Obtener(id);

            return new GastoDTO
            {
                Id = entidad.Id,
                Fecha = entidad.Fecha,
                ConceptoGastoId = entidad.ConceptoGastoId,
                Descripcion = entidad.Descripcion,
                Monto = entidad.Monto,
                Eliminado = entidad.EstaEliminado
            };
        }

        public IEnumerable<DtoBase> Obtener(string cadenaBuscar, bool mostrarTodos = false)
        {
            Expression<Func<Dominio.Entidades.Gasto, bool>> filtro =
                x => x.Descripcion.Contains(cadenaBuscar);

            if (!mostrarTodos)
            {
                filtro = filtro.And(x => !x.EstaEliminado);
            }

            return _unidadDeTrabajo.GastoRepositorio.Obtener(filtro)
                .Select(x => new GastoDTO
                {
                    Id = x.Id,
                    Fecha = x.Fecha,
                    ConceptoGastoId = x.ConceptoGastoId,
                    Descripcion = x.Descripcion,
                    Monto = x.Monto,
                    Eliminado = x.EstaEliminado
                })
                .OrderBy(x => x.Descripcion)
                .ToList();
        }
    }
}
