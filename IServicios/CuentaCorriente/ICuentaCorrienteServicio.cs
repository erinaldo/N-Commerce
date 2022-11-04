using System;
using System.Collections.Generic;
using IServicios.Comprobante.DTOs;
using IServicios.CuentaCorriente.DTOs;

namespace IServicios.CuentaCorriente
{
    public interface ICuentaCorrienteServicio
    {
        decimal ObtenerDeudaCliente(long clienteId);

        IEnumerable<CuentaCorrienteDto> Obtener(long entidadId, DateTime fechaDesde, DateTime fechaHasta, bool deuda);

        void Pagar(CtaCteComprobanteDto comprobanteDto);
    }
}
