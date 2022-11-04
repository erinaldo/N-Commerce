using IServicios.Base;

namespace IServicios.ListaPrecio
{
    public interface IListaPrecioServicio : IServicio
    {
        bool VerificarSiExiste(string datoVerificar, long? entidadId = null);
    }
}
