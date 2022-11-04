using System;
using System.Windows.Forms;
using Aplicacion.Constantes;
using IServicios.Caja;
using IServicios.Configuracion;
using Servicios.Configuracion;

namespace Presentacion.Core.Caja
{
    public partial class _00039_AperturaCaja : Form
    {
        private readonly IConfiguracionServicio _configuracionServicio;
        private readonly ICajaServicio _cajaServicio;
        public bool RealizoApertura { get; set; }

        public _00039_AperturaCaja(ConfiguracionServicio ConfiguracionServicio, ICajaServicio CajaServicio)
        {
            InitializeComponent();
            _configuracionServicio = ConfiguracionServicio;
            _cajaServicio = CajaServicio;
        }

        private void _00039_AperturaCaja_Load(object sender, System.EventArgs e)
        {
            var configuracion = _configuracionServicio.Obtener();

            if (configuracion == null)
            {
                MessageBox.Show(@"Por favor debe cargar la configuración del sistema.");
                Close();
            }

            if (configuracion.IngresoManualCajaInicial)
            {
                nudMonto.Value = 0m;
                nudMonto.Select(0, nudMonto.Text.Length);
            }
            else
            {
                nudMonto.Value = _cajaServicio.ObtenerMontoCajaAnterior(Identidad.UsuarioId);
                nudMonto.Select(0, nudMonto.Text.Length);
            }
        }

        private void btnEjecutar_Click(object sender, System.EventArgs e)
        {
            try
            {
                _cajaServicio.AbrirCaja(Identidad.UsuarioId, nudMonto.Value);
                RealizoApertura = true;
                MessageBox.Show("La apertura de caja se ejecuto correctamente.");
                Close();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                RealizoApertura = false;
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            nudMonto.Value = 0m;
            nudMonto.Select(0,nudMonto.Text.Length);
        }
    }
}
