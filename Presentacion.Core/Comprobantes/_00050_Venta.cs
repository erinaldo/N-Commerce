using Aplicacion.Constantes;
using IServicios.Articulo;
using IServicios.Articulo.DTOs;
using IServicios.Configuracion;
using IServicios.Configuracion.DTOs;
using IServicios.ListaPrecio;
using IServicios.ListaPrecio.DTOs;
using IServicios.Persona;
using IServicios.Persona.DTOs;
using IServicios.PuestoTrabajo;
using Presentacion.Core.Articulo;
using Presentacion.Core.Cliente;
using Presentacion.Core.Comprobantes.Clases;
using Presentacion.Core.Configuracion;
using Presentacion.Core.Empleado;
using Presentacion.Core.FormaPago;
using PresentacionBase.Formularios;
using StructureMap;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using IServicios.Comprobante;
using IServicios.Comprobante.DTOs;
using IServicios.Contador;
using static Aplicacion.Constantes.Cliente;

namespace Presentacion.Core.Comprobantes
{
    public partial class _00050_Venta : FormBase
    {
        private FacturaView _factura;
        private ClienteDto _clienteSeleccionado;
        private ConfiguracionDto _configuracion;
        private EmpleadoDto _vendedorSeleccionado;
        private ArticuloVentaDto _articuloSeleccionado;
        private bool _ingresoPorCodigoBascula;
        private bool _permiteAgregarPorCantidad;
        private bool _autorizaPermisoListaPrecio;
        private bool _articuloConPrecioAlternativo;
        private bool _cambiarCantidadErrorValidacion;
        private ItemView _itemSeleccionado;

        private readonly IClienteServicio _clienteServicio;
        private readonly IPersonaServicio _personaServicio;
        private readonly IPuestoTrabajoServicio _puestoTrabajoServicio;
        private readonly IConfiguracionServicio _configuracionServicio;
        private readonly IArticuloServicio _articuloServicio;
        private readonly IEmpleadoServicio _empleadoServicio;
        private readonly IListaPrecioServicio _listaPrecioServicio;
        private readonly IFacturaServicio _facturaServicio;
        private readonly IContadorServicio _contadorServicio;

        public _00050_Venta(IClienteServicio clienteServicio, IPersonaServicio personaServicio, IPuestoTrabajoServicio puestoTrabajoServicio, IConfiguracionServicio configuracionServicio, IArticuloServicio articuloServicio, IListaPrecioServicio listaPrecioServicio, IFacturaServicio facturaServicio, IContadorServicio contadorServicio)
        {
            InitializeComponent();

            _puestoTrabajoServicio = puestoTrabajoServicio;
            _personaServicio = personaServicio;
            _configuracionServicio = configuracionServicio;
            _factura = new FacturaView();
            _articuloServicio = articuloServicio;
            _listaPrecioServicio = listaPrecioServicio;
            _facturaServicio = facturaServicio;
            _contadorServicio = contadorServicio;

            _clienteSeleccionado = null;
            _articuloConPrecioAlternativo = false;
            _vendedorSeleccionado = null;

            _permiteAgregarPorCantidad = false;
            _autorizaPermisoListaPrecio = false;
            _ingresoPorCodigoBascula = false;
            _cambiarCantidadErrorValidacion = false;

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

        private void _00050_Venta_Load(object sender, System.EventArgs e)
        {
            CargarCabecera();
            CargarCuerpo();
            CargarPie();
        }

        private void CargarCabecera()
        {
            _clienteSeleccionado = ObtenerClienteConsumidorFinal();
            AsignarDatosCliente(_clienteSeleccionado);

            ////////////////////////////////////////////////////////////////////////////////////////////

            lblFechaActual.Text = DateTime.Today.ToShortDateString();

            ////////////////////////////////////////////////////////////////////////////////////////////

            PoblarComboBox(cmbTipoComprobante, Enum.GetValues(typeof(TipoComprobante)));
            cmbTipoComprobante.SelectedItem = TipoComprobante.B;

            ////////////////////////////////////////////////////////////////////////////////////////////

            PoblarComboBox(cmbPuestoVenta, _puestoTrabajoServicio.Obtener(string.Empty), "Descripcion", "Id");

            if (cmbPuestoVenta.Items.Count == 0)
            {
                MessageBox.Show("Por favor Cargue primeramente los puntos de Ventas");
                Close();
            }

            ActualizarTitulo();

            ////////////////////////////////////////////////////////////////////////////////////////////

            PoblarComboBox(cmbListaPrecio,
                _listaPrecioServicio.Obtener(string.Empty, false),
                "Descripcion",
                "Id");

            if (cmbListaPrecio.Items.Count == 0)
            {
                MessageBox.Show("Por favor Cargue primeramente las listas de Precio");
                Close();
            }

            cmbListaPrecio.SelectedValue = _configuracion.ListaPrecioPorDefectoId;

            ////////////////////////////////////////////////////////////////////////////////////////////

            _vendedorSeleccionado = ObtenerVendedorPorDefecto();
            AsignarDatosVendedor(_vendedorSeleccionado);
        }

        private void CargarCuerpo()
        {
            dgvGrilla.DataSource = _factura.Items.ToList();
            FormatearGrilla(dgvGrilla);

            if (_factura.Items.Any())
            {
                var ultimoItem = _factura.Items.Last();

                lblDescripcion.Text = ultimoItem.Descripcion.ToUpper();
                lblPrecioPorCantidad.Text = $"{ultimoItem.Cantidad} X {ultimoItem.PrecioStr} = {ultimoItem.SubTotalStr}";
            }
            else
            {
                lblDescripcion.Text = string.Empty;
                lblPrecioPorCantidad.Text = string.Empty;
            }
        }

        private void CargarPie()
        {
            txtSubTotal.Text = _factura.SubTotalStr;
            nudDescuento.Value = _factura.Descuento;
            txtTotal.Text = _factura.TotalStr;
        }

        private ClienteDto ObtenerClienteConsumidorFinal()
        {
            var clientes = (List<ClienteDto>)_personaServicio.Obtener(typeof(ClienteDto), Aplicacion.Constantes.Cliente.ConsumidorFinal);

            return clientes.FirstOrDefault();
        }

        private void AsignarDatosCliente(ClienteDto cliente)
        {
            txtDniCliente.Text = cliente.Dni;
            txtCliente.Text = cliente.ApyNom;
            txtDomicilioCliente.Text = cliente.Direccion;
            txtCondicionIvaCliente.Text = cliente.CondicionIva;
            txtTelefonoCliente.Text = cliente.Telefono;
        }

        private void btnBuscarCliente_Click(object sender, EventArgs e)
        {
            var lookUpCliente = ObjectFactory.GetInstance<ClienteLookUp>();
            lookUpCliente.ShowDialog();

            if (lookUpCliente.EntidadSeleccionada != null)
            {
                _clienteSeleccionado = ((ClienteDto)lookUpCliente.EntidadSeleccionada);
                AsignarDatosCliente((ClienteDto)lookUpCliente.EntidadSeleccionada);
            }
            else
            {
                _clienteSeleccionado = ObtenerClienteConsumidorFinal();
                AsignarDatosCliente(_clienteSeleccionado);
            }
        }

        private void cmbPuestoVenta_SelectionChangeCommitted(object sender, EventArgs e)
        {
            ActualizarTitulo();
        }

        private void ActualizarTitulo()
        {
            this.Text = $"TPV - {cmbPuestoVenta.Text}";
        }

        private void btnBuscarVendedor_Click(object sender, EventArgs e)
        {
            var lookUpVendedor = ObjectFactory.GetInstance<EmpleadoLookUp>();
            lookUpVendedor.ShowDialog();

            if (lookUpVendedor.EntidadSeleccionada != null)
            {
                _vendedorSeleccionado = (EmpleadoDto)lookUpVendedor.EntidadSeleccionada;
                AsignarDatosVendedor(_vendedorSeleccionado);
            }
            else
            {
                _vendedorSeleccionado = ObtenerVendedorPorDefecto();
                AsignarDatosVendedor(_vendedorSeleccionado);
            }
        }

        private EmpleadoDto ObtenerVendedorPorDefecto()
        {
            return (EmpleadoDto)_personaServicio.Obtener(typeof(EmpleadoDto), Identidad.EmpleadoId);
        }

        private void AsignarDatosVendedor(EmpleadoDto vendedorSeleccionado)
        {
            txtVendedor.Text = vendedorSeleccionado.ApyNom;
        }

        private void nudDescuento_ValueChanged(object sender, EventArgs e)
        {
            _factura.Descuento = nudDescuento.Value;
            CargarPie();
        }

        private void txtCodigo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtCodigo.Text))
            {
                if (!string.IsNullOrEmpty(txtCodigo.Text))
                {
                    if (e.KeyChar == (char)Keys.Enter)
                    {
                        if (txtCodigo.Text.Contains("*"))
                        {
                            if (AsignarArticuloAlternativo(txtCodigo.Text))
                            {
                                btnAgregar.PerformClick();
                                return;
                            }
                        }

                        if (txtCodigo.Text.Length == 13)
                        {
                            if (_configuracion.ActivarBascula && _configuracion.CodigoBascula == int.Parse(txtCodigo.Text.Substring(0, 4)))
                            {
                                if (AsignarArticuloPorBascula(txtCodigo.Text))
                                {
                                    btnAgregar.PerformClick();
                                    return;
                                }
                            }
                            else
                            {
                                _articuloSeleccionado = _articuloServicio.ObtenerPorCodigo(txtCodigo.Text, (long)cmbListaPrecio.SelectedValue, _configuracion.DepositoVentaId);
                            }
                        }
                        else
                        {
                            _articuloSeleccionado = _articuloServicio.ObtenerPorCodigo(txtCodigo.Text, (long)cmbListaPrecio.SelectedValue, _configuracion.DepositoVentaId);
                        }

                        if (_articuloSeleccionado != null)
                        {
                            if (_permiteAgregarPorCantidad)
                            {
                                txtCodigo.Text = _articuloSeleccionado.CodigoBarra;
                                txtDescripcion.Text = _articuloSeleccionado.Descripcion;
                                txtPrecioUnitario.Text = _articuloSeleccionado.PrecioStr;
                                nudCantidad.Focus();
                                nudCantidad.Select(0, nudCantidad.Text.Length);
                                return;
                            }
                            else
                            {
                                btnAgregar.PerformClick();
                            }
                        }
                        else
                        {
                            LimpiarParaNuevoItem();
                        }
                    }
                }

                e.Handled = false;
            }
        }

        private void LimpiarParaNuevoItem()
        {
            txtCodigo.Clear();
            txtDescripcion.Clear();
            txtPrecioUnitario.Clear();
            nudCantidad.Value = 1;
            nudCantidad.Enabled = false;
            _permiteAgregarPorCantidad = false;
            _articuloConPrecioAlternativo = false;
            _ingresoPorCodigoBascula = false;
            _articuloSeleccionado = null;
            txtCodigo.Focus();
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

            _articuloSeleccionado = _articuloServicio.ObtenerPorCodigo(codigoArticulo.ToString(), (long)cmbListaPrecio.SelectedValue, _configuracion.DepositoVentaId);

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

        private bool AsignarArticuloAlternativo(string codigo)
        {
            _articuloConPrecioAlternativo = true;
            var codigoArticulo = codigo.Substring(0, codigo.IndexOf('*'));
            if (!string.IsNullOrEmpty(codigoArticulo))
            {
                _articuloSeleccionado = _articuloServicio.ObtenerPorCodigo(codigoArticulo, (long)cmbListaPrecio.SelectedValue, _configuracion.DepositoVentaId);

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

        private void txtCodigo_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyValue)
            {
                //al presionar F4 -> Cobrar
                case 115:
                    btnCobrar.PerformClick();
                    break;
                //al presionar F5 -> Agregar por cantidad
                case 116:
                    _permiteAgregarPorCantidad = !_permiteAgregarPorCantidad;
                    nudCantidad.Enabled = _permiteAgregarPorCantidad;
                    break;
                //al presionar F6 -> Eliminar articulo
                case 117:
                    btnEliminarItem.PerformClick();
                    break;
                //al presionar F7 -> Cambiar cantidad
                case 118:
                    btnCambiarCantidad.PerformClick();
                    break;
                //al presionar F8 -> Buscar articulo
                case 119:
                    var lookUpArticulo = new ArticuloLookUp((long)cmbListaPrecio.SelectedValue);
                    lookUpArticulo.ShowDialog();

                    if (lookUpArticulo.EntidadSeleccionada != null)
                    {
                        _articuloSeleccionado = (ArticuloVentaDto)lookUpArticulo.EntidadSeleccionada;

                        if (_permiteAgregarPorCantidad)
                        {
                            txtCodigo.Text = _articuloSeleccionado.CodigoBarra;
                            txtDescripcion.Text = _articuloSeleccionado.Descripcion;
                            txtPrecioUnitario.Text = _articuloSeleccionado.PrecioStr;
                            nudCantidad.Focus();
                            nudCantidad.Select(0, nudCantidad.Text.Length);
                            return;
                        }
                        else
                        {
                            btnAgregar.PerformClick();
                            LimpiarParaNuevoItem();
                        }
                    }
                    else
                    {
                        LimpiarParaNuevoItem();
                    }

                    break;
            }
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            if (_articuloSeleccionado != null)
            {
                var listaPrecioSeleccionada = (ListaPrecioDto)cmbListaPrecio.SelectedItem;

                if (listaPrecioSeleccionada.NecesitaAutorizacion)
                {
                    if (!_autorizaPermisoListaPrecio)
                    {
                        var fAutorizacion = ObjectFactory.GetInstance<AutorizacionListaPrecio>();
                        fAutorizacion.ShowDialog();

                        if (!fAutorizacion.PermisoAutorizado) return;

                        _autorizaPermisoListaPrecio = fAutorizacion.PermisoAutorizado;
                        AgregarItem(_articuloSeleccionado, (long)cmbListaPrecio.SelectedValue, nudCantidad.Value);
                    }
                    else
                    {
                        AgregarItem(_articuloSeleccionado, (long)cmbListaPrecio.SelectedValue, nudCantidad.Value);
                    }
                }
                else
                {
                    AgregarItem(_articuloSeleccionado, (long)cmbListaPrecio.SelectedValue, nudCantidad.Value);
                }
            }

            LimpiarParaNuevoItem();
            CargarCuerpo();
            CargarPie();
        }

        private void AgregarItem(ArticuloVentaDto articulo, long listaPrecioId, decimal cantidad)
        {
            //Limite de Venta por cantidad
            if (articulo.TieneRestriccionPorCantidad)
            {
                var totalArticulosItems = _factura.Items.Where(x => x.ArticuloId == articulo.Id).Sum(x => x.Cantidad);

                if (cantidad + totalArticulosItems > articulo.Limite)
                {
                    _cambiarCantidadErrorValidacion = true;

                    var mensajeLimiteVenta = $"El articulo {articulo.Descripcion.ToUpper()} tiene una restricción" + Environment.NewLine + $"de Venta por una Cantidad Maxima de {articulo.Limite}.";

                    MessageBox.Show(mensajeLimiteVenta, "Atención", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }
            }
            if (articulo.TieneRestriccionHorario)
            {
                if (VerificarLimiteHorarioVenta(articulo.HoraDesde, articulo.HoraHasta))
                {
                    _cambiarCantidadErrorValidacion = true;

                    var mensajeLimiteHorario = $"El articulo {articulo.Descripcion.ToUpper()} tiene una restricción" +
                                               Environment.NewLine +
                                               $"de Venta por horario entre {articulo.HoraDesde.ToShortTimeString()} hasta {articulo.HoraHasta.ToShortTimeString()}.";

                    MessageBox.Show(mensajeLimiteHorario, "Atención", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }
            }

            if (!articulo.PermiteStockNegativo)
            {
                if (!VerificarStock(articulo, nudCantidad.Value))
                {
                    _cambiarCantidadErrorValidacion = true;

                    var mensajeStock = $"No hay Stock suficiente para el articulo {articulo.Descripcion.ToUpper()}" +
                                       Environment.NewLine + $"Stock Actual disponible: {articulo.Stock}.";

                    MessageBox.Show(mensajeStock, "Atención", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }
            }

            if (_articuloConPrecioAlternativo || _ingresoPorCodigoBascula)
            {
                _factura.Items.Add(AsignarDatosItem(articulo, listaPrecioId, cantidad));
            }
            else
            {
                if (_configuracion.UnificarRenglonesIngresarMismoProducto)
                {
                    var item = _factura.Items.FirstOrDefault(x =>
                        x.ArticuloId == articulo.Id && x.ListaPrecioId == listaPrecioId &&
                        (!x.EsArticuloAlternativo && !x.IngresoPorBascula));

                    if (item == null || item.EsArticuloAlternativo || item.IngresoPorBascula) // Primera vez por ingresar
                    {
                        _factura.Items.Add(AsignarDatosItem(articulo, listaPrecioId, cantidad));
                    }
                    else
                    {
                        item.Cantidad += cantidad;
                    }
                }
                else
                {
                    _factura.Items.Add(AsignarDatosItem(articulo, listaPrecioId, cantidad));
                }
            }
        }

        private bool VerificarStock(ArticuloVentaDto articulo, decimal cantidad)
        {
            var totalArticulosItems = _factura.Items.Where(x => x.ArticuloId == articulo.Id).Sum(x => x.Cantidad);
            return totalArticulosItems + cantidad <= articulo.Stock;
        }

        private bool VerificarLimiteHorarioVenta(DateTime limiteHoraDesde, DateTime limiteHoraHasta)
        {
            var _horaDesdeSistena = DateTime.Now.Hour;
            var _minutoDesdeSistema = DateTime.Now.Minute;

            var _horaDesdeInicioDia = 0;
            var _minutoDesdeInicioDia = 0;

            var _horaDesdeFinDia = 23;
            var _minutoDesdeFinDia = 59;


            if (limiteHoraDesde <= limiteHoraHasta)
            {
                if (_horaDesdeSistena >= limiteHoraDesde.Hour && _minutoDesdeSistema >= limiteHoraDesde.Minute)
                {
                    if (_horaDesdeSistena < limiteHoraHasta.Hour)
                    {
                        return true;
                    }
                    else if (_horaDesdeSistena == limiteHoraHasta.Hour && _minutoDesdeSistema <= limiteHoraHasta.Minute)
                    {
                        return true;
                    }
                }
            }
            else // Dias Diferentes -> Ej: 11:00 PM hasta 06:00 AM
            {
                if (_horaDesdeSistena >= limiteHoraDesde.Hour)
                {
                    //primer rango
                    return _horaDesdeSistena >= limiteHoraDesde.Hour
                           && _minutoDesdeSistema >= limiteHoraDesde.Minute
                           && _horaDesdeSistena <= _horaDesdeFinDia
                           && _minutoDesdeSistema <= _minutoDesdeFinDia;
                }
                else
                {
                    //segundo rango
                    if (_horaDesdeSistena >= _horaDesdeInicioDia && _minutoDesdeSistema >= _minutoDesdeInicioDia)
                    {
                        if (_horaDesdeSistena < limiteHoraHasta.Hour)
                        {
                            return true;
                        }
                        else if (_horaDesdeSistena == limiteHoraHasta.Hour &&
                                 _minutoDesdeSistema <= limiteHoraHasta.Minute)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
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

        private void nudCantidad_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                btnAgregar.PerformClick();
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

            dgv.Columns["Iva"].Visible = true;
            dgv.Columns["Iva"].Width = 100;
            dgv.Columns["Iva"].HeaderText = "Iva";
            dgv.Columns["Iva"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

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

        private void btnEliminarItem_Click(object sender, EventArgs e)
        {
            if (dgvGrilla.RowCount <= 0) return;

            if (_itemSeleccionado != null)
            {
                var mensajeBorrar = $"Esta seguro de eliminar el articulo {_itemSeleccionado.Descripcion}";

                if (MessageBox.Show(mensajeBorrar, "Atencion", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    _factura.Items.Remove(_itemSeleccionado);

                    CargarCuerpo();
                    CargarPie();
                    LimpiarParaNuevoItem();
                }
            }
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

        private void btnCambiarCantidad_Click(object sender, EventArgs e)
        {
            if (_itemSeleccionado == null) return;

            var respaldoItemSeleccionado = _itemSeleccionado;
            var respaldoCantidad = _itemSeleccionado.Cantidad;

            var formCambiarCantidad = new CambiarCantidad(_itemSeleccionado);
            formCambiarCantidad.ShowDialog();

            if (formCambiarCantidad.Item != null)
            {
            //    var item = _factura.Items.FirstOrDefault(x => x.Id == formCambiarCantidad.Item.Id);

            //    item.Cantidad = formCambiarCantidad.Item.Cantidad;

            //    CargarCuerpo();
            //    CargarPie();
            //    LimpiarParaNuevoItem();
            }

            if (formCambiarCantidad.Item != null)
            {
                var item = _factura.Items.FirstOrDefault(x => x.Id == formCambiarCantidad.Item.Id);

                _factura.Items.Remove(item);

                if (formCambiarCantidad.Item.Cantidad > 0)
                {
                    _articuloSeleccionado = _articuloServicio.ObtenerPorCodigo(_itemSeleccionado.CodigoBarra, _itemSeleccionado.ListaPrecioId, _configuracion.DepositoVentaId);

                    nudCantidad.Value = formCambiarCantidad.Item.Cantidad;

                    btnAgregar.PerformClick();

                    if (_cambiarCantidadErrorValidacion)
                    {
                        respaldoItemSeleccionado.Cantidad = respaldoCantidad;
                        _factura.Items.Add(respaldoItemSeleccionado);
                        _cambiarCantidadErrorValidacion = false;
                    }
                }                               
            }

            CargarCuerpo();
            CargarPie();
            LimpiarParaNuevoItem();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            if (dgvGrilla.RowCount > 0)
            {
                if (MessageBox.Show("¿Desea eliminar los articulos cargados?", "Atencion", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    _factura = new FacturaView();//al volver a generarlo se borra el anterior.

                    CargarCabecera();
                    CargarCuerpo();
                    CargarPie();
                }
            }
        }

        private void btnCobrar_Click(object sender, EventArgs e)
        {
            if (dgvGrilla.RowCount >= 0)
            {
                _factura.Cliente = _clienteSeleccionado;
                _factura.Vendedor = _vendedorSeleccionado;
                _factura.TipoComprobante = (TipoComprobante) cmbTipoComprobante.SelectedItem;
                _factura.PuntoVentaId = (long)cmbPuestoVenta.SelectedValue;
                _factura.UsuarioId = Identidad.UsuarioId;

                if (_configuracion.PuestoCajaSeparado)
                {
                    try
                    {
                        var nuevocomprobante = new FacturaDto()
                        {
                            EmpleadoId = _factura.Vendedor.Id,
                            TipoComprobante = _factura.TipoComprobante,
                            Fecha = DateTime.Now,
                            Descuento = _factura.Descuento,
                            SubTotal = _factura.SubTotal,
                            Iva105 = 0,
                            Iva21 = 0,
                            Total = _factura.Total,
                            UsuarioId = _factura.UsuarioId,
                            ClienteId = _factura.Cliente.Id,
                            Estado = Estado.Pendiente,
                            PuestoTrabajoId = _factura.PuntoVentaId,
                            VieneVentas = true,
                            Eliminado = false,
                        };

                        foreach (var item in _factura.Items)
                        {
                            nuevocomprobante.Items.Add( new DetalleComprobanteDto
                            {
                                Cantidad = item.Cantidad,
                                Precio = item.Precio,
                                Descripcion = item.Descripcion,
                                SubTotal = item.SubTotal,
                                Iva = item.Iva,
                                ArticuloId = item.ArticuloId,
                                Codigo = item.CodigoBarra,
                                Eliminado = false
                            });
                        }

                        _facturaServicio.Insertar(nuevocomprobante);

                        MessageBox.Show("Los datos se cargaron correctamente.");
                        LimpiarParaNuevaFactura();
                    }
                    catch (Exception exception)
                    {
                        MessageBox.Show(exception.Message);
                    }
                }
                else
                {
                    //puesto de venta y caja unificado.
                    var formFormaPago = new _00044_FormaPago(_factura);
                    formFormaPago.ShowDialog();
                    if (formFormaPago.RealizoVenta)
                    {
                        LimpiarParaNuevaFactura();
                        txtCodigo.Focus();
                    }
                }
            }
        }

        private void LimpiarParaNuevaFactura()
        {
            _factura = new FacturaView();

            CargarCabecera();
            CargarCuerpo();
            CargarPie();
        }

        private void btnPresupuesto_Click(object sender, EventArgs e)
        {
            var formFactura = new _00057_Comprobante(_factura);
            formFactura.ShowDialog();
            LimpiarParaNuevaFactura();
            txtCodigo.Focus();
        }
    }
}
