using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPN1EnWeb.Datos.Interfaces;
using TPN1EnWeb.Entidades;

namespace TPN1EnWeb.Datos.Repositorios
{
    public class StatesRepository : GenericRepository<State>, IStatesRepository
    {
        private readonly ShoesDbContext? _db;

        public StatesRepository(ShoesDbContext db) : base(db)
        {
            _db = db ?? throw new ArgumentException("Dependencies not set");
        }

        public bool Exist(State state)
        {
            if (state.StateId == 0)
            {
                return _db!.States.Any(s => s.StateName == state.StateName
                    && s.CountryId == state.CountryId);

            }
            return _db!.States.Any(s => s.StateName == state.StateName
                    && s.CountryId == state.CountryId
                    && s.StateId != state.StateId);
        }

        public bool ItsRelated(int id)
        {
            return _db!.Citys.Any(c => c.StateId == id);
        }

        public void Update(State state)
        {
            _db!.States.Update(state);
        }
    }
}
