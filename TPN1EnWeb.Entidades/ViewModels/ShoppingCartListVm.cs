using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPN1EnWeb.Entidades.ViewModels
{
    public class ShoppingCartListVm
    {
        public List<ShoppingCart>? ShoppingCarts { get; set; }
        public OrderHeaderEditVm? OrderHeader { get; set; }
    }
}
