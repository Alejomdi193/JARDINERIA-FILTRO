using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dominio.Entities;
using Dominio.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using Persistencia;

namespace Aplicacion.Repository
{
    public class PagoRepository : Generic<Pago>, IPago
    {
        private readonly JardineriaContext _context;
        public PagoRepository(JardineriaContext context) : base(context)
        {
            _context = context;
        }
        public override async Task<IEnumerable<Pago>> GetAllAsync()
        {
            return await _context.Pagos
                .ToListAsync();
        }

        public override async Task<Pago> GetByIdx2Async(int codigoCliente, string idTransaccion)
        {
            return await _context.Pagos
                .FirstOrDefaultAsync(p => p.CodigoCliente == codigoCliente && p.IdTransaccion == idTransaccion);
        }



     


    }
}