using IServicios.Configuracion;
using IServicios.Configuracion.DTOs;
using Presentacion.Core.Comprobantes.Clases;
using PresentacionBase.Formularios;
using StructureMap;
using System;
using System.Linq;
using System.Windows.Forms;

namespace Presentacion.Core.Comprobantes
{
    public partial class _00057_Comprobante : FormBase
    {
        private FacturaView _factura;
        private ConfiguracionDto configuracion;
        private readonly IConfiguracionServicio _configuracionServicio;
        public _00057_Comprobante()
        {
            InitializeComponent();
        }

        public _00057_Comprobante(FacturaView factura)
        : this()
        {
            _configuracionServicio = ObjectFactory.GetInstance<IConfiguracionServicio>();
            configuracion = _configuracionServicio.Obtener();
            _factura = factura;
            CargarDatos(_factura);
        }

        private void CargarDatos(FacturaView factura)
        {
            dgvDetalle.DataSource= _factura.Items.ToList();

            FormatearGrilla(dgvDetalle);
        }

        private void _00057_Comprobante_Load(object sender, System.EventArgs e)
        {
            this.reportViewer1.RefreshReport();
            lblEmpresa.Text = configuracion.RazonSocial;
            lblCuit.Text = configuracion.Cuit;
            lblTelefono.Text = configuracion.Telefono;
            lblDireccion.Text = configuracion.Direccion;

            lblFecha.Text = DateTime.Today.ToString();
            lblCliente.Text = "CONSUMIDOR FINAL";
            txtTotal.Text = _factura.TotalStr;
        }

        public override void FormatearGrilla(DataGridView dgv)
        {
            base.FormatearGrilla(dgv);

            dgv.Columns["CodigoBarra"].Visible = true;
            dgv.Columns["CodigoBarra"].Width = 100;
            dgv.Columns["CodigoBarra"].HeaderText = "Código";
            dgv.Columns["CodigoBarra"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgv.Columns["Descripcion"].Visible = true;
            dgv.Columns["Descripcion"].HeaderText = "Articulo";
            dgv.Columns["Descripcion"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            dgv.Columns["Iva"].Visible = true;
            dgv.Columns["Iva"].Width = 40;
            dgv.Columns["Iva"].HeaderText = "Iva";
            dgv.Columns["Iva"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgv.Columns["PrecioStr"].Visible = true;
            dgv.Columns["PrecioStr"].Width = 80;
            dgv.Columns["PrecioStr"].HeaderText = "Precio";
            dgv.Columns["PrecioStr"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgv.Columns["Cantidad"].Visible = true;
            dgv.Columns["Cantidad"].Width = 60;
            dgv.Columns["Cantidad"].HeaderText = "Cantidad";
            dgv.Columns["Cantidad"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgv.Columns["SubTotalStr"].Visible = true;
            dgv.Columns["SubTotalStr"].Width = 80;
            dgv.Columns["SubTotalStr"].HeaderText = "Sub-Total";
            dgv.Columns["SubTotalStr"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        }
    }
}
