using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPN1EnWeb.Entidades;

namespace TPN1EnWeb.Datos.Interfaces
{
    public interface IBrandRepository:IGenericRepository<Brand>
    {
        void Editar(Brand brand);
        bool EstaRelacionado(Brand brand);
        bool Existe(Brand? brand);
        int GetCantidad();
    }
}
