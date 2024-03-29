﻿using Dominio.MetaData;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dominio.Entidades
{
    [Table("FormaPago_Cheque")]
    [MetadataType(typeof(IFormaPagoCheque))]
    public class FormaPagoCheque : FormaPago
    {
        // Propiedades
        public long ChequeId { get; set; }

        // Propiedades de Navegacion
        public virtual Cheque Cheque { get; set; }
    }
}
