using System.Collections.Generic;
using System.Linq;
using Aplicacion.Constantes;
using IServicios.Persona.DTOs;

namespace Presentacion.Core.Comprobantes.Clases
{
    public class FacturaView
    {
        public FacturaView()
        {
            if (Items == null)
                Items = new List<ItemView>();
        }

        public int ContadorItem { get; set; } = 0; //inicializa

        //cabecera
        public ClienteDto Cliente { get; set; }
        public EmpleadoDto Vendedor { get; set; }
        public long PuntoVentaId { get; set; }
        public long UsuarioId { get; set; }
        public TipoComprobante TipoComprobante { get; set; }

        //cuerpo
        public List<ItemView> Items { get; set; }

        //pie
        public decimal SubTotal => Items.Sum(x => x.SubTotal);
        public string SubTotalStr => SubTotal.ToString("C");
        public decimal Descuento { get; set; }
        public decimal Total => (SubTotal - (Descuento / 100m));
        public string TotalStr => Total.ToString("C");
    }
}
