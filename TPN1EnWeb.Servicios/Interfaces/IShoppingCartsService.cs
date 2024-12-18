using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TPN1EnWeb.Entidades;

namespace TPN1EnWeb.Servicios.Interfaces
{
    public interface IShoppingCartsService
    {
        IEnumerable<ShoppingCart>? GetAll(Expression<Func<ShoppingCart, bool>>? filter = null,
            Func<IQueryable<ShoppingCart>, IOrderedQueryable<ShoppingCart>>? orderBy = null,
            string? propertiesNames = null);
        void Save(ShoppingCart shoppingCart);
        void Delete(ShoppingCart shoppingCart);
        ShoppingCart? Get(Expression<Func<ShoppingCart, bool>>? filter = null,
            string? propertiesNames = null,
            bool tracked = true);

    }
}
