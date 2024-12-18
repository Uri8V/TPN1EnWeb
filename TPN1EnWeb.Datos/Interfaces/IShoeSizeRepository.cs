using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPN1EnWeb.Entidades;

namespace TPN1EnWeb.Datos.Interfaces
{
    public interface IShoeSizeRepository:IGenericRepository<ShoeSizes>
    {
        public ShoeSizes GetIdShoeSize(int size, int shoe);
        void Editar(ShoeSizes shoeSizes);
        bool ItsRelated(ShoeSizes shoeSizes);

    }
}
