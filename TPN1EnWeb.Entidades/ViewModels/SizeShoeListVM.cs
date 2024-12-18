using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection.Metadata.Ecma335;

namespace TPN1EnWeb.Entidades.ViewModels
{
    public class SizeShoeListVM
    {
        public int SizeId { get; set; }
        [Required(ErrorMessage = "Size is required")]
        [Range(1, int.MaxValue, ErrorMessage = "You must select a Size")]
        [DisplayName("Size")]
        public decimal SizeNumber { get; set; }
        [DisplayName("Brand Name")]
        public string BrandName { get; set; }= null!;
        [DisplayName("Colour Name")]
        public string ColorName { get; set; } = null!;
        [DisplayName("Genre Name")]
        public string GenreName { get; set; } = null!;
        [DisplayName("Sport Name")]
        public string SportName { get; set; } = null!;
        [DisplayName("Model")]
        public string Model { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Price { get; set; } = null!;
        public int ShoeId { get; set; }
        public int Stock {  get; set; }
    }
}
