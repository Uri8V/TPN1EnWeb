using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X.PagedList;

namespace TPN1EnWeb.Entidades.ViewModels
{
    public class ShoeFilterVm
    {
        public IPagedList<ShoeListVM>? Shoes { get; set; }
        public List<SelectListItem>? Brands { get; set; }
        public List<SelectListItem>? Sports { get; set; }
        public List<SelectListItem>? Genres { get; set; } 
        public List<SelectListItem>? Colors { get; set; }
    }
}
