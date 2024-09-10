using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPN1EnWeb.Entidades;

namespace TPN1EnWeb.Datos.Interfaces
{
    public interface IColorRepository:IGenericRepository<Colour>
    {
        void Editar(Colour Colour);
        bool EstaRelacionado(Colour Colour);
        bool Existe(Colour? Colour);
        int GetCantidad();
    }
}
