using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TPN1EnWeb.Entidades.ViewModels;
using TPN1EnWeb.Entidades;
using TPN1EnWeb.Servicios.Interfaces;
using TPN1EnWeb.Servicios.Servicios;
using X.PagedList.Extensions;
using System.Drawing.Drawing2D;
using X.PagedList;
using Microsoft.AspNetCore.Authorization;

namespace TPN1EnWeb.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class SportController : Controller
    {
        private readonly ISportService? _sportService;
        private readonly IMapper? _mapper;
        private readonly IShoeService _shoeService;
        public SportController(ISportService? sportService, IMapper? mapper, IShoeService shoeService)
        {
            _sportService = sportService;
            _mapper = mapper;
            _shoeService = shoeService;
        }

        public IActionResult Index(int? page, string? searchTerm = null, int pageSize = 10, bool viewAll = false)
        {
            int pagenumber = page ?? 1;
            ViewBag.currentPageSize = pageSize;
            ViewBag.currentsearchTerm = searchTerm; //Un viewBag es como una variable de la vista que se usa en la vista para no ensuciar más el modelo de mi vista
            IEnumerable<Sport> sports;
            if (!viewAll)
            {
                if (!string.IsNullOrEmpty(searchTerm))
                {
                    sports = _sportService!.GetSports(orderBy: o => o.OrderBy(c => c.SportName), filter: f => f.SportName.Contains(searchTerm!))!;
                }
                else
                {
                    sports = _sportService!.GetSports(orderBy: o => o.OrderBy(b => b.SportName))!;
                }
            }
            else
            {
                searchTerm = string.Empty;
                ViewBag.currentsearchTerm = searchTerm;
                sports = _sportService!.GetSports(orderBy: o => o.OrderBy(b => b.SportName))!;

            }
            var listasportdvm = new List<SportListVM>();
            foreach (var sport in sports!)
            {
                var sportVm = new SportListVM()
                {
                    SportId = sport.SportId,
                    SportName = sport.SportName,
                    ShoeCount = _shoeService!.GetCantidad(s => s.SportId == sport.SportId)
                };
                listasportdvm.Add(sportVm);
            }
            PagedList<SportListVM> listaVm = new PagedList<SportListVM>(listasportdvm, pagenumber, pageSize);
            listaVm.ToPagedList(pagenumber, pageSize);
            return View(listaVm);
        }

        public IActionResult UpSert(int? id, string? returnurl = null)
        {
            SportEditVM? sportEditVM;
            if (id is null || id.Value == 0)
            {
                sportEditVM = new SportEditVM();
            }
            else
            {
                Sport? sport = _sportService?.GetSport(filter: s => s.SportId == id);
                if (sport == null)
                {
                    return NotFound();
                }
                sportEditVM = _mapper?.Map<SportEditVM>(sport);
                sportEditVM!.ReturnUrl = returnurl;
            }
            return View(sportEditVM);
        }
        [HttpPost]
        public IActionResult UpSert(SportEditVM? sportEditVM)
        {
            string? returnurl = sportEditVM!.ReturnUrl;
            if (!ModelState.IsValid)
            {
                return View(sportEditVM);
            }
            Sport? sport = _mapper?.Map<Sport>(sportEditVM);
            if (sport == null)
            {
                ModelState.AddModelError(string.Empty, "No Colour has been supplied");
                return View(sportEditVM);
            }
            if (_sportService?.Existe(sport) ?? true)
            {
                ModelState.AddModelError(string.Empty, "Registro Duplicado¡¡¡¡");
                return View(sportEditVM);
            }
            _sportService.Guardar(sport);
            TempData["success"] = "Record added/edited successfully";
            return !string.IsNullOrEmpty(returnurl)
            ? Redirect(returnurl)
            : RedirectToAction("Index");
        }
        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Sport? sport = _sportService?.GetSport(filter: s => s.SportId == id);
            if (sport == null)
            {
                return NotFound();
            }
            if (_sportService?.EstaRelacionado(sport) ?? true)
            {
                return Json(new
                {
                    success = false,
                    message = "Related Record..."
                });
            }
            _sportService?.Borrar(sport);
            return Json(new { success = true, message = "Record deleted successfully" });

        }
        public IActionResult Details(int? id)
        {
            Sport Sport = _sportService?.GetSport(filter: filter => filter.SportId == id)!;
            if (Sport is null)
            {
                return NotFound();
            }
            IEnumerable<Shoe> listaShoeFiltradaPorBrand;
            listaShoeFiltradaPorBrand = _shoeService?.GetShoes(filter: filter => filter.SportId == id, propertiesNames: "Genres,Color,Sports,Brands")!;
            var listavm = _mapper?.Map<IEnumerable<ShoeListVM>>(listaShoeFiltradaPorBrand).ToList();
            return View(listavm);
        }
    }
}
