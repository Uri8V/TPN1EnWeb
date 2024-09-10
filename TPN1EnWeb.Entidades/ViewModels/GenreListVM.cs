using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPN1EnWeb.Entidades.ViewModels
{
    public class GenreListVM
    {
        public int GenreId { get; set; }
        [Required(ErrorMessage = "{0} es requerido")]
        [StringLength(10, ErrorMessage = "{0} debe tener una longitud menor de 50 caracteres y mayor a 4", MinimumLength = 4)]
        [DisplayName("Genre Name")]
        public string GenreName { get; set; } = null!;
        [DisplayName("Count Shoe")]
        public int ShoeCount { get; set; }

    }
}
