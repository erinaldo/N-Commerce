using IServicios.Proveedor;
using PresentacionBase.Formularios;
using System.Windows.Forms;

namespace Presentacion.Core.Proveedor
{
    public partial class _00015_Proveedor : FormConsulta
    {
        public _00015_Proveedor()
        {
            InitializeComponent();
        }

        private readonly IProveedorServicio _proveedorServicio;

        public _00015_Proveedor(IProveedorServicio proveedorServicio)
        {
            InitializeComponent();

            _proveedorServicio = proveedorServicio;
        }

        public override void ActualizarDatos(DataGridView dgv, string cadenaBuscar)
        {
            dgv.DataSource = _proveedorServicio.Obtener(cadenaBuscar, chkMostrarEliminados.Checked);

            base.ActualizarDatos(dgv, cadenaBuscar);
        }

        public override void FormatearGrilla(DataGridView dgv)
        {
            base.FormatearGrilla(dgv);

            dgv.Columns["RazonSocial"].Visible = true;
            dgv.Columns["RazonSocial"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgv.Columns["RazonSocial"].HeaderText = "Razon Social";
            dgv.Columns["RazonSocial"].DisplayIndex = 0;

            dgv.Columns["CUIT"].Visible = true;
            dgv.Columns["CUIT"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgv.Columns["CUIT"].HeaderText = "CUIT";
            dgv.Columns["CUIT"].DisplayIndex = 1;

            dgv.Columns["Direccion"].Visible = true;
            dgv.Columns["Direccion"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgv.Columns["Direccion"].HeaderText = @"Dirección";
            dgv.Columns["Direccion"].DisplayIndex = 2;

            dgv.Columns["Telefono"].Visible = true;
            dgv.Columns["Telefono"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgv.Columns["Telefono"].HeaderText = "Telefono";
            dgv.Columns["Telefono"].DisplayIndex = 3;

            dgv.Columns["Mail"].Visible = true;
            dgv.Columns["Mail"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgv.Columns["Mail"].HeaderText = "Mail";
            dgv.Columns["Mail"].DisplayIndex = 4;

            dgv.Columns["Localidad"].Visible = true;
            dgv.Columns["Localidad"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgv.Columns["Localidad"].HeaderText = "Localidad";
            dgv.Columns["Localidad"].DisplayIndex = 5;

            dgv.Columns["CondicionIva"].Visible = true;
            dgv.Columns["CondicionIva"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgv.Columns["CondicionIva"].HeaderText = "Cond. IVA";
            dgv.Columns["CondicionIva"].DisplayIndex = 6;

            dgv.Columns["EliminadoStr"].Visible = true;
            dgv.Columns["EliminadoStr"].Width = 100;
            dgv.Columns["EliminadoStr"].HeaderText = "Eliminado";
            dgv.Columns["EliminadoStr"].DisplayIndex = 7;
        }

        public override bool EjecutarComando(TipoOperacion tipoOperacion, long? id = null)
        {
            var form = new _00016_Abm_Proveedor(tipoOperacion, id);
            form.ShowDialog();
            return form.RealizoAlgunaOperacion;
        }
    }
}
