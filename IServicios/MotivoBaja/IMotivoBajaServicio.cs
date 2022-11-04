using IServicios.Base;

namespace IServicios.MotivoBaja
{
    public interface IMotivoBajaServicio : IServicio
    {
        bool VerificarSiExiste(string datoVerificar, long? entidadId = null);
    }
}
