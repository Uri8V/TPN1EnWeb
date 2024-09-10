using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TPN1EnWeb.Entidades;
using TPN1EnWeb.Entidades.DTOS;

namespace TPN1EnWeb.Servicios.Interfaces
{
    public interface ISizeService
    {
        IEnumerable<Size> GetSizes(Expression<Func<Size, bool>>? filter = null,
           Func<IQueryable<Size>, IOrderedQueryable<Size>>? orderBy = null,
           string? propertiesNames = null);
        Size? GetSize(Expression<Func<Size, bool>>? filter = null,
           string? propertiesNames = null, bool tracked = true);
        List<ShoeListDto> GetShoePorSize(Size size);
        Size? GetSizePorId(int id, bool incluyeShoe = false);
    }
}
