﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPN1EnWeb.Entidades;

namespace TPN1EnWeb.Datos.Interfaces
{
    public interface IShoppingCartsRepository : IGenericRepository<ShoppingCart>
    {
        void Update(ShoppingCart shoppingCart);
    }
}
