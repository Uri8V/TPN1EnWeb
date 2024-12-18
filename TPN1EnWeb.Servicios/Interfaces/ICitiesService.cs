using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TPN1EnWeb.Entidades;

namespace TPN1EnWeb.Servicios.Interfaces
{
    public interface ICitiesService
    {
        bool Exist(City city);
        IEnumerable<City> GetAll(Expression<Func<City, bool>>? filter = null,
            Func<IQueryable<City>, IOrderedQueryable<City>>? orderBy = null,
            string? propertiesNames = null);
        City? Get(Expression<Func<City, bool>> filter,
            string? propertiesNames = null,
            bool tracked = true);
        bool ItsRelated(int cityId);
        void Remove(City city);
        void Save(City city);
    }
}
