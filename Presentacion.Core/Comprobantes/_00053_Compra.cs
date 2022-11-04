using Aplicacion.Constantes;
using IServicios.Articulo;
using IServicios.Articulo.DTOs;
using IServicios.Comprobante;
using IServicios.Comprobante.DTOs;
using IServicios.Configuracion;
using IServicios.Configuracion.DTOs;
using IServicios.Contador;
using IServicios.ListaPrecio;
using IServicios.Persona;
using IServicios.Persona.DTOs;
using IServicios.Proveedor.DTOs;
using Presentacion.Core.Articulo;
using Presentacion.Core.Comprobantes.Clases;
using Presentacion.Core.Configuracion;
using Presentacion.Core.FormaPago;
using Presentacion.Core.Proveedor;
using PresentacionBase.Formularios;
using StructureMap;
using System;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace Presentacion.Core.Comprobantes
{
    public partial class _00053_Compra : FormBase
    {
        private FacturaView _factura;
        private ItemView _itemSeleccionado;
        private ProveedorDTO _proveedorSeleccionado;
        private ConfiguracionDto _configuracion;
        private ArticuloVentaDto _articuloSeleccionado;
        private EmpleadoDto _vendedorSeleccionado;
        private bool _ingresoPorCodigoBascula;
        private bool _permiteAgregarPorCantidad;
        private bool _articuloConPrecioAlternativo;

        private readonly IConfiguracionServicio _configuracionServicio;
        private readonly IArticuloServicio _articuloServicio;
        private readonly IPersonaServicio _personaServicio;
        private readonly IListaPrecioServicio _listaPrecioServicio;
        private readonly IFacturaServicio _facturaServicio;
        private readonly IContadorServicio _contadorServicio;

        public _00053_Compra(IPersonaServicio personaServicio,IConfiguracionServicio configuracionServicio, IArticuloServicio articuloServicio, IListaPrecioServicio listaPrecioServicio, IFacturaServicio facturaServicio, IContadorServicio contadorServicio)
        {
            InitializeComponent();
            _configuracionServicio = configuracionServicio;
            _factura = new FacturaView();
            _articuloServicio = articuloServicio;
            _listaPrecioServicio = listaPrecioServicio;
            _facturaServicio = facturaServicio;
            _contadorServicio = contadorServicio;
            _personaServicio = personaServicio;
            _proveedorSeleccionado = null;
            _permiteAgregarPorCantidad = false;
            _ingresoPorCodigoBascula = false;
            _articuloConPrecioAlternativo = false;
            _configuracion = _configuracionServicio.Obtener();
            if (_configuracionServicio == null)
            {
                MessageBox.Show("Configure el sistema antes de iniciar.");
                var configuracion = ObjectFactory.GetInstance<_00012_Configuracion>();
                configuracion.ShowDialog();
                _configuracionServicio = configuracionServicio;

                if (_configuracionServicio == null)
                {
                    MessageBox.Show("No encontramos ninguna configuracion.");
                    Close();
                }
            }
        }

        private void btnBuscarCliente_Click(object sender, System.EventArgs e)
        {
            var lookUpProveedor = ObjectFactory.GetInstance<ProveedorLookUp>();
            lookUpProveedor.ShowDialog();

            if (lookUpProveedor.EntidadSeleccionada != null)
            {
                _proveedorSeleccionado = ((ProveedorDTO)lookUpProveedor.EntidadSeleccionada);
                AsignarDatosProveedor((ProveedorDTO)lookUpProveedor.EntidadSeleccionada);
            }
            else
            {
                AsignarDatosProveedor(_proveedorSeleccionado);
            }
        }

        private void AsignarDatosProveedor(ProveedorDTO proveedor)
        {
            txtCuit.Text = proveedor.CUIT;
            txtNombre.Text = proveedor.RazonSocial;
            txtDomicilio.Text = proveedor.Direccion;
            txtCondicionIva.Text = proveedor.CondicionIva;
            txtTelefono.Text = proveedor.Telefono;
        }

        private void txtCodigo_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtCodigo.Text))
            {
                if (!string.IsNullOrEmpty(txtCodigo.Text))
                {
                    if (e.KeyChar == (char)Keys.Enter)
                    {
                        _articuloSeleccionado = _articuloServicio.ObtenerPorCodigo(txtCodigo.Text, 1, _configuracion.DepositoVentaId, true);

                        if (_articuloSeleccionado != null)
                        {
                            txtDescripcion.Text = _articuloSeleccionado.Descripcion;
                            nudPrecioUnitario.Value = _articuloSeleccionado.Precio;
                            CalcularSubtotal();
                            nudCantidad.Focus();

                            if (_permiteAgregarPorCantidad)
                            {
                                txtCodigo.Text = _articuloSeleccionado.CodigoBarra;
                                txtDescripcion.Text = _articuloSeleccionado.Descripcion;
                                nudPrecioUnitario.Value = _articuloSeleccionado.Precio;
                                nudCantidad.Focus();
                                nudCantidad.Select(0, nudCantidad.Text.Length);
                                return;
                            }
                            else
                            {
                                //btnAgregar.PerformClick();
                            }
                        }
                        else
                        {
                            MessageBox.Show("El codigo ingresado no existe");
                            LimpiarParaNuevoItem();
                        }
                    }
                }

                e.Handled = false;
            }
        }

        private void CalcularSubtotal()
        {
            txtSubTotalLinea.Text = (nudPrecioUnitario.Value * nudCantidad.Value).ToString("C");
        }

        private bool AsignarArticuloAlternativo(string codigo)
        {
            _articuloConPrecioAlternativo = true;
            var codigoArticulo = codigo.Substring(0, codigo.IndexOf('*'));
            if (!string.IsNullOrEmpty(codigoArticulo))
            {
                _articuloSeleccionado = _articuloServicio.ObtenerPorCodigo(codigoArticulo, 1, _configuracion.DepositoVentaId);

                if (_articuloSeleccionado != null)
                {
                    var precioAlternativo = codigo.Substring(codigo.IndexOf('*') + 1);

                    if (!string.IsNullOrEmpty(precioAlternativo))
                    {
                        if (decimal.TryParse(precioAlternativo, out decimal _precio))
                        {
                            _articuloSeleccionado.Precio = _precio;
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }
            return false;
        }

        private bool AsignarArticuloPorBascula(string codigoBascula)
        {
            decimal _precioBascula = 0;
            decimal _pesoBascula = 0;

            _ingresoPorCodigoBascula = true;

            int.TryParse(codigoBascula.Substring(4, 3), out int codigoArticulo);

            var precioPesoArticulo = codigoBascula.Substring(7, 5);

            if (_configuracion.EtiquetasPorPrecio)
            {
                if (!decimal.TryParse(precioPesoArticulo.Insert(3, ","), NumberStyles.Number, new CultureInfo("es-Ar"), out _precioBascula))
                {
                    return false;
                }
            }
            else
            {
                if (!decimal.TryParse(precioPesoArticulo.Insert(2, ","), NumberStyles.Number, new CultureInfo("es-Ar"), out _pesoBascula))
                {
                    return false;
                }
            }

            _articuloSeleccionado = _articuloServicio.ObtenerPorCodigo(codigoArticulo.ToString(), 1, _configuracion.DepositoVentaId);

            if (_articuloSeleccionado != null)
            {
                if (_configuracion.EtiquetasPorPrecio)
                {
                    _articuloSeleccionado.Precio = _precioBascula;
                }
                else
                {
                    nudCantidad.Value = _pesoBascula;
                }
            }

            return false;
        }

        private void LimpiarParaNuevoItem()
        {
            txtCodigo.Clear();
            txtDescripcion.Clear();
            nudPrecioUnitario.Value = 1;
            nudCantidad.Value = 1;
            _articuloSeleccionado = null;
            txtCodigo.Focus();
        }

        private void btnAgregar_Click(object sender, System.EventArgs e)
        {
            if (_articuloSeleccionado != null) AgregarItem(_articuloSeleccionado, 1, nudCantidad.Value);
            LimpiarParaNuevoItem();
            CargarCuerpo();
            CargarPie();
        }

        private void AgregarItem(ArticuloVentaDto articulo, long listaPrecioId, decimal cantidad)
        {
            var item = _factura.Items.FirstOrDefault(x =>
                        x.ArticuloId == articulo.Id && x.ListaPrecioId == 1);

            if (item == null)
            {
                _factura.Items.Add(AsignarDatosItem(articulo, 1, cantidad));
            }
            else
            {
                item.Cantidad += cantidad;
            }
        }

        private ItemView AsignarDatosItem(ArticuloVentaDto articulo, long listaPrecioId, decimal cantidad)
        {
            _factura.ContadorItem++;

            return new ItemView
            {
                Id = _factura.ContadorItem,
                Descripcion = articulo.Descripcion,
                Iva = articulo.Iva,
                Precio = articulo.Precio,
                CodigoBarra = articulo.CodigoBarra,
                Cantidad = cantidad,
                ListaPrecioId = listaPrecioId,
                ArticuloId = articulo.Id,
                EsArticuloAlternativo = _articuloConPrecioAlternativo,
                IngresoPorBascula = _ingresoPorCodigoBascula
            };
        }

        private void CargarCuerpo()
        {
            dgvGrilla.DataSource = _factura.Items.ToList();
            FormatearGrilla(dgvGrilla);

            if (_factura.Items.Any())
            {
                var ultimoItem = _factura.Items.Last();
            }
        }

        private void CargarPie()
        {
            nudTotal.Value = _factura.Total;
        }

        private void dgvGrilla_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvGrilla.RowCount > 0)
            {
                _itemSeleccionado = (ItemView)dgvGrilla.Rows[e.RowIndex].DataBoundItem;
            }
            else
            {
                _itemSeleccionado = null;
            }
        }

        public override void FormatearGrilla(DataGridView dgv)
        {
            base.FormatearGrilla(dgv);

            dgv.Columns["CodigoBarra"].Visible = true;
            dgv.Columns["CodigoBarra"].Width = 100;
            dgv.Columns["CodigoBarra"].HeaderText = "Código";
            dgv.Columns["CodigoBarra"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            dgv.Columns["Descripcion"].Visible = true;
            dgv.Columns["Descripcion"].HeaderText = "Articulo";
            dgv.Columns["Descripcion"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            dgv.Columns["PrecioStr"].Visible = true;
            dgv.Columns["PrecioStr"].Width = 120;
            dgv.Columns["PrecioStr"].HeaderText = "Precio";
            dgv.Columns["PrecioStr"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            dgv.Columns["Cantidad"].Visible = true;
            dgv.Columns["Cantidad"].Width = 120;
            dgv.Columns["Cantidad"].HeaderText = "Cantidad";
            dgv.Columns["Cantidad"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgv.Columns["SubTotalStr"].Visible = true;
            dgv.Columns["SubTotalStr"].Width = 120;
            dgv.Columns["SubTotalStr"].HeaderText = "Sub-Total";
            dgv.Columns["SubTotalStr"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        }

        private void txtCodigo_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyValue)
            {
                //al presionar F8 -> Buscar articulo
                case 119:
                    var lookUpArticulo = new ArticuloLookUp(1);
                    lookUpArticulo.ShowDialog();

                    if (lookUpArticulo.EntidadSeleccionada != null)
                    {
                        _articuloSeleccionado = (ArticuloVentaDto)lookUpArticulo.EntidadSeleccionada;

                        txtCodigo.Text = _articuloSeleccionado.CodigoBarra;


                        _articuloSeleccionado = _articuloServicio.ObtenerPorCodigo(txtCodigo.Text, 1, _configuracion.DepositoVentaId, true);

                        if (_articuloSeleccionado != null)
                        {
                            txtDescripcion.Text = _articuloSeleccionado.Descripcion;
                            nudPrecioUnitario.Value = _articuloSeleccionado.Precio;
                            CalcularSubtotal();
                            nudCantidad.Focus();
                        }
                    }
                    else
                    {
                        LimpiarParaNuevoItem();
                    }

                    break;
            }
        }

        private void nudCantidad_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                btnAgregar.Focus();
            }
        }

        private void btnComprar_Click(object sender, System.EventArgs e)
        {

            try
            {
                var _facturaNueva = new FacturaCompraDto();
                _facturaNueva.EmpleadoId = _vendedorSeleccionado.Id;
                _facturaNueva.ClienteId = _proveedorSeleccionado.Id;
                _facturaNueva.TipoComprobante = TipoComprobante.Remito;
                _facturaNueva.Descuento = 0;
                _facturaNueva.SubTotal = _factura.SubTotal;
                _facturaNueva.Total = _factura.Total;
                _facturaNueva.Estado = Estado.Pagada;
                _facturaNueva.PuestoTrabajoId = _factura.PuntoVentaId;
                _facturaNueva.Fecha = DateTime.Now;
                _facturaNueva.Iva27 = nud27.Value;
                _facturaNueva.Iva105 = nud105.Value;
                _facturaNueva.Iva21 = nud21.Value;
                _facturaNueva.ImpInterno = nudImpuestoInterno.Value;
                _facturaNueva.PercTemp = nudPercepcionTemp.Value;
                _facturaNueva.PercPyP = nudPercepcionPyP.Value;
                _facturaNueva.PercIva = nudPercepcionIva.Value;
                _facturaNueva.PercIB = nudPercepcionIB.Value;
                _facturaNueva.UsuarioId = _factura.UsuarioId;

                foreach (var item in _factura.Items)
                {
                    _facturaNueva.Items.Add(new DetalleComprobanteDto
                    {
                        Cantidad = item.Cantidad,
                        Descripcion = item.Descripcion,
                        Precio = item.Precio,
                        ArticuloId = item.ArticuloId,
                        Codigo = item.CodigoBarra,
                        SubTotal = item.SubTotal,
                        Eliminado = false,
                    });
                }

                //var deuda = _cuentaCorrienteServicio.ObtenerDeudaCliente(_factura.Cliente.Id);

                _facturaNueva.FormasDePagos.Add(new FormaPagoCtaCteDto
                {
                    TipoPago = TipoPago.CtaCte,
                    ClienteId = _proveedorSeleccionado.Id,
                    Monto = nudTotal.Value,
                    Eliminado = false,
                });

                
                _facturaServicio.Insertar(_facturaNueva);
                MessageBox.Show("Los datos se grabaron correctamente");
                Close();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }

            _factura = new FacturaView();

            CargarCuerpo();
            CargarPie();
            txtCodigo.Focus();

            chk27.Checked = false;
            chk21.Checked = false;
            chk105.Checked = false;
            chkImpuestoInterno.Checked = false;
            chkPercepcionTemp.Checked = false;
            chkPercepcionPyP.Checked = false;
            chkPercepcionIva.Checked = false;
            chkPercepcionIB.Checked = false;
        }
        private void nudCantidad_ValueChanged(object sender, System.EventArgs e)
        {
            CalcularSubtotal();
        }

        private void chk27_CheckedChanged_1(object sender, EventArgs e)
        {
            if (((CheckBox)sender).Checked == false)
            {
                if (((CheckBox)sender).Text == "Iva 27 %") 
                {
                    nud27.Value = 0m;
                    nud27.Enabled = false;
                }
                if (((CheckBox)sender).Text == "Iva 21 %") 
                {
                    nud21.Value = 0m;
                    nud21.Enabled = false;
                }
                if (((CheckBox)sender).Text == "Iva 10.5 %")
                {
                    nud105.Value = 0m;
                    nud105.Enabled = false;
                }
                if (((CheckBox)sender).Text == "Imp. Interno")
                {
                    nudImpuestoInterno.Value = 0m;
                    nudImpuestoInterno.Enabled = false;
                }
                if (((CheckBox)sender).Text == "Percepción Temp.")
                {
                    nudPercepcionTemp.Value = 0m;
                    nudPercepcionTemp.Enabled = false;
                }
                if (((CheckBox)sender).Text == "Percepción P y P")
                {
                    nudPercepcionPyP.Value = 0m;
                    nudPercepcionPyP.Enabled = false;
                }
                if (((CheckBox)sender).Text == "Percepción Iva")
                {
                    nudPercepcionIva.Value = 0m;
                    nudPercepcionIva.Enabled = false;
                }
                if (((CheckBox)sender).Text == "Percepción IB")
                {
                    nudPercepcionIB.Value = 0m;
                    nudPercepcionIB.Enabled = false;
                }
            }
            else
            {
                if (((CheckBox)sender).Text == "Iva 27 %")
                {
                    nud27.Enabled = true;
                }
                if (((CheckBox)sender).Text == "Iva 21 %")
                {
                    nud21.Enabled = true;
                }
                if (((CheckBox)sender).Text == "Iva 10.5 %")
                {
                    nud105.Enabled = true;
                }
                if (((CheckBox)sender).Text == "Imp. Interno")
                {
                    nudImpuestoInterno.Enabled = true;
                }
                if (((CheckBox)sender).Text == "Percepción Temp.")
                {
                    nudPercepcionTemp.Enabled = true;
                }
                if (((CheckBox)sender).Text == "Percepción P y P")
                {
                    nudPercepcionPyP.Enabled = true;
                }
                if (((CheckBox)sender).Text == "Percepción Iva")
                {
                    nudPercepcionIva.Enabled = true;
                }
                if (((CheckBox)sender).Text == "Percepción IB")
                {
                    nudPercepcionIB.Enabled = true;
                }
            }
        }

        private void _00053_Compra_Load(object sender, EventArgs e)
        {
            _vendedorSeleccionado = (EmpleadoDto)_personaServicio.Obtener(typeof(EmpleadoDto), Identidad.EmpleadoId);
        }
    }
}
