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
    public class PedidoRepository : Generic<Pedido>, IPedido
    {
        private readonly JardineriaContext _context;
        public PedidoRepository(JardineriaContext context) : base(context)
        {
            _context = context;
        }
        public override async Task<IEnumerable<Pedido>> GetAllAsync()
        {
            return await _context.Pedidos
                .ToListAsync();
        }

        public override async Task<Pedido> GetByIdAsync(int codigoPedido)
        {
            return await _context.Pedidos
                .FirstOrDefaultAsync(p => p.CodigoPedido == codigoPedido);
        }


        public async Task<object> Consulta1()
        {
            var consulta = from p in _context.Pedidos
                           where p.FechaEsperada < p.FechaEntrega
                           select new
                           {
                               CodigoDePedido = p.CodigoPedido,
                               CodigoDeCliente = p.CodigoCliente,
                               FechaEsperada = p.FechaEsperada,
                               FechaEntrega = p.FechaEntrega,
                               Informacion = "No fue entregado a tiempo"
                           };

            var resultado = await consulta.ToListAsync();
            return resultado;
        }


    }
}