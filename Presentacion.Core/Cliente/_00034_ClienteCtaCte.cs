using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using IServicios.CuentaCorriente;
using IServicios.CuentaCorriente.DTOs;
using IServicios.Persona.DTOs;
using PresentacionBase.Formularios;
using StructureMap;
using Color = System.Drawing.Color;

namespace Presentacion.Core.Cliente
{
    public partial class _00034_ClienteCtaCte : FormBase
    {
        private ClienteDto _clienteSeleccionado;
        private ICuentaCorrienteServicio _cuentaCorrienteServicio; 
        public _00034_ClienteCtaCte(ICuentaCorrienteServicio cuentaCorrienteServicio)
        {
            InitializeComponent();

            _cuentaCorrienteServicio = cuentaCorrienteServicio;

            _clienteSeleccionado = null;

            dgvGrilla.DataSource = new List<CuentaCorrienteDto>();

            FormatearGrilla(dgvGrilla);

            var fechaInicioMes = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 1, 0, 0, 0);
            dtpFechaDesde.Value = fechaInicioMes;

            txtTotal.Text = 0.ToString("C");
        }

        private void btnBuscarCliente_Click(object sender, System.EventArgs e)
        {
            var formClienteLookUp = ObjectFactory.GetInstance<ClienteLookUp>();
            formClienteLookUp.ShowDialog();

            if (formClienteLookUp.EntidadSeleccionada != null)
            {
                _clienteSeleccionado = (ClienteDto) formClienteLookUp.EntidadSeleccionada;

                txtApyNom.Text = _clienteSeleccionado.ApyNom;
                txtCelular.Text = _clienteSeleccionado.Telefono;
                txtDni.Text = _clienteSeleccionado.Dni;

                CargarDatos();
            }
            else
            {
                txtCelular.Clear();
                txtApyNom.Clear();
                txtDni.Clear();
                _clienteSeleccionado = null;

                dgvGrilla.DataSource=new List<CuentaCorrienteDto>();
            }
        }

        private void CargarDatos()
        {
            var resultado = _cuentaCorrienteServicio.Obtener(_clienteSeleccionado.Id, dtpFechaDesde.Value,
                dtpFechaHasta.Value, false);//TODO: solo fechadesde y hasta y el rdbutton
            dgvGrilla.DataSource = resultado;
               
            FormatearGrilla(dgvGrilla);
            txtTotal.Text = resultado.Sum(x => x.Monto) >= 0
                ? resultado.Sum(x => x.Monto).ToString("C")
                : (resultado.Sum(x => x.Monto) * -1).ToString("C");

            txtTotal.BackColor = resultado.Sum(x => x.Monto) >= 0 ? Color.DarkGreen : Color.DarkRed;
            txtTotal.ForeColor=Color.WhiteSmoke;
        }

        public override void FormatearGrilla(DataGridView dgv)
        {
            base.FormatearGrilla(dgv);

            dgv.Columns["Descripcion"].Visible = true;
            dgv.Columns["Descripcion"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgv.Columns["Descripcion"].HeaderText = @"Descripción";

            dgv.Columns["FechaStr"].Visible = true;
            dgv.Columns["FechaStr"].Width = 100;
            dgv.Columns["FechaStr"].HeaderText = "Fecha";

            dgv.Columns["HoraStr"].Visible = true;
            dgv.Columns["HoraStr"].Width = 100;
            dgv.Columns["HoraStr"].HeaderText = "Hora";

            dgv.Columns["MontoStr"].Visible = true;
            dgv.Columns["MontoStr"].Width = 100;
            dgv.Columns["MontoStr"].HeaderText = "Monto";
        }

        private void btnBuscar_Click(object sender, System.EventArgs e)
        {
            if (_clienteSeleccionado == null) return;
            CargarDatos();
        }

        private void dtpFechaDesde_ValueChanged(object sender, System.EventArgs e)
        {
            if (_clienteSeleccionado == null) return;

            if (dtpFechaDesde.Value > dtpFechaHasta.Value)
            {
                dtpFechaHasta.Value = dtpFechaDesde.Value;
            }

            CargarDatos();
        }

        private void dtpFechaHasta_ValueChanged(object sender, System.EventArgs e)
        {
            if (_clienteSeleccionado == null) return;
            CargarDatos();
        }

        private void btnSalir_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }

        private void btnActualizar_Click(object sender, System.EventArgs e)
        {
            if (_clienteSeleccionado == null) return;
            CargarDatos();
        }

        private void btnRealizarPago_Click(object sender, EventArgs e)
        {
            if (_clienteSeleccionado != null)
            {
                var formPagoDeuda=new _00035_ClientePagoCtaCte(_clienteSeleccionado);
                formPagoDeuda.ShowDialog();

                if (formPagoDeuda.RealizoPago)
                {
                    CargarDatos();
                }
            }
        }

        private void btnCancelarPago_Click(object sender, EventArgs e)
        {
            MessageBox.Show("No implementado aun."); //TODO: cancelar pago.
        }

        private void btnImprimir_Click(object sender, EventArgs e)
        {
            MessageBox.Show("No implementado aun."); //TODO: Imprimir.
        }
    }
}
