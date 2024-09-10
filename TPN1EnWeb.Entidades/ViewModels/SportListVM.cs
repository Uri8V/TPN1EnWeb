using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPN1EnWeb.Entidades.ViewModels
{
    public class SportListVM
    {
        public int SportId { get; set; }
        [Required(ErrorMessage = "{0} es requerido")]
        [StringLength(50, ErrorMessage = "{0} debe tener una longitud menor de 50 caracteres y mayor a 3", MinimumLength = 4)]
        [DisplayName("Sport Name")]
        public string SportName { get; set; } = null!;
        [DisplayName("Shoe Count")]
        public int ShoeCount { get; set; }
    }
}
