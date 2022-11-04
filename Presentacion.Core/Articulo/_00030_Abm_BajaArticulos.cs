using IServicios.BajaArticulo;
using IServicios.BajaArticulo.DTOs;
using IServicios.MotivoBaja;
using PresentacionBase.Formularios;
using Servicios.MotivoBaja;
using StructureMap;
using System.Windows.Forms;
using static Aplicacion.Constantes.Imagen;

namespace Presentacion.Core.Articulo
{
    public partial class _00030_Abm_BajaArticulos : FormAbm
    {
        private readonly IBajaArticuloServicio _bajaArticuloServicio;
        private readonly IMotivoBajaServicio _motivoBajaServicio;
        public _00030_Abm_BajaArticulos(TipoOperacion tipoOperacion, long? entidadId = null)
            : base(tipoOperacion, entidadId)
        {
            InitializeComponent();

            _bajaArticuloServicio = ObjectFactory.GetInstance<IBajaArticuloServicio>();
            _motivoBajaServicio = ObjectFactory.GetInstance<IMotivoBajaServicio>();

            PoblarComboBox(cmbMotivoBaja, _motivoBajaServicio.Obtener(string.Empty, false), "Descripcion", "Id");
        }

        public override void CargarDatos(long? entidadId)
        {
            if (entidadId.HasValue)
            {
                var resultado = (BajaArticuloDto)_bajaArticuloServicio.Obtener(entidadId.Value);

                if (resultado == null)
                {
                    MessageBox.Show("Ocurrio un error al obtener el registro seleccionado");
                    Close();
                }

                txtArticulo.Text = resultado.ArticuloDescripcion;
                nudStockActual.Value = 0;
                nudStockActual.Value = 0;
                nudCantidadBaja.Value = resultado.Cantidad;
                txtObservacion.Text = resultado.Observacion;

                imgFotoArticulo.Image = ConvertirImagen(resultado.Foto);

                if (imgFotoArticulo.Image == null)
                {
                    imgFotoArticulo.Image = ImagenProductoPorDefecto;
                }

                if (TipoOperacion == TipoOperacion.Eliminar)
                    DesactivarControles(this);
            }
            else // Nuevo
            {
                LimpiarControles(this);
            }
        }

        private void btnBuscarArticulo_Click(object sender, System.EventArgs e)
        {
            ObjectFactory.GetInstance<ArticuloLookUp>().ShowDialog();
            this.CargarDatos(EntidadId);
        }
    }
}
