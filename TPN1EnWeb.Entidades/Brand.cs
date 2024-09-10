using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPN1EnWeb.Entidades
{
    public class Brand
    {
        public int BrandId { get; set; }
        public string BrandName { get; set; } = null!;
        public bool Active { get; set; } = true;
        public ICollection<Shoe>? Shoes { get; set; } = new List<Shoe>();
    }
}
