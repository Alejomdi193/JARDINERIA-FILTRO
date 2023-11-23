using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dominio.Entities;

namespace Dominio.Interfaces
{
    public interface IDetallePedido : IGeneric<DetallePedido>
    {

       Task<IEnumerable<object>> Consulta4();
       Task<IEnumerable<object>> Consulta5();
    }
}