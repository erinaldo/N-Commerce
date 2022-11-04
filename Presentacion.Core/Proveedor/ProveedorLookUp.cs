using IServicios.Proveedor;
using PresentacionBase.Formularios;
using System.Windows.Forms;

namespace Presentacion.Core.Proveedor
{
    public partial class ProveedorLookUp : FormLookUp
    {
        private readonly IProveedorServicio _proveedorServicio;
        public ProveedorLookUp(IProveedorServicio proveedorServicio)
        {
            InitializeComponent();
            _proveedorServicio = proveedorServicio;
            EntidadSeleccionada = null;
        }

        public override void ActualizarDatos(DataGridView dgv, string cadenaBuscar)
        {
            var proveedores = _proveedorServicio.Obtener(string.Empty, false);
            dgv.DataSource = proveedores;
        }

        public override void FormatearGrilla(DataGridView dgv)
        {
            base.FormatearGrilla(dgv);

            dgv.Columns["RazonSocial"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgv.Columns["RazonSocial"].HeaderText = "Proveedor";
            dgv.Columns["RazonSocial"].Visible = true;
            dgv.Columns["RazonSocial"].DisplayIndex = 1;

            dgv.Columns["CUIT"].Width = 100;
            dgv.Columns["CUIT"].HeaderText = "CUIT";
            dgv.Columns["CUIT"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.Columns["CUIT"].Visible = true;
            dgv.Columns["CUIT"].DisplayIndex = 2;

            dgv.Columns["Direccion"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgv.Columns["Direccion"].HeaderText = "Direccion";
            dgv.Columns["Direccion"].Visible = true;
            dgv.Columns["Direccion"].DisplayIndex = 3;

            dgv.Columns["Telefono"].Width = 100;
            dgv.Columns["Telefono"].HeaderText = "Telefono";
            dgv.Columns["Telefono"].Visible = true;
            dgv.Columns["Telefono"].DisplayIndex = 4;

            dgv.Columns["Mail"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgv.Columns["Mail"].HeaderText = "correo";
            dgv.Columns["Mail"].Visible = true;
            dgv.Columns["Mail"].DisplayIndex = 5;

            dgv.Columns["CondicionIva"].Width = 100;
            dgv.Columns["CondicionIva"].HeaderText = "Condicion IVA";
            dgv.Columns["CondicionIva"].Visible = true;
            dgv.Columns["CondicionIva"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgv.Columns["CondicionIva"].DisplayIndex = 6;
        }
    }
}
