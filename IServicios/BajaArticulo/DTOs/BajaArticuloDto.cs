using IServicios.BaseDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IServicios.BajaArticulo.DTOs
{
    public class BajaArticuloDto : DtoBase
    {
        public long ArticuloId { get; set; }
        public string ArticuloDescripcion { get; set; }
        public long MotivoBajaId { get; set; }
        public decimal Cantidad { get; set; }
        public DateTime Fecha { get; set; }
        public string Observacion { get; set; }
        public byte[] Foto { get; set; }
        
    }
}
