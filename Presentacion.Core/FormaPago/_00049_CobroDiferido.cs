using PresentacionBase.Formularios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Windows.Forms;
using IServicios.Comprobante;
using IServicios.Comprobante.DTOs;

namespace Presentacion.Core.FormaPago
{
    public partial class _00049_CobroDiferido : FormBase
    {
        private readonly IFacturaServicio _facturaServicio;
        private ComprobantePendienteDto comprobanteSeleccionado;

        public _00049_CobroDiferido(IFacturaServicio facturaServicio)
        {
            InitializeComponent();

            _facturaServicio = facturaServicio;

            comprobanteSeleccionado = null;

            dgvGrillaPedientePago.DataSource = _facturaServicio.ObtenerPendientesPago();
            FormatearGrilla(dgvGrillaPedientePago);

            // Libreria para que refresque cada 60 seg la grilla
            // con las facturas que estan pendientes de pago.
            //Observable.Interval(TimeSpan.FromSeconds(10))
            //    .ObserveOn(DispatcherScheduler.Current)
            //    .Subscribe(_ =>
            //    {
            // dgvGrillaPedientePago.DataSource = _facturaServicio.ObtenerPendientesPago();

            //FormatearGrilla(dgvGrillaPedientePago);
            //    });
        } 
        public override void FormatearGrilla(DataGridView dgv)
        {
            base.FormatearGrilla(dgv);

            dgv.Columns["Numero"].Visible = true;
            dgv.Columns["Numero"].Width = 100;
            dgv.Columns["Numero"].HeaderText = "Comprobante Nro";

            dgv.Columns["ClienteApyNom"].Visible = true;
            dgv.Columns["ClienteApyNom"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgv.Columns["ClienteApyNom"].HeaderText = "Cliente";

            dgv.Columns["MontoPagarStr"].Visible = true;
            dgv.Columns["MontoPagarStr"].Width = 80;
            dgv.Columns["MontoPagarStr"].HeaderText = "Total";
        }

        private void dgvGrillaPedientePago_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvGrillaPedientePago.RowCount <= 0)
            {
                comprobanteSeleccionado = null;
            }

            comprobanteSeleccionado =
                (ComprobantePendienteDto)dgvGrillaPedientePago.Rows[e.RowIndex].DataBoundItem;

            if (comprobanteSeleccionado == null) return;

            txtTotal.Text = comprobanteSeleccionado.MontoPagarStr;
            
            dgvGrillaDetalleComprobante.DataSource = null;

            dgvGrillaDetalleComprobante.DataSource = comprobanteSeleccionado.Items.ToList();

            FormatearGrillaDetalle(dgvGrillaDetalleComprobante);
        }

        private void FormatearGrillaDetalle(DataGridView dgv)
        {
            for (int i = 0; i < dgv.ColumnCount; i++)
            {
                dgv.Columns[i].Visible = false;
            }

            dgv.Columns["Descripcion"].Visible = true;
            dgv.Columns["Descripcion"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgv.Columns["Descripcion"].HeaderText = "Articulo";

            dgv.Columns["PrecioStr"].Visible = true;
            dgv.Columns["PrecioStr"].Width = 100;
            dgv.Columns["PrecioStr"].HeaderText = "Precio";

            dgv.Columns["Cantidad"].Visible = true;
            dgv.Columns["Cantidad"].Width = 100;
            dgv.Columns["Cantidad"].HeaderText = "Cantidad";
        }

        private void btnEjecutar_Click(object sender, EventArgs e)
        {
          
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void cronometro_Tick(object sender, EventArgs e)
        {
            dgvGrillaPedientePago.DataSource = _facturaServicio.ObtenerPendientesPago();

            FormatearGrilla(dgvGrillaPedientePago);
        }

        private void dgvGrillaPedientePago_DoubleClick(object sender, EventArgs e)
        {
            var formFormaPago=new _00044_FormaPago(comprobanteSeleccionado);
            formFormaPago.ShowDialog();

            if (formFormaPago.RealizoVenta)
            {
                MessageBox.Show("Los datos se grabaron correctamente.");

            }
        }
    }
}
