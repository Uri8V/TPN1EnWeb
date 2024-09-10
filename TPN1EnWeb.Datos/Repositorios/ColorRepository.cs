using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPN1EnWeb.Datos.Interfaces;
using TPN1EnWeb.Entidades;

namespace TPN1EnWeb.Datos.Repositorios
{
    public class ColorRepository:GenericRepository<Colour>, IColorRepository
    {
        private readonly ShoesDbContext _context;
        public ColorRepository(ShoesDbContext context):base(context) 
        {
            _context = context;
        }
        public void Editar(Colour Colour)
        {
            _context.Colors.Update(Colour);
        }
        public bool EstaRelacionado(Colour Colour)
        {
            return _context.Shoes.Any(c => c.ColourId == Colour.ColourId);
        }
        public bool Existe(Colour? Colour)
        {
            if (Colour!.ColourId == 0)
            {
                return _context.Colors.Any(c => c.ColorName == Colour.ColorName);
            }
            return _context.Colors.Any(c => c.ColorName == Colour.ColorName && c.ColourId != Colour.ColourId);
        }
        public int GetCantidad()
        {
            return _context.Colors.Count();
        }
    }
}
