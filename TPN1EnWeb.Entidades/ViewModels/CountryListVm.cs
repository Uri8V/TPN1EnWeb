using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPN1EnWeb.Entidades.ViewModels
{
    public class CountryListVm
    {
        public int CountryId { get; set; }
        [DisplayName("Country")]
        public string CountryName { get; set; } = null!;

    }
}
