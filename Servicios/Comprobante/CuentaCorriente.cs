using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Transactions;
using Aplicacion.Constantes;
using Dominio.Entidades;
using Dominio.Repositorio;
using IServicios.Comprobante;
using IServicios.Comprobante.DTOs;
using IServicios.Configuracion;
using IServicios.Contador;
using StructureMap;

namespace Servicios.Comprobante
{
    public class CuentaCorriente : Comprobante
    {
        private readonly IContadorServicio _contadorServicio;
        private readonly IConfiguracionServicio _configuracionServicio;
        private readonly ICtaCteComprobanteServicio _ctaCteComprobanteServicio;

        //public CuentaCorriente(ICtaCteComprobanteServicio ctaCteComprobanteServicio)
            public CuentaCorriente()
            : base()
        {
            //_ctaCteComprobanteServicio = ctaCteComprobanteServicio;
            _ctaCteComprobanteServicio = ObjectFactory.GetInstance<ICtaCteComprobanteServicio>();
            _contadorServicio = ObjectFactory.GetInstance<IContadorServicio>();
            _configuracionServicio = ObjectFactory.GetInstance<IConfiguracionServicio>();
        }

        public override long Insertar(ComprobanteDto comprobante)
        {
            using (var tran = new TransactionScope())
            {
                try
                {
                    int numeroComprobante = 0;
                    numeroComprobante = _contadorServicio.ObtenerSiguienteNroComprobante(comprobante.TipoComprobante);

                    var cajaActual =
                        _unidadDeTrabajo.CajaRepositorio.Obtener(x =>
                            x.UsuarioAperturaId == comprobante.UsuarioId && x.UsuarioCierreId == null, "DetalleCajas").FirstOrDefault();

                    if (cajaActual == null)
                        throw new Exception("Ocurrio un error al obtener la Caja");

                    var ctaCteComprobanteDto = (CtaCteComprobanteDto)comprobante;

                    CuentaCorrienteCliente _ctaCteNueva = new CuentaCorrienteCliente();

                    _ctaCteNueva = new CuentaCorrienteCliente
                    {
                        ClienteId = ctaCteComprobanteDto.ClienteId,
                        Descuento = ctaCteComprobanteDto.Descuento,
                        EmpleadoId = ctaCteComprobanteDto.EmpleadoId,
                        Fecha = ctaCteComprobanteDto.Fecha,
                        Iva105 = ctaCteComprobanteDto.Iva105,
                        Iva21 = ctaCteComprobanteDto.Iva21,
                        Numero = numeroComprobante,
                        SubTotal = ctaCteComprobanteDto.SubTotal,
                        Total = ctaCteComprobanteDto.Total,
                        TipoComprobante = ctaCteComprobanteDto.TipoComprobante,
                        UsuarioId = ctaCteComprobanteDto.UsuarioId,
                        DetalleComprobantes = new List<DetalleComprobante>(),
                        Movimientos = new List<Movimiento>(),
                        FormaPagos = new List<FormaPago>(),
                        EstaEliminado = false
                    };

                    _ctaCteNueva.Movimientos.Add(new Movimiento
                    {
                        ComprobanteId = _ctaCteNueva.Id,
                        CajaId = cajaActual.Id,
                        Fecha = ctaCteComprobanteDto.Fecha,
                        Monto = ctaCteComprobanteDto.Total,
                        UsuarioId = ctaCteComprobanteDto.UsuarioId,
                        TipoMovimiento = TipoMovimiento.Ingreso,
                        Descripcion = $"Pago Cta Cte -{ctaCteComprobanteDto.TipoComprobante.ToString()}-Nro{numeroComprobante}",
                        EstaEliminado = false
                    });

                    foreach (var fp in ctaCteComprobanteDto.FormasDePagos)
                    {
                        var fpCtaCTe = (FormaPagoCtaCteDto) fp;

                        _ctaCteNueva.FormaPagos.Add(new FormaPagoCtaCte
                        {
                            ComprobanteId = _ctaCteNueva.Id,
                            Monto = fpCtaCTe.Monto,
                            TipoPago = TipoPago.CtaCte,
                            ClienteId = fpCtaCTe.ClienteId,
                            EstaEliminado = false
                        });

                        _ctaCteNueva.Movimientos.Add(new MovimientoCuentaCorriente
                        {
                            ComprobanteId = _ctaCteNueva.Id,
                            CajaId = cajaActual.Id,
                            Fecha = ctaCteComprobanteDto.Fecha,
                            Monto = fpCtaCTe.Monto,
                            UsuarioId = ctaCteComprobanteDto.UsuarioId,
                            TipoMovimiento = TipoMovimiento.Ingreso,
                            Descripcion =
                                $"Pago Cta Cte - {ctaCteComprobanteDto.TipoComprobante.ToString()}-Nro {numeroComprobante}",
                            ClienteId = fpCtaCTe.ClienteId,
                            EstaEliminado = false
                        });
                        
                        cajaActual.TotalEntradaCtaCte += fpCtaCTe.Monto;

                        cajaActual.DetalleCajas.Add(new DetalleCaja
                        {
                            CajaId = cajaActual.Id,
                            Monto = fpCtaCTe.Monto,
                            TipoPago = TipoPago.CtaCte
                        });
                        
                        _unidadDeTrabajo.CajaRepositorio.Modificar(cajaActual);

                    }

                    _unidadDeTrabajo.CtaComprobanteRepositorio.Insertar(_ctaCteNueva);

                    _unidadDeTrabajo.Commit();

                    tran.Complete();

                    return 0;
                }
                catch (DbEntityValidationException ex)
                {
                    var error = ex.EntityValidationErrors.SelectMany(v => v.ValidationErrors).Aggregate(string.Empty,
                        (current, validationError) =>
                            current +
                            ($"Property: {validationError.PropertyName} Error: {validationError.ErrorMessage}. {Environment.NewLine}"
                            ));

                    tran.Dispose();
                    throw new Exception("Ocurrio un error grave al grabar la Factura. Error: {error}");
                }
            }

        }
    }
}
