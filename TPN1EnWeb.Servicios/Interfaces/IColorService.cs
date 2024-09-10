using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TPN1EnWeb.Entidades;

namespace TPN1EnWeb.Servicios.Interfaces
{
    public interface IColorService
    {
        void Guardar(Colour Colour);
        void Borrar(Colour Colour);
        bool EstaRelacionado(Colour Colour);
        bool Existe(Colour? Colour);
        int GetCantidad();
        IEnumerable<Colour>? GetColours(Expression<Func<Colour, bool>>? filter = null,
           Func<IQueryable<Colour>, IOrderedQueryable<Colour>>? orderBy = null,
           string? propertiesNames = null);
        Colour? GetColour(Expression<Func<Colour, bool>>? filter = null,
           string? propertiesNames = null, bool tracked = true);
    }
}
