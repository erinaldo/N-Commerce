using Aplicacion.Constantes;
using Presentacion.Core.Articulo;
using Presentacion.Core.Caja;
using Presentacion.Core.Cliente;
using Presentacion.Core.Comprobantes;
using Presentacion.Core.CondicionIva;
using Presentacion.Core.Configuracion;
using Presentacion.Core.Departamento;
using Presentacion.Core.Deposito;
using Presentacion.Core.Empleado;
using Presentacion.Core.FormaPago;
using Presentacion.Core.Localidad;
using Presentacion.Core.Proveedor;
using Presentacion.Core.Provincia;
using Presentacion.Core.Usuario;
using PresentacionBase.Formularios;
using StructureMap;
using System;
using System.Windows.Forms;

namespace CommerceApp
{
    public partial class Principal : Form
    {
        private bool actualizarImagen;
        private bool esAdministrador;
        public Principal()
        {
            InitializeComponent();

            lblApellido.Text = $"{Identidad.Apellido}";
            lblNombre.Text = $"{Identidad.Nombre}";
            imgFotoUsuarioLogin.Image = Imagen.ConvertirImagen(Identidad.Foto);
            actualizarImagen = false;
            esAdministrador = false;
            if (Identidad.Apellido == "Administrador") esAdministrador = true;
            if (!esAdministrador) imgFotoUsuarioLogin.Visible = true;
        }


        private void Principal_FormClosed(object sender, FormClosedEventArgs e)
        {
  
        }

        /*
        =======================================================
                               Aplicacion
        =======================================================
        */

        private void configuraciónToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ObjectFactory.GetInstance<_00012_Configuracion>().ShowDialog();
        }

        private void salirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        /*
        =======================================================
                                  ABM
        =======================================================
        */

        private void articuloToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new _00018_Abm_Articulo(TipoOperacion.Nuevo).ShowDialog();
        }

        private void bajaDeArticuloToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new _00030_Abm_BajaArticulos(TipoOperacion.Nuevo).ShowDialog();
        }

        private void rubroToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new _00020_Abm_Rubro(TipoOperacion.Nuevo).ShowDialog();
        }

        private void marcaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new _00022_Abm_Marca(TipoOperacion.Nuevo).ShowDialog();
        }

        private void unidadDeMedidaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new _00024_Abm_UnidadDeMedida(TipoOperacion.Nuevo).ShowDialog();
        }

        private void bancoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new _00048_Abm_Banco(TipoOperacion.Nuevo).ShowDialog();
        }

        private void conceptoDeGastosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new _00042_Abm_ConceptoGastos(TipoOperacion.Nuevo).ShowDialog();
        }

        private void condicionDeIVAToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new _00014_Abm_CondicionIva(TipoOperacion.Nuevo).ShowDialog();
        }

        private void gastosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new _00044_Abm_Gastos(TipoOperacion.Nuevo).ShowDialog();
        }

        private void iVAToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            var form = new _00026_Abm_Iva(TipoOperacion.Nuevo).ShowDialog();
        }

        private void listaDePreciosToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            var form = new _00033_Abm_ListaPrecio(TipoOperacion.Nuevo).ShowDialog();
        }

        private void proveedorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new _00016_Abm_Proveedor(TipoOperacion.Nuevo).ShowDialog();
        }

        private void tarjetaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new _00046_Abm_tarjeta(TipoOperacion.Nuevo).ShowDialog();
        }

        private void clienteToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            var form = new _00010_Abm_Cliente(TipoOperacion.Nuevo).ShowDialog();
        }

        private void empleadoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new _00008_Abm_Empleado(TipoOperacion.Nuevo).ShowDialog();
        }

        private void usuarioToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ObjectFactory.GetInstance<_00011_Usuario>().ShowDialog();
        }

        private void depositoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new _00055_Abm_Deposito(TipoOperacion.Nuevo).ShowDialog();
        }

        private void motivoDeBajaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new _00028_Abm_MotivoBaja(TipoOperacion.Nuevo).ShowDialog();
        }

        private void puestoDeTrabajoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new _00052_Abm_PuestoTrabajo(TipoOperacion.Nuevo).ShowDialog();
        }

        private void departamentoToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            var form = new _00004_Abm_Departamento(TipoOperacion.Nuevo).ShowDialog();
        }

        private void localidadToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            var form = new _00006_AbmLocalidad(TipoOperacion.Nuevo).ShowDialog();
        }

        private void provinciaToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            var form = new _00002_Abm_Provincia(TipoOperacion.Nuevo).ShowDialog();
        }

        /*
        =======================================================
                                Consultas
        =======================================================
        */

        private void articulosToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            ObjectFactory.GetInstance<_00017_Articulo>().ShowDialog();
        }

        private void bajasDeArticulosToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ObjectFactory.GetInstance<_00029_BajaDeArticulos>().ShowDialog();
        }

        private void rubrosToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ObjectFactory.GetInstance<_00019_Rubro>().ShowDialog();
        }

        private void marcasToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ObjectFactory.GetInstance<_00021_Marca>().ShowDialog();
        }

        private void unidadesDeMedidasToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ObjectFactory.GetInstance<_00023_UnidadDeMedida>().ShowDialog();
        }

        private void actualizarPreciosToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ObjectFactory.GetInstance<_00031_ActualizarPrecios>().ShowDialog();
        }

        private void bancosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ObjectFactory.GetInstance<_00047_Banco>().ShowDialog();
        }

        private void condicionDeIVAToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ObjectFactory.GetInstance<_00013_CondicionIva>().ShowDialog();
        }

        private void gastosToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ObjectFactory.GetInstance<_00043_Gastos>().ShowDialog();
        }

        private void iVAToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            ObjectFactory.GetInstance<_00025_Iva>().ShowDialog();
        }

        private void listasDePreciosToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ObjectFactory.GetInstance<_00032_ListaPrecio>().ShowDialog();
        }

        private void proveedorToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ObjectFactory.GetInstance<_00015_Proveedor>().ShowDialog();
        }

        private void tarjetasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ObjectFactory.GetInstance<_00045_Tarjeta>().ShowDialog();
        }

        private void clientesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ObjectFactory.GetInstance<_00009_Cliente>().ShowDialog();
        }

        private void empleadoToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ObjectFactory.GetInstance<_00007_Empleado>().ShowDialog();
        }

        private void usuariosToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            ObjectFactory.GetInstance<_00011_Usuario>().ShowDialog();
        }

        private void depositosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ObjectFactory.GetInstance<_00054_Deposito>().ShowDialog();
        }

        private void motivoDeBajasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ObjectFactory.GetInstance<_00027_MotivoBaja>().ShowDialog();
        }

        private void puestosDeTrabajosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ObjectFactory.GetInstance<_00051_PuestoTrabajo>().ShowDialog();
        }

        private void cajaToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (!esAdministrador)
            {
                ObjectFactory.GetInstance<_00038_Caja>().ShowDialog();
            }
            else
            {
                MessageBox.Show("Acceso denegado.");
            }
        }

        private void aperturaDeCajaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ObjectFactory.GetInstance<_00039_AperturaCaja>().ShowDialog();
        }

        private void cierreDeCajaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ObjectFactory.GetInstance<_00040_CierreCaja>().ShowDialog();
        }

        private void conceptoDeGastosToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ObjectFactory.GetInstance<_00041_ConceptoGastos>().ShowDialog();
        }

        private void departamentoToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            ObjectFactory.GetInstance<_00003_Departamento>().ShowDialog();
        }

        private void localidadesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ObjectFactory.GetInstance<_00005_Localidad>().ShowDialog();
        }

        private void provinciasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ObjectFactory.GetInstance<_00001_Provincia>().ShowDialog();
        }

        private void cuentaCorrienteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!esAdministrador)
            {
                ObjectFactory.GetInstance<_00034_ClienteCtaCte>().ShowDialog();
            }
            else
            {
                MessageBox.Show("Acceso denegado.");
            }
        }

        private void cobroDiferidoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!esAdministrador)
            {
                ObjectFactory.GetInstance<_00049_CobroDiferido>().Show();
            }
            else
            {
                MessageBox.Show("Acceso denegado.");
            }
        }


        /*
        =======================================================
                                Comprobantes
        =======================================================
        */

        private void comprasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ObjectFactory.GetInstance<_00053_Compra>().ShowDialog();
        }

        private void puestosDeTrabajosToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ObjectFactory.GetInstance<_00051_PuestoTrabajo>().ShowDialog();
        }

        private void ventasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!esAdministrador)
            {
                ObjectFactory.GetInstance<_00050_Venta>().Show();
            }
            else
            {
                MessageBox.Show("Acceso denegado.");
            }
            
        }

        private void cerrarSesionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Identidad.Limpiar();

            lblApellido.Text = string.Empty;
            lblNombre.Text = string.Empty;
            imgFotoUsuarioLogin.Image = null;
            imgFotoUsuarioLogin.Visible = false;
            esAdministrador = false;

            var flogin = ObjectFactory.GetInstance<Login>();
            flogin.ShowDialog();

            if (!flogin.PuedeAccederAlSistema)
            {
                Application.Exit();
            }
            else
            {
                if (Identidad.Apellido == "Administrador") esAdministrador = true;
                if (!esAdministrador) imgFotoUsuarioLogin.Visible = true;

                lblApellido.Text = $"{Identidad.Apellido}";
                lblNombre.Text = $"{Identidad.Nombre}";
                imgFotoUsuarioLogin.Image = Imagen.ConvertirImagen(Identidad.Foto);
                actualizarImagen = true;
            }
        }

        private void hijoReloj_Tick(object sender, EventArgs e)
        {
            lblHora.Text = $@"{DateTime.Now.Hour:00}:{DateTime.Now.Minute:00}:{DateTime.Now.Second:00}";

            lblFecha.Text = $@"{DateTime.Now.ToLongDateString()}";

            if (actualizarImagen) imgFotoUsuarioLogin.Image = Imagen.ConvertirImagen(Identidad.Foto);
        }

    }
}
