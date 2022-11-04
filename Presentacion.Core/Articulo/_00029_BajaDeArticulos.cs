using IServicios.BajaArticulo;
using PresentacionBase.Formularios;
using System.Windows.Forms;

namespace Presentacion.Core.Articulo
{
    public partial class _00029_BajaDeArticulos : FormConsulta
    {
        private readonly IBajaArticuloServicio _bajaArticuloServicio;

        public _00029_BajaDeArticulos(IBajaArticuloServicio bajaArticuloServicio)
        {
            InitializeComponent();

            _bajaArticuloServicio = bajaArticuloServicio;
        }

        public override void ActualizarDatos(DataGridView dgv, string cadenaBuscar)
        {
            dgv.DataSource = _bajaArticuloServicio.Obtener(cadenaBuscar, chkMostrarEliminados.Checked);
            base.ActualizarDatos(dgv, cadenaBuscar);
        }

        public override void FormatearGrilla(DataGridView dgv)
        {
            base.FormatearGrilla(dgv);

            dgv.Columns["Fecha"].Visible = true;
            dgv.Columns["Fecha"].Width = 100;
            dgv.Columns["Fecha"].HeaderText = "Fecha de Baja";
            dgv.Columns["Fecha"].DisplayIndex = 0;

            dgv.Columns["ArticuloDescripcion"].Visible = true;
            dgv.Columns["ArticuloDescripcion"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgv.Columns["ArticuloDescripcion"].HeaderText = "Articulo";
            dgv.Columns["ArticuloDescripcion"].DisplayIndex = 1;

            dgv.Columns["Observacion"].Visible = true;
            dgv.Columns["Observacion"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgv.Columns["Observacion"].HeaderText = "Observacion";
            dgv.Columns["Observacion"].DisplayIndex = 2;

            dgv.Columns["Cantidad"].Visible = true;
            dgv.Columns["Cantidad"].Width = 70;
            dgv.Columns["Cantidad"].HeaderText = "Cantidad";
            dgv.Columns["Cantidad"].DisplayIndex = 3;

            dgv.Columns["EliminadoStr"].Visible = true;
            dgv.Columns["EliminadoStr"].Width = 70;
            dgv.Columns["EliminadoStr"].HeaderText = "Eliminado";
            dgv.Columns["EliminadoStr"].DisplayIndex = 4;
        }

        public override bool EjecutarComando(TipoOperacion tipoOperacion, long? id = null)
        {
            var form = new _00030_Abm_BajaArticulos(tipoOperacion, id);
            form.ShowDialog();
            return form.RealizoAlgunaOperacion;
        }
    }
}
