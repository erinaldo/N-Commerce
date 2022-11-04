using IServicios.CondicionIva.DTOs;
using IServicios.Departamento;
using PresentacionBase.Formularios;
using StructureMap;
using System.Windows.Forms;

namespace Presentacion.Core.CondicionIva
{
    public partial class _00014_Abm_CondicionIva : FormAbm
    {
        private readonly ICondicionIvaServicio _condicionIvaServicio;

        public _00014_Abm_CondicionIva(TipoOperacion tipoOperacion, long? entidadId = null)
            : base(tipoOperacion, entidadId)
        {
            InitializeComponent();
            _condicionIvaServicio = ObjectFactory.GetInstance<ICondicionIvaServicio>();
        }

        public override void CargarDatos(long? entidadId)
        {
            base.CargarDatos(entidadId);

            if (entidadId.HasValue)
            {
                var resultado = (CondicionIvaDto)_condicionIvaServicio.Obtener(entidadId.Value);

                if (resultado == null)
                {
                    MessageBox.Show("Ocurrio un error al obtener el registro seleccionado");
                    Close();
                }

                txtDescripcion.Text = resultado.Descripcion;

                if (TipoOperacion == TipoOperacion.Eliminar)
                    DesactivarControles(this);
            }
            else // Nuevo
            {
                btnEjecutar.Text = "Nuevo";
            }
        }

        public override bool VerificarDatosObligatorios()
        {
            return !string.IsNullOrEmpty(txtDescripcion.Text);
        }

        public override bool VerificarSiExiste(long? id = null)
        {
            return _condicionIvaServicio.VerificarSiExiste(txtDescripcion.Text, id);
        }

        public override void EjecutarComandoNuevo()
        {
            var nuevoRegistro = new CondicionIvaDto();
            nuevoRegistro.Descripcion = txtDescripcion.Text;
            nuevoRegistro.Eliminado = false;

            _condicionIvaServicio.Insertar(nuevoRegistro);
        }

        public override void EjecutarComandoModificar()
        {
            var modificarRegistro = new CondicionIvaDto();
            modificarRegistro.Id = EntidadId.Value;
            modificarRegistro.Descripcion = txtDescripcion.Text;
            modificarRegistro.Eliminado = false;

            _condicionIvaServicio.Modificar(modificarRegistro);
        }


        public override void EjecutarComandoEliminar()
        {
            _condicionIvaServicio.Eliminar(EntidadId.Value);
        }

        public override void LimpiarControles(Form formulario)
        {
            base.LimpiarControles(formulario);

            txtDescripcion.Focus();
        }
    }
}
