using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.MetaData
{
    public interface ICuentaCorrienteCliente : IComprobante
    {
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        long ClienteId { get; set; }
    }
}
