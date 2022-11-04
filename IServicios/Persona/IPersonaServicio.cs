using IServicios.Persona.DTOs;
using System;
using System.Collections.Generic;

namespace IServicios.Persona
{
    public interface IPersonaServicio
    {
        void InicializadorDiccionario();
        long Insertar(PersonaDto persona);

        void Modificar(PersonaDto persona);

        void Eliminar(Type tipoEntidad, long id);

        PersonaDto Obtener(Type tipoEntidad, long id);

        IEnumerable<PersonaDto> Obtener(Type tipoEntidad, string cadenaBuscar);

        // ==================================================================== //

        void AgregarOpcionDiccionario(Type type, string value);
    }
}
