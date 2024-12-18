using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPN1EnWeb.Entidades.ViewModels
{
    public class ShoeSizeDetailsCustomerAreaVM
    {
        public int ShoeId { get; set; }
        public ShoeListCustomerAreaVM Shoe { get; set; }
        public int SizeId { get; set; }
        public Size Size { get; set; }
        public int AvailableStock { get; set; }
        public int Page { get; set; }

    }
}
