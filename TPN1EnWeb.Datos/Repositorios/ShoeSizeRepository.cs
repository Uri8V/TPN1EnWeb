using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPN1EnWeb.Datos.Interfaces;
using TPN1EnWeb.Entidades;

namespace TPN1EnWeb.Datos.Repositorios
{
    public class ShoeSizeRepository : GenericRepository<ShoeSizes>, IShoeSizeRepository
    {
        private readonly ShoesDbContext? _context;
        public ShoeSizeRepository(ShoesDbContext? context):base(context) 
        {
            _context = context;
        }
        public ShoeSizes GetIdShoeSize(int size, int shoe)
        {
            return _context!.ShoeSizes.FirstOrDefault(s => s.ShoeId == shoe && s.SizeId == size)!;
        }

        public void Editar(ShoeSizes shoeSizes)
        {
            _context!.Update(shoeSizes);
        }

        public bool ItsRelated(ShoeSizes shoeSizes)
        {
            return _context!.ShoppingCarts.Any(ss=>ss.ShoeSizeId==shoeSizes.ShoeSizeId) || _context.OrderDetails.Any(ss=>ss.ShoeSizeId==shoeSizes.ShoeSizeId);
        }
    }
}
