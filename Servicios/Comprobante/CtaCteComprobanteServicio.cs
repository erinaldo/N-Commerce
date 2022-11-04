using Dominio.UnidadDeTrabajo;
using IServicios.Comprobante;
using IServicios.Comprobante.DTOs;

namespace Servicios.Comprobante
{
    public class CtaCteComprobanteServicio : ComprobanteServicio, ICtaCteComprobanteServicio
    {
        public CtaCteComprobanteServicio(IUnidadDeTrabajo unidadDeTrabajo)
            : base(unidadDeTrabajo)
        {
        }
    }
}
