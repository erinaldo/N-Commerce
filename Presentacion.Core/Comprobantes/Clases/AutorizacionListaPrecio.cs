using System;
using System.Windows.Forms;
using IServicios.Seguridad;

namespace Presentacion.Core.Comprobantes.Clases
{
    public partial class AutorizacionListaPrecio : Form
    {
        private readonly ISeguridadServicio _seguridad;

        private bool _tienePermiso;
        public bool PermisoAutorizado => _tienePermiso;

        public AutorizacionListaPrecio(ISeguridadServicio seguridad)
        {
            InitializeComponent();

            _seguridad = seguridad;
            _tienePermiso = false;
        }

        private void btnVerPassword_MouseDown(object sender, MouseEventArgs e)
        {
            txtPass.PasswordChar = Char.MinValue;
        }

        private void btnVerPassword_MouseUp(object sender, MouseEventArgs e)
        {
            txtPass.PasswordChar = Char.Parse("*");
        }

        private void btnIngresar_Click(object sender, EventArgs e)
        {
            try
            {
                _tienePermiso = _seguridad.VerificarAcceso(txtUser.Text, txtPass.Text);

                if (_tienePermiso)
                {
                    this.Close();
                }
                else
                {
                    MessageBox.Show("El usuario o el Password son Icorrectos");
                    txtPass.Clear();
                    txtPass.Focus();
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
                txtPass.Clear();
                txtPass.Focus();
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            _tienePermiso = false;
            Close();
        }
    }
}