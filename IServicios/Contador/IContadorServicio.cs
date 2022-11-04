using Aplicacion.Constantes;

namespace IServicios.Contador
{
    public interface IContadorServicio
    {
        int ObtenerSiguienteNroComprobante(TipoComprobante tipoComprobante);
    }
}
