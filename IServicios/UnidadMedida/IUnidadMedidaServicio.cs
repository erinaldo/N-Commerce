using IServicios.Base;
using IServicios.BaseDto;

namespace IServicios.UnidadMedida
{
    public interface IUnidadMedidaServicio : IServicio
    {
        void Eliminar(long id);

        void Insertar(DtoBase dtoEntidad);

        void Modificar(DtoBase dtoEntidad);

        DtoBase Obtener(long id);

        bool VerificarSiExiste(string datoVerificar, long? entidadId = null);
    }
}
