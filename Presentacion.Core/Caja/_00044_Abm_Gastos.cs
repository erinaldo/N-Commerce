using IServicios.ConceptoGasto;
using IServicios.Gasto;
using IServicios.Gasto.DTOs;
using PresentacionBase.Formularios;
using StructureMap;
using System.Windows.Forms;

namespace Presentacion.Core.Caja
{
    public partial class _00044_Abm_Gastos : FormAbm
    {
        private readonly IConceptoGastoServicio _conceptoGastoServicio;
        private readonly IGastoServicio _gastoServicio;
        public _00044_Abm_Gastos(TipoOperacion tipoOperacion, long? entidadId = null)
            : base(tipoOperacion, entidadId)
        {
            InitializeComponent();
            _conceptoGastoServicio = ObjectFactory.GetInstance<IConceptoGastoServicio>();
            _gastoServicio = ObjectFactory.GetInstance<IGastoServicio>();
        }

        private void _00044_Abm_Gastos_Load(object sender, System.EventArgs e)
        {

        }

        public override void CargarDatos(long? entidadId)
        {
            if (entidadId.HasValue)
            {
                if (TipoOperacion == TipoOperacion.Eliminar)
                    DesactivarControles(this);

                var entidad = (GastoDTO)_gastoServicio.Obtener(entidadId.Value);

                if (entidad == null)
                {
                    MessageBox.Show("Ocuriro un error al obtener el registro seleciconado");
                    Close();
                }

                dtpFecha.Value = entidad.Fecha;

                PoblarComboBox(cmbConcepto, _conceptoGastoServicio.Obtener(string.Empty), "Descripcion", "Id");

                cmbConcepto.SelectedValue = entidad.Id;

                txtDescripcion.Text = entidad.Descripcion;

                nudMontoPagar.Value = entidad.Monto;
            }
            else
            {
                // Nuevo
                PoblarComboBox(cmbConcepto, _conceptoGastoServicio.Obtener(string.Empty), "Descripcion", "Id");

                //if (cmbConcepto.Items.Count > 0) // si tiene algo
                //{
                //    PoblarComboBox(cmbConcepto,
                //        _conceptoGastoServicio.ObtenerPorProvincia((long)cmbProvincia.SelectedValue)
                //        , "Descripcion",
                //        "Id");
                //}

                txtDescripcion.Clear();
            }
        }

        public override void EjecutarComandoNuevo()
        {
            _gastoServicio.Insertar(new GastoDTO
            {
                Fecha = dtpFecha.Value,
                ConceptoGastoId = (long)cmbConcepto.SelectedValue,
                Descripcion = txtDescripcion.Text,
                Monto = nudMontoPagar.Value,
            });
        }

        public override void EjecutarComandoModificar()
        {
            _gastoServicio.Modificar(new GastoDTO
            {
                Id = EntidadId.Value,
                Fecha = dtpFecha.Value,
                ConceptoGastoId = (long)cmbConcepto.SelectedValue,
                Descripcion = txtDescripcion.Text,
                Monto = nudMontoPagar.Value,
            });
        }

        public override void EjecutarComandoEliminar()
        {
            _gastoServicio.Eliminar(EntidadId.Value);
        }

        public override bool VerificarDatosObligatorios()
        {
            if (string.IsNullOrEmpty(txtDescripcion.Text))
                return false;

            if (cmbConcepto.Items.Count <= 0)
                return false;

            return true;
        }

        //public override bool VerificarSiExiste(long? id = null)
        //{
        //    return _gastoServicio.VerificarSiExiste(txtDescripcion.Text, (long)cmbConcepto.SelectedValue, id);
        //}

        //private void cmbProvincia_SelectionChangeCommitted(object sender, System.EventArgs e)
        //{
        //    PoblarComboBox(cmbDepartamento,
        //        _departamentoServicio.ObtenerPorProvincia((long)cmbProvincia.SelectedValue),
        //        "Descripcion",
        //        "Id");
        //}

        private void btnNuevoConcepto_Click(object sender, System.EventArgs e)
        {
            var formConceptoGasto = new _00042_Abm_ConceptoGastos(TipoOperacion.Nuevo);
            formConceptoGasto.ShowDialog();

            if (formConceptoGasto.RealizoAlgunaOperacion)
            {
                PoblarComboBox(cmbConcepto,
                    _conceptoGastoServicio.Obtener(string.Empty),
                    "Descripcion",
                    "Id");
            }
        }
    }
}
