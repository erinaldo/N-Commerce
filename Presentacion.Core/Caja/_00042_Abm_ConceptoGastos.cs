

using IServicios.ConceptoGasto;
using IServicios.ConceptoGasto.DTOs;
using PresentacionBase.Formularios;
using StructureMap;
using System.Windows.Forms;

namespace Presentacion.Core.Caja
{
    public partial class _00042_Abm_ConceptoGastos : FormAbm
    {

        private readonly IConceptoGastoServicio _conceptoGastoServicio;

        public _00042_Abm_ConceptoGastos(TipoOperacion tipoOperacion, long? entidadId = null)
            : base(tipoOperacion, entidadId)
        {
            InitializeComponent();

            _conceptoGastoServicio = ObjectFactory.GetInstance<IConceptoGastoServicio>();
        }

        public override void CargarDatos(long? entidadId)
        {
            base.CargarDatos(entidadId);

            if (entidadId.HasValue)
            {
                var resultado = (ConceptoGastoDTO)_conceptoGastoServicio.Obtener(entidadId.Value);

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
            return _conceptoGastoServicio.VerificarSiExiste(txtDescripcion.Text, id);
        }

        public override void EjecutarComandoNuevo()
        {
            var nuevoRegistro = new ConceptoGastoDTO();
            nuevoRegistro.Descripcion = txtDescripcion.Text;
            nuevoRegistro.Eliminado = false;

            _conceptoGastoServicio.Insertar(nuevoRegistro);
        }

        public override void EjecutarComandoModificar()
        {
            var modificarRegistro = new ConceptoGastoDTO();
            modificarRegistro.Id = EntidadId.Value;
            modificarRegistro.Descripcion = txtDescripcion.Text;
            modificarRegistro.Eliminado = false;

            _conceptoGastoServicio.Modificar(modificarRegistro);
        }


        public override void EjecutarComandoEliminar()
        {
            _conceptoGastoServicio.Eliminar(EntidadId.Value);
        }

        public override void LimpiarControles(Form formulario)
        {
            base.LimpiarControles(formulario);

            txtDescripcion.Focus();
        }
    }
}
