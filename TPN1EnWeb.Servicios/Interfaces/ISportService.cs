using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TPN1EnWeb.Entidades;

namespace TPN1EnWeb.Servicios.Interfaces
{
    public interface ISportService
    {
        void Borrar(Sport sport);
        void Guardar(Sport sport);
        bool EstaRelacionado(Sport sport);
        bool Existe(Sport? sport);
        int GetCantidad();
        IEnumerable<Sport>? GetSports(Expression<Func<Sport, bool>>? filter = null,
           Func<IQueryable<Sport>, IOrderedQueryable<Sport>>? orderBy = null,
           string? propertiesNames = null);
        Sport? GetSport(Expression<Func<Sport, bool>>? filter = null,
           string? propertiesNames = null, bool tracked = true);
    }
}
