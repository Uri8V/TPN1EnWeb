using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPN1EnWeb.Entidades;

namespace TPN1EnWeb.Datos.Interfaces
{
    public interface ISportRepository:IGenericRepository<Sport>
    {
        void Editar(Sport sport);
        bool EstaRelacionado(Sport sport);
        bool Existe(Sport? sport);
        int GetCantidad();
    }
}
