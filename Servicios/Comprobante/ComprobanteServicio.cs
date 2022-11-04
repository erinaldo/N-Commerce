using System;
using System.Collections.Generic;
using Dominio.UnidadDeTrabajo;
using IServicios.Base;
using IServicios.Comprobante;
using IServicios.Comprobante.DTOs;

namespace Servicios.Comprobante
{
    public class ComprobanteServicio : IComprobanteServicio
    {
        protected readonly IUnidadDeTrabajo _unidadDeTrabajo; //para poder ser vista por los hijos
        private Dictionary<Type, string> _diccionario;

        public ComprobanteServicio(IUnidadDeTrabajo unidadDeTrabajo)
        {
            _unidadDeTrabajo = unidadDeTrabajo;
            _diccionario = new Dictionary<Type, string>();

            InicializadorDiccionario();
        }

        private void InicializadorDiccionario()
        {
            _diccionario.Add(typeof(FacturaDto), "Servicios.Comprobante.Factura");
            _diccionario.Add(typeof(CtaCteComprobanteDto), "Servicios.Comprobante.CuentaCorriente");
        }

        public void AgregarOpcionDiccionario(Type type, string value)
        {
            _diccionario.Add(type, value);
        }

        public virtual long Insertar(ComprobanteDto dto)
        {
            var comprobante = GenericInstance<Comprobante>.InstanciarEntidad(dto, _diccionario); //clase comprobante de servicio <- TODO:p

            return comprobante.Insertar(dto);
        }
    }
}
