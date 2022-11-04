using IServicios.Persona;
using IServicios.Persona.DTOs;
using PresentacionBase.Formularios;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Presentacion.Core.Empleado
{
    public partial class _00007_Empleado : FormConsulta
    {
        private readonly IEmpleadoServicio _empleadoServicio;

        public _00007_Empleado(IEmpleadoServicio empleadoServicio, IPersonaServicio personaServicio)
        {
            InitializeComponent();

            _empleadoServicio = empleadoServicio;

            AsignarEvento_EnterLeave(this);
        }

        public override void ActualizarDatos(DataGridView dgv, string cadenaBuscar)
        {
            dgv.DataSource = (List<EmpleadoDto>)_empleadoServicio.Obtener(typeof(EmpleadoDto), !string.IsNullOrEmpty(cadenaBuscar) ? cadenaBuscar : string.Empty);//+CONSULT4R+

            FormatearGrilla(dgv);
        }

        public override void FormatearGrilla(DataGridView dgv)
        {
            base.FormatearGrilla(dgv); // Pongo Invisible las Columnas

            dgv.Columns["Legajo"].Visible = true;
            dgv.Columns["Legajo"].Width = 80;
            dgv.Columns["Legajo"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.Columns["Legajo"].HeaderText = "Legajo";

            dgv.Columns["ApyNom"].Visible = true;
            dgv.Columns["ApyNom"].HeaderText = "Apellido y Nombre";
            dgv.Columns["ApyNom"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            dgv.Columns["Dni"].Visible = true;
            dgv.Columns["Dni"].Width = 80;
            dgv.Columns["Dni"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.Columns["Dni"].HeaderText = "DNI";
        }

        public override bool EjecutarComando(TipoOperacion tipoOperacion, long? id = null)
        {
            var formulario = new _00008_Abm_Empleado(tipoOperacion, id);

            formulario.ShowDialog();

            return formulario.RealizoAlgunaOperacion;
        }
    }
}
