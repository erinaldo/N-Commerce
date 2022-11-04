using Dominio.Entidades;
using Dominio.Repositorio;

namespace Dominio.UnidadDeTrabajo
{
    public interface IUnidadDeTrabajo
    {
        // Metodos
        void Commit();

        void Disposed();

        // Propiedades

        IRepositorio<Provincia> ProvinciaRepositorio { get; }
        IRepositorio<Departamento> DepartamentoRepositorio { get; }
        IRepositorio<Proveedor> ProveedorRepositorio { get; }
        IRepositorio<Localidad> LocalidadRepositorio { get; }
        IRepositorio<CondicionIva> CondicionIvaRepositorio { get; }
        IRepositorio<Usuario> UsuarioRepositorio { get; }
        IRepositorio<Configuracion> ConfiguracionRepositorio { get; }
        IRepositorio<ListaPrecio> ListaPrecioRepositorio { get; }
        IRepositorio<UnidadMedida> UnidadMedidaRepositorio { get; }
        IRepositorio<Marca> MarcaRepositorio { get; }
        IRepositorio<Gasto> GastoRepositorio { get; }
        IRepositorio<Rubro> RubroRepositorio { get; }
        IRepositorio<ConceptoGasto> ConceptoGastoRepositorio { get; }
        IRepositorio<Iva> IvaRepositorio { get; }
        IRepositorio<Articulo> ArticuloRepositorio { get; }
        IRepositorio<Deposito> DepositoRepositorio { get; }
        IRepositorio<Precio> PrecioRepositorio { get; }
        IRepositorio<Stock> StockRepositorio { get; }
        IRepositorio<Contador> ContadorRepositorio { get; }
        IRepositorio<PuestoTrabajo> PuestoTrabajoRepositorio { get; }
        IRepositorio<BajaArticulo> BajaArticuloRepositorio { get; }
        IRepositorio<MotivoBaja> MotivoBajaRepositorio { get; }
        IRepositorio<Caja> CajaRepositorio { get; }
        IRepositorio<DetalleCaja> DetalleCajaRepositorio { get; }
        IRepositorio<Banco> BancoRepositorio { get; }
        IRepositorio<Tarjeta> TarjetaRepositorio { get; }
        IRepositorio<MovimientoCuentaCorriente> MovimientoCuentaCorrienteRepositorio { get; }
        //IRepositorio<MovimientoCuentaCorriente> CtaComprobanteRepositorio { get; }
        
        //<--              Genericos              -->//

        IClienteRepositorio ClienteRepositorio { get; }
        IEmpleadoRepositorio EmpleadoRepositorio { get; }
        IFacturaRepositorio FacturaRepositorio { get; }
        //IRepositorio<MovimientoCuentaCorriente> CtaComprobanteRepositorio { get; }
        ICtaCteClienteRepositorio CtaComprobanteRepositorio { get; }
        ICuentaCorrienteRepositorio CuentaCorrienteRepositorio { get; }

    }
}
