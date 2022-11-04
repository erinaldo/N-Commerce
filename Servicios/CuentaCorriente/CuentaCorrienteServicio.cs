using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Expressions;
using Aplicacion.Constantes;
using Dominio.Entidades;
using Dominio.UnidadDeTrabajo;
using IServicios.Comprobante;
using IServicios.Comprobante.DTOs;
using IServicios.Contador;
using IServicios.CuentaCorriente;
using IServicios.CuentaCorriente.DTOs;
using Servicios.Base;

namespace Servicios.CuentaCorriente
{
    public class CuentaCorrienteServicio : ICuentaCorrienteServicio
    {
        private readonly IUnidadDeTrabajo _unidadDeTrabajo;
        private readonly IContadorServicio _contadorServicio;
        private readonly ICtaCteComprobanteServicio _ctaCteComprobanteServicio;

        public CuentaCorrienteServicio(IUnidadDeTrabajo unidadDeTrabajo, IContadorServicio contadorServicio,
            ICtaCteComprobanteServicio ctaCteComprobanteServicio)
        :base()
        {
            _unidadDeTrabajo = unidadDeTrabajo;
            _contadorServicio = contadorServicio;
            _ctaCteComprobanteServicio = ctaCteComprobanteServicio;
        }

        public IEnumerable<CuentaCorrienteDto> Obtener(long entidadId, DateTime fechaDesde, DateTime fechaHasta,
            bool deuda)
        {
            var _fechaDesde = new DateTime(fechaDesde.Year, fechaDesde.Month, fechaDesde.Day, 0, 0, 0);
            var _fechaHasta = new DateTime(fechaHasta.Year, fechaHasta.Month, fechaHasta.Day, 23, 59, 59);

            Expression<Func<MovimientoCuentaCorriente, bool>> filtro = x => x.ClienteId == entidadId;

            filtro = filtro.And(x => x.Fecha >= _fechaDesde && x.Fecha <= _fechaHasta);

            if (deuda)
            {
                filtro = filtro.And(x => x.TipoMovimiento == TipoMovimiento.Egreso);
            }

            return _unidadDeTrabajo.CuentaCorrienteRepositorio.Obtener(filtro).Select(x => new CuentaCorrienteDto
            {
                Fecha = x.Fecha,
                Descripcion = x.Descripcion,
                Monto = (x.Monto * (int) x.TipoMovimiento) //multiplica por 1 o -1 para saber si es positivo o negativo
            }).OrderBy(x => x.Fecha).ToList();
        }

        public decimal ObtenerDeudaCliente(long clienteId)
        {
            var movimientos =
                _unidadDeTrabajo.CuentaCorrienteRepositorio.Obtener(x => !x.EstaEliminado && x.ClienteId == clienteId);

            return movimientos.Sum(x => x.Monto * (int) x.TipoMovimiento);
        }

        public void Pagar(CtaCteComprobanteDto comprobanteDto)
        {
            try
            {
                

                _ctaCteComprobanteServicio.Insertar(comprobanteDto);
            }
            catch (DbEntityValidationException exce)
            {
                var error = exce.EntityValidationErrors.SelectMany(v => v.ValidationErrors).Aggregate(string.Empty,
                    (current, validationError) =>
                        current +
                        ($"Property: {validationError.PropertyName} Error: {validationError.ErrorMessage}. {Environment.NewLine}"
                        ));

                throw new Exception($"Ocurrio un error grave al grabar. Error: {error}");
            }
        }
    }
}
