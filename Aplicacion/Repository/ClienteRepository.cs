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
    public class ClienteRepository : Generic<Cliente>, ICliente
    {
        private readonly JardineriaContext _context;
        public ClienteRepository(JardineriaContext context) : base(context)
        {
            _context = context;
        }
        public override async Task<IEnumerable<Cliente>> GetAllAsync()
        {
            return await _context.Clientes
                .ToListAsync();
        }

        public async Task<IEnumerable<object>> Consulta2()
        {
            var mensaje = "Nombre de los clientes que no han hecho pagos y el nombre de sus representantes junto con la ciudad de la oficina".ToUpper();

            var consulta = from cliente in _context.Clientes
                           join representante in _context.Empleados on cliente.CodigoEmpleadoRepVentas equals representante.CodigoEmpleado
                           join oficina in _context.Oficinas on representante.CodigoOficina equals oficina.CodigoOficina
                           where !_context.Pagos.Any(pago => pago.CodigoCliente == cliente.CodigoCliente)
                           select new
                           {
                               cliente.NombreCliente,
                               Representante = new
                               {
                                   representante.Nombre,
                                   representante.Apellido1,
                                   Oficina = new
                                   {
                                       oficina.Ciudad

                                   }
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