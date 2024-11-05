using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TPN1EnWeb.Entidades;

namespace TPN1EnWeb.Servicios.Interfaces
{
    public interface IShoeSizeService
    {
        public int GetId();
        public ShoeSizes GetIdShoeSize(int size, int shoe);
        IEnumerable<ShoeSizes>? GetShoeSizes(Expression<Func<ShoeSizes, bool>>? filter = null,
          Func<IQueryable<ShoeSizes>, IOrderedQueryable<ShoeSizes>>? orderBy = null,
          string? propertiesNames = null);
        ShoeSizes? GetShoeSize(Expression<Func<ShoeSizes, bool>>? filter = null,
           string? propertiesNames = null, bool tracked = true);
        void Editar(ShoeSizes shoeSizes);

    }
}
