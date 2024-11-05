using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPN1EnWeb.Entidades
{
    public class Shoe
    {
        public int ShoeId { get; set; }
        public int BrandId { get; set; }
        public Brand? Brands { get; set; }
        public int SportId { get; set; }
        public Sport? Sports { get; set; }
        public int GenreId { get; set; }
        public Genre? Genres { get; set; }
        public int ColourId { get; set; }
        public Colour? Color { get; set; }
        public string Model { get; set; } = null!;
        public string Descripcion { get; set; } = null!;
        public decimal Price { get; set; }
        public bool Active { get; set; } = true;
        public string? imageURL { get; set; }

        public ICollection<ShoeSizes> ShoeSize { get; set; } = new List<ShoeSizes>();
    }
}
