using IServicios.Persona;
using IServicios.Persona.DTOs;
using PresentacionBase.Formularios;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Presentacion.Core.Cliente
{
    public partial class _00009_Cliente : FormConsulta
    {
        private readonly IClienteServicio _clienteServicio;
        private readonly IPersonaServicio _personaServicio;
        public _00009_Cliente(IClienteServicio clienteServicio, IPersonaServicio personaServicio)
        {
            InitializeComponent();

            _clienteServicio = clienteServicio;
            _personaServicio = personaServicio;
        }

        public override void ActualizarDatos(DataGridView dgv, string cadenaBuscar)
        {
            var resultado = (List<ClienteDto>)_personaServicio.Obtener(typeof(ClienteDto), !string.IsNullOrEmpty(cadenaBuscar) ? cadenaBuscar : string.Empty);

            //quitamos a consumidor final
            dgv.DataSource = resultado.Where(x => x.Dni != Aplicacion.Constantes.Cliente.ConsumidorFinal).ToList(); 

            base.ActualizarDatos(dgv, cadenaBuscar);
        }

        public override void FormatearGrilla(DataGridView dgv)
        {
            base.FormatearGrilla(dgv);

            dgv.Columns["ApyNom"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgv.Columns["ApyNom"].HeaderText = "Apellido y nombre";
            dgv.Columns["ApyNom"].Visible = true;
            dgv.Columns["ApyNom"].DisplayIndex = 1;

            dgv.Columns["Dni"].Width = 70;
            dgv.Columns["Dni"].HeaderText = "DNI";
            dgv.Columns["Dni"].Visible = true;
            dgv.Columns["Dni"].DisplayIndex = 2;

            dgv.Columns["Telefono"].Width = 100;
            dgv.Columns["Telefono"].HeaderText = "Telefono";
            dgv.Columns["Telefono"].Visible = true;
            dgv.Columns["Telefono"].DisplayIndex = 3;

            dgv.Columns["EliminadoStr"].Width = 60;
            dgv.Columns["EliminadoStr"].HeaderText = "Eliminado";
            dgv.Columns["EliminadoStr"].Visible = true;
            dgv.Columns["EliminadoStr"].DisplayIndex = 4;
        }

        public override bool EjecutarComando(TipoOperacion tipoOperacion, long? id = null)
        {
            var formulario = new _00010_Abm_Cliente(tipoOperacion, id);
            formulario.ShowDialog();
            return formulario.RealizoAlgunaOperacion;
        }
    }
}
