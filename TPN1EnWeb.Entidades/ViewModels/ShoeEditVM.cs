using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;

namespace TPN1EnWeb.Entidades.ViewModels
{
    public class ShoeEditVM
    {
        public int shoeId { get; set; }
        [Required(ErrorMessage = "Brand is required")]
        [Range(1, int.MaxValue, ErrorMessage = "You must select a Brand")]
        [DisplayName("Brand")]
        public string? BrandId { get; set; }
        [Required(ErrorMessage = "Sport is required")]
        [Range(1, int.MaxValue, ErrorMessage = "You must select a Sport")]
        [DisplayName("Sport")]
        public string? SportId { get; set; }
        [Required(ErrorMessage = "Genre is required")]
        [Range(1, int.MaxValue, ErrorMessage = "You must select a Genre")]
        [DisplayName("Genre")]
        public string? GenreId { get; set; }
        [Required(ErrorMessage = "Color is required")]
        [Range(1, int.MaxValue, ErrorMessage = "You must select a Color")]
        [DisplayName("Color")]
        public string? ColorId { get; set; }
        [Required(ErrorMessage = "Model is required")]
        [DisplayName("Model")]
        public string model { get; set; }=null!;
        [Required(ErrorMessage = "Description is required")]
        [DisplayName("Description")]
        public string descripcion { get; set; } = null!;
        [Required(ErrorMessage = "Price is required")]
        [Range(1, int.MaxValue, ErrorMessage = "You must enter a Price")]
        [DisplayName("Price")]
        public decimal price { get; set; }
        public List<decimal> size { get; set; } = new List<decimal>();
        public List<int> Stock { get; set; } = new List<int>();

        [ValidateNever]
        public IEnumerable<SelectListItem> Brands { get; set; } = null!;
        [ValidateNever]
        public IEnumerable<SelectListItem> Sports { get; set; } = null!;
        [ValidateNever]
        public IEnumerable<SelectListItem> Genres { get; set; } = null!;
        [ValidateNever]
        public IEnumerable<SelectListItem> Colours { get; set; } = null!;
        [Display(Name = "Remove Image")]
        public bool RemoveImage { get; set; }  // Propiedad para borrar imagen cargada
        [DisplayName("Imagen")]
        public string? imageURL { get; set; }
        public IFormFile? ImageFile { get; set; }
        public string?  ReturnUrl { get; set; }
    }
}
