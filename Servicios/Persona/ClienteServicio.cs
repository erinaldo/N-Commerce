using Dominio.UnidadDeTrabajo;
using IServicios.Persona;
using IServicios.Persona.DTOs;
using System;
using System.Collections.Generic;

namespace Servicios.Persona
{
    public class ClienteServicio : PersonaServicio, IClienteServicio
    {
        public ClienteServicio(IUnidadDeTrabajo unidadDeTrabajo)
            : base(unidadDeTrabajo)
        {

        }

        public long Insertar(PersonaDto persona)
        {
            throw new NotImplementedException();
        }

        public void Modificar(PersonaDto persona)
        {
            throw new NotImplementedException();
        }

        PersonaDto IPersonaServicio.Obtener(Type tipoEntidad, long id)
        {
            throw new NotImplementedException();
        }

        IEnumerable<PersonaDto> IPersonaServicio.Obtener(Type tipoEntidad, string cadenaBuscar)
        {
            throw new NotImplementedException();
        }

        public int ObtenerCantidadClientes()
        {
            throw new NotImplementedException();
        }
    }
}
