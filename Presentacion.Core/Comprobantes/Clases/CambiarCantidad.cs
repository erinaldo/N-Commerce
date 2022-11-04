using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Presentacion.Core.Comprobantes.Clases
{
    public partial class CambiarCantidad : Form
    {
        private ItemView _itemSeleccionado;

        public ItemView Item => _itemSeleccionado; //expone el resultado hacia afuera.

        public CambiarCantidad(ItemView item)
        {
            InitializeComponent();
            _itemSeleccionado = item;
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            _itemSeleccionado = null;
            this.Close();
        }

        private void CambiarCantidad_Load(object sender, EventArgs e)
        {
            nudCantidad.Value = _itemSeleccionado.Cantidad;
            nudCantidad.Select(0, nudCantidad.Text.Length);
            lblDescripcion.Text = _itemSeleccionado.Descripcion;
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            _itemSeleccionado.Cantidad = nudCantidad.Value;
            this.Close();
        }
    }
}
