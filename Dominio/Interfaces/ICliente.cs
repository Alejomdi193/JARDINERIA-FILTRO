using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dominio.Entities;

namespace Dominio.Interfaces
{
    public interface ICliente : IGeneric<Cliente>
    {
        
        Task<IEnumerable<object>> Consulta2();
    }
}