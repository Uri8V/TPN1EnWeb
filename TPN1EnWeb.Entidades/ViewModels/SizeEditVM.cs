using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPN1EnWeb.Entidades.ViewModels
{
    public class SizeEditVM
    {
        public int SizeId { get; set; }
        [Required(ErrorMessage = "Size is required")]
        [Range(1, int.MaxValue, ErrorMessage = "You must select a Size")]
        [DisplayName("Size")]
        public decimal SizeNumber { get; set; }
    }
}
