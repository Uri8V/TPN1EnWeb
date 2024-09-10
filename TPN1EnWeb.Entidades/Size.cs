using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPN1EnWeb.Entidades
{
    public class Size
    {
        public int SizeId { get; set; }
        public decimal SizeNumber { get; set; }
        public ICollection<ShoeSizes> ShoeSize { get; set; } = new List<ShoeSizes>();
    }
}
