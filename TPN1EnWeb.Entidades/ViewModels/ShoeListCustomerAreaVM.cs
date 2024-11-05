using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPN1EnWeb.Entidades.ViewModels
{
    public class ShoeListCustomerAreaVM
    {
        public int shoeId { get; set; }
        public string? brand { get; set; }
        public string? sport { get; set; }
        public string? genre { get; set; }
        public string? color { get; set; }
        public string model { get; set; } = null!;
        public string descripcion { get; set; } = null!;
        public decimal price { get; set; }
        public decimal CashPrice { get; set; }
        public string? imageURL { get; set; }

    }
}
