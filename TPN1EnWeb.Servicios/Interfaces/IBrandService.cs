using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TPN1EnWeb.Entidades;

namespace TPN1EnWeb.Servicios.Interfaces
{
    public interface IBrandService
    {
        void Guardar(Brand brand);
        void Borrar(Brand brand);
        bool EstaRelacionado(Brand brand);
        bool Existe(Brand? brand);
        int GetCantidad();
        IEnumerable<Brand>? GetBrands(Expression<Func<Brand, bool>>? filter = null,
           Func<IQueryable<Brand>, IOrderedQueryable<Brand>>? orderBy = null,
           string? propertiesNames = null);
        Brand? GetBrand(Expression<Func<Brand, bool>>? filter = null,
           string? propertiesNames = null, bool tracked=true);
    }
}
