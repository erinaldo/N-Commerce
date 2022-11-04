using IServicios.UnidadMedida;
using PresentacionBase.Formularios;
using System.Windows.Forms;

namespace Presentacion.Core.Articulo
{
    public partial class _00023_UnidadDeMedida : FormConsulta
    {
        private readonly IUnidadMedidaServicio _unidadMedidaServicio;

        public _00023_UnidadDeMedida(IUnidadMedidaServicio unidadMedidaServicio)
        {
            InitializeComponent();

            _unidadMedidaServicio = unidadMedidaServicio;
        }

        public override void ActualizarDatos(DataGridView dgv, string cadenaBuscar)
        {
            dgv.DataSource = _unidadMedidaServicio.Obtener(cadenaBuscar, chkMostrarEliminados.Checked);

            base.ActualizarDatos(dgv, cadenaBuscar);
        }

        public override void FormatearGrilla(DataGridView dgv)
        {
            base.FormatearGrilla(dgv);

            dgv.Columns["Descripcion"].Visible = true;
            dgv.Columns["Descripcion"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgv.Columns["Descripcion"].HeaderText = "Descripcion";
            dgv.Columns["Descripcion"].DisplayIndex = 2;

            dgv.Columns["EliminadoStr"].Visible = true;
            dgv.Columns["EliminadoStr"].Width = 100;
            dgv.Columns["EliminadoStr"].HeaderText = "Eliminado";
            dgv.Columns["EliminadoStr"].DisplayIndex = 3;
        }

        public override bool EjecutarComando(TipoOperacion tipoOperacion, long? id = null)
        {
            var form = new _00024_Abm_UnidadDeMedida(tipoOperacion, id);
            form.ShowDialog();
            return form.RealizoAlgunaOperacion;
        }
    }
}
