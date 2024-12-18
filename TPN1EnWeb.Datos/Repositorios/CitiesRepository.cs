using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPN1EnWeb.Datos.Interfaces;
using TPN1EnWeb.Entidades;

namespace TPN1EnWeb.Datos.Repositorios
{
    public class CitiesRepository : GenericRepository<City>, ICitiesRepository
    {
        private readonly ShoesDbContext _context;
        public CitiesRepository(ShoesDbContext context) : base(context)
        {
            _context = context;
        }

        public bool Exist(City city)
        {
            return city.CityId == 0 ? _context.Citys.Any(c => c.CityName == city.CityName
                    && c.StateId == city.StateId
                    && c.CountryId == city.CountryId) : _context.Citys.Any(c => c.CityName == city.CityName
                    && c.StateId == city.StateId
                    && c.CountryId == city.CountryId
                    && c.CityId != city.CityId);

        }

        public bool ItsRelated(int cityId)
        {
            return false;
        }

        public void Update(City city)
        {
            _context.Update(city);
        }
    }
}
