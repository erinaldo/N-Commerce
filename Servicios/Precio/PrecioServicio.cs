using Dominio.UnidadDeTrabajo;
using IServicios.BaseDto;
using IServicios.Precio;
using Servicios.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Transactions;

namespace Servicios.Precio
{
    public class PrecioServicio : IPrecioServicio
    {
        private readonly IUnidadDeTrabajo _unidadDeTrabajo;

        public PrecioServicio(IUnidadDeTrabajo unidadDeTrabajo)
        {
            _unidadDeTrabajo = unidadDeTrabajo;
        }

        public void Eliminar(long id)
        {
            throw new NotImplementedException();
        }

        public void Insertar(DtoBase dtoEntidad)
        {
            throw new NotImplementedException();
        }

        public void Modificar(DtoBase dtoEntidad)
        {
            throw new NotImplementedException();
        }

        public DtoBase Obtener(long id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<DtoBase> Obtener(string cadenaBuscar, bool mostrarTodos = false)
        {
            throw new NotImplementedException();
        }

        public void ActualizarPrecio(decimal valor, bool esPorcentaje, DateTime fechaDeActualizacion,long? marcaId = null, long? rubroId = null, long? listaPrecioId = null, int? codigoInicial = null, int? codigoFinal = null)
        {

            using (var tran=new TransactionScope())
            {
                try
                {
                    Expression<Func<Dominio.Entidades.Articulo, bool>> filtro = x => true;

                    //filtramos los articulos a actualizar
                    if (marcaId.HasValue) filtro = filtro.And(x => x.MarcaId == marcaId.Value);

                    if (rubroId.HasValue) filtro = filtro.And(x => x.RubroId == rubroId.Value);

                    if (codigoInicial.HasValue && codigoFinal.HasValue)
                    {
                        filtro = filtro.And(x => x.Codigo >= codigoFinal.Value && x.Codigo <= codigoFinal);
                    }
                    else
                    {
                        if (codigoInicial.HasValue) filtro = filtro.And(x => x.Codigo >= codigoInicial.Value);

                        if (codigoFinal.HasValue) filtro = filtro.And(x => x.Codigo <= codigoFinal.Value);
                    }

                    //obtenemos los articulos y la lista de precios
                    var articulosFiltrados = _unidadDeTrabajo.ArticuloRepositorio.Obtener(filtro, "Precios");
                    var listasPrecios = _unidadDeTrabajo.ListaPrecioRepositorio.Obtener();

                    var fechaActual = DateTime.Now;

                    foreach (var articulo in articulosFiltrados)
                    {
                        if (listaPrecioId.HasValue) //actualiza la lista seleccionada
                        {
                            var precioReciente = articulo.Precios.FirstOrDefault(x => x.ListaPrecioId == listaPrecioId.Value && x.FechaActualizacion <= fechaDeActualizacion && x.FechaActualizacion == articulo.Precios.
                                                                           Where(p => p.ListaPrecioId == listaPrecioId.Value && x.FechaActualizacion <= fechaDeActualizacion).Max(f => f.FechaActualizacion));

                            var precioCosto = esPorcentaje
                                ? precioReciente.PrecioCosto + (precioReciente.PrecioCosto * (valor / 100m))
                                : precioReciente.PrecioCosto + valor;

                            var listaSeleccionada = listasPrecios.FirstOrDefault(x => x.Id == listaPrecioId.Value);

                            var precioPublico = precioCosto + (precioCosto * (listaSeleccionada.PorcentajeGanancia / 100));

                            var nuevoPrecio = new Dominio.Entidades.Precio
                            {
                                ArticuloId = articulo.Id,
                                ListaPrecioId = listaPrecioId.Value,
                                FechaActualizacion = fechaDeActualizacion,
                                PrecioCosto = precioCosto,
                                PrecioPublico = precioPublico,
                                EstaEliminado = false
                            };

                            _unidadDeTrabajo.PrecioRepositorio.Insertar(nuevoPrecio);
                        }
                        else//actualiza todas las listas
                        {
                            foreach (var lista in listasPrecios)
                            {
                                var precioReciente = articulo.Precios.FirstOrDefault(x =>
                                    x.ListaPrecioId == lista.Id && x.FechaActualizacion <= fechaDeActualizacion &&
                                    x.FechaActualizacion == articulo.Precios
                                        .Where(p => p.ListaPrecioId == lista.Id &&
                                                    x.FechaActualizacion <= fechaDeActualizacion)
                                        .Max(f => f.FechaActualizacion));

                                var precioCosto = esPorcentaje
                                ? precioReciente.PrecioCosto + (precioReciente.PrecioCosto * (valor / 100m))
                                : precioReciente.PrecioCosto + valor;

                                var precioPublico = precioCosto + (precioCosto * (lista.PorcentajeGanancia / 100));

                                var nuevoPrecio = new Dominio.Entidades.Precio
                                {
                                    ArticuloId = articulo.Id,
                                    ListaPrecioId = lista.Id,
                                    FechaActualizacion = fechaDeActualizacion,
                                    PrecioCosto = precioCosto,
                                    PrecioPublico = precioPublico,
                                    EstaEliminado = false
                                };

                                _unidadDeTrabajo.PrecioRepositorio.Insertar(nuevoPrecio);
                            }
                        }
                    }

                    _unidadDeTrabajo.Commit();

                    tran.Complete();
                }
                catch (Exception e)
                {
                    tran.Dispose();
                    throw new Exception(e.Message);
                }
            }
        }
    }
}
