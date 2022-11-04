using System.Collections;
using System.Collections.Generic;
using Aplicacion.Constantes;
using IServicios.Comprobante.DTOs;

namespace IServicios.Comprobante
{
    public interface IFacturaServicio : IComprobanteServicio
    {
        IEnumerable<ComprobantePendienteDto> ObtenerPendientesPago();
        
    }
}
