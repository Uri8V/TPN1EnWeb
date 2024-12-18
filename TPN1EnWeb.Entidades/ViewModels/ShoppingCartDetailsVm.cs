using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPN1EnWeb.Entidades.ViewModels
{
    public class ShoppingCartDetailVm
    {
        public int ShoppingCartId { get; set; }
        public int ShoeSizeId { get; set; }
        public int Quantity { get; set; }
        public string ApplicationUserId { get; set; } = null!;
        public ShoeSizeDetailsCustomerAreaVM ShoeSizeDetails { get; set; } = null!;

    }
}
