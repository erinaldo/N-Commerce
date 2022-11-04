using Aplicacion.Constantes;
using IServicios.Seguridad;
using PresentacionBase.Formularios;
using System;
using System.Windows.Forms;

namespace CommerceApp
{
    public partial class Login : FormBase
    {
        private readonly ISeguridadServicio _seguridadServicio;
        public bool PuedeAccederAlSistema { get; private set; }

        public Login(ISeguridadServicio seguridadServicio)
        {
            InitializeComponent();

            _seguridadServicio = seguridadServicio;

            AsignarEvento_EnterLeave(this);

            txtUsuario.Text = "fbazan";
            txtPassword.Text = "P$assword123";
        }

        private void btnCancelar_Click(object sender, System.EventArgs e)
        {
            Application.Exit();
        }

        private void btnIngresar_Click(object sender, System.EventArgs e)
        {
            try
            {
                if (UsuarioAdmin.Usuario == txtUsuario.Text && UsuarioAdmin.Password == txtPassword.Text)
                {
                    Identidad.Apellido = "Adminitrador";

                    PuedeAccederAlSistema = true;

                    this.Close();

                    return;
                }

                var puedeAcceder = _seguridadServicio.VerificarAcceso(txtUsuario.Text, txtPassword.Text);

                if (puedeAcceder)
                {
                    var usuarioLogin = _seguridadServicio.ObtenerUsuarioLogin(txtUsuario.Text);

                    if (usuarioLogin == null) throw new Exception("No se pudo obtener el usuario.");

                    if (usuarioLogin.EstaBloqueado)
                    {
                        MessageBox.Show($"El usuario {usuarioLogin.ApyNomEmpleado} esta bloqueado.", @"Atención", MessageBoxButtons.OK, MessageBoxIcon.Stop);

                        txtUsuario.Clear();
                        txtPassword.Clear();
                        txtUsuario.Focus();

                        return;
                    }

                    Identidad.EmpleadoId = usuarioLogin.EmpleadoId;
                    Identidad.Nombre = usuarioLogin.NombreEmpleado;
                    Identidad.Apellido = usuarioLogin.ApellidoEmpleado;
                    Identidad.Foto = usuarioLogin.FotoEmpleado;

                    Identidad.UsuarioId = usuarioLogin.Id;
                    Identidad.Usuario = usuarioLogin.NombreUsuario;

                    PuedeAccederAlSistema = puedeAcceder;

                    this.Close();
                }
                else
                {
                    MessageBox.Show("Credenciales invalidas");

                    txtUsuario.Clear();
                    txtPassword.Clear();
                    txtUsuario.Focus();
                }


            }
            catch (System.Exception)
            {

                throw;
            }


            //PuedeAccederAlSistema = _seguridadServicio.VerificarAcceso(txtUsuario.Text, txtPassword.Text);

            //if (PuedeAccederAlSistema)
            //{
            //    MessageBox.Show("Puede acceder al sistema;");
            //}
            //else
            //{
            //    MessageBox.Show("Por favor, verifique los datos de acceso.");
            //    //return;
            //}
            //this.Close();
        }

        private void txtUsuario_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                txtPassword.Focus();
            }
            e.Handled = false;
        }

        private void txtPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                btnIngresar.PerformClick();
            }
            e.Handled = false;
        }
    }
}
