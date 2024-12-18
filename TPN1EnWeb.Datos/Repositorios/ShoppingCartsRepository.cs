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
    public class ShoppingCartsRepository : GenericRepository<ShoppingCart>, IShoppingCartsRepository
    {
        private readonly ShoesDbContext _db;

        public ShoppingCartsRepository(ShoesDbContext db) : base(db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }
        public void Update(ShoppingCart shoppingCart)
        {
            _db.ShoppingCarts.Update(shoppingCart);
        }

        // Nuevo método para obtener carritos inactivos
        public IEnumerable<ShoppingCart> GetInactiveCarts(DateTime cutoffTime)
        {
            return _db.ShoppingCarts.Include(s => s.ShoeSize)
                .Where(sc => sc.ShoeSizeId == sc.ShoeSizeId
                 && sc.LastUpdated < cutoffTime).ToList();
        }
    }
}
