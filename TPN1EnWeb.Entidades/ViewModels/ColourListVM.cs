using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPN1EnWeb.Entidades.ViewModels
{
    public class ColourListVM
    {
        public int ColourId { get; set; }
        [Required(ErrorMessage = "{0} es requerido")]
        [StringLength(50, ErrorMessage = "{0} debe tener una longitud menor de 50 caracteres y mayor a 3", MinimumLength = 3)]
        [DisplayName("Color Name")]
        public string ColorName { get; set; } = null!;
        [DisplayName("Count Shoe")]
        public int ShoeCount { get; set; }
    }
}
