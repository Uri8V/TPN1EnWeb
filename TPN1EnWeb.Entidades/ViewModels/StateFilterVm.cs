using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X.PagedList;

namespace TPN1EnWeb.Entidades.ViewModels
{
    public class StateFilterVm
    {
        public IPagedList<StateListVm>? States { get; set; }
        public List<SelectListItem>? Countries { get; set; }
    }
}
