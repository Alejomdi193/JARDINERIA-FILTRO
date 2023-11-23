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
    public class ProductoRepository : Generic<Producto>, IProducto
    {
        private readonly JardineriaContext _context;
        public ProductoRepository(JardineriaContext context) : base(context)
        {
            _context = context;
        }
        public override async Task<IEnumerable<Producto>> GetAllAsync()
        {
            return await _context.Productos
                .ToListAsync();
        }

        public override async Task<Producto> GetByIdAsync(string codigoProducto)
        {
            return await _context.Productos
                .FirstOrDefaultAsync(p => p.CodigoProducto == codigoProducto);
        }



        public async Task<string> Consulta6()
                {
                    var nombreProducto = await _context.Productos
                        .OrderByDescending(p => p.DetallePedidos.Sum(dp => dp.Cantidad))
                        .Select(p => p.Nombre)
                        .FirstOrDefaultAsync();

                    return nombreProducto ?? "No hay productos vendidos";
                }


        public async Task<IEnumerable<object>> Consulta10()
        {
            var mensaje = "Productos que nunca han aparecido en un pedido".ToUpper();

            var consulta = from producto in _context.Productos
                           join detallePedido in _context.DetallePedidos on producto.CodigoProducto equals detallePedido.CodigoProducto into detallesPedidosJoin
                           from detallePedido in detallesPedidosJoin.DefaultIfEmpty()
                           where detallePedido == null
                           select new
                           {
                               Producto = new
                               {
                                   Nombre = producto.Nombre,
                                   Descripcion = producto.Descripcion,
                                   Imagen = producto.GamaNavigation.Image  // Utilizamos la propiedad Image de GamaProducto
                               }
                           };

            var resultado = new List<object>
    {
        new { Informacion = mensaje, Resultado = await consulta.ToListAsync() }
    };

            return resultado;
        }

      
    }
}