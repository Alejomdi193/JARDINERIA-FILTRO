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
    public class EmpleadoRepository : Generic<Empleado>, IEmpleado
    {
        private readonly JardineriaContext _context;
        public EmpleadoRepository(JardineriaContext context) : base(context)
        {
            _context = context;
        }
        public override async Task<IEnumerable<Empleado>> GetAllAsync()
        {
            return await _context.Empleados
                .ToListAsync();
        }

        public override async Task<Empleado> GetByIdAsync(int codigoEmpleado)
        {
            return await _context.Empleados
                .FirstOrDefaultAsync(p => p.CodigoEmpleado == codigoEmpleado);
        }



       
    }
}