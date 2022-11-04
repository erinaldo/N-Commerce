using System.Linq;
using System.Windows.Forms;
using Aplicacion.Constantes;
using IServicios.Caja;
using IServicios.Caja.DTOs;
using PresentacionBase.Formularios;
using StructureMap;

namespace Presentacion.Core.Caja
{
    public partial class _00040_CierreCaja : FormBase
    {
        private readonly long _cajaId;
        private readonly ICajaServicio _cajaServicio;
        private CajaDto _caja;
        public _00040_CierreCaja(long cajaId)
        {
            InitializeComponent();

            _cajaId = cajaId;

            _cajaServicio = ObjectFactory.GetInstance<ICajaServicio>();

            _caja = _cajaServicio.Obtener(cajaId);

            CargarDatos(_cajaId);
            
        }

        private void CargarDatos(long cajaId)
        {
            if (_caja == null)
            {
                MessageBox.Show("Ocurrio un error al obtener la caja");
            }

            txtCajaInicial.Text = _caja.MontoAperturaStr;
            txtFechaAperturaCaja.Text = _caja.FechaAperturaStr;

            nudDiferenciaEfectivo.Value = _caja.Detalles.Where(x => x.TipoPago == TipoPago.Efectivo).Sum(x => x.Monto) * -1;
            txtVentasEfectivo.Text = (nudDiferenciaEfectivo.Value * -1).ToString();

            nudDiferenciaCtaCte.Value = _caja.Detalles.Where(x => x.TipoPago == TipoPago.CtaCte).Sum(x => x.Monto) * -1;
            txtVentasCtaCte.Text = (nudDiferenciaCtaCte.Value * -1).ToString();

            nudDiferenciaCheque.Value = _caja.Detalles.Where(x => x.TipoPago == TipoPago.Cheque).Sum(x => x.Monto) * -1;
            txtVentasCheque.Text = (nudDiferenciaCheque.Value * -1).ToString();

            nudDiferenciaTarjeta.Value = _caja.Detalles.Where(x => x.TipoPago == TipoPago.Tarjeta).Sum(x => x.Monto) * -1;
            txtVentasTarjeta.Text = (nudDiferenciaTarjeta.Value * -1).ToString();
        }

        private void btnVentasEfectivo_Click(object sender, System.EventArgs e)
        {
            var comprobantes = _cajaServicio.Obtener(_cajaId, TipoMovimiento.Ingreso).Detalles.Where(x => x.TipoPago.Equals(TipoPago.Efectivo)).ToList();
            var detalle = new VerComprobantesCaja(comprobantes);
            detalle.ShowDialog();
        }

        private void btnVentasCtaCte_Click(object sender, System.EventArgs e)
        {
            var comprobantes = _cajaServicio.Obtener(_cajaId, TipoMovimiento.Ingreso).Detalles.Where(x => x.TipoPago.Equals(TipoPago.CtaCte)).ToList();
            var detalle = new VerComprobantesCaja(comprobantes);
            detalle.ShowDialog();
        }

        private void btnVentasTarjeta_Click(object sender, System.EventArgs e)
        {
            var comprobantes = _cajaServicio.Obtener(_cajaId, TipoMovimiento.Ingreso).Detalles.Where(x => x.TipoPago.Equals(TipoPago.Tarjeta)).ToList();
            var detalle = new VerComprobantesCaja(comprobantes);
            detalle.ShowDialog();
        }

        private void btnVentasCheque_Click(object sender, System.EventArgs e)
        {
            var comprobantes = _cajaServicio.Obtener(_cajaId, TipoMovimiento.Ingreso).Detalles.Where(x => x.TipoPago.Equals(TipoPago.Cheque)).ToList();
            var detalle = new VerComprobantesCaja(comprobantes);
            detalle.ShowDialog();
        }

        private void btnComprasEfectivo_Click(object sender, System.EventArgs e)
        {
            MessageBox.Show("No implementado aun");
        }

        private void btnComprasCtaCte_Click(object sender, System.EventArgs e)
        {
            MessageBox.Show("No implementado aun");
        }

        private void btnComprasTarjeta_Click(object sender, System.EventArgs e)
        {
            MessageBox.Show("No implementado aun");
        }

        private void btnComprasCheque_Click(object sender, System.EventArgs e)
        {
            MessageBox.Show("No implementado aun");
        }

        private void nudComprobantesEfectivo_ValueChanged(object sender, System.EventArgs e)
        {
            if (((NumericUpDown)sender).Name == "nudComprobantesEfectivo") nudDiferenciaEfectivo.Value = nudComprobantesEfectivo.Value - decimal.Parse(txtVentasEfectivo.Text);
            if (((NumericUpDown)sender).Name == "nudComprobantesCtaCte") nudDiferenciaCtaCte.Value = nudComprobantesCtaCte.Value - decimal.Parse(txtVentasCtaCte.Text);
            if (((NumericUpDown)sender).Name == "nudComprobantesCheque") nudDiferenciaCheque.Value = nudComprobantesCheque.Value - decimal.Parse(txtVentasCheque.Text);
            if (((NumericUpDown)sender).Name == "nudComprobantesTarjeta") nudDiferenciaTarjeta.Value = nudComprobantesTarjeta.Value - decimal.Parse(txtVentasTarjeta.Text);
            txtTotal.Text = (_caja.MontoApertura + nudComprobantesEfectivo.Value).ToString();
        }

        private void btnCierreCaja_Click(object sender, System.EventArgs e)
        {
            _caja.MontoCierre = decimal.Parse(txtTotal.Text);
            _caja.TotalEntradaCheque = decimal.Parse(txtVentasCheque.Text);
            _caja.TotalEntradaCtaCte = decimal.Parse(txtVentasCtaCte.Text);
            _caja.TotalEntradaTarjeta = decimal.Parse(txtVentasTarjeta.Text);
            _caja.TotalEntradaEfectivo = decimal.Parse(txtVentasEfectivo.Text);
            _caja.TotalSalidaCheque = nudComprobantesCheque.Value;
            _caja.TotalSalidaTarjeta = nudComprobantesTarjeta.Value;
            _caja.TotalSalidaCtaCte = nudComprobantesCtaCte.Value;
            _caja.TotalSalidaEfectivo = nudComprobantesEfectivo.Value;

            _cajaServicio.CerrarCaja(_caja);
        }
    }
}
