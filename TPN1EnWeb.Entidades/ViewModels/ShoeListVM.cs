using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPN1EnWeb.Entidades.ViewModels
{
    public class ShoeListVM
    {
        public int shoeId { get; set; }
        public string? brand { get; set; }
        public string? sport { get; set; }
        public string? genre { get; set; }
        public string? color { get; set; }
        public string model { get; set; } = null!;
        public string descripcion { get; set; } = null!;
        public decimal price { get; set; }
        public List<decimal> size { get; set; } = new List<decimal>();
        public List<int> Stock { get; set; } = new List<int>();
    }
}
