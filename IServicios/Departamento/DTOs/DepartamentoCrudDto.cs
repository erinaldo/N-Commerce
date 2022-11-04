using IServicios.BaseDto;

namespace IServicios.Departamento.DTOs
{
    public class DepartamentoCrudDto : DtoBase
    {
        public long ProvinciaId { get; set; }
        public string Descripcion { get; set; }
    }
}
