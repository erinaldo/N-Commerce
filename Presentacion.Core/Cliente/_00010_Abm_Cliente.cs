using IServicios.Departamento;
using IServicios.Localidad;
using IServicios.Persona;
using IServicios.Persona.DTOs;
using IServicios.Provincia;
using PresentacionBase.Formularios;
using StructureMap;
using System.Windows.Forms;

namespace Presentacion.Core.Cliente
{
    public partial class _00010_Abm_Cliente : FormAbm
    {
        private readonly IProvinciaServicio _provinciaServicio;
        private readonly IDepartamentoServicio _departamentoServicio;
        private readonly ILocalidadServicio _localidadServicio;
        private readonly ICondicionIvaServicio _condicionIvaServicio;
        private readonly IPersonaServicio _personaServicio;
        private readonly IClienteServicio _clienteServicio;

        public _00010_Abm_Cliente(TipoOperacion tipoOperacion, long? entidadId = null)
            : base(tipoOperacion, entidadId)
        {
            InitializeComponent();

            _provinciaServicio = ObjectFactory.GetInstance<IProvinciaServicio>();
            _departamentoServicio = ObjectFactory.GetInstance<IDepartamentoServicio>();
            _localidadServicio = ObjectFactory.GetInstance<ILocalidadServicio>();
            _condicionIvaServicio = ObjectFactory.GetInstance<ICondicionIvaServicio>();
            _personaServicio = ObjectFactory.GetInstance<IPersonaServicio>();
            _clienteServicio = ObjectFactory.GetInstance<IClienteServicio>();
        }

        private void _00010_Abm_Cliente_Load(object sender, System.EventArgs e)
        {
            PoblarComboBox(cmbProvincia, _provinciaServicio.Obtener(string.Empty), "Descripcion", "Id");
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
                txtApellido.Focus();
            }
            else
            {
                var empleado = (ClienteDto)_personaServicio.Obtener(typeof(ClienteDto), entidadId.Value);

                if (empleado == null)
                {
                    MessageBox.Show("La entidad no existe.");
                    this.Close();
                }

                txtApellido.Text = empleado.Apellido;
                txtNombre.Text = empleado.Nombre;
                txtDni.Text = empleado.Dni;
                txtTelefono.Text = empleado.Telefono;
                txtDomicilio.Text = empleado.Direccion;
                cmbProvincia.SelectedValue = empleado.ProvinciaId;
                cmbDepartamento.SelectedValue = empleado.DepartamentoId;
                cmbLocalidad.SelectedValue = empleado.LocalidadId;//+CONSULT4R+
                txtMail.Text = empleado.Mail;
                cmbCondicionIva.SelectedValue = empleado.CondicionIvaId;
                chkActivarCuentaCorriente.Checked = empleado.ActivarCtaCte;
                chkLimiteCompra.Checked = empleado.TieneLimiteCompra;

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

            var nuevoRegistro = new ClienteDto
            {
                Apellido = txtApellido.Text,
                Nombre = txtNombre.Text,
                Dni = txtDni.Text,
                Telefono = txtTelefono.Text,
                Direccion = txtDomicilio.Text,
                ProvinciaId = (long)cmbProvincia.SelectedValue,
                DepartamentoId = (long)cmbDepartamento.SelectedValue,
                LocalidadId = (long)cmbLocalidad.SelectedValue,
                Mail = txtMail.Text,
                CondicionIvaId = (long)cmbCondicionIva.SelectedValue,
                ActivarCtaCte = chkActivarCuentaCorriente.Checked,
                TieneLimiteCompra = chkLimiteCompra.Checked,
                MontoMaximoCtaCte = nudLimiteCompra.Value
            };
            _personaServicio.Insertar(nuevoRegistro);
        }
        public override bool VerificarDatosObligatorios()
        {
            return true;
        }
    }
}
