using Aplicacion.Constantes;
using IServicios.Articulo;
using IServicios.Articulo.DTOs;
using IServicios.Iva;
using IServicios.Marca;
using IServicios.Rubro;
using IServicios.UnidadMedida;
using PresentacionBase.Formularios;
using StructureMap;
using System.Drawing;
using System.Windows.Forms;
using static Aplicacion.Constantes.Imagen;


namespace Presentacion.Core.Articulo
{
    public partial class _00018_Abm_Articulo : FormAbm
    {
        private readonly IArticuloServicio _articuloServicio;
        private readonly IMarcaServicio _marcaServicio;
        private readonly IRubroServicio _rubroServicio;
        private readonly IUnidadMedidaServicio _unidadMedidaServicio;
        private readonly IIvaServicio _ivaServicio;

        public _00018_Abm_Articulo(TipoOperacion tipoOperacion, long? entidadId = null)
            : base(tipoOperacion, entidadId)
        {
            InitializeComponent();

            _articuloServicio = ObjectFactory.GetInstance<IArticuloServicio>();
            _marcaServicio = ObjectFactory.GetInstance<IMarcaServicio>();
            _rubroServicio = ObjectFactory.GetInstance<IRubroServicio>();
            _unidadMedidaServicio = ObjectFactory.GetInstance<IUnidadMedidaServicio>();
            _ivaServicio = ObjectFactory.GetInstance<IIvaServicio>();

            if (imgFoto.Image == null)
            {
                imgFoto.Image = ImagenProductoPorDefecto;
            }

            PoblarComboBox(cmbMarca, _marcaServicio.Obtener(string.Empty, false), "Descripcion", "Id");
            PoblarComboBox(cmbRubro, _rubroServicio.Obtener(string.Empty, false), "Descripcion", "Id");
            PoblarComboBox(cmbUnidad, _unidadMedidaServicio.Obtener(string.Empty, false), "Descripcion", "Id");
            PoblarComboBox(cmbIva, _ivaServicio.Obtener(string.Empty, false), "Descripcion", "Id");
        }

        public override void CargarDatos(long? entidadId)
        {
            base.CargarDatos(entidadId);

            if (entidadId.HasValue)
            {
                groupPrecio.Enabled = false;
                nudStock.Enabled = false;

                var resultado = (ArticuloDto)_articuloServicio.Obtener(entidadId.Value);

                if (resultado == null)
                {
                    MessageBox.Show("Ocurrio un error al obtener el registro seleccionado");
                    Close();
                }

                // ==================================================== //
                // ===============     Datos del Articulo    ========== //
                // ==================================================== //

                txtCodigo.Text = resultado.Codigo.ToString();
                txtcodigoBarra.Text = resultado.CodigoBarra;
                txtDescripcion.Text = resultado.Descripcion;
                txtAbreviatura.Text = resultado.Abreviatura;
                txtDetalle.Text = resultado.Detalle;
                txtUbicacion.Text = resultado.Ubicacion;
                cmbMarca.SelectedValue = resultado.MarcaId;
                cmbRubro.SelectedValue = resultado.RubroId;
                cmbUnidad.SelectedValue = resultado.UnidadMedidaId;
                cmbIva.SelectedValue = resultado.IvaId;

                // ==================================================== //
                // ===============     Datos del Generales   ========== //
                // ==================================================== //

                nudStockMin.Value = resultado.StockMinimo;
                chkDescontarStock.Checked = resultado.DescuentaStock;
                chkPermitirStockNeg.Checked = resultado.PermiteStockNegativo;
                chkActivarLimite.Checked = resultado.ActivarLimiteVenta;
                nudLimiteVenta.Value = resultado.LimiteVenta;
                chkActivarHoraVenta.Checked = resultado.ActivarHoraVenta;
                dtpHoraDesde.Value = resultado.HoraLimiteVentaDesde;
                dtpHoraHasta.Value = resultado.HoraLimiteVentaHasta;

                // ==================================================== //
                // ===============     Foto del Articulo    ========== //
                // ==================================================== //

                imgFoto.Image = ConvertirImagen(resultado.Foto);

                if (imgFoto.Image == null)
                {
                    imgFoto.Image = ImagenProductoPorDefecto;
                }

                if (TipoOperacion == TipoOperacion.Eliminar)
                    DesactivarControles(this);
            }
            else // Nuevo
            {
                btnEjecutar.Text = "Grabar";
                LimpiarControles(this);
                //txtCodigo.Text = _articuloServicio.ObtenerSiguienteNroCodigo().ToString();
                //txtCodigo.Enabled = false;
            }
        }

        public override bool VerificarDatosObligatorios()
        {
            if (string.IsNullOrEmpty(txtCodigo.Text)) return false;

            if (string.IsNullOrEmpty(txtcodigoBarra.Text)) return false;

            if (string.IsNullOrEmpty(txtDescripcion.Text)) return false;

            if (cmbMarca.Items.Count <= 0) return false;

            if (cmbRubro.Items.Count <= 0) return false;

            if (cmbUnidad.Items.Count <= 0) return false;

            if (cmbIva.Items.Count <= 0) return false;

            return true;
        }

        public override bool VerificarSiExiste(long? id = null)
        {
            return _articuloServicio.VerificarSiExiste(txtDescripcion.Text, id);
        }

        public override void EjecutarComandoNuevo()
        {
            //var nuevoRegistro = new ArticuloCrudDto
            var nuevoRegistro = new ArticuloCrudDto
            {
                Codigo = int.Parse(txtCodigo.Text),
                CodigoBarra = txtcodigoBarra.Text,
                Descripcion = txtDescripcion.Text,
                Abreviatura = txtAbreviatura.Text,
                Detalle = txtDetalle.Text,
                Ubicacion = txtUbicacion.Text,
                MarcaId = (long)cmbMarca.SelectedValue,
                RubroId = (long)cmbRubro.SelectedValue,
                UnidadMedidaId = (long)cmbUnidad.SelectedValue,
                IvaId = (long)cmbIva.SelectedValue,
                PrecioCosto = nudPrecioCosto.Value,
                //------------------------------------------------//
                StockActual = nudStock.Value,
                StockMinimo = nudStockMin.Value,
                DescuentaStock = chkDescontarStock.Checked,
                PermiteStockNegativo = chkPermitirStockNeg.Checked,
                ActivarLimiteVenta = chkActivarLimite.Checked,
                LimiteVenta = nudLimiteVenta.Value,
                ActivarHoraVenta = chkActivarHoraVenta.Checked,
                HoraLimiteVentaDesde = dtpHoraDesde.Value,
                HoraLimiteVentaHasta = dtpHoraHasta.Value,
                //------------------------------------------------//
                Foto = ConvertirImagen(imgFoto.Image),
                Eliminado = false
            };

            _articuloServicio.Insertar(nuevoRegistro);            
        }

        public override void EjecutarComandoModificar()
        {
            var modificarRegistro = new ArticuloCrudDto
            {
                Id = EntidadId.Value,
                Codigo = int.Parse(txtCodigo.Text),
                CodigoBarra = txtcodigoBarra.Text,
                Descripcion = txtDescripcion.Text,
                Abreviatura = txtAbreviatura.Text,
                Detalle = txtDetalle.Text,
                Ubicacion = txtUbicacion.Text,
                MarcaId = (long)cmbMarca.SelectedValue,
                RubroId = (long)cmbRubro.SelectedValue,
                UnidadMedidaId = (long)cmbUnidad.SelectedValue,
                IvaId = (long)cmbIva.SelectedValue,
                PrecioCosto = nudPrecioCosto.Value,
                //------------------------------------------------//
                StockActual = nudStock.Value,
                StockMinimo = nudStockMin.Value,
                DescuentaStock = chkDescontarStock.Checked,
                PermiteStockNegativo = chkPermitirStockNeg.Checked,
                ActivarLimiteVenta = chkActivarLimite.Checked,
                LimiteVenta = nudLimiteVenta.Value,
                ActivarHoraVenta = chkActivarHoraVenta.Checked,
                HoraLimiteVentaDesde = dtpHoraDesde.Value,
                HoraLimiteVentaHasta = dtpHoraHasta.Value,
                //------------------------------------------------//
                Foto = ConvertirImagen(imgFoto.Image),
                Eliminado = false
            };

            _articuloServicio.Modificar(modificarRegistro);
        }


        public override void EjecutarComandoEliminar()
        {
            _articuloServicio.Eliminar(EntidadId.Value);
        }

        public override void LimpiarControles(object obj, bool tieneValorAsociado = false)
        {
            base.LimpiarControles(obj, tieneValorAsociado);

            //txtCodigo.Text = _articuloServicio.ObtenerSiguienteNroCodigo().ToString();
            //txtCodigo.Enabled = false;
            txtcodigoBarra.Focus();
        }

        private void btnNuevaMarca_Click(object sender, System.EventArgs e)
        {
            var fNuevaMarca = new _00022_Abm_Marca(TipoOperacion.Nuevo);
            fNuevaMarca.ShowDialog();
            if (fNuevaMarca.RealizoAlgunaOperacion)
            {
                PoblarComboBox(cmbMarca, _marcaServicio.Obtener(string.Empty, true));
            }
        }

        private void btnNuevoRubro_Click(object sender, System.EventArgs e)
        {
            var fNuevoRubro = new _00020_Abm_Rubro(TipoOperacion.Nuevo);
            fNuevoRubro.ShowDialog();
            if (fNuevoRubro.RealizoAlgunaOperacion)
            {
                PoblarComboBox(cmbRubro, _rubroServicio.Obtener(string.Empty, true));
            }
        }

        private void button1_Click(object sender, System.EventArgs e)
        {
            var fNuevaIva = new _00026_Abm_Iva(TipoOperacion.Nuevo);
            fNuevaIva.ShowDialog();
            if (fNuevaIva.RealizoAlgunaOperacion)
            {
                PoblarComboBox(cmbIva, _ivaServicio.Obtener(string.Empty, true));
            }
        }

        private void btnNuevaUnidad_Click(object sender, System.EventArgs e)
        {
            var fNuevaUnidad = new _00024_Abm_UnidadDeMedida(TipoOperacion.Nuevo);
            fNuevaUnidad.ShowDialog();
            if (fNuevaUnidad.RealizoAlgunaOperacion)
            {
                PoblarComboBox(cmbUnidad, _unidadMedidaServicio.Obtener(string.Empty, true));
            }
        }

        private void btnAgregarImagen_Click(object sender, System.EventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter = "Image Files(*.jpg; *.jpeg; *.gif; *.bmp; *.png)|*.jpg; *.jpeg; *.gif; *.bmp; *.png";
            if (openFile.ShowDialog() == DialogResult.OK)
            {
                imgFoto.Image = string.IsNullOrEmpty(openFile.FileName) ? ImagenEmpleadoPorDefecto : new Bitmap(openFile.FileName);
            }
        }
    }
}
