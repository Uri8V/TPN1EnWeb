using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TPN1EnWeb.Entidades;

namespace TPN1EnWeb.Servicios.Interfaces
{
    public interface ICountriesService
    {
        IEnumerable<Country>? GetPaged(int pageSize, int? page, Expression<Func<Country, bool>>? filter = null,
         Func<IQueryable<Country>, IOrderedQueryable<Country>>? orderBy = null,
         string? propertiesNames = null);
        IEnumerable<Country> GetAll(Expression<Func<Country, bool>>? filter = null,
            Func<IQueryable<Country>, IOrderedQueryable<Country>>? orderBy = null,
            string? propertiesNames = null);
        void Save(Country country);
        void Delete(Country country);
        Country? Get(Expression<Func<Country, bool>>? filter = null,
            string? propertiesNames = null,
            bool tracked = true);
        bool Exist(Country country);
        bool ItsRelated(int id);

    }
}
