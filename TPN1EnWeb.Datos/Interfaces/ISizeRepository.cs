using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPN1EnWeb.Entidades.DTOS;
using TPN1EnWeb.Entidades;

namespace TPN1EnWeb.Datos.Interfaces
{
    public interface ISizeRepository:IGenericRepository<Size>
    {
        void AgregarSizeShoe(ShoeSizes nuevarelacion);
        Size? GetSizePorId(int id, bool incluyeShoe = false);
        List<ShoeListDto> GetShoePoSize(Size size);
    }
}
