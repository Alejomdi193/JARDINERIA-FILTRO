using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dominio.Entities;
using Dominio.Interfaces;
using Microsoft.EntityFrameworkCore;
using Persistencia;

namespace Aplicacion.Repository
{
    public class DetallePedidoRepository : Generic<DetallePedido>, IDetallePedido
    {
        private readonly JardineriaContext _context;
        public DetallePedidoRepository(JardineriaContext context) : base(context)
        {
            _context = context;
        }

        public override async Task<IEnumerable<DetallePedido>> GetAllAsync()
        {
            return await _context.DetallePedidos
                .ToListAsync();
        }


        public override async Task<DetallePedido> GetByIdx1Async(int codigoPedido, string codigoProducto)
        {
            return await _context.DetallePedidos
                .FirstOrDefaultAsync(p => p.CodigoPedido == codigoPedido && p.CodigoProducto == codigoProducto);
        }

        public async Task<IEnumerable<object>> Consulta4()
                {
                    var consulta = await (
                        from grupoDetalles in _context.DetallePedidos
                            .GroupBy(detalle => detalle.CodigoProducto)
                            .Select(grupoDetalles => new
                            {
                                CodigoProducto = grupoDetalles.Key,
                                UnidadesVendidas = grupoDetalles.Sum(detalle => detalle.Cantidad)
                            })
                            .OrderByDescending(resultado => resultado.UnidadesVendidas)
                            .Take(20)
                        join producto in _context.Productos
                            on grupoDetalles.CodigoProducto equals producto.CodigoProducto
                        select new
                        {
                            grupoDetalles.CodigoProducto,
                            producto.Nombre,
                            grupoDetalles.UnidadesVendidas
                        })
                        .ToListAsync();

                    return consulta;
                }

    public async Task<IEnumerable<object>> Consulta5()
            {
                var consulta = await _context.DetallePedidos
                    .GroupBy(detallePedido => detallePedido.CodigoProducto)
                    .ToListAsync();
                
                var _consulta =  consulta
                    .Join(
                        _context.Productos,
                        grupo => grupo.Key,
                        producto => producto.CodigoProducto,
                        (consulta, producto) => new
                        {
                            NombreProducto = producto.Nombre,
                            UnidadesVendidad = consulta.Sum(p => p.Cantidad),
                            TotalFacturado = consulta.Sum(p => p.Cantidad * p.PrecioUnidad),
                            TotalFacturadoIva = consulta.Sum(p => p.Cantidad * p.PrecioUnidad * (decimal)1.21)
                        })
                        .Where(resultado => resultado.TotalFacturadoIva > 3000)
                        .OrderByDescending(resultado => resultado.TotalFacturadoIva)
                        .ToList();

                    return _consulta;
                ;
            }



       
    }
}