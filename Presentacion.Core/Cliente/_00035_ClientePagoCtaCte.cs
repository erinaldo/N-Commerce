using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Aplicacion.Constantes;
using IServicios.Comprobante.DTOs;
using IServicios.CuentaCorriente;
using IServicios.Persona.DTOs;
using PresentacionBase.Formularios;
using Servicios.CuentaCorriente;
using StructureMap;

namespace Presentacion.Core.Cliente
{
    public partial class _00035_ClientePagoCtaCte : FormBase
    {
        private readonly ClienteDto _clienteDto;
        private readonly ICuentaCorrienteServicio _cuentaCorrienteServicio; //revisar
        private decimal montoDeuda;
        public bool RealizoPago { get; set; }

        public _00035_ClientePagoCtaCte(ClienteDto clienteDto)
        {
            InitializeComponent();

            _clienteDto = clienteDto;
            _cuentaCorrienteServicio = ObjectFactory.GetInstance<ICuentaCorrienteServicio>();
            RealizoPago = false;
            montoDeuda = 0;
        }

        private void _00035_ClientePagoCtaCte_Load(object sender, System.EventArgs e)
        {
            if (_clienteDto != null)
            {
                var deuda = _cuentaCorrienteServicio.ObtenerDeudaCliente(_clienteDto.Id);

                montoDeuda = deuda >= 0 ? deuda : (deuda * -1);

                txtMontoDeuda.Text = montoDeuda.ToString("C");

                nudMontoPagar.Select(0, nudMontoPagar.Text.Length);
            }
        }

        private void btnSalir_Click(object sender, System.EventArgs e)
        {
            RealizoPago = false;
            Close();
        }

        private void btnLimpiar_Click(object sender, System.EventArgs e)
        {
            nudMontoPagar.Value = 0;
            nudMontoPagar.Select(0,nudMontoPagar.Text.Length);
        }

        private void btnPagar_Click(object sender, System.EventArgs e)
        {
            try
            {
                if (nudMontoPagar.Value > 0)
                {
                    if (nudMontoPagar.Value > montoDeuda)
                    {
                        var msj = "El monto a pagar es mayor al adeudado."
                                      + Environment.NewLine
                                      + "¿Desea realizar el pago?";

                        if (MessageBox.Show(msj, "Atencion", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) ==
                            DialogResult.Cancel) return;
                    }

                    var nuevoComprobante = new CtaCteComprobanteDto
                    {
                        ClienteId = _clienteDto.Id,
                        Descuento = 0,
                        SubTotal = nudMontoPagar.Value,
                        Total = nudMontoPagar.Value,
                        EmpleadoId = Identidad.EmpleadoId,
                        UsuarioId = Identidad.UsuarioId,
                        Fecha = DateTime.Now,
                        Iva105 = 0,
                        Iva21 = 0,
                        TipoComprobante = TipoComprobante.CuentaCorriente,
                        FormasDePagos = new List<FormaPagoDto>(),
                        Items = new List<DetalleComprobanteDto>(),
                        Eliminado = false
                    };

                    nuevoComprobante.FormasDePagos.Add(new FormaPagoCtaCteDto
                    {
                        ClienteId = _clienteDto.Id,
                        Monto = nudMontoPagar.Value,
                        TipoPago = TipoPago.CtaCte,
                        Eliminado = false
                    });

                    _cuentaCorrienteServicio.Pagar(nuevoComprobante); //TODO: ver con servicio cuentaCorrienteServicio
                    MessageBox.Show("Los datos se grabaron correctamente.");
                    Close();
                }
                else
                {
                    MessageBox.Show("Debe ingresar un monto mayor a cero.");
                    nudMontoPagar.Select(0, nudMontoPagar.Text.Length);
                }

            }
            catch (Exception)
            {
                MessageBox.Show("Ocurrio un error al realizar el pago.");//TODO: asdasd

                Close();
            }
        }
    }
}
