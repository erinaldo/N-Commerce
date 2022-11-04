using Dominio.Repositorio;
using Dominio.UnidadDeTrabajo;
using Infraestructura.Repositorio;
using Infraestructura.UnidadDeTrabajo;
using IServicios.Articulo;
using IServicios.BajaArticulo;
using IServicios.Configuracion;
using IServicios.Contador;
using IServicios.Departamento;
using IServicios.Deposito;
using IServicios.Iva;
using IServicios.ListaPrecio;
using IServicios.Localidad;
using IServicios.Marca;
using IServicios.MotivoBaja;
using IServicios.Persona;
using IServicios.Precio;
using IServicios.Provincia;
using IServicios.PuestoTrabajo;
using IServicios.Rubro;
using IServicios.Seguridad;
using IServicios.UnidadMedida;
using IServicios.Usuario;
using Servicios.Articulo;
using Servicios.BajaArticulo;
using Servicios.CondicionIva;
using Servicios.Configuracion;
using Servicios.Contador;
using Servicios.Departamento;
using Servicios.Deposito;
using Servicios.Iva;
using Servicios.ListaPrecio;
using Servicios.Localidad;
using Servicios.Marca;
using Servicios.MotivoBaja;
using Servicios.Persona;
using Servicios.Precio;
using Servicios.Provincia;
using Servicios.PuestoTrabajo;
using Servicios.Rubro;
using Servicios.Seguridad;
using Servicios.UnidadMedida;
using Servicios.Usuario;
using StructureMap;
using System.Data.Entity;
using IServicios.Banco;
using IServicios.Caja;
using IServicios.Comprobante;
using IServicios.CuentaCorriente;
using IServicios.Tarjeta;
using Servicios.Banco;
using Servicios.Caja;
using Servicios.Comprobante;
using Servicios.CuentaCorriente;
using Servicios.Tarjeta;
using IServicios.ConceptoGasto;
using Servicios.ConceptoGasto;
using IServicios.Gasto;
using Servicios.Gasto;
using IServicios.Proveedor;
using Servicios.Proveedor;

namespace Aplicacion.IoC
{
    public class StructureMapContainer
    {
        public void Configure()
        {
            ObjectFactory.Configure(x =>
            {
                x.For(typeof(IRepositorio<>)).Use(typeof(Repositorio<>));

                x.ForSingletonOf<DbContext>();

                x.For<IUnidadDeTrabajo>().Use<UnidadDeTrabajo>();

                // =================================================================== //

                x.For<IDepositoServicio>().Use<DepositoServicio>();

                x.For<IArticuloServicio>().Use<ArticuloServicio>();

                x.For<IConceptoGastoServicio>().Use<ConceptoGastoServicio>();

                x.For<IMarcaServicio>().Use<MarcaServicio>();

                x.For<IRubroServicio>().Use<RubroServicio>();

                x.For<IUnidadMedidaServicio>().Use<UnidadMedidaServicio>();

                x.For<IIvaServicio>().Use<IvaServicio>();

                x.For<IProvinciaServicio>().Use<ProvinciaServicio>();

                x.For<IProveedorServicio>().Use<ProveedorServicio>();

                x.For<IDepartamentoServicio>().Use<DepartamentoServicio>();

                x.For<ILocalidadServicio>().Use<LocalidadServicio>();

                x.For<ICondicionIvaServicio>().Use<CondicionIvaServicio>();

                x.For<IPersonaServicio>().Use<PersonaServicio>();

                x.For<IClienteServicio>().Use<ClienteServicio>();

                x.For<IGastoServicio>().Use<GastoServicio>();

                x.For<IEmpleadoServicio>().Use<EmpleadoServicio>();

                x.For<IUsuarioServicio>().Use<UsuarioServicio>();

                x.For<ISeguridadServicio>().Use<SeguridadServicio>();

                x.For<IConfiguracionServicio>().Use<ConfiguracionServicio>();

                x.For<IListaPrecioServicio>().Use<ListaPrecioServicio>();

                x.For<IContadorServicio>().Use<ContadorServicio>();

                x.For<IPrecioServicio>().Use<PrecioServicio>();

                x.For<IPuestoTrabajoServicio>().Use<PuestoTrabajoServicio>();

                x.For<IBajaArticuloServicio>().Use<BajaArticuloServicio>();

                x.For<IMotivoBajaServicio>().Use<MotivoBajaServicio>();

                x.For<ICajaServicio>().Use<CajaServicio>();

                x.For<IBancoServicio>().Use<BancoServicio>();

                x.For<ITarjetaServicio>().Use<TarjetaServicio>();

                x.For<IFacturaServicio>().Use<FacturaServicio>();

                x.For<ICuentaCorrienteServicio>().Use<CuentaCorrienteServicio>();

                x.For<ICtaCteComprobanteServicio>().Use<CtaCteComprobanteServicio>();

                x.For<IComprobanteServicio>().Use<ComprobanteServicio>();

                x.For<ICuentaCorrienteServicio>().Use<CuentaCorrienteServicio>();
            });
        }
    }
}
