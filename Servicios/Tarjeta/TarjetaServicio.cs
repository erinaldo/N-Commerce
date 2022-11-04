using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Dominio.UnidadDeTrabajo;
using IServicios.BaseDto;
using IServicios.Provincia.DTOs;
using IServicios.Tarjeta;
using IServicios.Tarjeta.DTOs;
using Servicios.Base;

namespace Servicios.Tarjeta
{
    public class TarjetaServicio:ITarjetaServicio
    {
        private readonly IUnidadDeTrabajo _unidadDeTrabajo;

        public TarjetaServicio(IUnidadDeTrabajo unidadDeTrabajo)
        {
            _unidadDeTrabajo = unidadDeTrabajo;
        }

        public void Eliminar(long id)
        {
            _unidadDeTrabajo.TarjetaRepositorio.Eliminar(id);
            _unidadDeTrabajo.Commit();
        }

        public void Insertar(DtoBase dtoEntidad)
        {
            var dto = (TarjetaDto)dtoEntidad;

            var entidad = new Dominio.Entidades.Tarjeta
            {
                Descripcion = dto.Descripcion,
                EstaEliminado = false
            };

            _unidadDeTrabajo.TarjetaRepositorio.Insertar(entidad);
            _unidadDeTrabajo.Commit();
        }

        public void Modificar(DtoBase dtoEntidad)
        {
            var dto = (TarjetaDto)dtoEntidad;

            var entidad = _unidadDeTrabajo.TarjetaRepositorio.Obtener(dto.Id);

            if (entidad == null) throw new Exception("Ocurrio un Error al Obtener la Provincia");

            entidad.Descripcion = dto.Descripcion;

            _unidadDeTrabajo.TarjetaRepositorio.Modificar(entidad);
            _unidadDeTrabajo.Commit();
        }

        public DtoBase Obtener(long id)
        {
            var entidad = _unidadDeTrabajo.TarjetaRepositorio.Obtener(id);

            return new TarjetaDto
            {
                Id = entidad.Id,
                Descripcion = entidad.Descripcion,
                Eliminado = entidad.EstaEliminado
            };
        }

        public IEnumerable<DtoBase> Obtener(string cadenaBuscar, bool mostrarTodos = false)
        {
            Expression<Func<Dominio.Entidades.Tarjeta, bool>> filtro =
                x => x.Descripcion.Contains(cadenaBuscar);

            if (!mostrarTodos)
            {
                filtro = filtro.And(x => !x.EstaEliminado);
            }

            return _unidadDeTrabajo.TarjetaRepositorio.Obtener(filtro)
                .Select(x => new ProvinciaDto
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
                ? _unidadDeTrabajo.TarjetaRepositorio.Obtener(x => !x.EstaEliminado
                                                                   && x.Id != entidadId.Value
                                                                   && x.Descripcion.Equals(datoVerificar,
                                                                       StringComparison.CurrentCultureIgnoreCase))
                    .Any()
                : _unidadDeTrabajo.TarjetaRepositorio.Obtener(x => !x.EstaEliminado
                                                                   && x.Descripcion.Equals(datoVerificar,
                                                                       StringComparison.CurrentCultureIgnoreCase))
                    .Any();
        }
    }
}
