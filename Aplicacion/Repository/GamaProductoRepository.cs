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
    public class GamaProductoRepository : Generic<GamaProducto>, IGamaProducto
    {
        private readonly JardineriaContext _context;
        public GamaProductoRepository(JardineriaContext context) : base(context)
        {
            _context = context;
        }

        public override async Task<IEnumerable<GamaProducto>> GetAllAsync()
        {
            return await _context.GamaProductos
                .ToListAsync();
        }

        public override async Task<GamaProducto> GetByIdAsync(string gama)
        {
            return await _context.GamaProductos
                .FirstOrDefaultAsync(p => p.Gama == gama);
        }

    }
}