using IServicios.Base;

namespace IServicios.PuestoTrabajo
{
    public interface IPuestoTrabajoServicio : IServicio
    {
        int ObtenerSiguienteNroCodigo();
    }
}
