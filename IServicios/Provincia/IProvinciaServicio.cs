using IServicios.Base;

namespace IServicios.Provincia
{
    public interface IProvinciaServicio : IServicio
    {
        bool VerificarSiExiste(string datoVerificar, long? entidadId = null);
    }
}
