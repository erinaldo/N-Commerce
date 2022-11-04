using IServicios.Base;

namespace IServicios.Departamento
{
    public interface ICondicionIvaServicio : IServicio
    {
        bool VerificarSiExiste(string datoVerificar, long? entidadId = null);
    }
}
