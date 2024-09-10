using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPN1EnWeb.Entidades
{
    public class ShoeSizes
    {
        public int ShoeSizeId { get; set; }
        public int ShoeId { get; set; }
        public Shoe Shoe { get; set; }
        public int SizeId { get; set; }
        public Size Size { get; set; }
        public int QuantityInStock { get; set; }
    }
}
