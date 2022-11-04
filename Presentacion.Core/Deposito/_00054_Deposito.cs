using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using IServicios.Deposito;
using IServicios.Deposito.DTOs;
using PresentacionBase.Formularios;

namespace Presentacion.Core.Deposito
{
    public partial class _00054_Deposito : FormConsulta
    {
        private readonly IDepositoServicio _depositoServicio;

        public _00054_Deposito(IDepositoServicio depositoServicio)
        {
            InitializeComponent();

            _depositoServicio = depositoServicio;
        }

        public override void ActualizarDatos(DataGridView dgv, string cadenaBuscar)
        {
            bool mostrarTodos;
            if (chkMostrarEliminados.Checked)
            {
                mostrarTodos = true;
            }
            else
            {
                mostrarTodos = false;
            }

            // Codigo agregado por Nosotros
            dgv.DataSource = _depositoServicio.Obtener(cadenaBuscar, mostrarTodos);

            // Codigo del PAPA
            base.ActualizarDatos(dgv, cadenaBuscar);
        }

        public override void FormatearGrilla(DataGridView dgv)
        {
            base.FormatearGrilla(dgv); // Pongo Invisible las Columnas

            dgv.Columns["Descripcion"].Visible = true;
            dgv.Columns["Descripcion"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgv.Columns["Descripcion"].HeaderText = @"Descripción";

            dgv.Columns["EliminadoStr"].Visible = true;
            dgv.Columns["EliminadoStr"].Width = 100;
            dgv.Columns["EliminadoStr"].HeaderText = "Eliminado";
            dgv.Columns["EliminadoStr"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        }

        public override bool EjecutarComando(TipoOperacion tipoOperacion, long? id = null)
        {
            var formulario = new _00055_Abm_Deposito(tipoOperacion, id);

            formulario.ShowDialog();

            return formulario.RealizoAlgunaOperacion;
        }
    }
}