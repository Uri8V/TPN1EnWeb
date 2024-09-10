using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPN1EnWeb.Datos.Interfaces;
using TPN1EnWeb.Entidades;

namespace TPN1EnWeb.Datos.Repositorios
{
    public class SportRepository : GenericRepository<Sport>, ISportRepository
    {
        private readonly ShoesDbContext _context;
        public SportRepository(ShoesDbContext context) : base(context)
        {
            _context = context;
        }
        public void Editar(Sport sport)
        {
            _context.Sports.Update(sport);
        }
        public bool EstaRelacionado(Sport sport)
        {
            return _context.Shoes.Any(s => s.SportId == sport.SportId);
        }
        public bool Existe(Sport? sport)
        {
            if (sport!.SportId == 0)
            {
                return _context.Sports.Any(s => s.SportName == sport.SportName);
            }
            return _context.Sports.Any(s => s.SportName == sport.SportName && s.SportId != sport.SportId);
        }
        public int GetCantidad()
        {
            return _context.Sports.Count();
        }
    }
}

