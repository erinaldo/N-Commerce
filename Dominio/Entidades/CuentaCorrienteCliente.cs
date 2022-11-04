using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Dominio.MetaData;

namespace Dominio.Entidades
{
    [Table("Comprobante_CuentaCorriente")]
    [MetadataType(typeof(ICuentaCorrienteCliente))]
    public class CuentaCorrienteCliente : Comprobante
    {
        public long ClienteId { get; set; }
        public virtual Cliente Cliente { get; set; }
    }
}
