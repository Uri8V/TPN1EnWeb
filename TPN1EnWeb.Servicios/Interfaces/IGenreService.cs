using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TPN1EnWeb.Entidades;

namespace TPN1EnWeb.Servicios.Interfaces
{
    public interface IGenreService
    {
        void Borrar(Genre genre);
        void Guardar(Genre genre);
        bool EstaRelacionado(Genre? genre);
        bool Existe(Genre? genre);
        int GetCantidad();
        IEnumerable<Genre>? GetGenres(Expression<Func<Genre, bool>>? filter = null,
           Func<IQueryable<Genre>, IOrderedQueryable<Genre>>? orderBy = null,
           string? propertiesNames = null);
        Genre? GetGenre(Expression<Func<Genre, bool>>? filter = null,
           string? propertiesNames = null, bool tracked = true);
    }
}
