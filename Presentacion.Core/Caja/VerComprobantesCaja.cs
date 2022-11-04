using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using IServicios.Caja.DTOs;
using IServicios.Comprobante;

namespace Presentacion.Core.Caja
{
    public partial class VerComprobantesCaja : Form
    {
        private readonly IFacturaServicio _facturaServicio;
        public VerComprobantesCaja(List<CajaDetalleDto> comprobantes)
        {
            InitializeComponent();

            dgvGrilla.DataSource = comprobantes.ToList();
            this.FormatearGrilla(this.dgvGrilla);
            //lblNroComprobante.Text = comprobante.Numero.ToString();
            //lblFecha.Text = comprobante.Fecha.ToString("d");
        }

        public void FormatearGrilla(DataGridView dgv)
        {
            for (int i = 0; i < dgv.ColumnCount; i++)
            {
                dgv.Columns[i].Visible = false;

                dgv.Columns[i].HeaderCell.Style.Alignment
                    = DataGridViewContentAlignment.MiddleCenter;
            }

            dgv.Columns["Monto"].Visible = true;
            dgv.Columns["Monto"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgv.Columns["Monto"].HeaderText = "Precio";
            dgv.Columns["Monto"].DisplayIndex = 1;

            dgv.Columns["TipoPago"].Visible = true;
            dgv.Columns["TipoPago"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgv.Columns["TipoPago"].HeaderText = "Tipo de pago";
            dgv.Columns["TipoPago"].DisplayIndex = 2;

            //dgv.Columns["Codigo"].Visible = true;
            //dgv.Columns["Codigo"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            //dgv.Columns["Codigo"].HeaderText = "Codigo";
            //dgv.Columns["Codigo"].DisplayIndex = 1;
            //dgv.Columns["Codigo"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            //dgv.Columns["Descripcion"].Visible = true;
            //dgv.Columns["Descripcion"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            //dgv.Columns["Descripcion"].HeaderText = "Descripcion";
            //dgv.Columns["Descripcion"].DisplayIndex = 2;

            //dgv.Columns["Cantidad"].Visible = true;
            //dgv.Columns["Cantidad"].Width = 100;
            //dgv.Columns["Cantidad"].HeaderText = "Cantidad";
            //dgv.Columns["Cantidad"].DisplayIndex = 3;

            //dgv.Columns["Iva"].Visible = true;
            //dgv.Columns["Iva"].Width = 100;
            //dgv.Columns["Iva"].HeaderText = "Iva";
            //dgv.Columns["Iva"].DisplayIndex = 4;

            //dgv.Columns["Precio"].Visible = true;
            //dgv.Columns["Precio"].Width = 100;
            //dgv.Columns["Precio"].HeaderText = "Precio";
            //dgv.Columns["Precio"].DisplayIndex = 5;

            //dgv.Columns["SubTotal"].Visible = true;
            //dgv.Columns["SubTotal"].Width = 100;
            //dgv.Columns["SubTotal"].HeaderText = "SubTotal";
            //dgv.Columns["SubTotal"].DisplayIndex = 6;
        }
        private void btnSalir_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void VerComprobantesCaja_Load(object sender, EventArgs e)
        {

        }
    }
}
