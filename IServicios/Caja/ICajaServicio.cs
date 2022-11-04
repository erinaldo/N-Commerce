using System;
using System.Collections.Generic;
using Aplicacion.Constantes;
using IServicios.Caja.DTOs;

namespace IServicios.Caja
{
    public interface ICajaServicio
    {

        decimal ObtenerMontoCajaAnterior(long usuarioId);

        bool VerificarSiExisteCajaAbierta(long usuarioId);

        void AbrirCaja(long usuarioId, decimal monto);

        void CerrarCaja(CajaDto caja);

        IEnumerable<CajaDto> Obtener(string cadenaBuscar, bool filtroPorFecha, DateTime fechaDesde,
            DateTime fechaHasta);

        CajaDto Obtener(long cajaId);
        CajaDto Obtener(long cajaId, TipoMovimiento tipoMovimiento);
    }

}
