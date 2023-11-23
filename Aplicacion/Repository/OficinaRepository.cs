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
    public class OficinaRepository : Generic<Oficina>, IOficina
    {
        private readonly JardineriaContext _context;
        public OficinaRepository(JardineriaContext context) : base(context)
        {
            _context = context;
        }

        public override async Task<IEnumerable<Oficina>> GetAllAsync()
        {
            return await _context.Oficinas
                .ToListAsync();
        }

        public override async Task<Oficina> GetByIdAsync(string codigoOficina)
        {
            return await _context.Oficinas
                .FirstOrDefaultAsync(p => p.CodigoOficina == codigoOficina);
        }

 







    }
}