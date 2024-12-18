using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TPN1EnWeb.Entidades;

namespace TPN1EnWeb.Servicios.Interfaces
{
    public interface IStatesService
    {
        IEnumerable<State> GetAll(Expression<Func<State, bool>>? filter = null,
    Func<IQueryable<State>, IOrderedQueryable<State>>? orderBy = null,
    string? propertiesNames = null);
        void Save(State state);
        void Delete(State state);
        State? Get(Expression<Func<State, bool>>? filter = null,
            string? propertiesNames = null,
            bool tracked = true);
        bool Exist(State state);
        bool ItsRelated(int id);


    }
}
