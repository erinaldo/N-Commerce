using Aplicacion.Constantes;
using IServicios.Departamento;
using IServicios.Localidad;
using IServicios.Persona;
using IServicios.Persona.DTOs;
using IServicios.Provincia;
using Presentacion.Core.Departamento;
using Presentacion.Core.Localidad;
using Presentacion.Core.Provincia;
using PresentacionBase.Formularios;
using StructureMap;
using System.Drawing;
using System.Windows.Forms;
using static Aplicacion.Constantes.Imagen;

namespace Presentacion.Core.Empleado
{
    public partial class _00008_Abm_Empleado : FormAbm
    {
        private readonly IProvinciaServicio _provinciaServicio;
        private readonly IDepartamentoServicio _departamentoServicio;
        private readonly ILocalidadServicio _localidadServicio;
        private readonly IPersonaServicio _personaServicio;
        private readonly IEmpleadoServicio _empleadoServicio;

        public _00008_Abm_Empleado(TipoOperacion tipoOperacion, long? entidadId = null)
            : base(tipoOperacion, entidadId)
        {
            InitializeComponent();

            _provinciaServicio = ObjectFactory.GetInstance<IProvinciaServicio>();
            _departamentoServicio = ObjectFactory.GetInstance<IDepartamentoServicio>();
            _localidadServicio = ObjectFactory.GetInstance<ILocalidadServicio>();
            _personaServicio = ObjectFactory.GetInstance<IPersonaServicio>();
            _empleadoServicio = ObjectFactory.GetInstance<IEmpleadoServicio>();
        }

        private void _00008_Abm_Empleado_Load(object sender, System.EventArgs e)
        {
            PoblarComboBox(cmbProvincia, _provinciaServicio.Obtener(string.Empty, true), "Descripcion", "Id");
            PoblarComboBox(cmbDepartamento, _departamentoServicio.ObtenerPorProvincia((long)cmbProvincia.SelectedValue), "Descripcion", "Id");
            PoblarComboBox(cmbLocalidad, _localidadServicio.ObtenerPorDepartamento((long)cmbDepartamento.SelectedValue), "Descripcion", "Id");
            txtLegajo.Enabled = false;
        }

        public override void CargarDatos(long? entidadId)
        {
            base.CargarDatos(entidadId);

            if (entidadId == null)
            {
                LimpiarControles(this);
                txtLegajo.Text = _empleadoServicio.ObtenerSiguienteLegajo().ToString();
                imgFoto.Image = ImagenEmpleadoPorDefecto;
                txtApellido.Focus();
            }
            else
            {
                var empleado = (EmpleadoDto)_personaServicio.Obtener(typeof(EmpleadoDto), entidadId.Value);

                if (empleado == null)
                {
                    MessageBox.Show("La entidad no existe.");
                    this.Close();
                }
                
                txtLegajo.Text = empleado.Legajo.ToString();
                txtApellido.Text = empleado.Apellido;
                txtNombre.Text = empleado.Nombre;
                txtDni.Text = empleado.Dni;
                txtTelefono.Text = empleado.Telefono;
                txtDomicilio.Text = empleado.Direccion;
                cmbLocalidad.SelectedValue = empleado.LocalidadId;//+CONSULT4R+
                txtMail.Text = empleado.Mail;
                imgFoto.Image = ConvertirImagen(empleado.Foto);

                if (imgFoto.Image == null)
                {
                    imgFoto.Image = ImagenEmpleadoPorDefecto;
                }

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

            var nuevoRegistro = new EmpleadoDto
            {
                Legajo = int.Parse(txtLegajo.Text),
                Apellido = txtApellido.Text,
                Nombre = txtNombre.Text,
                Dni = txtDni.Text,
                Telefono = txtTelefono.Text,
                Direccion = txtDomicilio.Text,
                //ProvinciaId = (long)cmbProvincia.SelectedValue,
                //DepartamentoId = (long)cmbDepartamento.SelectedValue,
                LocalidadId = (long)cmbLocalidad.SelectedValue,
                Mail = txtMail.Text,
                Foto = ConvertirImagen(imgFoto.Image),
                Eliminado = false
            };
            _empleadoServicio.Insertar(nuevoRegistro);
        }

        public override void EjecutarComandoModificar()
        {
            base.EjecutarComandoModificar();

            var modificarRegistro = new EmpleadoDto
            {
                Id=EntidadId.Value,
                Legajo = int.Parse(txtLegajo.Text),
                Apellido = txtApellido.Text,
                Nombre = txtNombre.Text,
                Dni = txtDni.Text,
                Telefono = txtTelefono.Text,
                Direccion = txtDomicilio.Text,
                ProvinciaId = (long)cmbProvincia.SelectedValue,//+CONSULT4R+
                DepartamentoId = (long)cmbDepartamento.SelectedValue,//+CONSULT4R+
                LocalidadId = (long)cmbLocalidad.SelectedValue,
                Mail = txtMail.Text,
                Foto = ConvertirImagen(imgFoto.Image),
                Eliminado = false
            };
            _empleadoServicio.Modificar(modificarRegistro);
        }

        public override void EjecutarComandoEliminar()
        {
            base.EjecutarComandoEliminar();

            var eliminarRegistro = new EmpleadoDto
            {
                Legajo = int.Parse(txtLegajo.Text),
                Apellido = txtApellido.Text,
                Nombre = txtNombre.Text,
                Dni = txtDni.Text,
                Telefono = txtTelefono.Text,
                Direccion = txtDomicilio.Text,
                LocalidadId = (long)cmbLocalidad.SelectedValue,
                Mail = txtMail.Text,
                Foto = ConvertirImagen(imgFoto.Image),
                Eliminado = false
            };
            _empleadoServicio.Eliminar(typeof(EmpleadoDto), EntidadId.Value);
        }

        public override bool VerificarDatosObligatorios()
        {
            return true;
        }

        private void btnImagen_Click(object sender, System.EventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter = "Image Files(*.jpg; *.jpeg; *.gif; *.bmp; *.png)|*.jpg; *.jpeg; *.gif; *.bmp; *.png";
            if (openFile.ShowDialog() == DialogResult.OK)
            {
                imgFoto.Image = string.IsNullOrEmpty(openFile.FileName) ? ImagenEmpleadoPorDefecto : new Bitmap(openFile.FileName);
            }
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
    }
}
