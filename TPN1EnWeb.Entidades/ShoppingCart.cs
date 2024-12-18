using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPN1EnWeb.Entidades
{
    public class ShoppingCart
    {
        //TODO: Migrar o crear tabla a mano
        public int ShoppingCartId { get; set; }
        public int ShoeSizeId { get; set; }
        public int Quantity { get; set; }
        public string ApplicationUserId { get; set; } = null!;
        public DateTime LastUpdated { get; set; } = DateTime.Now;
        public ShoeSizes ShoeSize { get; set; } = null!;
        public ApplicationUser ApplicationUser { get; set; } = null!;
    }
}
