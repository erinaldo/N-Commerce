using IServicios.PuestoTrabajo;
using IServicios.PuestoTrabajo.DTOs;
using PresentacionBase.Formularios;
using StructureMap;
using System.Windows.Forms;

namespace Presentacion.Core.Comprobantes
{
    public partial class _00052_Abm_PuestoTrabajo : FormAbm
    {
        private readonly IPuestoTrabajoServicio _puestoTrabajoServicio;
        public _00052_Abm_PuestoTrabajo(TipoOperacion tipoOperacion, long? entidadId = null)
            : base(tipoOperacion, entidadId)
        {
            InitializeComponent();

            _puestoTrabajoServicio = ObjectFactory.GetInstance<IPuestoTrabajoServicio>();
        }

        public override void CargarDatos(long? entidadId)
        {
            if (entidadId.HasValue)
            {
                if (TipoOperacion == TipoOperacion.Eliminar)
                    DesactivarControles(this);

                var entidad = (PuestoTrabajoDto)_puestoTrabajoServicio.Obtener(entidadId.Value);

                if (entidad == null)
                {
                    MessageBox.Show("Ocurrio un error al Obtener el registro seleccionado");
                    Close();
                }

                txtCodigo.Text = entidad.Codigo.ToString();
                txtDescripcion.Text = entidad.Descripcion;
            }
            else
            {
                txtDescripcion.Clear();
                txtDescripcion.Focus();
            }
        }
        public override void EjecutarComandoNuevo()
        {
            _puestoTrabajoServicio.Insertar(new PuestoTrabajoDto
            {
                Codigo=int.Parse(txtCodigo.Text),
                Descripcion = txtDescripcion.Text,
                Eliminado = false
            });
        }
        public override void EjecutarComandoModificar()
        {
            _puestoTrabajoServicio.Modificar(new PuestoTrabajoDto
            {
                Id = EntidadId.Value,
                Codigo = int.Parse(txtCodigo.Text),
                Descripcion = txtDescripcion.Text,
                Eliminado = false
            });
        }
        public override void EjecutarComandoEliminar()
        {
            _puestoTrabajoServicio.Eliminar(EntidadId.Value);
        }
        public override bool VerificarDatosObligatorios()
        {
            if (string.IsNullOrEmpty(txtCodigo.Text))
                return false;

            if (string.IsNullOrEmpty(txtDescripcion.Text))
                return false;

            return true;
        }
    }
}
