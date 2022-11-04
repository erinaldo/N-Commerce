using IServicios.ListaPrecio;
using PresentacionBase.Formularios;
using System.Windows.Forms;

namespace Presentacion.Core.Articulo
{
    public partial class _00032_ListaPrecio : FormConsulta
    {
        private readonly IListaPrecioServicio _listaPrecioServicio;
        public _00032_ListaPrecio(IListaPrecioServicio listaPrecioServicio)
        {
            InitializeComponent();

            _listaPrecioServicio = listaPrecioServicio;
        }

        public override void ActualizarDatos(DataGridView dgv, string cadenaBuscar)
        {
            dgv.DataSource = _listaPrecioServicio.Obtener(cadenaBuscar, chkMostrarEliminados.Checked);
            base.ActualizarDatos(dgv, cadenaBuscar);
        }

        public override void FormatearGrilla(DataGridView dgv)
        {
            base.FormatearGrilla(dgv);

            dgv.Columns["Descripcion"].Visible = true;
            dgv.Columns["Descripcion"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgv.Columns["Descripcion"].HeaderText = @"Descripción";

            dgv.Columns["PorcentajeGanancia"].Visible = true;
            dgv.Columns["PorcentajeGanancia"].Width = 120;
            dgv.Columns["PorcentajeGanancia"].HeaderText = "Porcentaje de ganancia";
            dgv.Columns["PorcentajeGanancia"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgv.Columns["AutorizacionStr"].Visible = true;
            dgv.Columns["AutorizacionStr"].Width = 120;
            dgv.Columns["AutorizacionStr"].HeaderText = "Requiere Autorizacion";
            dgv.Columns["AutorizacionStr"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgv.Columns["EliminadoStr"].Visible = true;
            dgv.Columns["EliminadoStr"].Width = 100;
            dgv.Columns["EliminadoStr"].HeaderText = "Eliminado";
            dgv.Columns["EliminadoStr"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        }

        public override bool EjecutarComando(TipoOperacion tipoOperacion, long? id = null)
        {
            var form = new _00033_Abm_ListaPrecio(tipoOperacion, id);
            form.ShowDialog();
            return form.RealizoAlgunaOperacion;
        }
    }
}
