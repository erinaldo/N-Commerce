using Aplicacion.IoC;
using StructureMap;
using System;
using System.Windows.Forms;
using Presentacion;

namespace CommerceApp
{
    static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        static void Main()
        {
            
            // Configuracion del Inyector (StructureMap)
            new StructureMapContainer().Configure();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var fLogin = ObjectFactory.GetInstance<Login>().ShowDialog();
            //var flogin = new Login();
            //flogin.ShowDialog();
            //var tof = fLogin.
            //if (fLogin.PuedeAccederAlSistema)
            //{
            Application.Run(ObjectFactory.GetInstance<Principal>());
            //}
            //else
            //{
            //    Application.Exit();
            //}
        }
    }
}
