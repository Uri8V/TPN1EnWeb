using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPN1EnWeb.Datos.Interfaces;

namespace TPN1EnWeb.Datos.Repositorios
{
    public class ShoeSizeRepository : IShoeSizeRepository
    {
        private readonly ShoesDbContext? _context;
        public ShoeSizeRepository(ShoesDbContext? context)
        {
            _context = context;
        }
        public int GetId()
        {
            return _context!.ShoeSizes.Max(i => i.ShoeSizeId) + 1;
        }
    }
}
