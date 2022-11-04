using IServicios.Gasto;
using PresentacionBase.Formularios;
using System.Windows.Forms;

namespace Presentacion.Core.Caja
{
    public partial class _00043_Gastos : FormConsulta
    {
        public _00043_Gastos()
        {
            InitializeComponent();
        }

        private readonly IGastoServicio _gastoServicio;

        public _00043_Gastos(IGastoServicio gastoServicio)
        {
            InitializeComponent();
            _gastoServicio = gastoServicio;
        }

        public override void ActualizarDatos(DataGridView dgv, string cadenaBuscar)
        {
            dgv.DataSource = _gastoServicio.Obtener(cadenaBuscar, chkMostrarEliminados.Checked);
            base.ActualizarDatos(dgv, cadenaBuscar);
        }

        public override void FormatearGrilla(DataGridView dgv)
        {
            base.FormatearGrilla(dgv);

            dgv.Columns["Fecha"].Visible = true;
            dgv.Columns["Fecha"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgv.Columns["Fecha"].HeaderText = "Fecha";

            dgv.Columns["Descripcion"].Visible = true;
            dgv.Columns["Descripcion"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgv.Columns["Descripcion"].HeaderText = @"Descripción";

            dgv.Columns["Monto"].Visible = true;
            dgv.Columns["Monto"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgv.Columns["Monto"].HeaderText = "Monto";

            dgv.Columns["EliminadoStr"].Visible = true;
            dgv.Columns["EliminadoStr"].Width = 100;
            dgv.Columns["EliminadoStr"].HeaderText = "Eliminado";
            dgv.Columns["EliminadoStr"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        }

        public override bool EjecutarComando(TipoOperacion tipoOperacion, long? id = null)
        {
            var form = new _00044_Abm_Gastos(tipoOperacion, id);
            form.ShowDialog();
            return form.RealizoAlgunaOperacion;
        }
    }
}
