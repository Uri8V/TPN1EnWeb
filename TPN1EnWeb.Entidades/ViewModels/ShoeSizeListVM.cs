using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X.PagedList;

namespace TPN1EnWeb.Entidades.ViewModels
{
    public class ShoeSizeListVM
    {
        public int ShoeSizeId { get; set; }
        public int ShoeId { get; set; }
        public Shoe? shoe { get; set; }
        public PagedList<Size>? sizes { get; set; }
        public List<int>? Stocks { get; set; } = new List<int>();
    }
}
