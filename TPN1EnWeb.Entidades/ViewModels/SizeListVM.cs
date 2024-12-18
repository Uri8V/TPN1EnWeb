using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPN1EnWeb.Entidades.ViewModels
{
    public class SizeListVM
    {
        public int ShoeId { get; set; }
        public Shoe Shoe { get; set; }
        public List<Size> Sizes { get; set;} = new List<Size>();
        public List<ShoeSizes> ShoesSizes { get; set; } = new List<ShoeSizes>();
    }
}
