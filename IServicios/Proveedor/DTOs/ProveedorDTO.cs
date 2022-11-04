using IServicios.BaseDto;

namespace IServicios.Proveedor.DTOs
{
    public class ProveedorDTO : DtoBase
    {
        public string RazonSocial { get; set; }
        public string CUIT { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
        public string Mail { get; set; }
        public string Localidad { get; set; }
        public long LocalidadId { get; set; }
        public string CondicionIva { get; set; }
        public long CondicionIvaId { get; set; }
    }
}
