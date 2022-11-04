

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Aplicacion.Constantes;
using Dominio.Entidades;
using Dominio.UnidadDeTrabajo;
using IServicios.Caja;
using IServicios.Caja.DTOs;
using Servicios.Base;

namespace Servicios.Caja
{
    public class CajaServicio : ICajaServicio
    {
        private readonly IUnidadDeTrabajo _unidadDeTrabajo;
        public CajaServicio(IUnidadDeTrabajo unidadDeTrabajo)
        {
            _unidadDeTrabajo = unidadDeTrabajo;
        }
        public void AbrirCaja(long usuarioId, decimal monto)
        {
            try
            {
                var nuevaCaja = new Dominio.Entidades.Caja
                {
                    UsuarioAperturaId = usuarioId,
                    FechaApertura = DateTime.Now,
                    MontoInicial = monto,
                    UsuarioCierreId = (long?)null,
                    FechaCierre = (DateTime?)null,
                    MontoCierre = (decimal?)null,
                    TotalEntradaCheque = 0m,
                    TotalEntradaCtaCte = 0m,
                    TotalEntradaTarjeta = 0m,
                    TotalEntradaEfectivo = 0m,
                    TotalSalidaCheque = 0m,
                    TotalSalidaCtaCte = 0m,
                    TotalSalidaTarjeta = 0m,
                    TotalSalidaEfectivo = 0m,
                    EstaEliminado = false
                };
                _unidadDeTrabajo.CajaRepositorio.Insertar(nuevaCaja);
                _unidadDeTrabajo.Commit();
            }
            catch
            {
                throw new Exception("Ocurrio un error al abrir la caja.");
            }
        }

        public void CerrarCaja(CajaDto caja)
        {
            var entidad = _unidadDeTrabajo.CajaRepositorio.Obtener(x => x.Id == caja.Id, "DetalleCajas, Movimientos").Select(x =>
                    new Dominio.Entidades.Caja
                    {
                        Id = x.Id,
                        UsuarioAperturaId = x.UsuarioAperturaId,
                        FechaApertura = x.FechaApertura,
                        MontoInicial = x.MontoInicial,
                        UsuarioCierreId = caja.UsuarioCierreId,
                        FechaCierre = DateTime.Now,
                        MontoCierre = x.MontoCierre,
                        TotalEntradaCheque = x.TotalEntradaCheque,
                        TotalEntradaCtaCte = x.TotalEntradaCtaCte,
                        TotalEntradaEfectivo = x.TotalEntradaEfectivo,
                        TotalEntradaTarjeta = x.TotalEntradaTarjeta,
                        TotalSalidaCheque = caja.TotalSalidaCheque,
                        TotalSalidaCtaCte = caja.TotalSalidaCtaCte,
                        TotalSalidaEfectivo = caja.TotalSalidaEfectivo,
                        TotalSalidaTarjeta = caja.TotalSalidaTarjeta,
                        EstaEliminado = x.EstaEliminado,
                        DetalleCajas = x.DetalleCajas.Select(d => new DetalleCaja
                        {
                            Id = d.Id,
                            CajaId = d.CajaId,
                            TipoPago = d.TipoPago,
                            Monto = d.Monto,
                            EstaEliminado = d.EstaEliminado
                        }).ToList(),
                        Movimientos = x.Movimientos.Select(c => new Movimiento
                        {
                            Id = c.Id,
                            CajaId = c.CajaId,
                            ComprobanteId = c.ComprobanteId,
                            UsuarioId = c.UsuarioId,
                            Monto = c.Monto,
                            Descripcion = c.Descripcion,
                            TipoMovimiento = c.TipoMovimiento,
                            EstaEliminado = c.EstaEliminado
                        }).ToList(),
                    }).FirstOrDefault();

            _unidadDeTrabajo.CajaRepositorio.Modificar(entidad);
            _unidadDeTrabajo.Commit();
        }

        public IEnumerable<CajaDto> Obtener(string cadenaBuscar, bool filtroPorFecha, DateTime fechaDesde, DateTime fechaHasta)
        {
            Expression<Func<Dominio.Entidades.Caja, bool>> filtro = x =>
                (!x.EstaEliminado && x.UsuarioApertura.Nombre.Contains(cadenaBuscar)) ||
                (!x.EstaEliminado && x.UsuarioCierre.Nombre.Contains(cadenaBuscar));
            
            if (filtroPorFecha)
            {
                var _fechaDesde = new DateTime(fechaDesde.Year, fechaDesde.Month, fechaDesde.Day, 0, 0, 0);
                var _fechaHasta = new DateTime(fechaHasta.Year, fechaHasta.Month, fechaHasta.Day, 23, 59, 59);

                filtro = filtro.And(x => x.FechaApertura >= _fechaDesde && x.FechaApertura <= _fechaHasta);
            }
            return _unidadDeTrabajo.CajaRepositorio.Obtener(filtro, "UsuarioApertura, UsuarioCierre").Select(x => new CajaDto
            {
                Id = x.Id,
                UsuarioAperturaId = x.UsuarioAperturaId,
                UsuarioApertura = x.UsuarioApertura.Nombre,
                FechaApertura = x.FechaApertura,
                MontoApertura = x.MontoInicial,
                UsuarioCierreId = x.UsuarioCierreId,
                UsuarioCierre = x.UsuarioCierreId.HasValue ? x.UsuarioCierre.Nombre : "----",
                FechaCierre = x.FechaCierre,
                MontoCierre = x.MontoCierre,
                TotalEntradaCheque = x.TotalEntradaCheque,
                TotalEntradaCtaCte = x.TotalEntradaCtaCte,
                TotalEntradaEfectivo = x.TotalEntradaEfectivo,
                TotalEntradaTarjeta = x.TotalEntradaTarjeta,
                TotalSalidaCheque = x.TotalSalidaCheque,
                TotalSalidaCtaCte = x.TotalSalidaCtaCte,
                TotalSalidaEfectivo = x.TotalSalidaEfectivo,
                TotalSalidaTarjeta = x.TotalSalidaTarjeta,
                Eliminado = x.EstaEliminado
            }).ToList();
        }

        public CajaDto Obtener(long cajaId)
        {
            return _unidadDeTrabajo.CajaRepositorio.Obtener(x => x.Id == cajaId, "UsuarioApertura, UsuarioCierre, DetalleCajas, Movimientos, Movimientos.Comprobante").Select(x =>
                    new CajaDto
                    {
                        Id = x.Id,
                        UsuarioAperturaId = x.UsuarioAperturaId,
                        UsuarioApertura = x.UsuarioApertura.Nombre,
                        FechaApertura = x.FechaApertura,
                        MontoApertura = x.MontoInicial,
                        UsuarioCierreId = x.UsuarioCierreId,
                        UsuarioCierre = x.UsuarioCierreId.HasValue ? x.UsuarioCierre.Nombre : "----",
                        FechaCierre = x.FechaCierre,
                        MontoCierre = x.MontoCierre,
                        TotalEntradaCheque = x.TotalEntradaCheque,
                        TotalEntradaCtaCte = x.TotalEntradaCtaCte,
                        TotalEntradaEfectivo = x.TotalEntradaEfectivo,
                        TotalEntradaTarjeta = x.TotalEntradaTarjeta,
                        TotalSalidaCheque = x.TotalSalidaCheque,
                        TotalSalidaCtaCte = x.TotalSalidaCtaCte,
                        TotalSalidaEfectivo = x.TotalSalidaEfectivo,
                        TotalSalidaTarjeta = x.TotalSalidaTarjeta,
                        Eliminado = x.EstaEliminado,
                        Detalles = x.DetalleCajas.Select(d => new CajaDetalleDto
                        {
                            Monto = d.Monto,
                            TipoPago = d.TipoPago,
                            Eliminado = d.EstaEliminado
                        }).ToList(),
                        Comprobantes = x.Movimientos.Select(c => new ComprobanteCajaDto
                        {
                            Fecha = c.Comprobante.Fecha,
                            Numero = c.Comprobante.Numero,
                            Total = c.Comprobante.Total,
                            Vendedor = $"{c.Comprobante.EmpleadoId}",
                            Eliminado = c.Comprobante.EstaEliminado
                        }).ToList(),
                    }).FirstOrDefault();
        }

        public CajaDto Obtener(long cajaId, TipoMovimiento tipoMovimiento)
        {
            return _unidadDeTrabajo.CajaRepositorio.Obtener(x => x.Id == cajaId, "UsuarioApertura, UsuarioCierre, DetalleCajas, Movimientos, Movimientos.Comprobante").Select(x =>
                    new CajaDto
                    {
                        Id = x.Id,
                        UsuarioAperturaId = x.UsuarioAperturaId,
                        UsuarioApertura = x.UsuarioApertura.Nombre,
                        FechaApertura = x.FechaApertura,
                        MontoApertura = x.MontoInicial,
                        UsuarioCierreId = x.UsuarioCierreId,
                        UsuarioCierre = x.UsuarioCierreId.HasValue ? x.UsuarioCierre.Nombre : "----",
                        FechaCierre = x.FechaCierre,
                        MontoCierre = x.MontoCierre,
                        TotalEntradaCheque = x.TotalEntradaCheque,
                        TotalEntradaCtaCte = x.TotalEntradaCtaCte,
                        TotalEntradaEfectivo = x.TotalEntradaEfectivo,
                        TotalEntradaTarjeta = x.TotalEntradaTarjeta,
                        TotalSalidaCheque = x.TotalSalidaCheque,
                        TotalSalidaCtaCte = x.TotalSalidaCtaCte,
                        TotalSalidaEfectivo = x.TotalSalidaEfectivo,
                        TotalSalidaTarjeta = x.TotalSalidaTarjeta,
                        Eliminado = x.EstaEliminado,
                        Detalles = x.DetalleCajas.Select(d => new CajaDetalleDto
                        {
                            Monto = d.Monto,
                            TipoPago = d.TipoPago,
                            Eliminado = d.EstaEliminado
                        }).ToList(),

                        Comprobantes = x.Movimientos.Where(m => m.TipoMovimiento.Equals(tipoMovimiento)).Select(c => new ComprobanteCajaDto
                        {
                            Fecha = c.Comprobante.Fecha,
                            Numero = c.Comprobante.Numero,
                            Total = c.Comprobante.Total,
                            Vendedor = $"{c.Comprobante.EmpleadoId}",
                            Eliminado = c.Comprobante.EstaEliminado
                        }).ToList(),
                    }).FirstOrDefault();
        }

        public decimal ObtenerMontoCajaAnterior(long usuarioId)
        {
            var cajasUsuario = _unidadDeTrabajo.CajaRepositorio.Obtener(x => x.UsuarioAperturaId == usuarioId && x.UsuarioCierre != null);
            var ultimaCaja = cajasUsuario.Where(x => x.FechaApertura == cajasUsuario.Max(f => f.FechaApertura)).LastOrDefault();
            return ultimaCaja == null ? 0m : ultimaCaja.MontoCierre.Value;
        }
        public bool VerificarSiExisteCajaAbierta(long usuarioId)
        {
            return _unidadDeTrabajo.CajaRepositorio.Obtener(x => x.UsuarioAperturaId == usuarioId && x.UsuarioCierreId == null).Any();
        }
    }
}
