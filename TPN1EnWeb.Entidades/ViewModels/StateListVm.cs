using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPN1EnWeb.Entidades.ViewModels
{
    public class StateListVm
    {
        public int StateId { get; set; }
        public string StateName { get; set; } = null!;
        public string Country { get; set; } = null!;

    }
}
