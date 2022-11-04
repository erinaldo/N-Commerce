using IServicios.Base;

namespace IServicios.Banco
{
    public interface IBancoServicio:IServicio
    {
        bool VerificarSiExiste(string datoVerificar, long? entidadId = null);
    }
}
