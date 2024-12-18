using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPN1EnWeb.Entidades.ViewModels
{
    public class CityListVm
    {
        public int CityId { get; set; }
        [DisplayName("City")]
        public string? CityName { get; set; }
        [DisplayName("State")]

        public string? StateName { get; set; }
        [DisplayName("Country")]

        public string? CountryName { get; set; }
    }
}
