using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TPN1EnWeb.Datos.Interfaces;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace TPN1EnWeb.Datos.Repositorios
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly ShoesDbContext? _db;
        internal DbSet<T> dbSet { get; set; }
        //Cuando se implemente este repo yo le paso una entidad, por lo que el repo se amolda a esa entidad
        // por lo cual el dbSet va a hacer lo mismo
        public GenericRepository(ShoesDbContext? db) //me traigo el contexto con el injector de dependencia
        {
            _db = db?? throw new ArgumentException("Dependencies not set");
            dbSet = _db.Set<T>();//Preguntar porque no entendimos bien
        }

        public void Add(T entity)
        {
            try
            {
                dbSet.Add(entity);
            }
            catch (Exception)
            {

                throw new Exception("Error while adding an entity");
            }
        }

        public void Delete(T entity)
        {
            try
            {
                dbSet.Remove(entity);
            }
            catch (Exception)
            {

                throw new Exception("Error while removing an entity");
            }
        }

        public T? Get(Expression<Func<T, bool>>? filter = null, string? propertiesNames = null, bool tracked = true)
        {
            IQueryable<T> query = dbSet.AsQueryable();
            if (!string.IsNullOrWhiteSpace(propertiesNames))
            {
                foreach (var property in propertiesNames.Split(',', StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(property); //me va a traer todas las propiedades que yo agregue en el string
                    //separo las propiedades con la coma (,)
                }
            }
            if (filter != null)
            {
                query = query.Where(filter);
            }
            return tracked? query.FirstOrDefault(): query.AsNoTracking().FirstOrDefault();
        }

        public IEnumerable<T>? GetAll(Expression<Func<T, bool>>? filter = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, string? propertiesNames = null)
        {
            IQueryable<T> query = dbSet.AsNoTracking();
            if (!string.IsNullOrWhiteSpace(propertiesNames))
            {
                foreach (var property in propertiesNames.Split(',', StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(property); //me va a traer todas las propiedades que yo agregue en el string
                    //separo las propiedades con la coma (,)
                }
            }
            if (filter != null)
            {
                query = query.Where(filter);
            }
            if (orderBy!=null)
            {
                query = orderBy(query);
            }
            return query.ToList();
        }

        public IEnumerable<T>? GetPaged(int pageSize, int? page, Expression<Func<T, bool>>? filter = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, string? propertiesNames = null)
        {
            IQueryable<T> query = dbSet.AsNoTracking();
            if (!string.IsNullOrWhiteSpace(propertiesNames))
            {
                foreach (var property in propertiesNames.Split(',', StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(property); //me va a traer todas las propiedades que yo agregue en el string
                    //separo las propiedades con la coma (,)
                }
            }
            if (filter != null)
            {
                query = query.Where(filter);
            }
            if (orderBy != null)
            {
                query = orderBy(query);
            }
            return query.Skip((int)(page * pageSize)!)//Saltea estos registros
                .Take(pageSize)//Muestra estos
                .ToList();
        }
    }
}
