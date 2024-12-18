using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPN1EnWeb.Datos.Interfaces;
using TPN1EnWeb.Entidades;

namespace TPN1EnWeb.Datos.Repositorios
{
    public class CountriesRepository : GenericRepository<Country>, ICountriesRepository
    {
        private readonly ShoesDbContext _db;

        public CountriesRepository(ShoesDbContext db) : base(db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }



        public bool Exist(Country country)
        {
            if (country.CountryId == 0)
            {
                return _db.Countries.Any(c => c.CountryName == country.CountryName);
            }
            return _db.Countries.Any(c => c.CountryName == country.CountryName && c.CountryId != country.CountryId);
        }


        public bool ItsRelated(int id)
        {
            return _db.States.Any(p => p.CountryId == id);
        }

        public void Update(Country country)
        {
            _db.Countries.Update(country);

        }

    }
}
