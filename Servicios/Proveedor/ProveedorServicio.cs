using Dominio.UnidadDeTrabajo;
using IServicios.BaseDto;
using IServicios.Proveedor;
using IServicios.Proveedor.DTOs;
using Servicios.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Servicios.Proveedor
{
    public class ProveedorServicio : IProveedorServicio
    {
        private readonly IUnidadDeTrabajo _unidadDeTrabajo;

        public ProveedorServicio(IUnidadDeTrabajo unidadDeTrabajo)
        {
            _unidadDeTrabajo = unidadDeTrabajo;
        }

        public void Eliminar(long id)
        {
            _unidadDeTrabajo.ProveedorRepositorio.Eliminar(id);
            _unidadDeTrabajo.Commit();
        }

        public void Insertar(DtoBase dtoEntidad)
        {
            var dto = (ProveedorDTO)dtoEntidad;

            var entidad = new Dominio.Entidades.Proveedor
            {
                RazonSocial = dto.RazonSocial,
                CUIT = dto.CUIT,
                Direccion = dto.Direccion,
                Telefono = dto.Telefono,
                Mail = dto.Mail,
                LocalidadId = dto.LocalidadId,
                CondicionIvaId = dto.CondicionIvaId,
                EstaEliminado = false
            };

            _unidadDeTrabajo.ProveedorRepositorio.Insertar(entidad);
            _unidadDeTrabajo.Commit();
        }

        public void Modificar(DtoBase dtoEntidad)
        {
            var dto = (ProveedorDTO)dtoEntidad;

            var entidad = _unidadDeTrabajo.ProveedorRepositorio.Obtener(dto.Id);

            if (entidad == null) throw new Exception("Ocurrio un Error al Obtener la Provincia");

            entidad.RazonSocial = dto.RazonSocial;
            entidad.CUIT = dto.CUIT;
            entidad.Direccion = dto.Direccion;
            entidad.Telefono = dto.Telefono;
            entidad.Mail = dto.Mail;
            entidad.LocalidadId = dto.LocalidadId;
            entidad.CondicionIvaId = dto.CondicionIvaId;

            _unidadDeTrabajo.ProveedorRepositorio.Modificar(entidad);
            _unidadDeTrabajo.Commit();
        }

        public DtoBase Obtener(long id)
        {
            var entidad = _unidadDeTrabajo.ProveedorRepositorio.Obtener(id);

            return new ProveedorDTO
            {
                Id = entidad.Id,
                RazonSocial = entidad.RazonSocial,
                CUIT = entidad.CUIT,
                Direccion = entidad.Direccion,
                Telefono = entidad.Telefono,
                Mail = entidad.Mail,
                Localidad = null,
                LocalidadId = entidad.LocalidadId,
                CondicionIva = null,
                CondicionIvaId = entidad.CondicionIvaId,
                Eliminado = entidad.EstaEliminado
            };
        }

        public IEnumerable<DtoBase> Obtener(string cadenaBuscar, bool mostrarTodos = false)
        {
            Expression<Func<Dominio.Entidades.Proveedor, bool>> filtro =
                x => x.RazonSocial.Contains(cadenaBuscar);

            if (!mostrarTodos)
            {
                filtro = filtro.And(x => !x.EstaEliminado);
            }

            return _unidadDeTrabajo.ProveedorRepositorio.Obtener(filtro, "Localidad, CondicionIva")
                .Select(x => new ProveedorDTO
                {
                    Id = x.Id,
                    RazonSocial = x.RazonSocial,
                    CUIT = x.CUIT,
                    Direccion = x.Direccion,
                    Telefono = x.Telefono,
                    Mail = x.Mail,
                    Localidad = x.Localidad.Descripcion,
                    LocalidadId = x.LocalidadId,
                    CondicionIva = x.CondicionIva.Descripcion,
                    CondicionIvaId = x.CondicionIvaId,
                    Eliminado = x.EstaEliminado
                })
                .OrderBy(x => x.RazonSocial)
                .ToList();
        }
    }
}
