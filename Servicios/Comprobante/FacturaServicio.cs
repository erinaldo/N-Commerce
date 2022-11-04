using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Aplicacion.Constantes;
using Dominio.UnidadDeTrabajo;
using IServicios.Comprobante;
using IServicios.Comprobante.DTOs;
using IServicios.Persona.DTOs;

namespace Servicios.Comprobante
{
    public class FacturaServicio : ComprobanteServicio, IFacturaServicio
    {
        public FacturaServicio(IUnidadDeTrabajo unidadDeTrabajo)
            : base(unidadDeTrabajo)
        {
        }


        public IEnumerable<ComprobantePendienteDto> ObtenerPendientesPago()
        {
            return _unidadDeTrabajo.FacturaRepositorio.Obtener(x => !x.EstaEliminado && x.Estado == Estado.Pendiente, "Cliente, DetalleComprobantes")
                .Select(x => new ComprobantePendienteDto
                {
                    Id = x.Id,
                    Cliente = new ClienteDto
                    {
                        Eliminado = x.EstaEliminado,
                        Id = x.Cliente.Id,
                        Dni = x.Cliente.Dni,
                        Nombre = x.Cliente.Nombre,
                        Apellido = x.Cliente.Apellido,
                        Telefono = x.Cliente.Telefono,
                        Direccion = x.Cliente.Direccion,
                        ActivarCtaCte = x.Cliente.ActivarCtaCte,
                        TieneLimiteCompra = x.Cliente.TieneLimiteCompra,
                        MontoMaximoCtaCte = x.Cliente.MontoMaximoCtaCte,
                    },
                    ClienteApyNom = $"{x.Cliente.Apellido} {x.Cliente.Nombre}",
                    Fecha = x.Fecha,
                    Direccion = x.Cliente.Direccion,
                    Dni = x.Cliente.Dni,
                    Telefono = x.Cliente.Telefono,
                    MontoPagar = x.Total,
                    Numero = x.Numero,
                    TipoComprobante = x.TipoComprobante,
                    Eliminado = x.EstaEliminado,
                    Items = x.DetalleComprobantes.Select(d=> new DetallePendienteDto
                    {
                        Id = d.Id,
                        SubTotal = d.SubTotal,
                        Precio = d.Precio,
                        Descripcion = d.Descripcion,
                        Cantidad = d.Cantidad,
                        Eliminado = d.EstaEliminado,
                    }).ToList()
                })
                .OrderByDescending(x => x.Fecha).ToList();
        }
    }
}
