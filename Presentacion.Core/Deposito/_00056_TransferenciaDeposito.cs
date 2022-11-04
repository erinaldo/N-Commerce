using IServicios.Articulo;
using IServicios.Articulo.DTOs;
using IServicios.Deposito;
using PresentacionBase.Formularios;
using StructureMap;
using System.Linq;
using System.Windows.Forms;

namespace Presentacion.Core.Deposito
{
    public partial class _00056_TransferenciaDeposito : FormAbm
    {
        private readonly IArticuloServicio _articuloServicio;
        private readonly IDepositoServicio _depositoServicio;

        public _00056_TransferenciaDeposito(TipoOperacion tipoOperacion, long? entidadId = null) : base(tipoOperacion, entidadId)
        {
            InitializeComponent();

            _articuloServicio = ObjectFactory.GetInstance<IArticuloServicio>();
            _depositoServicio = ObjectFactory.GetInstance<IDepositoServicio>();
        }

        public override void CargarDatos(long? entidadId)
        {
            base.CargarDatos(entidadId);

            if (entidadId.HasValue)
            {
                var articulo = (ArticuloDto)_articuloServicio.Obtener(entidadId.Value);
                txtArticulo.Text = articulo.Descripcion;
                var deposito = _articuloServicio.ObtenerStock(entidadId.Value);

                if (deposito == null)
                {
                    MessageBox.Show("Ocurrio un error al obtener el registro seleccionado");
                    Close();
                }

                PoblarComboBox(cmbDepositoOrigen, deposito, "Deposito", "Cantidad");
                CargarCampos();
            }
        }
        private void CargarCampos()
        {
            nudOrigen.Value = (decimal)cmbDepositoOrigen.SelectedValue;
            nudDestino.Maximum = (decimal)cmbDepositoOrigen.SelectedValue;

            var deposito = _depositoServicio.Obtener(string.Empty, false, cmbDepositoOrigen.Text);

            if (deposito == null)
            {
                MessageBox.Show("Ocurrio un error al obtener el registro seleccionado");
                Close();
            }

            PoblarComboBox(cmbDepositoDestino, deposito, "Descripcion", "Id");
        }

        public override void EjecutarComandoModificar()
        {
            base.EjecutarComandoModificar();

            var depositoOrigen = _depositoServicio.Obtener(cmbDepositoOrigen.Text).FirstOrDefault();

            _articuloServicio.TransferirArticulo(new StockDepositoDto
            {
                Id = depositoOrigen.Id,
                Cantidad = nudOrigen.Value - nudDestino.Value
            }, new StockDepositoDto
            {
                Id = (long)cmbDepositoDestino.SelectedValue,
                Deposito = cmbDepositoDestino.Text,
                Cantidad = nudDestino.Value,
                Eliminado = false,
            }, EntidadId.Value);

            Close();
        }

        private void cmbDepositoOrigen_SelectionChangeCommitted(object sender, System.EventArgs e)
        {
            CargarCampos();
        }

        public override bool VerificarDatosObligatorios()
        {
            if (nudDestino.Value == 0)
            {
                MessageBox.Show("El valor de destino no puede ser 0");
                return false;
            }
            return true;
        }
    }
}
