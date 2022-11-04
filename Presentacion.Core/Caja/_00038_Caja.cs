using Aplicacion.Constantes;
using PresentacionBase.Formularios;
using StructureMap;
using System;
using System.Windows.Forms;
using IServicios.Caja;
using IServicios.Caja.DTOs;

namespace Presentacion.Core.Caja
{
    public partial class _00038_Caja : FormBase
    {
        private readonly ICajaServicio _cajaServicio;
        private CajaDto _cajaSeleccionada;
        public _00038_Caja(ICajaServicio cajaServicio)
        {
            InitializeComponent();

            _cajaServicio = cajaServicio;
            _cajaSeleccionada = null;
        }

        private void btnSalir_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }

        private void btnAbrirCaja_Click(object sender, System.EventArgs e)
        {
            //verificar si esta abierta para el usuario actual
            if (!_cajaServicio.VerificarSiExisteCajaAbierta(Identidad.UsuarioId))
            {
                var formAbrirCaja = ObjectFactory.GetInstance<_00039_AperturaCaja>();
                formAbrirCaja.ShowDialog();

                if (formAbrirCaja.RealizoApertura)
                {
                    ActualizarDatos();
                }
            }
            else
            {
                MessageBox.Show($"El usuario {Identidad.Apellido} {Identidad.Nombre} ya posee una caja abierta.");
            }
        }

        private void ActualizarDatos()
        {
            dgvGrilla.DataSource =
                _cajaServicio.Obtener(!string.IsNullOrEmpty(txtBuscar.Text) ? txtBuscar.Text : string.Empty,
                    chkFiltroFecha.Checked, dtpFechaDesde.Value, dtpFechaHasta.Value);

            FormatearGrilla(dgvGrilla);
        }

        public override void FormatearGrilla(DataGridView dgv)
        {
            base.FormatearGrilla(dgv);

            dgv.Columns["UsuarioApertura"].Visible = true;
            dgv.Columns["UsuarioApertura"].Width = 100;
            dgv.Columns["UsuarioApertura"].HeaderText = "Usuario";
            dgv.Columns["UsuarioApertura"].DisplayIndex = 0;

            dgv.Columns["FechaAperturaStr"].Visible = true;
            dgv.Columns["FechaAperturaStr"].Width = 70;
            dgv.Columns["FechaAperturaStr"].HeaderText = "Fecha de apertura";
            dgv.Columns["FechaAperturaStr"].DisplayIndex = 1;

            dgv.Columns["MontoaperturaStr"].Visible = true;
            dgv.Columns["MontoaperturaStr"].Width = 70;
            dgv.Columns["MontoaperturaStr"].HeaderText = "Monto inicial";
            dgv.Columns["MontoaperturaStr"].DisplayIndex = 2;

            dgv.Columns["UsuarioCierre"].Visible = true;
            dgv.Columns["UsuarioCierre"].Width = 100;
            dgv.Columns["UsuarioCierre"].HeaderText = "Cerrada por";
            dgv.Columns["UsuarioCierre"].DisplayIndex = 3;

            dgv.Columns["FechaCierreStr"].Visible = true;
            dgv.Columns["FechaCierreStr"].Width = 70;
            dgv.Columns["FechaCierreStr"].HeaderText = "Fecha de cierre";
            dgv.Columns["FechaCierreStr"].DisplayIndex = 4;

            dgv.Columns["MontoCierreStr"].Visible = true;
            dgv.Columns["MontoCierreStr"].Width = 70;
            dgv.Columns["MontoCierreStr"].HeaderText = "Monto final";
            dgv.Columns["MontoCierreStr"].DisplayIndex = 5;

            ///////////////////////////////////////////////////////////////

            dgv.Columns["TotalEntradaEfectivoStr"].Visible = true;
            dgv.Columns["TotalEntradaEfectivoStr"].Width = 60;
            dgv.Columns["TotalEntradaEfectivoStr"].HeaderText = "Efectivo entrada";
            dgv.Columns["TotalEntradaEfectivoStr"].DisplayIndex = 6;

            dgv.Columns["TotalEntradaCtaCteStr"].Visible = true;
            dgv.Columns["TotalEntradaCtaCteStr"].Width = 60;
            dgv.Columns["TotalEntradaCtaCteStr"].HeaderText = "Cta. cte. entrada";
            dgv.Columns["TotalEntradaCtaCteStr"].DisplayIndex = 7;

            dgv.Columns["TotalEntradaTarjetaStr"].Visible = true;
            dgv.Columns["TotalEntradaTarjetaStr"].Width = 60;
            dgv.Columns["TotalEntradaTarjetaStr"].HeaderText = "Tarjeta entrada";
            dgv.Columns["TotalEntradaTarjetaStr"].DisplayIndex = 8;

            dgv.Columns["TotalEntradaChequeStr"].Visible = true;
            dgv.Columns["TotalEntradaChequeStr"].Width = 60;
            dgv.Columns["TotalEntradaChequeStr"].HeaderText = "Cheque entrada";
            dgv.Columns["TotalEntradaChequeStr"].DisplayIndex = 9;

            ///////////////////////////////////////////////////////////////

            dgv.Columns["TotalSalidaEfectivoStr"].Visible = true;
            dgv.Columns["TotalSalidaEfectivoStr"].Width = 60;
            dgv.Columns["TotalSalidaEfectivoStr"].HeaderText = "Efectivo salida";
            dgv.Columns["TotalSalidaEfectivoStr"].DisplayIndex = 10;

            dgv.Columns["TotalSalidaCtaCteStr"].Visible = true;
            dgv.Columns["TotalSalidaCtaCteStr"].Width = 60;
            dgv.Columns["TotalSalidaCtaCteStr"].HeaderText = "Cta. cte. salida";
            dgv.Columns["TotalSalidaCtaCteStr"].DisplayIndex = 11;

            dgv.Columns["TotalSalidaTarjetaStr"].Visible = true;
            dgv.Columns["TotalSalidaTarjetaStr"].Width = 60;
            dgv.Columns["TotalSalidaTarjetaStr"].HeaderText = "Tarjeta salida";
            dgv.Columns["TotalSalidaTarjetaStr"].DisplayIndex = 12;

            dgv.Columns["TotalSalidaChequeStr"].Visible = true;
            dgv.Columns["TotalSalidaChequeStr"].Width = 60;
            dgv.Columns["TotalSalidaChequeStr"].HeaderText = "Cheque salida";
            dgv.Columns["TotalSalidaChequeStr"].DisplayIndex = 13;
        }

        private void chkFiltroFecha_CheckedChanged(object sender, EventArgs e)
        {
            dtpFechaDesde.Enabled = chkFiltroFecha.Checked;
            dtpFechaHasta.Enabled = chkFiltroFecha.Checked;

            if (!chkFiltroFecha.Checked) return;

            dtpFechaDesde.Value = DateTime.Now;
            dtpFechaHasta.Value = DateTime.Now;
        }

        private void dtpFechaDesde_ValueChanged(object sender, EventArgs e)
        {
            dtpFechaHasta.Value = dtpFechaDesde.Value;
            dtpFechaHasta.MinDate = dtpFechaDesde.Value;
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            ActualizarDatos();
        }

        private void _00038_Caja_Load(object sender, EventArgs e)
        {
            dtpFechaDesde.Value = DateTime.Today;
            dtpFechaHasta.Value = DateTime.Today;
            txtBuscar.Clear();

            ActualizarDatos();
        }

        private void btnCierreCaja_Click(object sender, EventArgs e)
        {
            var formCierreCaja=new _00040_CierreCaja(_cajaSeleccionada.Id);
            formCierreCaja.ShowDialog();

            ActualizarDatos();
        }

        private void dgvGrilla_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvGrilla.RowCount <= 0)
            {
                _cajaSeleccionada = null;
                return;
            }

            _cajaSeleccionada = (CajaDto) dgvGrilla.Rows[e.RowIndex].DataBoundItem;

        }
    }
}
