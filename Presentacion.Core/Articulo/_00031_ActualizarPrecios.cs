using IServicios.Articulo;
using IServicios.ListaPrecio;
using IServicios.Marca;
using IServicios.Precio;
using IServicios.Rubro;
using PresentacionBase.Formularios;
using System;
using System.Windows.Forms;

namespace Presentacion.Core.Articulo
{
    public partial class _00031_ActualizarPrecios : FormBase
    {
        private readonly IMarcaServicio _marcaServicio;
        private readonly IRubroServicio _rubroServicio;
        private readonly IArticuloServicio _articuloServicio;
        private readonly IListaPrecioServicio _listaPrecioServicio;
        private readonly IPrecioServicio _precioServicio;

        public _00031_ActualizarPrecios(IPrecioServicio precioServicio, IMarcaServicio marcaServicio, IRubroServicio rubroServicio, IArticuloServicio articuloServicio, IListaPrecioServicio listaPrecioServicio)
        {
            InitializeComponent();

            _marcaServicio = marcaServicio;
            _rubroServicio = rubroServicio;
            _listaPrecioServicio = listaPrecioServicio;
            _precioServicio = precioServicio;

            PoblarComboBox(cmbMarca, _marcaServicio.Obtener(string.Empty), "Descripcion", "Id");
            PoblarComboBox(cmbRubro, _rubroServicio.Obtener(string.Empty), "Descripcion", "Id");
            PoblarComboBox(cmbListaPrecio, _listaPrecioServicio.Obtener(string.Empty), "Descripcion", "Id");
        }

        private void _00031_ActualizarPrecios_Load(object sender, System.EventArgs e)
        {
            cmbMarca.Enabled = false;
            cmbRubro.Enabled = false;
            cmbListaPrecio.Enabled = false;
            nudCodigoDesde.Enabled = false;
            nudCodigoHasta.Enabled = false;
        }

        private void chkMarca_CheckedChanged(object sender, System.EventArgs e)
        {
            if (chkMarca.Checked)
            {
                if (cmbMarca.SelectedValue == null)
                {
                    MessageBox.Show("El campo no se puede habilitar ya que no tiene asociada ninguna lista.");
                    chkMarca.Checked = false;
                    return;
                }
                else
                {
                    cmbMarca.Enabled = true;
                }  
            }
            else
            {
                cmbMarca.Enabled = false;
            }
        }

        private void chkRubro_CheckedChanged(object sender, System.EventArgs e)
        {
            if (chkRubro.Checked)
            {
                if (cmbRubro.SelectedValue == null)
                {
                    MessageBox.Show("El campo no se puede habilitar ya que no tiene asociada ninguna lista.");
                    chkRubro.Checked = false;
                    return;
                }
                else
                {
                    cmbRubro.Enabled = true;
                }
            }
            else
            {
                cmbRubro.Enabled = false;
            }
        }

        private void chkArticulo_CheckedChanged(object sender, System.EventArgs e)
        {
            if (chkCodigoDesde.Checked)
            {
                nudCodigoDesde.Enabled = true;            
            }
            else
            {
                nudCodigoDesde.Enabled = false;
                nudCodigoDesde.Value = 0;
            }
        }

        private void rdbPorcentaje_CheckedChanged(object sender, System.EventArgs e)
        {
            if (rdbPorcentaje.Checked)
            {
                RdbPrecio.Checked = false;
            }
        }

        private void RdbPrecio_CheckedChanged(object sender, System.EventArgs e)
        {
            if (RdbPrecio.Checked)
            {
                rdbPorcentaje.Checked = false;
            }
        }

        private void btnSalir_Click(object sender, System.EventArgs e)
        {
            Close();
        }

        private void chkListaPrecio_CheckedChanged(object sender, System.EventArgs e)
        {
            if (chkListaPrecio.Checked)
            {
                if (cmbListaPrecio.SelectedValue == null)
                {
                    MessageBox.Show("El campo no se puede habilitar ya que no tiene asociada ninguna lista.");
                    chkListaPrecio.Checked = false;
                    return;
                }
                else
                {
                    cmbListaPrecio.Enabled = true;
                }
                cmbListaPrecio.Enabled = true;
            }
            else
            {
                cmbListaPrecio.Enabled = false;
            }
        }

        private void btnEjecutar_Click(object sender, System.EventArgs e)
        {
            //===================creacion de variables===================

            long? marca = null;
            long? rubro = null;
            long? listaPrecio = null;
            int? desde = null;
            int? hasta = null;
            DateTime fechaActualizacion;
            bool esPorcentaje = rdbPorcentaje.Checked;
            decimal valor = nudValor.Value;

            //===================validaciones previas===================

            if (dtpFecha.Value < DateTime.Today)
            {
                MessageBox.Show("La fecha seleccionada debe ser posterior a la actual.");
                dtpFecha.Focus();
                return;
            }

            fechaActualizacion = dtpFecha.Value;

            if (chkMarca.Checked) marca = (long)cmbMarca.SelectedValue;

            if (chkRubro.Checked) rubro = (long)cmbRubro.SelectedValue;

            if (chkListaPrecio.Checked) listaPrecio = (long)cmbListaPrecio.SelectedValue;

            

            if (chkCodigoDesde.Checked && chkCodigoHasta.Checked)
            {
                if (nudCodigoHasta.Value < nudCodigoDesde.Value)
                {
                    MessageBox.Show("El campo 'Codigo hasta' no puede ser menor al campo 'Codigo desde'.");
                    nudCodigoHasta.Focus();
                    return;
                }
            }
            else
            {
                if (chkCodigoDesde.Checked && nudCodigoDesde.Value == 0)
                {
                    MessageBox.Show("El campo 'Codigo desde' no puede ser cero.");
                    nudCodigoDesde.Focus();
                    return;
                }
                else
                {
                    desde = (int)nudCodigoDesde.Value;
                }

                if (chkCodigoHasta.Checked && nudCodigoHasta.Value == 0)
                {
                    MessageBox.Show("El campo 'Codigo hasta' no puede ser cero.");
                    nudCodigoHasta.Focus();
                    return;
                }
                else
                {
                    desde = (int)nudCodigoDesde.Value;
                }
            }

            if (nudValor.Value == 0)
            {
                MessageBox.Show("Por favor, ingrese un valor distinto a cero.");
                nudValor.Focus();
                return;
            }

            //guarda los cambios

            _precioServicio.ActualizarPrecio(nudValor.Value, esPorcentaje, fechaActualizacion, marca, rubro, listaPrecio, desde, hasta);

            MessageBox.Show("los precios fueron actualizados correctamente.");

            btnLimpiar.PerformClick();
        }

        private void chkCodigoHasta_CheckedChanged(object sender, EventArgs e)
        {
            if (chkCodigoHasta.Checked)
            {
                nudCodigoHasta.Enabled = true;
            }
            else
            {
                nudCodigoHasta.Enabled = false;
                nudCodigoHasta.Value = 0;
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarControles(this);
            dtpFecha.Value = DateTime.Now;
            chkMarca.Checked = false;
            chkRubro.Checked = false;
            chkCodigoDesde.Checked = false;
            chkCodigoHasta.Checked = false;
            chkListaPrecio.Checked = false;
            rdbPorcentaje.Checked=true;
            nudValor.Value = 0;
        }
    }
}
