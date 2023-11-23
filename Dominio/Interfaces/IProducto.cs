using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dominio.Entities;

namespace Dominio.Interfaces
{
    public interface IProducto : IGeneric<Producto>
    {
        Task<string> Consulta6();
        Task<IEnumerable<object>> Consulta10();
    }
}