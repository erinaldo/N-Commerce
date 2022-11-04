using IServicios.BaseDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IServicios.PuestoTrabajo.DTOs
{
    public class PuestoTrabajoDto : DtoBase
    {
        public int Codigo { get; set; }
        public string Descripcion { get; set; }
    }
}
