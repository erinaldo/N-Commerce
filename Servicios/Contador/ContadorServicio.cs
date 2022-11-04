using Aplicacion.Constantes;
using Dominio.UnidadDeTrabajo;
using IServicios.Contador;
using System;
using System.Linq;

namespace Servicios.Contador
{
    public class ContadorServicio : IContadorServicio
    {
        private IUnidadDeTrabajo _unidadDeTrabajo;
        public ContadorServicio(IUnidadDeTrabajo unidadDeTrabajo)
        {
            _unidadDeTrabajo = unidadDeTrabajo;
        }
        public int ObtenerSiguienteNroComprobante(TipoComprobante tipoComprobante)
        {
            //obtiene el comprobante segun el tipo que ingresa y lo incrementa.

            var nroComprobante = _unidadDeTrabajo.ContadorRepositorio.Obtener(x => x.TipoComprobante == tipoComprobante).FirstOrDefault();

            if (nroComprobante == null)
            {
                throw new Exception("Ocurrio un error al Obtener el numero de comprobante");
            }

            nroComprobante.Valor++;

            _unidadDeTrabajo.ContadorRepositorio.Modificar(nroComprobante);
            _unidadDeTrabajo.Commit();

            return nroComprobante.Valor;
        }

    }
}
