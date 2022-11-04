﻿using IServicios.Rubro;
using PresentacionBase.Formularios;
using System.Windows.Forms;

namespace Presentacion.Core.Articulo
{
    public partial class _00019_Rubro : FormConsulta
    {
        private readonly IRubroServicio _rubroServicio;

        public _00019_Rubro(IRubroServicio rubroServicio)
        {
            InitializeComponent();

            _rubroServicio = rubroServicio;
        }
        public override void ActualizarDatos(DataGridView dgv, string cadenaBuscar)
        {
            // Codigo agregado por Nosotros
            dgv.DataSource = _rubroServicio.Obtener(cadenaBuscar, chkMostrarEliminados.Checked);

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
            var formulario = new _00020_Abm_Rubro(tipoOperacion, id);

            formulario.ShowDialog();

            return formulario.RealizoAlgunaOperacion;
        }
    }
}
