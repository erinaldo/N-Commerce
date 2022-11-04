using Aplicacion.Constantes;
using IServicios.Configuracion;
using IServicios.Configuracion.DTOs;
using IServicios.Departamento;
using IServicios.Deposito;
using IServicios.ListaPrecio;
using IServicios.Localidad;
using IServicios.Provincia;
using Presentacion.Core.Articulo;
using Presentacion.Core.Departamento;
using Presentacion.Core.Deposito;
using Presentacion.Core.Localidad;
using Presentacion.Core.Provincia;
using PresentacionBase.Formularios;
using System;
using System.Linq;
using System.Windows.Forms;
using static Aplicacion.Constantes.Clases.ValidacionDatosEntrada;

namespace Presentacion.Core.Configuracion
{
    public partial class _00012_Configuracion : FormBase
    {
        private readonly IProvinciaServicio _provinciaServicio;
        private readonly IDepartamentoServicio _departamentoServicio;
        private readonly ILocalidadServicio _localidadServicio;
        private readonly IListaPrecioServicio _listaPrecioServicio;
        private readonly IConfiguracionServicio _configuracionServicio;
        private readonly IDepositoServicio _depositoSevicio;

        private ConfiguracionDto configuracion;

        //no tiene consultas, solo altas y modificacion

        public _00012_Configuracion(IProvinciaServicio provinciaServicio,
            IDepartamentoServicio departamentoServicio,
            ILocalidadServicio localidadServicio,
            IListaPrecioServicio listaPrecioServicio,
            IConfiguracionServicio configuracionServicio,
            IDepositoServicio depositoSevicio)
        {
            InitializeComponent();

            _provinciaServicio = provinciaServicio;
            _departamentoServicio = departamentoServicio;
            _localidadServicio = localidadServicio;
            _listaPrecioServicio = listaPrecioServicio;
            _configuracionServicio = configuracionServicio;
            _depositoSevicio = depositoSevicio;

            PoblarComboBox(cmbListaPrecioDefecto, _listaPrecioServicio.Obtener(string.Empty), "Descripcion", "Id");
            PoblarComboBox(cmbTipoPagoCompraPorDefecto, Enum.GetValues(typeof(TipoPago))); //poblar cmb con enum
            PoblarComboBox(cmbTipoPagoVentasPorDefecto, Enum.GetValues(typeof(TipoPago)));
            PoblarComboBox(cmbDepositoStockId, _depositoSevicio.Obtener(string.Empty), "Descripcion", "Id");
            PoblarComboBox(cmbDepositoVentaId, _depositoSevicio.Obtener(string.Empty), "Descripcion", "Id");

            AsignarEvento_EnterLeave(this);

            //-----validar datos de entrada

            txtCUIL.KeyPress += delegate (object sender, KeyPressEventArgs args)
            {
                NoInyeccion(sender, args);
                NoSimbolos(sender, args);
                NoLetras(sender, args);
            };

            txtTelefono.KeyPress += delegate (object sender, KeyPressEventArgs args)
            {
                NoInyeccion(sender, args);
                NoSimbolos(sender, args);
                NoLetras(sender, args);
            };

            txtCelular.KeyPress += delegate (object sender, KeyPressEventArgs args)
            {
                NoInyeccion(sender, args);
                NoSimbolos(sender, args);
                NoLetras(sender, args);
            };
        }

        private void _00012_Configuracion_Load(object sender, System.EventArgs e)
        {
            configuracion = _configuracionServicio.Obtener();

            if (configuracion != null) //consulta si ya existe una configuracion pre-existente
            {
                configuracion.EsPrimeraVez = false;

                //datos: empresa

                txtRazonSocial.Text = configuracion.RazonSocial;
                txtNombreFantasia.Text = configuracion.NombreFantasia;
                txtCUIL.Text = configuracion.Cuit;
                txtTelefono.Text = configuracion.Telefono;
                txtCelular.Text = configuracion.Celular;
                txtDireccion.Text = configuracion.Direccion;

                PoblarComboBox(cmbProvincia, _provinciaServicio.Obtener(string.Empty), "Descripcion", "Id");
                cmbProvincia.SelectedValue = configuracion.ProvinciaId;

                PoblarComboBox(cmbDepartamento, _departamentoServicio.ObtenerPorProvincia(configuracion.ProvinciaId), "Descripcion", "Id");
                cmbDepartamento.SelectedValue = configuracion.DepartamentoId;

                PoblarComboBox(cmbLocalidad, _localidadServicio.ObtenerPorDepartamento(configuracion.DepartamentoId), "Descripcion", "Id");
                cmbLocalidad.SelectedValue = configuracion.LocalidadId;

                txtEmail.Text = configuracion.Email;

                //datos: stock

                chkFacturaDescuentaStock.Checked = configuracion.FacturaDescuentaStock;
                chkPresupuestoDescuentaStock.Checked = configuracion.PresupuestoDescuentaStock;
                chkRemitoDescuentaStock.Checked = configuracion.RemitoDescuentaStock;
                chkActualizaCostoDesdeCompra.Checked = configuracion.ActualizaCostoDesdeCompra;
                chkModificaPrevioVentaDesdeCompra.Checked = configuracion.ModificaPrecioVentaDesdeCompra;
                cmbTipoPagoCompraPorDefecto.SelectedItem = configuracion.TipoFormaPagoPorDefectoCompra;//Enum
                cmbDepositoStockId.SelectedValue = configuracion.DepositoStockId;

                //datos: venta

                txtObservacionFactura.Text = configuracion.ObservacionEnPieFactura;
                cmbListaPrecioDefecto.SelectedValue = configuracion.ListaPrecioPorDefectoId;
                chkRenglonesFactura.Checked = configuracion.UnificarRenglonesIngresarMismoProducto;
                cmbTipoPagoVentasPorDefecto.SelectedItem = configuracion.TipoFormaPagoPorDefectoVenta;
                cmbDepositoVentaId.SelectedValue = configuracion.DepositoVentaId;

                //datos: caja

                if (configuracion.IngresoManualCajaInicial)
                {
                    rdbIngresoManualCaja.Checked = true;
                }
                else
                {
                    rdbIngresoPorCierreDelDIaAnterior.Checked = true;
                }

                chkPuestoSeparado.Checked = configuracion.PuestoCajaSeparado;
                chkRetiroDineroCaja.Checked = configuracion.ActivarRetiroDeCaja;
                nudMontoMaximo.Value = configuracion.MontoMaximoRetiroCaja;
                nudMontoMaximo.Enabled = chkRetiroDineroCaja.Checked;

                //datos: bascula

                chkActivarBascula.Checked = configuracion.ActivarBascula;
                rdbEtiquetaPorPrecio.Checked = configuracion.EtiquetasPorPrecio;
                txtCodigoBascula.Text = configuracion.CodigoBascula.ToString();

            }
            else
            {
                configuracion = new ConfiguracionDto();
                configuracion.EsPrimeraVez = true;

                LimpiarControles(this);

                var provincias = _provinciaServicio.Obtener(string.Empty);

                PoblarComboBox(cmbProvincia, provincias, "Descripcion", "Id");

                if (provincias.Any())
                {
                    var departamentos = _departamentoServicio.ObtenerPorProvincia((long)cmbProvincia.SelectedValue);
                    PoblarComboBox(cmbDepartamento, departamentos, "Descripcion", "Id");

                    if (departamentos.Any())
                    {
                        var localidades = _localidadServicio.ObtenerPorDepartamento((long)cmbDepartamento.SelectedValue);
                        PoblarComboBox(cmbLocalidad, localidades, "Descripcion", "Id");
                    }

                    txtRazonSocial.Focus();
                }
            }
        }



        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("¿Desea borrar los campos cargados?", "Atencion", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                LimpiarControles(this);
            }
        }

        private void btnEjecutar_Click(object sender, EventArgs e)
        {
            if (VerificarDatosObligatorios())
            {
                //datos: empresa

                configuracion.RazonSocial = txtRazonSocial.Text;
                configuracion.NombreFantasia = txtNombreFantasia.Text;
                configuracion.Cuit = txtCUIL.Text;
                configuracion.Telefono = txtTelefono.Text;
                configuracion.Celular = txtCelular.Text;
                configuracion.Direccion = txtDireccion.Text;
                configuracion.LocalidadId = (long)cmbLocalidad.SelectedValue;
                configuracion.Email = txtEmail.Text;

                //datos: stock

                configuracion.FacturaDescuentaStock = chkFacturaDescuentaStock.Checked;
                configuracion.PresupuestoDescuentaStock = chkPresupuestoDescuentaStock.Checked;
                configuracion.RemitoDescuentaStock = chkRemitoDescuentaStock.Checked;
                configuracion.ActualizaCostoDesdeCompra = chkActualizaCostoDesdeCompra.Checked;
                configuracion.TipoFormaPagoPorDefectoCompra = (TipoPago)cmbTipoPagoCompraPorDefecto.SelectedItem;
                configuracion.DepositoStockId = (long)cmbDepositoStockId.SelectedValue;

                //datos: venta

                configuracion.ObservacionEnPieFactura = txtObservacionFactura.Text;
                configuracion.ListaPrecioPorDefectoId = (long)cmbListaPrecioDefecto.SelectedValue;
                configuracion.UnificarRenglonesIngresarMismoProducto = chkRenglonesFactura.Checked;
                configuracion.TipoFormaPagoPorDefectoVenta = (TipoPago)cmbTipoPagoVentasPorDefecto.SelectedItem;//Enum
                configuracion.DepositoVentaId = (long)cmbDepositoVentaId.SelectedValue;

                //datos: caja

                configuracion.IngresoManualCajaInicial = rdbIngresoManualCaja.Checked;

                configuracion.PuestoCajaSeparado = chkPuestoSeparado.Checked;
                configuracion.ActivarRetiroDeCaja = chkRetiroDineroCaja.Checked;
                configuracion.MontoMaximoRetiroCaja = nudMontoMaximo.Value;

                //datos: bascula

                configuracion.ActivarBascula = chkActivarBascula.Checked;
                configuracion.EtiquetasPorPrecio = rdbEtiquetaPorPrecio.Checked;
                configuracion.CodigoBascula = int.Parse(txtCodigoBascula.Text);

                _configuracionServicio.Grabar(configuracion);

                MessageBox.Show("Los datos se grabaron correctamente.");
                Close();
            }
            else
            {
                MessageBox.Show("Por favor, verifique los datos Obligatorios");
            }
        }

        private bool VerificarDatosObligatorios()
        {
            if (string.IsNullOrEmpty(txtRazonSocial.Text)) return false;
            if (string.IsNullOrEmpty(txtCUIL.Text)) return false;
            if (string.IsNullOrEmpty(txtDireccion.Text)) return false;
            if (cmbLocalidad.Items.Count <= 0) return false;
            if (cmbListaPrecioDefecto.Items.Count <= 0) return false;
            if (cmbDepositoStockId.Items.Count <= 0) return false;
            if (cmbDepositoVentaId.Items.Count <= 0) return false;

            return true;
        }

        private void cmbProvincia_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (cmbProvincia.Items.Count <= 0) return;

            PoblarComboBox(cmbDepartamento, _departamentoServicio.ObtenerPorProvincia((long)cmbProvincia.SelectedValue), "Descripcion", "Id");

            if (cmbDepartamento.Items.Count <= 0) return;

            PoblarComboBox(cmbLocalidad, _localidadServicio.ObtenerPorDepartamento((long)cmbDepartamento.SelectedValue), "Descripcion", "Id");
        }

        private void cmbDepartamento_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (cmbDepartamento.Items.Count <= 0) return;

            PoblarComboBox(cmbLocalidad, _localidadServicio.ObtenerPorDepartamento((long)cmbDepartamento.SelectedValue), "Descripcion", "Id");
        }

        private void btnNuevaLocalidad_Click(object sender, EventArgs e)
        {
            var form = new _00006_AbmLocalidad(TipoOperacion.Nuevo).ShowDialog();

            PoblarComboBox(cmbLocalidad, _localidadServicio.ObtenerPorDepartamento(configuracion.DepartamentoId), "Descripcion", "Id");
            cmbLocalidad.SelectedValue = configuracion.LocalidadId;
        }

        private void btnNuevoDepartamento_Click(object sender, EventArgs e)
        {
            var form = new _00004_Abm_Departamento(TipoOperacion.Nuevo).ShowDialog();

            PoblarComboBox(cmbDepartamento, _departamentoServicio.ObtenerPorProvincia(configuracion.ProvinciaId), "Descripcion", "Id");
            cmbDepartamento.SelectedValue = configuracion.DepartamentoId;
        }

        private void btnNuevaProvincia_Click(object sender, EventArgs e)
        {
            var form = new _00002_Abm_Provincia(TipoOperacion.Nuevo).ShowDialog();

            PoblarComboBox(cmbProvincia, _provinciaServicio.Obtener(string.Empty), "Descripcion", "Id");
            cmbProvincia.SelectedValue = configuracion.ProvinciaId;
        }

        private void btnNuevaListaPrecioDefecto_Click(object sender, EventArgs e)
        {
            var form = new _00033_Abm_ListaPrecio(TipoOperacion.Nuevo).ShowDialog();

            PoblarComboBox(cmbTipoPagoCompraPorDefecto, Enum.GetValues(typeof(TipoPago))); //poblar cmb con enum
            cmbTipoPagoCompraPorDefecto.SelectedItem = configuracion.TipoFormaPagoPorDefectoCompra;//Enum

        }

        private void btnNuevoDepositoDefectoGrabarNuevoArticulo_Click(object sender, EventArgs e)
        {
            var form = new _00055_Abm_Deposito(TipoOperacion.Nuevo).ShowDialog();

            PoblarComboBox(cmbDepositoStockId, _depositoSevicio.Obtener(string.Empty), "Descripcion", "Id");
            PoblarComboBox(cmbDepositoVentaId, _depositoSevicio.Obtener(string.Empty), "Descripcion", "Id");
            cmbDepositoStockId.SelectedValue = configuracion.DepositoStockId;
            cmbDepositoVentaId.SelectedValue = configuracion.DepositoStockId;
        }

        private void rdbEtiquetaPorPeso_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbEtiquetaPorPeso.Checked) rdbEtiquetaPorPrecio.Checked = false;
        }

        private void rdbEtiquetaPorPrecio_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbEtiquetaPorPrecio.Checked) rdbEtiquetaPorPeso.Checked = false;
        }

        private void chkActivarBascula_CheckedChanged(object sender, EventArgs e)
        {
            if (!chkActivarBascula.Checked)
            {
                rdbEtiquetaPorPeso.Enabled = false;
                rdbEtiquetaPorPrecio.Enabled = false;
                txtCodigoBascula.Enabled = false;
            }
            else
            {
                txtCodigoBascula.Enabled = true;
                rdbEtiquetaPorPeso.Enabled = true;
                rdbEtiquetaPorPrecio.Enabled = true;
            }
        }

        private void chkRetiroDineroCaja_CheckedChanged(object sender, EventArgs e)
        {
            nudMontoMaximo.Enabled = chkRetiroDineroCaja.Checked;
        }
    }
}
