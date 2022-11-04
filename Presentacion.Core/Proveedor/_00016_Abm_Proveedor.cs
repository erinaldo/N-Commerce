using IServicios.Departamento;
using IServicios.Localidad;
using IServicios.Localidad.DTOs;
using IServicios.Proveedor;
using IServicios.Proveedor.DTOs;
using IServicios.Provincia;
using Presentacion.Core.CondicionIva;
using Presentacion.Core.Departamento;
using Presentacion.Core.Localidad;
using Presentacion.Core.Provincia;
using PresentacionBase.Formularios;
using StructureMap;
using System.Windows.Forms;

namespace Presentacion.Core.Proveedor
{
    public partial class _00016_Abm_Proveedor : FormAbm
    {
        private readonly IProvinciaServicio _provinciaServicio;
        private readonly IDepartamentoServicio _departamentoServicio;
        private readonly ILocalidadServicio _localidadServicio;
        private readonly ICondicionIvaServicio _condicionIvaServicio;
        private readonly IProveedorServicio _proveedorServicio;

        public _00016_Abm_Proveedor(TipoOperacion tipoOperacion, long? entidadId = null)
            : base(tipoOperacion, entidadId)
        {
            InitializeComponent();

            _provinciaServicio = ObjectFactory.GetInstance<IProvinciaServicio>();
            _departamentoServicio = ObjectFactory.GetInstance<IDepartamentoServicio>();
            _localidadServicio = ObjectFactory.GetInstance<ILocalidadServicio>();
            _condicionIvaServicio = ObjectFactory.GetInstance<ICondicionIvaServicio>();
            _proveedorServicio = ObjectFactory.GetInstance<IProveedorServicio>();
        }

        private void _00016_Abm_Proveedor_Load(object sender, System.EventArgs e)
        {
            PoblarComboBox(cmbProvincia, _provinciaServicio.Obtener(string.Empty, true), "Descripcion", "Id");
            PoblarComboBox(cmbDepartamento, _departamentoServicio.ObtenerPorProvincia((long)cmbProvincia.SelectedValue), "Descripcion", "Id");
            PoblarComboBox(cmbLocalidad, _localidadServicio.ObtenerPorDepartamento((long)cmbDepartamento.SelectedValue), "Descripcion", "Id");
            PoblarComboBox(cmbCondicionIva, _condicionIvaServicio.Obtener(string.Empty), "Descripcion", "Id");
        }

        public override void CargarDatos(long? entidadId)
        {
            base.CargarDatos(entidadId);

            if (entidadId == null)
            {
                LimpiarControles(this);
                txtRazonSocial.Focus();
            }
            else
            {
                var proveedor = (ProveedorDTO)_proveedorServicio.Obtener(entidadId.Value);

                if (proveedor == null)
                {
                    MessageBox.Show("La entidad no existe.");
                    this.Close();
                }

                txtRazonSocial.Text = proveedor.RazonSocial;
                txtCUIT.Text = proveedor.CUIT;
                txtTelefono.Text = proveedor.Telefono;
                txtDomicilio.Text = proveedor.Direccion;
                //var dir = (LocalidadDto)_localidadServicio.ObtenerPorDepartamento(proveedor.LocalidadId);
                //cmbProvincia.SelectedValue = dir.ProvinciaId;
                //PoblarComboBox(cmbDepartamento, _departamentoServicio.ObtenerPorProvincia((long)cmbProvincia.SelectedValue), "Descripcion", "Id");
                //cmbDepartamento.SelectedValue = dir.DepartamentoId;
                //PoblarComboBox(cmbLocalidad, _localidadServicio.ObtenerPorDepartamento((long)cmbDepartamento.SelectedValue), "Descripcion", "Id");
                cmbLocalidad.SelectedValue = proveedor.LocalidadId;
                txtMail.Text = proveedor.Mail;
                cmbCondicionIva.SelectedValue = proveedor.CondicionIvaId;

                if (TipoOperacion == TipoOperacion.Eliminar)
                {
                    DesactivarControles(this);
                }
            }
        }

        private void cmbProvincia_SelectionChangeCommitted(object sender, System.EventArgs e)
        {
            if (cmbProvincia.Items.Count <= 0) return;

            PoblarComboBox(cmbDepartamento, _departamentoServicio.ObtenerPorProvincia((long)cmbProvincia.SelectedValue), "Descripcion", "Id");

            if (cmbDepartamento.Items.Count <= 0) return;

            PoblarComboBox(cmbLocalidad, _localidadServicio.ObtenerPorDepartamento((long)cmbDepartamento.SelectedValue), "Descripcion", "Id");
        }

        private void cmbDepartamento_SelectionChangeCommitted(object sender, System.EventArgs e)
        {
            if (cmbDepartamento.Items.Count <= 0) return;

            PoblarComboBox(cmbLocalidad, _localidadServicio.ObtenerPorDepartamento((long)cmbDepartamento.SelectedValue), "Descripcion", "Id");
        }

        public override void EjecutarComandoNuevo()
        {
            base.EjecutarComandoNuevo();

            var nuevoRegistro = new ProveedorDTO
            {
                RazonSocial = txtRazonSocial.Text,
                CUIT = txtCUIT.Text,
                Telefono = txtTelefono.Text,
                Direccion = txtDomicilio.Text,
                LocalidadId = (long)cmbLocalidad.SelectedValue,
                Mail=txtMail.Text,
                CondicionIvaId = (long)cmbCondicionIva.SelectedValue
            };
            _proveedorServicio.Insertar(nuevoRegistro);//TODO: NullReference
        }

        public override void EjecutarComandoModificar()
        {
            base.EjecutarComandoModificar();

            var modificarRegistro = new ProveedorDTO
            {
                Id = EntidadId.Value,
                RazonSocial = txtRazonSocial.Text,
                CUIT = txtCUIT.Text,
                Telefono = txtTelefono.Text,
                Direccion = txtDomicilio.Text,
                LocalidadId = (long)cmbLocalidad.SelectedValue,
                Mail = txtMail.Text,
                CondicionIvaId = (long)cmbCondicionIva.SelectedValue,
                Eliminado = false
            };
            _proveedorServicio.Modificar(modificarRegistro);
        }

        public override void EjecutarComandoEliminar()
        {
            base.EjecutarComandoEliminar();
            _proveedorServicio.Eliminar(EntidadId.Value);
        }

        public override bool VerificarDatosObligatorios()
        {
            return true;
        }

        private void btnNuevaProvincia_Click(object sender, System.EventArgs e)
        {
            var form = new _00002_Abm_Provincia(TipoOperacion.Nuevo).ShowDialog();
        }

        private void btnNuevoDepartamento_Click(object sender, System.EventArgs e)
        {
            var form = new _00004_Abm_Departamento(TipoOperacion.Nuevo).ShowDialog();

        }

        private void btnNuevaLocalidad_Click(object sender, System.EventArgs e)
        {
            var form = new _00006_AbmLocalidad(TipoOperacion.Nuevo).ShowDialog();
        }

        private void btnNuevaCondicionIva_Click(object sender, System.EventArgs e)
        {
            var form = new _00014_Abm_CondicionIva(TipoOperacion.Nuevo).ShowDialog();
        }

        private void cmbProvincia_SelectionChangeCommitted_1(object sender, System.EventArgs e)
        {
            PoblarComboBox(cmbDepartamento, _departamentoServicio.ObtenerPorProvincia((long)cmbProvincia.SelectedValue), "Descripcion", "Id");
            PoblarComboBox(cmbLocalidad, _localidadServicio.ObtenerPorDepartamento((long)cmbDepartamento.SelectedValue), "Descripcion", "Id");
        }

        private void cmbDepartamento_SelectionChangeCommitted_1(object sender, System.EventArgs e)
        {
            PoblarComboBox(cmbLocalidad, _localidadServicio.ObtenerPorDepartamento((long)cmbDepartamento.SelectedValue), "Descripcion", "Id");
        }
    }
}
