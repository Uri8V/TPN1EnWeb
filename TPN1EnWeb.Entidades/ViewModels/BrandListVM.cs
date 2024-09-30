using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace TPN1EnWeb.Entidades.ViewModels
{
    public class BrandListVM
    {
        public int BrandId { get; set; }
        [Required(ErrorMessage = "{0} es requerido")]
        [StringLength(50, ErrorMessage = "{0} debe tener una longitud menor de 50 caracteres y mayor a 1", MinimumLength = 2)]
        [DisplayName("Brand Name")]
        public string BrandName { get; set; } = null!;
        public bool Active { get; set; } = true;

        [DisplayName("Shoes Count")]
        public int ShoeCount { get; set; }
        public string? imageURL { get; set; }
    }
}
