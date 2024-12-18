using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace TPN1EnWeb.Datos.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
       IEnumerable<T>? GetAll(Expression<Func<T,bool>>? filter=null, 
           Func<IQueryable<T>,IOrderedQueryable<T>>? orderBy=null, 
           string? propertiesNames=null);
        IEnumerable<T>? GetPaged(int pageSize,int? page,Expression<Func<T, bool>>? filter = null,
          Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
          string? propertiesNames = null);
        T? Get(Expression<Func<T, bool>>? filter = null,
            string? propertiesNames=null,
            bool tracked=true);
        void Add(T entity);
        void Delete(T entity);
    }
}
