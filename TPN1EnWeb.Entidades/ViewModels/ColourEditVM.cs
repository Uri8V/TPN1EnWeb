using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TPN1EnWeb.Entidades.ViewModels
{
    public class ColourEditVM
    {
        public int ColourId { get; set; }
        [Required(ErrorMessage = "{0} es requerido")]
        [StringLength(50, ErrorMessage = "{0} debe tener una longitud menor de 50 caracteres y mayor a 3", MinimumLength = 3)]
        [DisplayName("Color Name")]
        public string ColorName { get; set; } = null!;
        public string? ReturnUrl { get; set; }
    }
}
