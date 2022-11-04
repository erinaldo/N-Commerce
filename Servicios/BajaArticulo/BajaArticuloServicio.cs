using Dominio.UnidadDeTrabajo;
using IServicios.BajaArticulo;
using IServicios.BajaArticulo.DTOs;
using IServicios.BaseDto;
using Servicios.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Transactions;

namespace Servicios.BajaArticulo
{
    public class BajaArticuloServicio :IBajaArticuloServicio
    {
        private readonly IUnidadDeTrabajo _unidadDeTrabajo;
        public BajaArticuloServicio(IUnidadDeTrabajo unidadDeTrabajo)
        {
            _unidadDeTrabajo = unidadDeTrabajo;
        }

        public void Eliminar(long id)
        {
            _unidadDeTrabajo.BajaArticuloRepositorio.Eliminar(id);
            _unidadDeTrabajo.Commit();
        }

        public void Insertar(DtoBase dtoEntidad)
        {
            //using (var tras = new TransactionScope())
            //{
            //    try
            //    {
            //        var dto = (ArticuloCrudDto)dtoEntidad;

            //        var entidad = new Dominio.Entidades.Articulo
            //        {
            //            MarcaId = dto.MarcaId,
            //            RubroId = dto.RubroId,
            //            UnidadMedidaId = dto.UnidadMedidaId,
            //            IvaId = dto.IvaId,
            //            Codigo = dto.Codigo,
            //            CodigoBarra = dto.CodigoBarra,
            //            Abreviatura = dto.Abreviatura,
            //            Descripcion = dto.Descripcion,
            //            Detalle = dto.Detalle,
            //            Ubicacion = dto.Ubicacion,
            //            ActivarLimiteVenta = dto.ActivarLimiteVenta,
            //            LimiteVenta = dto.LimiteVenta,
            //            ActivarHoraVenta = dto.ActivarHoraVenta,
            //            HoraLimiteVentaDesde = dto.HoraLimiteVentaDesde,
            //            HoraLimiteVentaHasta = dto.HoraLimiteVentaHasta,
            //            PermiteStockNegativo = dto.PermiteStockNegativo,
            //            DescuentaStock = dto.DescuentaStock,
            //            StockMinimo = dto.StockMinimo,
            //            Foto = dto.Foto,

            //            EstaEliminado = false
            //        };

            //        _unidadDeTrabajo.ArticuloRepositorio.Insertar(entidad);
            //        _unidadDeTrabajo.Commit();

            //        var listaPreciosLista = _unidadDeTrabajo.ListaPrecioRepositorio.Obtener(x => !x.EstaEliminado);

            //        try
            //        {
            //            foreach (var listaPreciosRegistro in listaPreciosLista)
            //            {
            //                var precioEntidad = new Dominio.Entidades.Precio
            //                {
            //                    ArticuloId = entidad.Id,
            //                    ListaPrecioId = listaPreciosRegistro.Id,
            //                    PrecioCosto = dto.PrecioCosto,
            //                    PrecioPublico = dto.PrecioCosto + ((dto.PrecioCosto * listaPreciosRegistro.PorcentajeGanancia) / 100),
            //                    FechaActualizacion = DateTime.Now
            //                };
            //                _unidadDeTrabajo.PrecioRepositorio.Insertar(precioEntidad);
            //            }
            //        }
            //        catch (Exception)
            //        {

            //            throw new Exception("Error en insertarPrecio");
            //        }



            //        var config = _unidadDeTrabajo.ConfiguracionRepositorio.Obtener().FirstOrDefault();
            //        if (config == null)
            //            throw new Exception("Error");

            //        _unidadDeTrabajo.StockRepositorio.Insertar(new Dominio.Entidades.Stock
            //        {
            //            ArticuloId = entidad.Id,
            //            DepositoId = config.DepositoPorDefectoId,
            //            Cantidad = dto.StockActual,
            //            EstaEliminado = false

            //        });

            //        _unidadDeTrabajo.Commit();

            //        tras.Complete();
            //    }
            //    catch (Exception ex)
            //    {
            //        tras.Dispose();
            //        throw new Exception(ex.Message);
            //    }
            //}
        }

        public void Modificar(DtoBase dtoEntidad)
        {
            //var dto = (BajaArticuloDto)dtoEntidad;

            //var entidad = _unidadDeTrabajo.ArticuloRepositorio.Obtener(dto.Id);

            //if (entidad == null) throw new Exception("Ocurrio un Error al Obtener el Artículo");

            //entidad.MarcaId = dto.MarcaId;
            //entidad.RubroId = dto.RubroId;
            //entidad.UnidadMedidaId = dto.UnidadMedidaId;
            //entidad.IvaId = dto.IvaId;
            //entidad.Codigo = dto.Codigo;
            //entidad.CodigoBarra = dto.CodigoBarra;
            //entidad.Abreviatura = dto.Abreviatura;
            //entidad.Descripcion = dto.Descripcion;
            //entidad.Detalle = dto.Detalle;
            //entidad.Ubicacion = dto.Ubicacion;
            //entidad.ActivarLimiteVenta = dto.ActivarLimiteVenta;
            //entidad.LimiteVenta = dto.LimiteVenta;
            //entidad.ActivarHoraVenta = dto.ActivarHoraVenta;
            //entidad.HoraLimiteVentaDesde = dto.HoraLimiteVentaDesde;
            //entidad.HoraLimiteVentaHasta = dto.HoraLimiteVentaHasta;
            //entidad.PermiteStockNegativo = dto.PermiteStockNegativo;
            //entidad.DescuentaStock = dto.DescuentaStock;
            //entidad.StockMinimo = dto.StockMinimo;
            //entidad.Foto = dto.Foto;

            //_unidadDeTrabajo.ArticuloRepositorio.Modificar(entidad);
            //_unidadDeTrabajo.Commit();
        }

        public DtoBase Obtener(long id)
            {
                var entidad = _unidadDeTrabajo.BajaArticuloRepositorio.Obtener(id, "Articulo, Stocks, Stocks.Deposito");

            return new BajaArticuloDto
            {
                ArticuloId = entidad.ArticuloId,
                ArticuloDescripcion = entidad.Articulo.Descripcion,
                MotivoBajaId = entidad.MotivoBajaId,
                Cantidad = entidad.Cantidad,
                Fecha = entidad.Fecha,
                Observacion = entidad.Observacion,
                Foto = entidad.Foto
            };
            }

        public IEnumerable<DtoBase> Obtener(string cadenaBuscar, bool mostrarTodos = false)
        {
            Expression<Func<Dominio.Entidades.BajaArticulo, bool>> filtro =
                x => x.EstaEliminado;

            if (!mostrarTodos)
            {
                filtro = filtro.And(x => !x.EstaEliminado);
            }

            return _unidadDeTrabajo.BajaArticuloRepositorio.Obtener(filtro, "Articulo")
                .Select(x => new BajaArticuloDto
                {
                    Id = x.Id,
                    ArticuloId = x.ArticuloId,
                    ArticuloDescripcion = x.Articulo.Descripcion,
                    Cantidad = x.Cantidad,
                    Fecha = x.Fecha,
                    Observacion = x.Observacion,
                    Foto = x.Foto,
                    Eliminado = x.EstaEliminado
                }).ToList().OrderBy(x => x.Fecha).ToList();
        }
    }
}
