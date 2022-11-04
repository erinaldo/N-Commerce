using IServicios.Base;

namespace IServicios.Tarjeta
{
    public interface ITarjetaServicio:IServicio
    {
        bool VerificarSiExiste(string datoVerificar, long? entidadId = null);
    }
}
